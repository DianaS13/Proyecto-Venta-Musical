using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models.ListaUsuario
{
    public class EditarDato
    {
        // private const string RestriccionCedula = @"^[0-9]{9}$"; desde el momento que se crea el usuario, no se tendra que usar esta validacion
        private const string RestriccionCedula = @"^\d-\d{4}-\d{4}$";
        private const string RestriccionTelefono = @"^\d{4}-\d{4}$";
        private const string RestriccionNombre = @"^[a-zA-Z\s]+$";
        private const string RestriccionTarjeta = @"^(\d{4}-){3}\d{3,4}$";
        private const string RestriccionCorreo = @"^\b[A-Za-z0-9._%+-]+@(gmail\.com|outlook\.com|hotmail.com|icloud.com|yahoo.com)\b$";

        [Required(ErrorMessage = "El telefono es requerido")]
        [RegularExpression(RestriccionTelefono, ErrorMessage = "El numero de telefono debe contener exactamente 8 números '0000-0000'")]
        public string Telefono { get; set; }

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

        //El rol por defecto es "Usuario", al editar este, lo que realiza es desde el java scrip qeu esta en la vista cambia el parametro
        [Required]
        public string IdROl { get; set; }

        //No se ven, se agrega editar, ya que en el Correo, cuando se edita, el mismo parametro envia junto a estas propiedades para que realice el cambio perfectamente
        [Required]
        public string Username { get; set; }
        [Required]
        public string NormalizedName { get; set; }

    }
}
