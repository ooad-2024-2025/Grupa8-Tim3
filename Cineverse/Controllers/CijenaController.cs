﻿using System;
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
    public class CijenaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CijenaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cijena
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cijena.ToListAsync());
        }

        // GET: Cijena/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cijena = await _context.Cijena
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cijena == null)
            {
                return NotFound();
            }

            return View(cijena);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Cijena/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cijena/Create

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OsnovnaCijena,FilmId")] Cijena cijena)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cijena);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cijena);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Cijena/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cijena = await _context.Cijena.FindAsync(id);
            if (cijena == null)
            {
                return NotFound();
            }
            return View(cijena);
        }

        // POST: Cijena/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OsnovnaCijena,FilmId")] Cijena cijena)
        {
            if (id != cijena.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cijena);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CijenaExists(cijena.Id))
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
            return View(cijena);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Cijena/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cijena = await _context.Cijena
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cijena == null)
            {
                return NotFound();
            }

            return View(cijena);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Cijena/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cijena = await _context.Cijena.FindAsync(id);
            if (cijena != null)
            {
                _context.Cijena.Remove(cijena);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CijenaExists(int id)
        {
            return _context.Cijena.Any(e => e.Id == id);
        }
    }
}
