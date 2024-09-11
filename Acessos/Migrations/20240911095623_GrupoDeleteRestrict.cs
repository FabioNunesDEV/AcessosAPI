using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acessos.Migrations
{
    /// <inheritdoc />
    public partial class GrupoDeleteRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioGrupo_Grupo_GrupoId",
                table: "UsuarioGrupo");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioGrupo_Grupo_GrupoId",
                table: "UsuarioGrupo",
                column: "GrupoId",
                principalTable: "Grupo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioGrupo_Grupo_GrupoId",
                table: "UsuarioGrupo");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioGrupo_Grupo_GrupoId",
                table: "UsuarioGrupo",
                column: "GrupoId",
                principalTable: "Grupo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
