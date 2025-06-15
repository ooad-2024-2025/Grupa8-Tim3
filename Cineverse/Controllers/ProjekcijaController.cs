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
    public class ProjekcijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjekcijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projekcija
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projekcija.ToListAsync());
        }

        // GET: Projekcija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projekcija == null)
            {
                return NotFound();
            }

            return View(projekcija);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Projekcija/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projekcija/Create

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DvoranaId,Lokacija,Datum,Vrijeme,FilmId")] Projekcija projekcija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projekcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projekcija);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Projekcija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija.FindAsync(id);
            if (projekcija == null)
            {
                return NotFound();
            }
            return View(projekcija);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Projekcija/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DvoranaId,Lokacija,Datum,Vrijeme,FilmId")] Projekcija projekcija)
        {
            if (id != projekcija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projekcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjekcijaExists(projekcija.Id))
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
            return View(projekcija);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Projekcija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projekcija == null)
            {
                return NotFound();
            }

            return View(projekcija);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Projekcija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projekcija = await _context.Projekcija.FindAsync(id);
            if (projekcija != null)
            {
                _context.Projekcija.Remove(projekcija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjekcijaExists(int id)
        {
            return _context.Projekcija.Any(e => e.Id == id);
        }
    }
}
