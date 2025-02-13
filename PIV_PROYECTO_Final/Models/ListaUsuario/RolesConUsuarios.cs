using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models.ListaUsuario
{
    public class RolesConUsuarios
    {
        //Este es el index el que muestra todos los datos
        [Required]
        public string RolId { get; set; }
        [Required]
        public string RolNombre { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        [Required]
        public string UsuarioNombre { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required]
        public string TipoTarjeta { get; set; }
        [Required]
        public string NumeroTarjeta { get; set; }
        [Required]
        public string Telefono { get; set; }
    }
}
