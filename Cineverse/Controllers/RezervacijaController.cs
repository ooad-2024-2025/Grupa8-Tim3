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

namespace Cineverse.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Potvrda(int projekcijaId, List<int> odabranaSjedista)
        {
            if (odabranaSjedista == null || !odabranaSjedista.Any())
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija
                .FirstOrDefaultAsync(p => p.Id == projekcijaId);
            if (projekcija == null) return NotFound("Projekcija nije pronađena.");

            var dvorana = await _context.Dvorana
                .FirstOrDefaultAsync(d => d.Id == projekcija.DvoranaId);
            var film = await _context.Film
                .FirstOrDefaultAsync(f => f.Id == projekcija.FilmId);
            if (dvorana == null || film == null) return NotFound("Dvorana ili film nije pronađen.");

            var cijena = await _context.Cijena
                .FirstOrDefaultAsync(c => c.FilmId == film.Id);
            if (cijena == null) return NotFound("Cijena nije pronađena.");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var korisnik = await _context.Users
                .FirstOrDefaultAsync(k => k.Id == userId);

            if (korisnik == null)
                return Unauthorized();

            decimal popustProcenat = 0m;

            if (korisnik.DatumRodjenja == null)
            {
                popustProcenat = 0m;
            }
            else
            {
                var danas = DateTime.Today;
                var godinaRodjenja = korisnik.DatumRodjenja.Value;
                int godine = danas.Year - godinaRodjenja.Year;
                if (godinaRodjenja > danas.AddYears(-godine))
                    godine--;

                if (korisnik.Email.Contains("kinoradnik"))
                {
                    popustProcenat = 0.10m;
                }
                else if (godine >= 65)
                {
                    popustProcenat = 0.30m;
                }
                else if (godine >= 15 && godine <= 26)
                {
                    popustProcenat = 0.15m;
                }
            }

            var ukupnaCijena = (decimal)cijena.OsnovnaCijena * (1 - popustProcenat) * odabranaSjedista.Count;

            var sjedistaInfo = new List<SjedisteInfo>();
            foreach (var sjedisteId in odabranaSjedista)
            {
                var sjediste = await _context.Sjediste
                    .FirstOrDefaultAsync(s => s.Id == sjedisteId);
                if (sjediste != null)
                {
                    sjedistaInfo.Add(new SjedisteInfo
                    {
                        Red = sjediste.Red,
                        Kolona = sjediste.Kolona
                    });
                }
            }

            var model = new PotvrdaRezervacijeViewModel
            {
                Film = film.NazivFilma,
                Dvorana = dvorana.NazivDvorane,
                Sjedista = sjedistaInfo,
                OsnovnaCijena = (decimal)cijena.OsnovnaCijena,
                Popust = popustProcenat,
                UkupnaCijena = ukupnaCijena,
                BrojSjedista = odabranaSjedista.Count,
                Poster = film.Poster,
                Datum = projekcija.Datum,
                Vrijeme = projekcija.Vrijeme
            };

            ViewBag.ProjekcijaId = projekcijaId;
            ViewBag.OdabranaSjedista = odabranaSjedista;

            return View(model);

        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rezervacija.ToListAsync());
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjekcijaId,Status,KorisnikId,CijenaId")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjekcijaId,Status,KorisnikId,CijenaId")] Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.Id))
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
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacija.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.Id == id);
        }
    }
}
