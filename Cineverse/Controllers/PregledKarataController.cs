using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cineverse.Data;
using Cineverse.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Cineverse.Services;

namespace Cineverse.Controllers
{
    [Authorize]
    public class PregledKarataController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly QrCodeService _qrService;


        public PregledKarataController(ApplicationDbContext context, QrCodeService qrService)
        {
            _context = context;
            _qrService = qrService;
        }

        // GET: PregledKarata
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var rezultat = await (from r in _context.Rezervacija
                                  join k in _context.Karta on r.Id equals k.RezervacijaId
                                  join p in _context.Projekcija on r.ProjekcijaId equals p.Id
                                  join f in _context.Film on p.FilmId equals f.Id
                                  join d in _context.Dvorana on p.DvoranaId equals d.Id
                                  join s in _context.Sjediste on k.SjedisteId equals s.Id
                                  join c in _context.Cijena on r.CijenaId equals c.Id
                                  where r.KorisnikId.ToString() == userId
                                  select new
                                  {
                                      KartaId = k.Id,
                                      RezervacijaId = r.Id,
                                      NazivFilma = f.NazivFilma,
                                      Poster = f.Poster,
                                      Vrijeme = p.Vrijeme,
                                      Datum = p.Datum,
                                      NazivDvorane = d.NazivDvorane,
                                      Red = s.Red,
                                      Kolona = s.Kolona,
                                      OsnovnaCijena = c.OsnovnaCijena,
                                      Lokacija = p.Lokacija
                                  }).ToListAsync();

            var viewModelList = new List<PregledKarataViewModel>();

            foreach (var item in rezultat)
            {
                string qrText = $"rezervacijaid:{item.RezervacijaId}|Korisnik:{userId}|Film:{item.NazivFilma}|Datum:{item.Datum:yyyy-MM-dd}|Vrijeme:{item.Vrijeme:HH:mm}";
                string qrKodBase64 = _qrService.GenerateQrCodeBase64(qrText);

                viewModelList.Add(new PregledKarataViewModel
                {
                    KartaId = item.KartaId,
                    QRKod = qrKodBase64,
                    NazivFilma = item.NazivFilma,
                    SlikaFilmaUrl = item.Poster,
                    VrijemeProjekcije = item.Vrijeme,
                    DatumProjekcije = item.Datum,
                    Sala = item.NazivDvorane,
                    Red = item.Red.ToString(),
                    Sjediste = item.Kolona.ToString(),
                    Iznos = item.OsnovnaCijena,
                    Lokacija = item.Lokacija
                });
            }

            return View(viewModelList);
        }

        public async Task<IActionResult> PastReservations()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var danas = DateOnly.FromDateTime(DateTime.Now);

            var rezultat = await (from r in _context.Rezervacija
                                  join k in _context.Karta on r.Id equals k.RezervacijaId
                                  join p in _context.Projekcija on r.ProjekcijaId equals p.Id
                                  join f in _context.Film on p.FilmId equals f.Id
                                  join d in _context.Dvorana on p.DvoranaId equals d.Id
                                  join s in _context.Sjediste on k.SjedisteId equals s.Id
                                  join c in _context.Cijena on r.CijenaId equals c.Id
                                  where r.KorisnikId.ToString() == userId && p.Datum < danas
                                  select new
                                  {
                                      KartaId = k.Id,
                                      RezervacijaId = r.Id,
                                      NazivFilma = f.NazivFilma,
                                      Poster = f.Poster,
                                      Vrijeme = p.Vrijeme,
                                      Datum = p.Datum,
                                      NazivDvorane = d.NazivDvorane,
                                      Red = s.Red,
                                      Kolona = s.Kolona,
                                      OsnovnaCijena = c.OsnovnaCijena,
                                      Lokacija = p.Lokacija
                                  }).ToListAsync();

            var viewModelList = rezultat.Select(item => new PregledKarataViewModel
            {
                KartaId = item.KartaId,
                QRKod = "N/A",
                NazivFilma = item.NazivFilma,
                SlikaFilmaUrl = item.Poster,
                VrijemeProjekcije = item.Vrijeme,
                DatumProjekcije = item.Datum,
                Sala = item.NazivDvorane,
                Red = item.Red.ToString(),
                Sjediste = item.Kolona.ToString(),
                Iznos = item.OsnovnaCijena,
                Lokacija = item.Lokacija
            }).ToList();

            return View(viewModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> kreirajPregledKarata()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var postojeciQrKodovi = await _context.PregledKarata
                .Where(pk => pk.KorisnikId == userId)
                .Select(pk => pk.QRKod)
                .ToHashSetAsync();

            var karte = await (from k in _context.Karta
                               join r in _context.Rezervacija on k.RezervacijaId equals r.Id
                               join p in _context.Projekcija on r.ProjekcijaId equals p.Id
                               join f in _context.Film on p.FilmId equals f.Id
                               where r.KorisnikId == userId
                               select new
                               {
                                   KartaId = k.Id,
                                   RezervacijaId = r.Id,
                                   FilmNaziv = f.NazivFilma,
                                   DatumProjekcije = p.Datum,
                                   VrijemeProjekcije = p.Vrijeme
                               }).ToListAsync();

            var noviPregledi = new List<PregledKarata>();

            foreach (var karta in karte)
            {
                var qrText = $"rezervacijaid:{karta.RezervacijaId}|Korisnik:{userId}|Film:{karta.FilmNaziv}|Datum:{karta.DatumProjekcije:yyyy-MM-dd}|Vrijeme:{karta.VrijemeProjekcije:HH:mm}";


                var qrImageBase64 = _qrService.GenerateQrCodeBase64(qrText);

                if (!postojeciQrKodovi.Contains(qrImageBase64))
                {
                    var noviPregled = new PregledKarata
                    {
                        QRKod = qrImageBase64,
                        KorisnikId = userId
                    };

                    noviPregledi.Add(noviPregled);
                    postojeciQrKodovi.Add(qrImageBase64);
                }
            }
            if (noviPregledi.Any())
            {
                _context.PregledKarata.AddRange(noviPregledi);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "PregledKarata");
        }

        // GET: PregledKarata/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregledKarata = await _context.PregledKarata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregledKarata == null)
            {
                return NotFound();
            }

            return View(pregledKarata);
        }

        // GET: PregledKarata/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PregledKarata/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QRKod,KorisnikId")] PregledKarata pregledKarata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pregledKarata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pregledKarata);
        }

        // GET: PregledKarata/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregledKarata = await _context.PregledKarata.FindAsync(id);
            if (pregledKarata == null)
            {
                return NotFound();
            }
            return View(pregledKarata);
        }

        // POST: PregledKarata/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QRKod,KorisnikId")] PregledKarata pregledKarata)
        {
            if (id != pregledKarata.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pregledKarata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PregledKarataExists(pregledKarata.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pregledKarata);
        }

        // GET: PregledKarata/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregledKarata = await _context.PregledKarata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregledKarata == null)
            {
                return NotFound();
            }

            return View(pregledKarata);
        }

        // POST: PregledKarata/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pregledKarata = await _context.PregledKarata.FindAsync(id);
            if (pregledKarata != null)
            {
                _context.PregledKarata.Remove(pregledKarata);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PregledKarataExists(int id)
        {
            return _context.PregledKarata.Any(e => e.Id == id);
        }
        public IActionResult QrImage(string text)
        {
            var qrBytes = _qrService.GenerateQrCode(text ?? "Prazno");
            return File(qrBytes, "image/png");
        }
    }
}