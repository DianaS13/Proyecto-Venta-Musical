using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PIV_PROYECTO_Final.Data;

namespace PIV_PROYECTO_Final.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<ListaUsuarioController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;



        public RolesController(UserManager<Usuario> userManager, ILogger<ListaUsuarioController> logger, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        //Roles
        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        // Detalles del Rol 
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            return role == null
                ? NotFound()
                : View(role);
        }
        //Crear Roles

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            // Validar el modelo antes de crear el rol.
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                // Verificar si el rol ya existe.
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);

                if (!roleExists)
                {
                    // Si no existe, crearlo.
                    await _roleManager.CreateAsync(model);
                }

                // Mostrar mensaje de éxito o redirigir.
                TempData["SuccessMessage"] = "Rol creado exitosamente.";
                ViewBag.Mensaje = "El Rol se agregó correctamente.";
                return RedirectToAction("Index"); // Redirigir a la acción Index después de crear el rol.
            }
            catch (Exception ex)
            {
                // Manejar las excepciones.
                ViewBag.Error = "Se produjo un error al intentar crear el producto. El error fue: " + ex;
                return View(model);
            }
        }
        // Editar Roles
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {

                return NotFound();
            }
            return View(role);
        }
        //Editar Roles
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, IdentityRole model)
        {
            var rolexiste = await _roleManager.FindByIdAsync(id);

            if (rolexiste == null)
            {
                ViewBag.Error = "Hubo error, vuelve a intentarlo";
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    rolexiste.Name = model.Name;
                }

                var result = await _roleManager.UpdateAsync(rolexiste);

                if (result.Succeeded)
                {
                    ViewBag.Mensaje = "Se edito el Rol con exito";
                }
            }
            return View(rolexiste);
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View("Delete", role);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            // Verificar si hay usuarios asociados a este rol
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                ModelState.AddModelError(string.Empty, "No se puede eliminar este rol porque hay usuarios asociados a él.");
                return View("Delete", role);
            }

            // Si no hay usuarios asociados, eliminar el rol
            var eliminar = await _roleManager.DeleteAsync(role);

            if (eliminar.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Error al eliminar el rol.");

            return View("Delete", role);
        }

        private async Task<bool> RolExistsAsync(string id) 
        {
            return await _roleManager.RoleExistsAsync(id);
        }
    }

}
