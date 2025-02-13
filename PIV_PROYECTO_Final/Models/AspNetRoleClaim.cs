using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class AspNetRoleClaim
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string RoleId { get; set; } = null!;
        [Required]
        public string? ClaimType { get; set; }
        [Required]
        public string? ClaimValue { get; set; }
        [Required]
        public virtual AspNetRole Role { get; set; } = null!;
    }
}
