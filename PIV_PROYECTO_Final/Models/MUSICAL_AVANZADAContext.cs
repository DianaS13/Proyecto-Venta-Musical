using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PIV_PROYECTO_Final.Models
{
    public partial class MUSICAL_AVANZADAContext : DbContext
    {
        public MUSICAL_AVANZADAContext()
        {
        }

        public MUSICAL_AVANZADAContext(DbContextOptions<MUSICAL_AVANZADAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<CANCIONES_USUARIO> CANCIONES_USUARIO { get; set; } = null!;
        public virtual DbSet<GeneroMusical> GeneroMusical { get; set; } = null!;
        public virtual DbSet<ModuloCancione> ModuloCancione { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<VentaCancione> VentaCancione { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-SLGDH4S\\SQLEXPRESS;Initial Catalog=MUSICAL_AVANZADA;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<CANCIONES_USUARIO>(entity =>
            {
                entity.HasKey(e => e.ID_CANCIONES_USUARIO)
                    .HasName("PK__CANCIONE__07E3F772C02EBF6E");

                entity.ToTable("CANCIONES_USUARIO", "SCH_MUSICAL_AVANZADA");

                entity.Property(e => e.ID_CANCIONES_USUARIO).HasColumnName("ID_CANCIONES_USUARIO");

                entity.Property(e => e.ID_CANCION)
                    .HasMaxLength(20)
                    .HasColumnName("ID_CANCION");

                entity.Property(e => e.ID_USUARIO)
                    .HasMaxLength(450)
                    .HasColumnName("ID_USUARIO");

                entity.Property(e => e.ESTADO)
                    .HasMaxLength(450)
                    .HasColumnName("ESTADO");

                entity.HasOne(d => d.IdCancionNavigation)
                    .WithMany(p => p.CancionesUsuarios)
                    .HasForeignKey(d => d.ID_CANCION)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CANCIONES_USUARIO_CANCION");
                
            });

            modelBuilder.Entity<GeneroMusical>(entity =>
            {
                entity.HasKey(e => e.CodigoGenero);

                entity.ToTable("GENERO_MUSICAL", "SCH_MUSICAL_AVANZADA");

                entity.Property(e => e.CodigoGenero)
                    .HasMaxLength(20)
                    .HasColumnName("CODIGO_GENERO");

                entity.Property(e => e.DescripcionGenero)
                    .HasMaxLength(300)
                    .HasColumnName("DESCRIPCION_GENERO");
            });

            modelBuilder.Entity<ModuloCancione>(entity =>
            {
                entity.HasKey(e => e.CodigoCancion);

                entity.ToTable("MODULO_CANCIONES", "SCH_MUSICAL_AVANZADA");

                entity.Property(e => e.CodigoCancion)
                    .HasMaxLength(20)
                    .HasColumnName("CODIGO_CANCION");

                entity.Property(e => e.CodigoGenero)
                    .HasMaxLength(20)
                    .HasColumnName("CODIGO_GENERO");

                entity.Property(e => e.NombreCancion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_CANCION");

                entity.Property(e => e.PrecioCancion)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("PRECIO_CANCION");

                entity.HasOne(d => d.CodigoGeneroNavigation)
                    .WithMany(p => p.ModuloCanciones)
                    .HasForeignKey(d => d.CodigoGenero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MODULO_CANCIONES_CODIGO_GENERO");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Genero)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'NUll')");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(N'NUll')");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.NumeroTarjeta)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'NUll')");

                entity.Property(e => e.TipoTarjeta)
                    .HasMaxLength(25)
                    .HasDefaultValueSql("(N'NUll')");

                entity.Property(e => e.UserName)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(N'Null')");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<Usuario>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<VentaCancione>(entity =>
            {
                entity.HasKey(e => e.NumeroFactura);

                entity.ToTable("VENTA_CANCIONES", "SCH_MUSICAL_AVANZADA");

                entity.Property(e => e.NumeroFactura)
                    .HasMaxLength(20)
                    .HasColumnName("NUMERO_FACTURA");

                entity.Property(e => e.FechaCompra)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_COMPRA");

                entity.Property(e => e.IdCancionesUsuario).HasColumnName("ID_CANCIONES_USUARIO");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("SUBTOTAL");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("TOTAL");

                entity.Property(e => e.UsuariosIdentificacion)
                    .HasMaxLength(9)
                    .HasColumnName("USUARIOS_IDENTIFICACION");

                entity.HasOne(d => d.IdCancionesUsuarioNavigation)
                    .WithMany(p => p.ModuloVentaCancione)
                    .HasForeignKey(d => d.IdCancionesUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VENTA_CANCIONES_ID_CANCIONES_USUARIO");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
