#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PIV_PROYECTO_Final.Data;

namespace PIV_PROYECTO_Final.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IUserStore<Usuario> _userStore;
        private readonly IUserEmailStore<Usuario> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        ///metodos privados para el modelo///
        private const string RestriccionCedula = @"^\d-\d{4}-\d{4}$";
        private const string RestriccionTelefono = @"^\d{4}-\d{4}$";
        private const string RestriccionNombre = @"^[a-zA-Z\s]+$";
        private const string RestriccionTarjeta = @"^(\d{4}-){3}\d{3,4}$";
        private const string RestriccionCorreo = @"^\b[A-Za-z0-9._%+-]+@(gmail\.com|outlook\.com|hotmail.com|icloud.com|yahoo.com)\b$";
        //-------------------------------///

        public RegisterModel(
            UserManager<Usuario> userManager,
            IUserStore<Usuario> userStore,
            SignInManager<Usuario> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "El correo es requerido")]
            [RegularExpression(RestriccionCorreo, ErrorMessage = "El correo no tiene un formato válido")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "La contrasena es requerido")]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "La confirmacion de contrasena es requerido")]
            [Compare("Password", ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
            public string ConfirmPassword { get; set; }


            [Required(ErrorMessage = "El nombre es requerido")]
            [RegularExpression(RestriccionNombre, ErrorMessage = "El nombre no puede contener números,  ni tildes")]
            public string NombreCompleto { get; set; }


            [Required(ErrorMessage = "La cédula es requerida")]
            [RegularExpression(RestriccionCedula, ErrorMessage = "La cédula debe contener exactamente 9 números y el formato valido '0-0000-0000'")]
            public string cedula { get; set; }


            [Required(ErrorMessage = "El genero es requerido")]
            public string Genero { get; set; }


            [Required(ErrorMessage = "El tipo de tarjeta es requerido")]
            [RegularExpression("^(?!Desconocida$).*$", ErrorMessage = "Tipo de tarjeta inválido")]
            public string TipoTarjeta { get; set; }


            [Required(ErrorMessage = "El número de tarjeta es requerido")]
            [RegularExpression(RestriccionTarjeta, ErrorMessage = "Tarjeta no valida")]
            public string NumeroTarjeta { get; set; }


            [Required(ErrorMessage = "El telefono es requerido")]
            [RegularExpression(RestriccionTelefono, ErrorMessage = "El numero de telefono debe contener exactamente 8 números '0000-0000'")]
            public string Telefono { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var UsuarioExistente = await _userManager.FindByIdAsync(Input.cedula);
                if (UsuarioExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "La cedula ya existe, no se puede crear el usuario.");
                    return Page();
                }

                // Verifica el correo si esta en la base de datos 
                var ExisteCorreo = await _userManager.FindByEmailAsync(Input.Email);
                if (ExisteCorreo != null)
                {
                    ModelState.AddModelError(string.Empty, "El correo electrónico ya está en uso.");
                    return Page();
                }

                var user = CreateUser();

                //No se utiliza

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                user.NombreCompleto = Input.NombreCompleto;
                user.Id = Input.cedula;
                user.Genero = Input.Genero;
                user.TipoTarjeta = Input.TipoTarjeta;
                user.NumeroTarjeta = Input.NumeroTarjeta;
                user.PhoneNumber = Input.Telefono;

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _userManager.AddToRoleAsync(user, "Usuario"); // para agregar roles

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

        private Usuario CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Usuario>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Usuario)}'. " +
                    $"Ensure that '{nameof(Usuario)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Usuario> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Usuario>)_userStore;
        }
    }
}
