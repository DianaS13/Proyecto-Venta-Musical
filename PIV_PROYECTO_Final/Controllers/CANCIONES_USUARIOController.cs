using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIV_PROYECTO_Final.Data;
using PIV_PROYECTO_Final.Models;

namespace PIV_PROYECTO_Final.Controllers
{
    public class CANCIONES_USUARIOController : Controller
    {
        private readonly MUSICAL_AVANZADAContext _context;

        public CANCIONES_USUARIOController(MUSICAL_AVANZADAContext context)
        {
            _context = context;
        }


        private string ObtenerId()
        {
            string correo = @User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(cu => cu.Email == correo);
            string id = usuario != null ? usuario.Id : null;

            return id;
        }

        private decimal ObtenerTotalPrecioCancionesPorCodigo(string codigoCancion, string idUsuario)
        {
            var precioCancion = _context.ModuloCancione
                .Where(mc => mc.CodigoCancion == codigoCancion)
                .Select(mc => mc.PrecioCancion)
                .FirstOrDefault();

            return precioCancion;
        }
        [Authorize(Roles ="Administrador, Usuario")]
        public async Task<IActionResult> Index()
        {
            string idUsuario = ObtenerId().Replace("-", "");
            ViewBag.IdUsuario = idUsuario;
            var cancionesUsuario = await _context.CANCIONES_USUARIO
                .Include(cu => cu.IdCancionNavigation)
                .Where(c => c.ID_USUARIO == idUsuario && c.ESTADO == 1)
                .ToListAsync();

            decimal total = 0;
            foreach (var cancion in cancionesUsuario)
            {
                total += ObtenerTotalPrecioCancionesPorCodigo(cancion.ID_CANCION, idUsuario);
            }

            ViewBag.TotalCanciones = total;
            return View(cancionesUsuario);
        }
        [Authorize(Roles = "Administrador, Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CANCIONES_USUARIO == null)
            {
                return NotFound();
            }

            var cANCIONES_USUARIO = await _context.CANCIONES_USUARIO
                .FirstOrDefaultAsync(m => m.ID_CANCIONES_USUARIO == id);
            if (cANCIONES_USUARIO == null)
            {
                return NotFound();
            }

            return View(cANCIONES_USUARIO);
        }
        [Authorize(Roles = "Administrador, Usuario")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Usuario")]
        public async Task<IActionResult> Create([Bind("ID_CANCIONES_USUARIO,ID_USUARIO,ID_CANCION,ESTADO")] CANCIONES_USUARIO cANCIONES_USUARIO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cANCIONES_USUARIO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cANCIONES_USUARIO);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CANCIONES_USUARIO == null)
            {
                return NotFound();
            }

            var cANCIONES_USUARIO = await _context.CANCIONES_USUARIO.FindAsync(id);
            if (cANCIONES_USUARIO == null)
            {
                return NotFound();
            }
            return View(cANCIONES_USUARIO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("ID_CANCIONES_USUARIO,ID_USUARIO,ID_CANCION,ESTADO")] CANCIONES_USUARIO cANCIONES_USUARIO)
        {
            if (id != cANCIONES_USUARIO.ID_CANCIONES_USUARIO)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cANCIONES_USUARIO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CANCIONES_USUARIOExists(cANCIONES_USUARIO.ID_CANCIONES_USUARIO))
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
            return View(cANCIONES_USUARIO);
        }
        [Authorize(Roles = "Administrador, Usuario")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CANCIONES_USUARIO == null)
            {
                return NotFound();
            }

            var cANCIONES_USUARIO = await _context.CANCIONES_USUARIO
                .Include(cu => cu.IdCancionNavigation)
                .FirstOrDefaultAsync(m => m.ID_CANCIONES_USUARIO == id);
            if (cANCIONES_USUARIO == null)
            {
                return NotFound();
            }

            return View(cANCIONES_USUARIO);
        }
        [Authorize(Roles = "Administrador, Usuario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CANCIONES_USUARIO == null)
            {
                return Problem("Entity set 'MUSICAL_AVANZADAContext.CANCIONES_USUARIO'  is null.");
            }
            var cANCIONES_USUARIO = await _context.CANCIONES_USUARIO.FindAsync(id);
            if (cANCIONES_USUARIO != null)
            {
                _context.CANCIONES_USUARIO.Remove(cANCIONES_USUARIO);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CANCIONES_USUARIOExists(int id)
        {
            return (_context.CANCIONES_USUARIO?.Any(e => e.ID_CANCIONES_USUARIO == id)).GetValueOrDefault();
        }
    }
}
