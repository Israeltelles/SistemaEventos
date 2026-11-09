using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEventos.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Ra = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Evento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Data = table.Column<DateTime>(type: "date", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hora_inicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    hora_fim = table.Column<TimeSpan>(type: "time", nullable: false),
                    CoordenadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Evento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Evento_tbl_Usuario_CoordenadorId",
                        column: x => x.CoordenadorId,
                        principalTable: "tbl_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Certificado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    data_emissao = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Certificado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Certificado_tbl_Evento_EventoId",
                        column: x => x.EventoId,
                        principalTable: "tbl_Evento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Certificado_tbl_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "tbl_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Inscricao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    data_inscricao = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Inscricao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Inscricao_tbl_Evento_EventoId",
                        column: x => x.EventoId,
                        principalTable: "tbl_Evento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Inscricao_tbl_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "tbl_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Presenca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo_presensa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    Confirmacao = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Presenca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Presenca_tbl_Evento_EventoId",
                        column: x => x.EventoId,
                        principalTable: "tbl_Evento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Presenca_tbl_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "tbl_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Certificado_EventoId",
                table: "tbl_Certificado",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Certificado_UsuarioId_EventoId",
                table: "tbl_Certificado",
                columns: new[] { "UsuarioId", "EventoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Evento_CoordenadorId",
                table: "tbl_Evento",
                column: "CoordenadorId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Inscricao_EventoId",
                table: "tbl_Inscricao",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Inscricao_UsuarioId_EventoId",
                table: "tbl_Inscricao",
                columns: new[] { "UsuarioId", "EventoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Presenca_EventoId",
                table: "tbl_Presenca",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Presenca_UsuarioId_EventoId",
                table: "tbl_Presenca",
                columns: new[] { "UsuarioId", "EventoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Usuario_Tipo",
                table: "tbl_Usuario",
                column: "Tipo",
                unique: true,
                filter: "Tipo = 'Diretor'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Certificado");

            migrationBuilder.DropTable(
                name: "tbl_Inscricao");

            migrationBuilder.DropTable(
                name: "tbl_Presenca");

            migrationBuilder.DropTable(
                name: "tbl_Evento");

            migrationBuilder.DropTable(
                name: "tbl_Usuario");
        }
    }
}
