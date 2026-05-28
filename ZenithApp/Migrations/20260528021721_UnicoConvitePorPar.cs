using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenithApp.Migrations
{
    public partial class UnicoConvitePorPar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ConvitesTreinador_IdTreinador_IdAtleta",
                table: "ConvitesTreinador",
                columns: new[] { "IdTreinador", "IdAtleta" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConvitesTreinador_IdTreinador_IdAtleta",
                table: "ConvitesTreinador");
        }
    }
}