using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenithApp.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMetricasEConcluido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Concluido",
                table: "Treinos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Frequencia",
                table: "Atletas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MassaMuscular",
                table: "Atletas",
                type: "decimal(5,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concluido",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "Frequencia",
                table: "Atletas");

            migrationBuilder.DropColumn(
                name: "MassaMuscular",
                table: "Atletas");
        }
    }
}
