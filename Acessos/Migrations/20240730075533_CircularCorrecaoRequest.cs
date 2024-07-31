using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acessos.Migrations
{
    /// <inheritdoc />
    public partial class CircularCorrecaoRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Conteudo",
                table: "Circular",
                type: "nvarchar(3000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Assunto",
                table: "Circular",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Conteudo",
                table: "Circular",
                type: "nvarchar(3000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)");

            migrationBuilder.AlterColumn<string>(
                name: "Assunto",
                table: "Circular",
                type: "nvarchar(300)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");
        }
    }
}
