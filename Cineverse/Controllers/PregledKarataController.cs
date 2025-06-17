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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var korisnik = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var popustProcenat = IzracunajPopust(korisnik);

            var rezultat = await (from r in _context.Rezervacija
                                  join k in _context.Karta on r.Id equals k.RezervacijaId
                                  join p in _context.Projekcija on r.ProjekcijaId equals p.Id
                                  join f in _context.Film on p.FilmId equals f.Id
                                  join d in _context.Dvorana on p.DvoranaId equals d.Id
                                  join s in _context.Sjediste on k.SjedisteId equals s.Id
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
                                      Lokacija = p.Lokacija,
                                      FilmId = f.Id,
                                      red = s.Red,
                                      kolona = s.Kolona
                                  }).ToListAsync();

            var viewModelList = new List<PregledKarataViewModel>();

            foreach (var item in rezultat)
            {
                var cijena = await _context.Cijena.FirstOrDefaultAsync(c => c.FilmId == item.FilmId);
                decimal osnovnaCijena = (decimal)(cijena?.OsnovnaCijena ?? 0);

                string qrText = $"kartaid:{item.KartaId}|rezervacijaid:{item.RezervacijaId}|Korisnik:{userId}|Film:{item.NazivFilma}|Datum:{item.Datum:yyyy-MM-dd}|Vrijeme:{item.Vrijeme:HH:mm}|Red:{item.Red}|Kolona:{item.Kolona}";
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
                    Iznos = (double)Math.Round(osnovnaCijena * (1 - popustProcenat), 2),
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

            var korisnik = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var popustProcenat = IzracunajPopust(korisnik);
            var danas = DateOnly.FromDateTime(DateTime.Now);

            var rezultat = await (from r in _context.Rezervacija
                                  join k in _context.Karta on r.Id equals k.RezervacijaId
                                  join p in _context.Projekcija on r.ProjekcijaId equals p.Id
                                  join f in _context.Film on p.FilmId equals f.Id
                                  join d in _context.Dvorana on p.DvoranaId equals d.Id
                                  join s in _context.Sjediste on k.SjedisteId equals s.Id
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
                                      Lokacija = p.Lokacija,
                                      FilmId = f.Id,
                                      red = s.Red,
                                      kolona = s.Kolona
                                  }).ToListAsync();

            var viewModelList = new List<PregledKarataViewModel>();

            foreach (var item in rezultat)
            {
                var cijena = await _context.Cijena.FirstOrDefaultAsync(c => c.FilmId == item.FilmId);
                decimal osnovnaCijena = (decimal)(cijena?.OsnovnaCijena ?? 0);

                viewModelList.Add(new PregledKarataViewModel
                {
                    KartaId = item.KartaId,
                    QRKod = "N/A", // Ne prikazujemo QR kod za prošle karte
                    NazivFilma = item.NazivFilma,
                    SlikaFilmaUrl = item.Poster,
                    VrijemeProjekcije = item.Vrijeme,
                    DatumProjekcije = item.Datum,
                    Sala = item.NazivDvorane,
                    Red = item.Red.ToString(),
                    Sjediste = item.Kolona.ToString(),
                    Iznos = (double)Math.Round(osnovnaCijena * (1 - popustProcenat), 2),
                    Lokacija = item.Lokacija
                });
            }

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
                               join s in _context.Sjediste on k.SjedisteId equals s.Id
                               where r.KorisnikId == userId
                               select new
                               {
                                   KartaId = k.Id,
                                   RezervacijaId = r.Id,
                                   FilmNaziv = f.NazivFilma,
                                   DatumProjekcije = p.Datum,
                                   VrijemeProjekcije = p.Vrijeme,
                                   Red = s.Red,
                                   Kolona = s.Kolona
                               }).ToListAsync();

            var noviPregledi = new List<PregledKarata>();

            foreach (var karta in karte)
            {
                var qrText = $"kartaid:{karta.KartaId}|rezervacijaid:{karta.RezervacijaId}|Korisnik:{userId}|Film:{karta.FilmNaziv}|Datum:{karta.DatumProjekcije:yyyy-MM-dd}|Vrijeme:{karta.VrijemeProjekcije:HH:mm}|Red:{karta.Red}|Kolona:{karta.Kolona}";

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int kartaId)
        {
            var karta = await _context.Karta
                .Include(k => k.Rezervacija)
                .Include(k => k.Sjediste)
                .FirstOrDefaultAsync(k => k.Id == kartaId);

            if (karta == null) return NotFound();

            var projekcija = await _context.Projekcija
                .Include(p => p.Film)
                .FirstOrDefaultAsync(p => p.Id == karta.Rezervacija.ProjekcijaId);

            if (projekcija == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            // Regeneriši QR kod
            string qrText = $"kartaid:{karta.Id}|rezervacijaid:{karta.RezervacijaId}|Korisnik:{userId}|Film:{projekcija.Film.NazivFilma}|Datum:{projekcija.Datum:yyyy-MM-dd}|Vrijeme:{projekcija.Vrijeme:HH:mm}|Red:{karta.Sjediste.Red}|Kolona:{karta.Sjediste.Kolona}";
            string qrKodBase64 = _qrService.GenerateQrCodeBase64(qrText);

            // Pronađi odgovarajući pregled karata
            var pregled = await _context.PregledKarata
                .FirstOrDefaultAsync(pk => pk.KorisnikId == userId && pk.QRKod == qrKodBase64);

            if (pregled != null)
            {
                _context.PregledKarata.Remove(pregled);
            }

            _context.Karta.Remove(karta);

            // Obriši rezervaciju ako je ovo bila posljednja karta
            bool imaJosKarata = await _context.Karta.AnyAsync(k => k.RezervacijaId == karta.RezervacijaId && k.Id != karta.Id);
            if (!imaJosKarata)
            {
                var rezervacija = await _context.Rezervacija.FindAsync(karta.RezervacijaId);
                if (rezervacija != null)
                    _context.Rezervacija.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Rezervacija uspješno otkazana.";

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
        public IActionResult Uspjesno(string kod)
        {
            ViewBag.Kod = kod;
            return View();
        }

        // racunanje popusta
        private decimal IzracunajPopust(Korisnik korisnik)
        {
            decimal popustProcenat = 0m;

            if (korisnik.DatumRodjenja == null)
            {
                if (korisnik.Email.Contains("kinoradnik"))
                {
                    popustProcenat = 0.10m;
                }
            }
            else
            {
                var danas = DateTime.Today;
                var godinaRodjenja = korisnik.DatumRodjenja.Value;
                int godine = danas.Year - godinaRodjenja.Year;
                if (godinaRodjenja > danas.AddYears(-godine))
                    godine--;

                if (godine >= 65)
                {
                    popustProcenat = 0.30m;
                }
                else if (godine >= 15 && godine <= 26)
                {
                    popustProcenat = 0.15m;
                }
            }

            return popustProcenat;
        }

        //moze primat qr kod a i ne mora ugl treba ga validirati
        [HttpPost]
        public async Task<IActionResult> validacijaQRKoda(string qrText, int kartaId)
        {
            if (string.IsNullOrEmpty(qrText))
                return Json(new { valid = false, message = "QR kod je prazan" });

            try
            {
                var parts = qrText.Split('|');

                var kartaIdPart = parts.FirstOrDefault(p => p.StartsWith("kartaid:"));
                if (kartaIdPart == null)
                    return Json(new { valid = false, message = "QR kod ne sadrži ID karte" });

                if (!int.TryParse(kartaIdPart.Split(':')[1], out int qrKartaId))
                    return Json(new { valid = false, message = "ID karte u QR kodu nije validan broj" });

                if (qrKartaId != kartaId)
                    return Json(new { valid = false, message = "QR kod ne pripada ovoj karti" });

                var karta = await _context.Karta.FindAsync(kartaId);
                if (karta == null)
                    return Json(new { valid = false, message = "Karta sa datim ID-om ne postoji" });

                var rezervacijaIdPart = parts.FirstOrDefault(p => p.StartsWith("rezervacijaid:"));
                if (rezervacijaIdPart != null && int.TryParse(rezervacijaIdPart.Split(':')[1], out int qrRezervacijaId))
                {
                    if (qrRezervacijaId != karta.RezervacijaId)
                        return Json(new { valid = false, message = "QR kod ne odgovara rezervaciji za ovu kartu" });
                }

                var korisnikPart = parts.FirstOrDefault(p => p.StartsWith("Korisnik:"));
                if (korisnikPart != null)
                {
                    var qrKorisnikId = korisnikPart.Split(':')[1];
                    var trenutniKorisnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (qrKorisnikId != trenutniKorisnikId)
                        return Json(new { valid = false, message = "QR kod ne pripada trenutnom korisniku" });
                }

                return Json(new { valid = true, message = "QR kod je valjan i odgovara ovoj karti" });
            }
            catch (Exception ex)
            {
                return Json(new { valid = false, message = "Greška pri validaciji QR koda: " + ex.Message });
            }
        }
    }
}