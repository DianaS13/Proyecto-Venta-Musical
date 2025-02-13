using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            Users = new HashSet<Usuario>();
        }
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? NormalizedName { get; set; }
        [Required]
        public string? ConcurrencyStamp { get; set; }

        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }

        public virtual ICollection<Usuario> Users { get; set; }
    }
}
