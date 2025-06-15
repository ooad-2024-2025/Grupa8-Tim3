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

            // Direktno dohvati sve rezervacije za korisnika
            var rezervacije = await _context.Rezervacija
                .Where(r => r.KorisnikId.ToString() == userId)
                .ToListAsync();

            var viewModelList = new List<PregledKarataViewModel>();

            foreach (var rezervacija in rezervacije)
            {
                // Pronađi kartu za ovu rezervaciju
                var karta = await _context.Karta.FirstOrDefaultAsync(k => k.RezervacijaId == rezervacija.Id);
                if (karta == null) continue;

                // Pronađi QR kod iz PregledKarata (ako postoji)
                var pregledKarta = await _context.PregledKarata
                    .FirstOrDefaultAsync(pk => pk.KorisnikId.ToString() == userId);
                string qrKod = pregledKarta?.QRKod ?? "N/A";
                


                // Pronađi projekciju
                var projekcija = await _context.Projekcija.FirstOrDefaultAsync(p => p.Id == rezervacija.ProjekcijaId);
                if (projekcija == null) continue;

                // Pronađi film
                var film = await _context.Film.FirstOrDefaultAsync(f => f.Id == projekcija.FilmId);
                if (film == null) continue;

                // Pronađi dvoranu
                var dvorana = await _context.Dvorana.FirstOrDefaultAsync(d => d.Id == projekcija.DvoranaId);
                if (dvorana == null) continue;

                // Pronađi sjedište
                var sjediste = await _context.Sjediste.FirstOrDefaultAsync(s => s.Id == karta.SjedisteId);
                if (sjediste == null) continue;

                // Pronađi cijenu
                var cijena = await _context.Cijena.FirstOrDefaultAsync(c => c.Id == rezervacija.CijenaId);
                if (cijena == null) continue;
                string qrText = $"RezervacijaID:{rezervacija.Id}|Korisnik:{userId}|Film:{film.NazivFilma}|Datum:{projekcija.Datum:yyyy-MM-dd}|Vrijeme:{projekcija.Vrijeme:HH:mm}";

                string qrKodBase64 = _qrService.GenerateQrCodeBase64(qrText);
                // Kombinuj datum i vrijeme za prikaz

                viewModelList.Add(new PregledKarataViewModel
                {
                    /*QRKod = qrKod,*/QRKod = qrKodBase64,
                    NazivFilma = film.NazivFilma,
                    SlikaFilmaUrl = film.Poster,
                    VrijemeProjekcije = projekcija.Vrijeme,
                    DatumProjekcije = projekcija.Datum,
                    Sala = dvorana.NazivDvorane,
                    Red = sjediste.Red.ToString(),
                    Sjediste = sjediste.Kolona.ToString(),
                    Iznos = cijena.OsnovnaCijena,
                    Lokacija = projekcija.Lokacija
                });
            }

            return View(viewModelList);
        }

        
        public async Task<IActionResult> PastReservations()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            
            var rezervacije = await _context.Rezervacija
                .Where(r => r.KorisnikId.ToString() == userId)
                .ToListAsync();

            var viewModelList = new List<PregledKarataViewModel>();

            foreach (var rezervacija in rezervacije)
            {
                // Pronađi kartu za ovu rezervaciju
                var karta = await _context.Karta.FirstOrDefaultAsync(k => k.RezervacijaId == rezervacija.Id);
                if (karta == null) continue;

                // Pronađi QR kod iz PregledKarata (ako postoji)
                var pregledKarta = await _context.PregledKarata
                    .FirstOrDefaultAsync(pk => pk.KorisnikId.ToString() == userId);
                string qrKod = pregledKarta?.QRKod ?? "N/A";

                // Pronađi projekciju
                var projekcija = await _context.Projekcija.FirstOrDefaultAsync(p => p.Id == rezervacija.ProjekcijaId);
                if (projekcija == null) continue;

                // Pronađi film
                var film = await _context.Film.FirstOrDefaultAsync(f => f.Id == projekcija.FilmId);
                if (film == null) continue;

                // Pronađi dvoranu
                var dvorana = await _context.Dvorana.FirstOrDefaultAsync(d => d.Id == projekcija.DvoranaId);
                if (dvorana == null) continue;

                // Pronađi sjedište
                var sjediste = await _context.Sjediste.FirstOrDefaultAsync(s => s.Id == karta.SjedisteId);
                if (sjediste == null) continue;

                // Pronađi cijenu
                var cijena = await _context.Cijena.FirstOrDefaultAsync(c => c.Id == rezervacija.CijenaId);
                if (cijena == null) continue;

                // Kombinuj datum i vrijeme za prikaz

                viewModelList.Add(new PregledKarataViewModel
                {
                    QRKod = qrKod,
                    NazivFilma = film.NazivFilma,
                    SlikaFilmaUrl = film.Poster,
                    VrijemeProjekcije = projekcija.Vrijeme,
                    DatumProjekcije = projekcija.Datum,
                    Sala = dvorana.NazivDvorane,
                    Red = sjediste.Red.ToString(),
                    Sjediste = sjediste.Kolona.ToString(),
                    Iznos = cijena.OsnovnaCijena,
                    Lokacija = projekcija.Lokacija
                });
            }

            return View(viewModelList);
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