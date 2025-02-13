using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Roles = new HashSet<AspNetRole>();
        }

        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public string? NombreCompleto { get; set; }
        [Required]
        public string? Genero { get; set; }
        [Required]
        public string? TipoTarjeta { get; set; }
        [Required]
        public string? NumeroTarjeta { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? NormalizedUserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? NormalizedEmail { get; set; }
        
        public bool EmailConfirmed { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
