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

namespace Cineverse.Controllers
{
    [Authorize]
    public class PregledKarataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PregledKarataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PregledKarata
        public async Task<IActionResult> Index()
        {
            //return View(await _context.PregledKarata.ToListAsync());
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Filtriraj karte po korisniku
            var karte = await _context.PregledKarata
                .Where(k => k.KorisnikId.ToString() == userId)
                .ToListAsync();

            // Ako želiš prikazati i podatke o filmu, trebaš proširiti upit (vidi sledeći korak)
            return View(karte);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
    }
}
