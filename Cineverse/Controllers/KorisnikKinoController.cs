using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cineverse.Data;
using Cineverse.Models;

namespace Cineverse.Controllers
{
    public class KorisnikKinoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikKinoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KorisnikKino
        public async Task<IActionResult> Index()
        {
            return View(await _context.KorisnikKino.ToListAsync());
        }

        // GET: KorisnikKino/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikKino = await _context.KorisnikKino
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnikKino == null)
            {
                return NotFound();
            }

            return View(korisnikKino);
        }

        // GET: KorisnikKino/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KorisnikKino/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] KorisnikKino korisnikKino)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnikKino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(korisnikKino);
        }

        // GET: KorisnikKino/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikKino = await _context.KorisnikKino.FindAsync(id);
            if (korisnikKino == null)
            {
                return NotFound();
            }
            return View(korisnikKino);
        }

        // POST: KorisnikKino/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] KorisnikKino korisnikKino)
        {
            if (id != korisnikKino.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnikKino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikKinoExists(korisnikKino.Id))
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
            return View(korisnikKino);
        }

        // GET: KorisnikKino/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikKino = await _context.KorisnikKino
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnikKino == null)
            {
                return NotFound();
            }

            return View(korisnikKino);
        }

        // POST: KorisnikKino/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnikKino = await _context.KorisnikKino.FindAsync(id);
            if (korisnikKino != null)
            {
                _context.KorisnikKino.Remove(korisnikKino);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikKinoExists(int id)
        {
            return _context.KorisnikKino.Any(e => e.Id == id);
        }
    }
}
