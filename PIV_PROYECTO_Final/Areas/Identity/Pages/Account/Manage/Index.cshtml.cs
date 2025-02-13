// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIV_PROYECTO_Final.Data;

namespace PIV_PROYECTO_Final.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        //metodos privados
        private const string RestriccionCedula = @"^\d-\d{4}-\d{4}$";
        private const string RestriccionTelefono = @"^\d{4}-\d{4}$";
        private const string RestriccionNombre = @"^[a-zA-Z\s]+$";
        private const string RestriccionTarjeta = @"^(\d{4}-){3}\d{3,4}$";
        private const string RestriccionCorreo = @"^\b[A-Za-z0-9._%+-]+@(gmail\.com|outlook\.com|hotmail.com|icloud.com|yahoo.com)\b$";

        //

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "El telefono es requerido")]
            [RegularExpression(RestriccionTelefono, ErrorMessage = "El numero de telefono debe contener exactamente 8 números '0000-0000'")]
            public string PhoneNumber { get; set; }



            [Required(ErrorMessage = "El nombre es requerido")]
            [RegularExpression(RestriccionNombre, ErrorMessage = "El nombre no puede contener números, ni tildes")]
            public string NombreCompleto { get; set; }


            [Required(ErrorMessage = "La cédula es requerida")]
            [RegularExpression(RestriccionCedula, ErrorMessage = "La cédula debe contener un formato valido")]
            public string cedula { get; set; }


            [Required(ErrorMessage = "El género es requerido")]
            public string Genero { get; set; }


            [Required(ErrorMessage = "El tipo de tarjeta es requerido")]
            [RegularExpression("^(?!Desconocida$).*$", ErrorMessage = "Tipo de tarjeta inválido")]
            public string TipoTarjeta { get; set; }


            [Required(ErrorMessage = "El número de tarjeta es requerido")]
            [RegularExpression(RestriccionTarjeta, ErrorMessage = "El número de tarjeta debe contener entre 15 y 16 números")]
            public string NumeroTarjeta { get; set; }


            [Required(ErrorMessage = "El correo es requerido")]
            [RegularExpression(RestriccionCorreo, ErrorMessage = "El correo no tiene un formato válido")]
            [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
            public string Correo { get; set; }


            [Required]
            public string Username { get; set; }


            [Required]
            public string NormalizedName { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var nombre = user.NombreCompleto;
            var id = user.Id;
            var genero = user.Genero;
            var ttargeta = user.TipoTarjeta;
            var ntarjeta = user.NumeroTarjeta;
            var corre = user.Email;



            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                NombreCompleto = nombre,
                cedula = id,
                Genero = genero,
                TipoTarjeta = ttargeta,
                NumeroTarjeta = ntarjeta,
                Correo = corre,
                Username = user.UserName,
                NormalizedName = user.NormalizedUserName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se puede cargar el usuario con ID'{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se puede cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el Numero de telefono.";
                    return RedirectToPage();
                }
            }

            if (Input.NombreCompleto != user.NombreCompleto)
            {
                user.NombreCompleto = Input.NombreCompleto;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el Nombre Completo.";
                    return RedirectToPage();
                }
            }
            if (Input.cedula != user.Id)
            {
                user.Id = Input.cedula;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar La cedula.";
                    return RedirectToPage();
                }
            }

            if (Input.Genero != user.Genero)
            {
                user.Genero = Input.Genero;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el género.";
                    return RedirectToPage();
                }
            }
            if (Input.TipoTarjeta != user.TipoTarjeta)
            {
                user.TipoTarjeta = Input.TipoTarjeta;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el tipo de tarjeta.";
                    return RedirectToPage();
                }
            }
            if (Input.NumeroTarjeta != user.NumeroTarjeta)
            {
                user.NumeroTarjeta = Input.NumeroTarjeta;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el Numero de tarjeta.";
                    return RedirectToPage();
                }
            }
            if (Input.Correo != user.Email)
            {
                // Verificar si ya existe un usuario con el mismo correo

                var UsuarioExistente = await _userManager.FindByEmailAsync(Input.Correo);
                if (UsuarioExistente != null)
                {
                    StatusMessage = "Ya existe un usuario registrado con este correo electrónico";
                    return RedirectToPage();
                }

                user.Email = Input.Correo;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el correo";
                    return RedirectToPage();
                }
            }

            if (Input.Username != user.UserName)
            {
                user.UserName = Input.Username;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el Correo.";
                    return RedirectToPage();
                }
            }
            if (Input.NormalizedName != user.NormalizedUserName)
            {
                user.NormalizedUserName = Input.NormalizedName;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar actualizar el Correo.";
                    return RedirectToPage();
                }
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Tu perfil ha sido actualizado";
            return RedirectToPage();

        }
    }
}
