using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIV_PROYECTO_Final.Models;

namespace PIV_PROYECTO_Final.Controllers
{
    public class GeneroMusicalController : Controller
    {
        private readonly MUSICAL_AVANZADAContext _context;

        public GeneroMusicalController(MUSICAL_AVANZADAContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var generos = await _context.GeneroMusical
                              .Where(g => g.CodigoGenero != "1")
                              .ToListAsync();

            return View(generos);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.GeneroMusical == null)
            {
                return NotFound();
            }

            var generoMusical = await _context.GeneroMusical
                .FirstOrDefaultAsync(m => m.CodigoGenero == id);
            if (generoMusical == null)
            {
                return NotFound();
            }

            return View(generoMusical);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("CodigoGenero,DescripcionGenero")] GeneroMusical generoMusical)
        {
            try
            {
                if (!_context.GeneroMusical.Any(g => g.CodigoGenero == generoMusical.CodigoGenero))
                {
                    _context.Add(generoMusical);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("CodigoGenero", "La codigo del género musical ya está en uso.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(generoMusical);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.GeneroMusical == null)
            {
                return NotFound();
            }

            var generoMusical = await _context.GeneroMusical.FindAsync(id);
            if (generoMusical == null)
            {
                return NotFound();
            }
            return View(generoMusical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id, [Bind("CodigoGenero,DescripcionGenero")] GeneroMusical generoMusical)
        {
            if (id != generoMusical.CodigoGenero)
            {
                return NotFound();
            }
            try { 
                try
                {
                    _context.Update(generoMusical);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneroMusicalExists(generoMusical.CodigoGenero))
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
            catch 
            {
                return View(generoMusical);
            }
        }

        // GET: GeneroMusical/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.GeneroMusical == null)
            {
                return NotFound();
            }

            var generoMusical = await _context.GeneroMusical
                .FirstOrDefaultAsync(m => m.CodigoGenero == id);
            if (generoMusical == null)
            {
                return NotFound();
            }

            return View(generoMusical);
        }

        // POST: GeneroMusical/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.GeneroMusical == null)
            {
                return Problem("Entity set 'MUSICAL_AVANZADAContext.GeneroMusical'  is null.");
            }
            var generoMusical = await _context.GeneroMusical.FindAsync(id);
            if (generoMusical != null)
            {
                _context.GeneroMusical.Remove(generoMusical);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeneroMusicalExists(string id)
        {
          return (_context.GeneroMusical?.Any(e => e.CodigoGenero == id)).GetValueOrDefault();
        }
    }
}
