using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class VentaCancione
    {
        [Required(ErrorMessage = "Es requerido el Numero de la factura")]
        public string NumeroFactura { get; set; } = null!;
        [Required]
        public decimal Subtotal { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        public DateTime FechaCompra { get; set; }
        public string UsuariosIdentificacion { get; set; } = null!;
        public int IdCancionesUsuario { get; set; }

        public virtual CANCIONES_USUARIO IdCancionesUsuarioNavigation { get; set; } = null!;
    }
}
