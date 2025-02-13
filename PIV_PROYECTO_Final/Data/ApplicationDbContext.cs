using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PIV_PROYECTO_Final.Data
{
    public class ApplicationDbContext : IdentityDbContext <Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Usuario>(EntityTypeBuilder =>
            {
                EntityTypeBuilder.ToTable("Usuarios");
                EntityTypeBuilder.Property(u => u.UserName)
                .HasMaxLength(250) // toma la cantidad
                .IsUnicode(true) //acepta palabras unicodes
                .HasDefaultValue("Null"); // valor por defecto es null

                EntityTypeBuilder.Property(u => u.NombreCompleto)
                .HasMaxLength(250)
                .HasDefaultValue("NUll");

                EntityTypeBuilder.Property(u => u.Genero)
               .HasMaxLength(50)
               .HasDefaultValue("NUll");

                EntityTypeBuilder.Property(u => u.TipoTarjeta)
               .HasMaxLength(25)
               .HasDefaultValue("NUll");

                EntityTypeBuilder.Property(u => u.NumeroTarjeta)
               .HasMaxLength(20)
               .HasDefaultValue("NUll");
            });
        }
    }
    public class Usuario : IdentityUser
    {

        //los que estan comentado los tiene el identity

        //public string? Identificacion { get; set; }
        public string? NombreCompleto { get; set; } //contiene el user name, pero no deja poner espacios en los nombres y tampoco utilizar los mismo nombres, se utiliza uno nuevo
        public string? Genero { get; set; } //masculino o femenino // desde la vista se agrega los dos tipos de generos
        //public string CorreoElectronico { get; set; }
        public string? TipoTarjeta { get; set; } // se agrega en la vistas los 3 tipos de tarjetas
        public string? NumeroTarjeta { get; set; } // se agrega el formato en el controlador
                                                  //public string Contrasena { get; set; } lo contiene el identity

    }
}