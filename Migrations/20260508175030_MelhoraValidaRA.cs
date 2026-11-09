using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEventos.Migrations
{
    /// <inheritdoc />
    public partial class MelhoraValidaRA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_Usuario_Tipo",
                table: "tbl_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Usuario_Email",
                table: "tbl_Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Usuario_Ra",
                table: "tbl_Usuario",
                column: "Ra",
                unique: true,
                filter: "[Ra] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Usuario_Tipo",
                table: "tbl_Usuario",
                column: "Tipo",
                unique: true,
                filter: "[Tipo] = 'Diretor'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_Usuario_Email",
                table: "tbl_Usuario");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Usuario_Ra",
                table: "tbl_Usuario");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Usuario_Tipo",
                table: "tbl_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Usuario_Tipo",
                table: "tbl_Usuario",
                column: "Tipo",
                unique: true,
                filter: "Tipo = 'Diretor'");
        }
    }
}
