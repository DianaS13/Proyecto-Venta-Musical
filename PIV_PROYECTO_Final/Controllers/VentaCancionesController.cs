using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIV_PROYECTO_Final.Models;

namespace PIV_PROYECTO_Final.Controllers
{
    public class VentaCancionesController : Controller
    {
        private readonly MUSICAL_AVANZADAContext _context;

        public VentaCancionesController(MUSICAL_AVANZADAContext context)
        {
            _context = context;
        }
		[Authorize(Roles = "Administrador,Usuario")]
		public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string UsuarioRegistrado = userId.Replace("-", "");
            bool esAdministrador = User.IsInRole("Administrador");

            IQueryable<VentaCancione> ventasCancionesQuery;

            if (esAdministrador)
            {
                ventasCancionesQuery = _context.VentaCancione
                    .Include(v => v.IdCancionesUsuarioNavigation);
            }
            else
            {
                ventasCancionesQuery = _context.VentaCancione
                    .Include(v => v.IdCancionesUsuarioNavigation)
                    .Where(v => v.UsuariosIdentificacion.Replace("-", "") == UsuarioRegistrado);
            }

            var ventasCancionesUsuario = await ventasCancionesQuery
                .ToListAsync();

            return View(ventasCancionesUsuario);
        }

		[Authorize(Roles = "Administrador,Usuario")]
		public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.VentaCancione == null)
            {
                return NotFound();
            }

            var ventaCancione = await _context.VentaCancione
                .Include(v => v.IdCancionesUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.NumeroFactura == id);
            if (ventaCancione == null)
            {
                return NotFound();
            }

            return View(ventaCancione);
        }



		[Authorize(Roles = "Administrador,Usuario")]
		public IActionResult Create()
        {
            ViewData["CodigoCancion"] = new SelectList(_context.ModuloCancione, "CodigoCancion", "CodigoCancion");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Administrador,Usuario")]
		public async Task<IActionResult> Create([Bind("NumeroFactura,Subtotal,Total,FechaCompra,UsuariosIdentificacion,IdCancionesUsuario")] VentaCancione ventaCancione)
        {
            if (ventaCancione.Subtotal == 0.00m)
            {
                return RedirectToAction("Index", "ModuloCanciones");
            }
            if (_context.VentaCancione.Any(v => v.NumeroFactura == ventaCancione.NumeroFactura))
            {
                ModelState.AddModelError("NumeroFactura", "Ya existe una factura con este número.");
                ViewData["CodigoCancion"] = new SelectList(_context.ModuloCancione, "CodigoCancion", "CodigoCancion", ventaCancione.IdCancionesUsuario);
                return View(ventaCancione);
            }

            _context.Add(ventaCancione);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["CodigoCancion"] = new SelectList(_context.ModuloCancione, "CodigoCancion", "CodigoCancion", ventaCancione.IdCancionesUsuario);
            return View(ventaCancione);
        }
		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.VentaCancione == null)
            {
                return NotFound();
            }

            var ventaCancione = await _context.VentaCancione.FindAsync(id);
            if (ventaCancione == null)
            {
                return NotFound();
            }
            ViewData["CodigoCancion"] = new SelectList(_context.ModuloCancione, "CodigoCancion", "CodigoCancion", ventaCancione.IdCancionesUsuario);
            return View(ventaCancione);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Edit(string id, [Bind("NumeroFactura,Subtotal,Total,FechaCompra,UsuariosIdentificacion,CodigoCancion")] VentaCancione ventaCancione)
        {
            if (id != ventaCancione.NumeroFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ventaCancione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaCancioneExists(ventaCancione.NumeroFactura))
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
            ViewData["CodigoCancion"] = new SelectList(_context.ModuloCancione, "CodigoCancion", "CodigoCancion", ventaCancione.IdCancionesUsuario);
            return View(ventaCancione);
        }
		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.VentaCancione == null)
            {
                return NotFound();
            }

            var ventaCancione = await _context.VentaCancione
                .Include(v => v.IdCancionesUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.NumeroFactura == id);
            if (ventaCancione == null)
            {
                return NotFound();
            }

            return View(ventaCancione);
        }
		[Authorize(Roles = "Administrador")]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.VentaCancione == null)
            {
                return Problem("Entity set 'MUSICAL_AVANZADAContext.VentaCancione'  is null.");
            }
            var ventaCancione = await _context.VentaCancione.FindAsync(id);
            if (ventaCancione != null)
            {
                _context.VentaCancione.Remove(ventaCancione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaCancioneExists(string id)
        {
            return (_context.VentaCancione?.Any(e => e.NumeroFactura == id)).GetValueOrDefault();
        }
    }
}
