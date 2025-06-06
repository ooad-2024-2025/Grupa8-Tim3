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
    public class DvoranaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DvoranaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator,Korisnik, Kino radnik")]
        // GET: Dvorana
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dvorana.ToListAsync());
        }

        // GET: Dvorana/Details/5
        [Authorize(Roles = "Administrator,Korisnik, Kino radnik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvorana = await _context.Dvorana
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dvorana == null)
            {
                return NotFound();
            }

            return View(dvorana);
        }

        // GET: Dvorana/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dvorana/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,NazivDvorane,Kapacitet,SlobodnaMjesta")] Dvorana dvorana)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dvorana);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dvorana);
        }

        // GET: Dvorana/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvorana = await _context.Dvorana.FindAsync(id);
            if (dvorana == null)
            {
                return NotFound();
            }
            return View(dvorana);
        }

        // POST: Dvorana/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NazivDvorane,Kapacitet,SlobodnaMjesta")] Dvorana dvorana)
        {
            if (id != dvorana.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dvorana);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DvoranaExists(dvorana.Id))
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
            return View(dvorana);
        }

        // GET: Dvorana/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvorana = await _context.Dvorana
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dvorana == null)
            {
                return NotFound();
            }

            return View(dvorana);
        }

        // POST: Dvorana/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dvorana = await _context.Dvorana.FindAsync(id);
            if (dvorana != null)
            {
                _context.Dvorana.Remove(dvorana);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DvoranaExists(int id)
        {
            return _context.Dvorana.Any(e => e.Id == id);
        }
    }
}
