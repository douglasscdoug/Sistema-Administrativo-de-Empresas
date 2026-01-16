using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameStatusToAtivoAndCep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CEP",
                table: "Enderecos",
                newName: "Cep");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Empresas",
                newName: "Ativo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cep",
                table: "Enderecos",
                newName: "CEP");

            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Empresas",
                newName: "Status");
        }
    }
}
