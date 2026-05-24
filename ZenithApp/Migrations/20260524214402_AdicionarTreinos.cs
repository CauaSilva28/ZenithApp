using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenithApp.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTreinos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
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
                name: "ConvitesTreinador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTreinador = table.Column<int>(type: "int", nullable: false),
                    IdAtleta = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvitesTreinador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvitesTreinador_Atletas_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Atletas",
                        principalColumn: "IdAtleta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConvitesTreinador_Treinadores_IdTreinador",
                        column: x => x.IdTreinador,
                        principalTable: "Treinadores",
                        principalColumn: "IdTreinador",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MetasSemana",
                columns: table => new
                {
                    IdMeta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Concluida = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdAtleta = table.Column<int>(type: "int", nullable: false),
                    IdTreinador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetasSemana", x => x.IdMeta);
                    table.ForeignKey(
                        name: "FK_MetasSemana_Atletas_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Atletas",
                        principalColumn: "IdAtleta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MetasSemana_Treinadores_IdTreinador",
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
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observacoes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdTreinador = table.Column<int>(type: "int", nullable: false),
                    IdAtleta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treinos", x => x.IdTreino);
                    table.ForeignKey(
                        name: "FK_Treinos_Atletas_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Atletas",
                        principalColumn: "IdAtleta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treinos_Treinadores_IdTreinador",
                        column: x => x.IdTreinador,
                        principalTable: "Treinadores",
                        principalColumn: "IdTreinador",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Exercicios",
                columns: table => new
                {
                    IdExercicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Series = table.Column<int>(type: "int", nullable: false),
                    Repeticoes = table.Column<int>(type: "int", nullable: false),
                    IdTreino = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercicios", x => x.IdExercicio);
                    table.ForeignKey(
                        name: "FK_Exercicios_Treinos_IdTreino",
                        column: x => x.IdTreino,
                        principalTable: "Treinos",
                        principalColumn: "IdTreino",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Atletas_IdLogin",
                table: "Atletas",
                column: "IdLogin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConvitesTreinador_IdAtleta",
                table: "ConvitesTreinador",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_ConvitesTreinador_IdTreinador",
                table: "ConvitesTreinador",
                column: "IdTreinador");

            migrationBuilder.CreateIndex(
                name: "IX_Exercicios_IdTreino",
                table: "Exercicios",
                column: "IdTreino");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_Usuario",
                table: "Logins",
                column: "Usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetasSemana_IdAtleta",
                table: "MetasSemana",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_MetasSemana_IdTreinador",
                table: "MetasSemana",
                column: "IdTreinador");

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
                name: "IX_Treinos_IdAtleta",
                table: "Treinos",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_IdTreinador",
                table: "Treinos",
                column: "IdTreinador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvitesTreinador");

            migrationBuilder.DropTable(
                name: "Exercicios");

            migrationBuilder.DropTable(
                name: "MetasSemana");

            migrationBuilder.DropTable(
                name: "TreinadorAtletas");

            migrationBuilder.DropTable(
                name: "Treinos");

            migrationBuilder.DropTable(
                name: "Atletas");

            migrationBuilder.DropTable(
                name: "Treinadores");

            migrationBuilder.DropTable(
                name: "Logins");
        }
    }
}
