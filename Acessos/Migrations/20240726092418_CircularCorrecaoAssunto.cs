using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acessos.Migrations
{
    /// <inheritdoc />
    public partial class CircularCorrecaoAssunto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Assunto",
                table: "Circular",
                type: "nvarchar(300)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Assunto",
                table: "Circular",
                type: "nvarchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldNullable: true);
        }
    }
}
