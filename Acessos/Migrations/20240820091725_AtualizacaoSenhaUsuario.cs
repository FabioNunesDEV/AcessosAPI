using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acessos.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoSenhaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmacaoSenha",
                table: "Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmacaoSenha",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
