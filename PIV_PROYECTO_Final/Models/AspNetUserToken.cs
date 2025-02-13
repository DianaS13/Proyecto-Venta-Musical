using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class AspNetUserToken
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string LoginProvider { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string? Value { get; set; }

        public virtual Usuario User { get; set; } = null!;
    }
}
