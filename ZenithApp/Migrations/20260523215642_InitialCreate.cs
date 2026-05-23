using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenithApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Alimentos",
                columns: table => new
                {
                    IdAlimento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Calorias = table.Column<int>(type: "int", nullable: true),
                    Proteinas = table.Column<int>(type: "int", nullable: true),
                    Carboidratos = table.Column<int>(type: "int", nullable: true),
                    IdSistema = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alimentos", x => x.IdAlimento);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    IdLogin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Usuario = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelAcesso = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.IdLogin);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Atletas",
                columns: table => new
                {
                    IdAtleta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idade = table.Column<int>(type: "int", nullable: true),
                    Altura = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Largura = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Peso = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CompCorporal = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Biotipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdLogin = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atletas", x => x.IdAtleta);
                    table.ForeignKey(
                        name: "FK_Atletas_Logins_IdLogin",
                        column: x => x.IdLogin,
                        principalTable: "Logins",
                        principalColumn: "IdLogin");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Treinadores",
                columns: table => new
                {
                    IdTreinador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idade = table.Column<int>(type: "int", nullable: true),
                    Cargo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdLogin = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treinadores", x => x.IdTreinador);
                    table.ForeignKey(
                        name: "FK_Treinadores_Logins_IdLogin",
                        column: x => x.IdLogin,
                        principalTable: "Logins",
                        principalColumn: "IdLogin");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegistrosPerformance",
                columns: table => new
                {
                    IdRegistro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Forca = table.Column<int>(type: "int", nullable: true),
                    Velocidade = table.Column<int>(type: "int", nullable: true),
                    Cardio = table.Column<int>(type: "int", nullable: true),
                    IdAtleta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosPerformance", x => x.IdRegistro);
                    table.ForeignKey(
                        name: "FK_RegistrosPerformance_Atletas_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Atletas",
                        principalColumn: "IdAtleta",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreinadorAlimentos",
                columns: table => new
                {
                    IdTreinador = table.Column<int>(type: "int", nullable: false),
                    IdAlimento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreinadorAlimentos", x => new { x.IdTreinador, x.IdAlimento });
                    table.ForeignKey(
                        name: "FK_TreinadorAlimentos_Alimentos_IdAlimento",
                        column: x => x.IdAlimento,
                        principalTable: "Alimentos",
                        principalColumn: "IdAlimento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreinadorAlimentos_Treinadores_IdTreinador",
                        column: x => x.IdTreinador,
                        principalTable: "Treinadores",
                        principalColumn: "IdTreinador",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreinadorAtletas",
                columns: table => new
                {
                    IdTreinador = table.Column<int>(type: "int", nullable: false),
                    IdAtleta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreinadorAtletas", x => new { x.IdTreinador, x.IdAtleta });
                    table.ForeignKey(
                        name: "FK_TreinadorAtletas_Atletas_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Atletas",
                        principalColumn: "IdAtleta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreinadorAtletas_Treinadores_IdTreinador",
                        column: x => x.IdTreinador,
                        principalTable: "Treinadores",
                        principalColumn: "IdTreinador",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Treinos",
                columns: table => new
                {
                    IdTreino = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duracao = table.Column<int>(type: "int", nullable: true),
                    DataTreino = table.Column<DateOnly>(type: "date", nullable: true),
                    Carga = table.Column<int>(type: "int", nullable: true),
                    IdSistema = table.Column<int>(type: "int", nullable: true),
                    IdTreinador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treinos", x => x.IdTreino);
                    table.ForeignKey(
                        name: "FK_Treinos_Treinadores_IdTreinador",
                        column: x => x.IdTreinador,
                        principalTable: "Treinadores",
                        principalColumn: "IdTreinador");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Atletas_IdLogin",
                table: "Atletas",
                column: "IdLogin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logins_Usuario",
                table: "Logins",
                column: "Usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPerformance_IdAtleta",
                table: "RegistrosPerformance",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_TreinadorAlimentos_IdAlimento",
                table: "TreinadorAlimentos",
                column: "IdAlimento");

            migrationBuilder.CreateIndex(
                name: "IX_TreinadorAtletas_IdAtleta",
                table: "TreinadorAtletas",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_Treinadores_IdLogin",
                table: "Treinadores",
                column: "IdLogin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_IdTreinador",
                table: "Treinos",
                column: "IdTreinador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosPerformance");

            migrationBuilder.DropTable(
                name: "TreinadorAlimentos");

            migrationBuilder.DropTable(
                name: "TreinadorAtletas");

            migrationBuilder.DropTable(
                name: "Treinos");

            migrationBuilder.DropTable(
                name: "Alimentos");

            migrationBuilder.DropTable(
                name: "Atletas");

            migrationBuilder.DropTable(
                name: "Treinadores");

            migrationBuilder.DropTable(
                name: "Logins");
        }
    }
}
