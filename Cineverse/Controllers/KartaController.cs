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
    [Authorize]
    public class KartaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KartaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> kreirajKartu(int rezervacijaId, List<int> sjedista)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }


            var rezervacija = await _context.Rezervacija.FirstOrDefaultAsync(r => r.Id == rezervacijaId);
            if (rezervacija == null)
            {
                TempData["Error"] = "Rezervacija nije pronađena.";
                return RedirectToAction("Index");
            }

            if (sjedista == null || !sjedista.Any())
            {
                TempData["Error"] = "Morate odabrati sjedišta.";
                return RedirectToAction("Details", new { id = rezervacijaId });
            }

            try
            {

                foreach (var sjedisteId in sjedista)
                {
                    // Provjeri da li sjedište već ima kartu za ovu rezervaciju
                    var postojecaKarta = await _context.Karta
                        .FirstOrDefaultAsync(k => k.RezervacijaId == rezervacijaId && k.SjedisteId == sjedisteId);

                    if (postojecaKarta == null)
                    {
                        var karta = new Karta
                        {
                            RezervacijaId = rezervacijaId,
                            SjedisteId = sjedisteId
                        };
                        _context.Karta.Add(karta);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Rezervacija je uspješno kreirana!";
                return RedirectToAction("Uspjeh", "Karta", new { rezervacijaId = rezervacija.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Došlo je do greške prilikom kreiranja karata.";
                return RedirectToAction("Details", new { id = rezervacijaId });
            }
        }


        public IActionResult Uspjeh(int rezervacijaId)
        {
            ViewBag.RezervacijaId = rezervacijaId;
            return View();
        }

        // GET: Karta
        public async Task<IActionResult> Index()
        {
            return View(await _context.Karta.ToListAsync());
        }

        // GET: Karta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var karta = await _context.Karta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (karta == null)
            {
                return NotFound();
            }

            return View(karta);
        }

        // GET: Karta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Karta/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RezervacijaId,SjedisteId")] Karta karta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(karta);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Karta je uspješno rezervisana";
                return RedirectToAction("Details", new { id = karta.Id });
            }

            return View(karta);
        }


        // GET: Karta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var karta = await _context.Karta.FindAsync(id);
            if (karta == null)
            {
                return NotFound();
            }
            return View(karta);
        }

        // POST: Karta/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RezervacijaId,SjedisteId")] Karta karta)
        {
            if (id != karta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(karta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KartaExists(karta.Id))
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
            return View(karta);
        }

        // GET: Karta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var karta = await _context.Karta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (karta == null)
            {
                return NotFound();
            }

            return View(karta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // id je ID karte koja se briše
        {
            // Pronađi kartu po ID-ju
            var karta = await _context.Karta.FindAsync(id);
            if (karta == null)
                return NotFound();

            // Uzmi ID rezervacije povezane s ovom kartom
            int rezervacijaId = karta.RezervacijaId;

            // Dohvati sve karte povezane s ovom rezervacijom
            var karteZaBrisanje = await _context.Karta
                .Where(k => k.RezervacijaId == rezervacijaId)
                .ToListAsync();

            // Obriši sve karte povezane s rezervacijom
            _context.Karta.RemoveRange(karteZaBrisanje);

            // Obriši rezervaciju
            var rezervacija = await _context.Rezervacija.FindAsync(rezervacijaId);
            if (rezervacija != null)
            {
                _context.Rezervacija.Remove(rezervacija);
            }

            // Spremi promjene
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool KartaExists(int id)
        {
            return _context.Karta.Any(e => e.Id == id);
        }
    }
}