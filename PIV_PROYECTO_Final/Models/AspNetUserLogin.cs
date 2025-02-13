using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class AspNetUserLogin
    {
        [Required]
        public string LoginProvider { get; set; } = null!;
        [Required]
        public string ProviderKey { get; set; } = null!;
        [Required]
        public string? ProviderDisplayName { get; set; }
        [Required]
        public string UserId { get; set; } = null!;

        public virtual Usuario User { get; set; } = null!;
    }
}
