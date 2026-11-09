using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEventos.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoErros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "codigo_presensa",
                table: "tbl_Presenca",
                newName: "codigo_presenca");

            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "tbl_Usuario",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "codigo_presenca",
                table: "tbl_Presenca",
                newName: "codigo_presensa");

            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "tbl_Usuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
