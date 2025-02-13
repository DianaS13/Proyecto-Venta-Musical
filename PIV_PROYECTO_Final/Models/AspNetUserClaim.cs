using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class AspNetUserClaim
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string? ClaimType { get; set; }
        [Required]
        public string? ClaimValue { get; set; }

        public virtual Usuario User { get; set; } = null!;
    }
}
