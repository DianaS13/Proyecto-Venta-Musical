using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class ModuloCancione
    {
        public ModuloCancione()
        {
            CancionesUsuarios = new HashSet<CANCIONES_USUARIO>();
        }
        [Required(ErrorMessage = "Es requerido el Codigo Cancion")]
        public string CodigoCancion { get; set; } = null!;

        [Required(ErrorMessage = "Es requerido Nombre de la Cancion")]
        public string NombreCancion { get; set; } = null!;

        [Required(ErrorMessage = "Es requerido el Precio De la cancion")]
        public decimal PrecioCancion { get; set; }
        public string CodigoGenero { get; set; } = null!;

        public virtual GeneroMusical CodigoGeneroNavigation { get; set; } = null!;
        public virtual ICollection<CANCIONES_USUARIO> CancionesUsuarios { get; set; }
    }
}
