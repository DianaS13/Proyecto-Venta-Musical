using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PIV_PROYECTO_Final.Data;
using System.Text.Encodings.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;
using PIV_PROYECTO_Final.Models.ListaUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PIV_PROYECTO_Final.Controllers
{
    public class ListaUsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<ListaUsuarioController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;



        public ListaUsuarioController(UserManager<Usuario> userManager, ILogger<ListaUsuarioController> logger, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }
        // Index
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var rolesConUsuarios = new List<RolesConUsuarios>();

            // Todos los usuarios
            var usuarios = await _userManager.Users.ToListAsync();

            foreach (var usuario in usuarios)
            {
                // Contiene roles asociados
                var rolesUsuario = await _userManager.GetRolesAsync(usuario);

                foreach (var rolNombre in rolesUsuario)
                {
                    //Rol por nnombre
                    var rol = await _roleManager.FindByNameAsync(rolNombre);

                    var rolConUsuarios = new RolesConUsuarios
                    {
                        RolId = rol.Id, 
                        RolNombre = rol.Name, 
                        UsuarioId = usuario.Id,
                        UsuarioNombre = usuario.NombreCompleto, 
                        Email = usuario.Email,
                        Genero = usuario.Genero,
                        TipoTarjeta = usuario.TipoTarjeta,
                        NumeroTarjeta = usuario.NumeroTarjeta,
                        Telefono = usuario.PhoneNumber,

                    };

                    // Agregar este objeto al modelo
                    rolesConUsuarios.Add(rolConUsuarios);
                }
            }

            return View(rolesConUsuarios);
        }

        //Details
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var User = await _userManager.FindByIdAsync(id);
            return User == null
                ? NotFound()
                : View(User);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(CrearDato model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var UsuarioId = await _userManager.FindByIdAsync(model.cedula);
            if (UsuarioId != null)
            {
                ViewBag.UsuarioExistente = true;
                return View(model);
            }
            else
            {
                ViewBag.UsuarioExistente = false;
            }

            var UsuarioCorreo = await _userManager.FindByNameAsync(model.Email);
            if (UsuarioCorreo != null)
            {
                ViewBag.UsuarioCorreo = true;
				ViewBag.ErrorMessage = "El correo electrónico ya está en uso.";
				return View(model);
            }
            else
            {
                ViewBag.UsuarioCorreo = false;
            }


            var newUser = new Usuario
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                Id = model.cedula,
                Genero = model.Genero,
                TipoTarjeta = model.TipoTarjeta,
                NumeroTarjeta = model.NumeroTarjeta,
                PhoneNumber = model.Telefono
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Usuario creado exitosamente.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = newUser.Id, code = code },
                    protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(newUser.Email, "Confirmar tu correo electrónico",
                    $"Por favor confirma tu cuenta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aquí</a>.");

                await _userManager.AddToRoleAsync(newUser, "Usuario");

                return RedirectToAction("Index", "ListaUsuario");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Obtener el rol actual del usuario
            var roleUsuario = await _userManager.GetRolesAsync(user);
            var rolActual = roleUsuario.FirstOrDefault();

            var model = new EditarDato
            {
                Telefono = user.PhoneNumber,
                NombreCompleto = user.NombreCompleto,
                cedula = user.Id,
                Genero = user.Genero,
                TipoTarjeta = user.TipoTarjeta,
                NumeroTarjeta = user.NumeroTarjeta,
                Correo = user.Email,
                Username = user.UserName,
                NormalizedName = user.NormalizedUserName,
                IdROl = rolActual // Asignar el rol actual al modelo
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id, EditarDato model)
        {
            if (id != model.cedula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.Email != model.Correo)
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Correo);
                    if (existingUser != null && existingUser.Id != user.Id)
                    {
                        ModelState.AddModelError("Correo", "El correo electrónico ya está en uso por otro usuario.");
                        return View(model);
                    }
                }

                // Actualizar los datos del usuario
                user.PhoneNumber = model.Telefono;
                user.NombreCompleto = model.NombreCompleto;
                user.Genero = model.Genero;
                user.TipoTarjeta = model.TipoTarjeta;
                user.NumeroTarjeta = model.NumeroTarjeta;
                user.Email = model.Correo;
                user.UserName = model.Username;
                user.NormalizedUserName = model.NormalizedName;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) 
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                var roleUsuario = await _userManager.GetRolesAsync(user);
                var rolActual = roleUsuario.FirstOrDefault();

                if (rolActual != model.IdROl)
                {
                    await _userManager.RemoveFromRoleAsync(user, rolActual);
                    var resultAddRole = await _userManager.AddToRoleAsync(user, model.IdROl);
                    if (!resultAddRole.Succeeded)
                    {
                        foreach (var error in resultAddRole.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CambiarContrasena(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID de usuario no proporcionado.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"No se pudo encontrar al usuario con ID '{id}'.");
            }

            var model = new Contrasena
            {
                Cedula = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CambiarContrasena(Contrasena model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Cedula);
            if (user == null)
            {
                return NotFound($"No se pudo encontrar al usuario con ID '{model.Cedula}'.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NuevaContrasena);

            if (result.Succeeded)
            {
                // Contraseña cambiada exitosamente
                return RedirectToAction("Index", "ListaUsuario"); 
            }
            else
            {
                // Manejar fallo en el cambio de contraseña
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }
    }
}
