using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models.ListaUsuario
{
    public class Contrasena
    {

        [Required(ErrorMessage = "La Nueva contrasena es requerida")]
        [StringLength(100, ErrorMessage = "Contiene con una longitud de 100", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NuevaContrasena")]
        public string NuevaContrasena { get; set; }

        [Required]
        public string Cedula { get; set; }

    }
}
