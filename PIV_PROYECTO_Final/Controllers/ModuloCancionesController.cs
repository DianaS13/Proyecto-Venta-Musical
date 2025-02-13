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
    public class ModuloCancionesController : Controller
    {

        private readonly MUSICAL_AVANZADAContext _context;

        public ModuloCancionesController(MUSICAL_AVANZADAContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<IActionResult> Index()
        {
            var canciones = await _context.ModuloCancione
                                .Include(m => m.CodigoGeneroNavigation)
                                .Where(c => c.CodigoCancion != "01" && c.CodigoGenero != "1")
                                .ToListAsync();

            return View(canciones);
        }

        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ModuloCancione == null)
            {
                return NotFound();
            }

            var moduloCancione = await _context.ModuloCancione
                .Include(m => m.CodigoGeneroNavigation)
                .FirstOrDefaultAsync(m => m.CodigoCancion == id);
            if (moduloCancione == null)
            {
                return NotFound();
            }

            return View(moduloCancione);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            var generosExcluidos = _context.GeneroMusical.Where(g => g.CodigoGenero != "1").ToList();
            ViewData["CodigoGenero"] = new SelectList(generosExcluidos, "CodigoGenero", "CodigoGenero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("CodigoCancion,NombreCancion,PrecioCancion,CodigoGenero")] ModuloCancione moduloCancione)
        {
            if (!_context.ModuloCancione.Any(c => c.CodigoCancion == moduloCancione.CodigoCancion))
            {
                _context.Add(moduloCancione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("CodigoCancion", "El código de canción ya está en uso.");
            }
            ViewData["CodigoGenero"] = new SelectList(_context.GeneroMusical, "CodigoGenero", "CodigoGenero", moduloCancione.CodigoGenero);
            return View(moduloCancione);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id)

        {
            if (id == null || _context.ModuloCancione == null)
            {
                return NotFound();
            }

            var moduloCancione = await _context.ModuloCancione.FindAsync(id);
            if (moduloCancione == null)
            {
                return NotFound();
            }
            var generosExcluidos = _context.GeneroMusical.Where(g => g.CodigoGenero != "1").ToList();
            ViewData["CodigoGenero"] = new SelectList(generosExcluidos, "CodigoGenero", "CodigoGenero", moduloCancione.CodigoGenero);
            return View(moduloCancione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id, [Bind("CodigoCancion,NombreCancion,PrecioCancion,CodigoGenero")] ModuloCancione moduloCancione)
        {
            if (id != moduloCancione.CodigoCancion)
            {
                return NotFound();
            }

            try { 
                try
            {
                _context.Update(moduloCancione);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuloCancioneExists(moduloCancione.CodigoCancion))
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
            catch { throw; }
            //ViewData["CodigoGenero"] = new SelectList(_context.GeneroMusical, "CodigoGenero", "CodigoGenero", moduloCancione.CodigoGenero);
            //return View(moduloCancione);
        }



        //-----------------------------------------------------------------------------------------------------------------------------------//

        private string ObtenerId()
        {
            string correo = @User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(cu => cu.Email == correo);
            string id = usuario != null ? usuario.Id : null;

            return id;
        }






        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<IActionResult> Comprar(string id)
        {
            if (id == null || _context.ModuloCancione == null)
            {
                return NotFound();
            }

            var moduloCancione = await _context.ModuloCancione.FindAsync(id);
            if (moduloCancione == null)
            {
                return NotFound();
            }
            ViewData["PrecioCancion"] = moduloCancione.PrecioCancion;

            ViewData["CodigoGenero"] = new SelectList(_context.GeneroMusical, "CodigoGenero", "CodigoGenero", moduloCancione.CodigoGenero);
            return View(moduloCancione);
        }

        [Authorize(Roles = "Administrador,Usuario")]//---> esto fue lo que le hice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comprar(string id, string ID_USUARIO, string ID_CANCION)
        {
            //--> y esto
            if (!User.Identity.IsAuthenticated)
            {
                
                throw new InvalidOperationException("Por favor inicie sesión para realizar una compra.");
            }

            string cedula = ObtenerId();
            cedula = cedula.Replace("-", "");
            @ViewBag.IdUsuario = cedula;

            CANCIONES_USUARIO moduloCancionesUsuario = new CANCIONES_USUARIO(0, cedula, ID_CANCION, 1);

            if (id != moduloCancionesUsuario.ID_CANCION)
            {
                return NotFound();
            }

            try
            {
                _context.Add(moduloCancionesUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuloCancioneExists(moduloCancionesUsuario.ID_CANCION))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduloCancione = await _context.ModuloCancione
                .Include(m => m.CodigoGeneroNavigation)
                .FirstOrDefaultAsync(m => m.CodigoCancion == id);

            if (moduloCancione == null)
            {
                return NotFound();
            }

            return View(moduloCancione);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ModuloCancione == null)
            {
                return Problem("Entity set 'MUSICAL_AVANZADAContext.ModuloCancione'  is null.");
            }
            var moduloCancione = await _context.ModuloCancione.FindAsync(id);
            if (moduloCancione != null)
            {
                _context.ModuloCancione.Remove(moduloCancione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool ModuloCancioneExists(string id)
        {
            return (_context.ModuloCancione?.Any(e => e.CodigoCancion == id)).GetValueOrDefault();
        }
    }
}

