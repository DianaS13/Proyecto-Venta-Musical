using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class CANCIONES_USUARIO
    {
        public CANCIONES_USUARIO()
        {
            ModuloVentaCancione = new HashSet<VentaCancione>();
        }

        public CANCIONES_USUARIO(int ID_CANCIONES_USUARIO, string iD_USUARIO, string iD_CANCION, int estado)
        {
            this.ID_CANCIONES_USUARIO = ID_CANCIONES_USUARIO;
            this.ID_USUARIO = iD_USUARIO;
            this.ID_CANCION = iD_CANCION;
            this.ESTADO = estado;
        }
        [Required]
        public int ID_CANCIONES_USUARIO { get; set; }
        [Required]
        public string ID_USUARIO { get; set; }
        [Required]
        public string ID_CANCION { get; set; }
        [Required]
        public int ESTADO { get; set; }

        public virtual ModuloCancione IdCancionNavigation { get; set; } = null!;
        public virtual ICollection<VentaCancione> ModuloVentaCancione { get; set; }


    }
}
