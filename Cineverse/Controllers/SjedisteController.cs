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
    public class SjedisteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SjedisteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sjediste
        [Authorize(Roles = "Administrator, Korisnik, Kino radnik")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sjediste.ToListAsync());
        }

        // GET: Sjediste/Details/5
        [Authorize(Roles = "Administrator, Korisnik, Kino radnik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sjediste = await _context.Sjediste
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sjediste == null)
            {
                return NotFound();
            }

            return View(sjediste);
        }

        // GET: Sjediste/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sjediste/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Red,Kolona,TipSjedista,DvoranaId")] Sjediste sjediste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sjediste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sjediste);
        }

        // GET: Sjediste/Edit/5
        [Authorize(Roles = "Administrator, Kino radnik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sjediste = await _context.Sjediste.FindAsync(id);
            if (sjediste == null)
            {
                return NotFound();
            }
            return View(sjediste);
        }

        // POST: Sjediste/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Kino radnik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Red,Kolona,TipSjedista,DvoranaId")] Sjediste sjediste)
        {
            if (id != sjediste.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sjediste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SjedisteExists(sjediste.Id))
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
            return View(sjediste);
        }

        // GET: Sjediste/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sjediste = await _context.Sjediste
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sjediste == null)
            {
                return NotFound();
            }

            return View(sjediste);
        }

        // POST: Sjediste/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sjediste = await _context.Sjediste.FindAsync(id);
            if (sjediste != null)
            {
                _context.Sjediste.Remove(sjediste);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SjedisteExists(int id)
        {
            return _context.Sjediste.Any(e => e.Id == id);
        }
    }
}
