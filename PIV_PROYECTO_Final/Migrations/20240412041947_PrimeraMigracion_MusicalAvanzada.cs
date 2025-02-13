using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIV_PROYECTO_Final.Migrations
{
    public partial class PrimeraMigracion_MusicalAvanzada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SCH_MUSICAL_AVANZADA");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GENERO_MUSICAL",
                schema: "SCH_MUSICAL_AVANZADA",
                columns: table => new
                {
                    CODIGO_GENERO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DESCRIPCION_GENERO = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GENERO_MUSICAL", x => x.CODIGO_GENERO);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, defaultValueSql: "(N'NUll')"),
                    Genero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValueSql: "(N'NUll')"),
                    TipoTarjeta = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true, defaultValueSql: "(N'NUll')"),
                    NumeroTarjeta = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValueSql: "(N'NUll')"),
                    UserName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, defaultValueSql: "(N'Null')"),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MODULO_CANCIONES",
                schema: "SCH_MUSICAL_AVANZADA",
                columns: table => new
                {
                    CODIGO_CANCION = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NOMBRE_CANCION = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PRECIO_CANCION = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CODIGO_GENERO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MODULO_CANCIONES", x => x.CODIGO_CANCION);
                    table.ForeignKey(
                        name: "FK_MODULO_CANCIONES_CODIGO_GENERO",
                        column: x => x.CODIGO_GENERO,
                        principalSchema: "SCH_MUSICAL_AVANZADA",
                        principalTable: "GENERO_MUSICAL",
                        principalColumn: "CODIGO_GENERO");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CANCIONES_USUARIO",
                schema: "SCH_MUSICAL_AVANZADA",
                columns: table => new
                {
                    ID_CANCIONES_USUARIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_USUARIO = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ID_CANCION = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ESTADO = table.Column<int>(type: "int", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CANCIONE__07E3F772C02EBF6E", x => x.ID_CANCIONES_USUARIO);
                    table.ForeignKey(
                        name: "FK_CANCIONES_USUARIO_CANCION",
                        column: x => x.ID_CANCION,
                        principalSchema: "SCH_MUSICAL_AVANZADA",
                        principalTable: "MODULO_CANCIONES",
                        principalColumn: "CODIGO_CANCION");
                });

            migrationBuilder.CreateTable(
                name: "VENTA_CANCIONES",
                schema: "SCH_MUSICAL_AVANZADA",
                columns: table => new
                {
                    NUMERO_FACTURA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SUBTOTAL = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TOTAL = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FECHA_COMPRA = table.Column<DateTime>(type: "datetime", nullable: false),
                    USUARIOS_IDENTIFICACION = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ID_CANCIONES_USUARIO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENTA_CANCIONES", x => x.NUMERO_FACTURA);
                    table.ForeignKey(
                        name: "FK_VENTA_CANCIONES_ID_CANCIONES_USUARIO",
                        column: x => x.ID_CANCIONES_USUARIO,
                        principalSchema: "SCH_MUSICAL_AVANZADA",
                        principalTable: "CANCIONES_USUARIO",
                        principalColumn: "ID_CANCIONES_USUARIO");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CANCIONES_USUARIO_ID_CANCION",
                schema: "SCH_MUSICAL_AVANZADA",
                table: "CANCIONES_USUARIO",
                column: "ID_CANCION");

            migrationBuilder.CreateIndex(
                name: "IX_MODULO_CANCIONES_CODIGO_GENERO",
                schema: "SCH_MUSICAL_AVANZADA",
                table: "MODULO_CANCIONES",
                column: "CODIGO_GENERO");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuarios",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuarios",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_VENTA_CANCIONES_ID_CANCIONES_USUARIO",
                schema: "SCH_MUSICAL_AVANZADA",
                table: "VENTA_CANCIONES",
                column: "ID_CANCIONES_USUARIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "VENTA_CANCIONES",
                schema: "SCH_MUSICAL_AVANZADA");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "CANCIONES_USUARIO",
                schema: "SCH_MUSICAL_AVANZADA");

            migrationBuilder.DropTable(
                name: "MODULO_CANCIONES",
                schema: "SCH_MUSICAL_AVANZADA");

            migrationBuilder.DropTable(
                name: "GENERO_MUSICAL",
                schema: "SCH_MUSICAL_AVANZADA");
        }
    }
}
