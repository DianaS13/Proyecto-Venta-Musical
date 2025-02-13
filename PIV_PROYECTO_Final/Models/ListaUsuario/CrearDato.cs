using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models.ListaUsuario
{
    public class CrearDato
    {
        //
        // Metodos privados
        private const string RestriccionCedula = @"^\d-\d{4}-\d{4}$";
        private const string RestriccionTelefono = @"^\d{4}-\d{4}$";
        private const string RestriccionNombre = @"^[a-zA-Z\s]+$";
        private const string RestriccionTarjeta = @"^(\d{4}-){3}\d{3,4}$";
        private const string RestriccionCorreo = @"^\b[A-Za-z0-9._%+-]+@(gmail\.com|outlook\.com|hotmail.com|icloud.com|yahoo.com)\b$";

        //
        //
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
}
