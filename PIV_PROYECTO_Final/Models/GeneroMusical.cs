using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class GeneroMusical
    {
        public GeneroMusical()
        {
            ModuloCanciones = new HashSet<ModuloCancione>();
        }
        [Required(ErrorMessage = "Es requerido el Codigo del Genero")]
        public string CodigoGenero { get; set; } = null!;
        [Required(ErrorMessage = "Es requerido la Descripcion del Genero")]
        public string DescripcionGenero { get; set; } = null!;

        public virtual ICollection<ModuloCancione> ModuloCanciones { get; set; }
    }
}
