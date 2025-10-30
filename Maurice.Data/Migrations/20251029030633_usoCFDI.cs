using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maurice.Data.Migrations
{
    /// <inheritdoc />
    public partial class usoCFDI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegimenFiscal_Users_UserId",
                table: "RegimenFiscal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegimenFiscal",
                table: "RegimenFiscal");

            migrationBuilder.RenameTable(
                name: "RegimenFiscal",
                newName: "RegimenesFiscales");

            migrationBuilder.RenameIndex(
                name: "IX_RegimenFiscal_UserId",
                table: "RegimenesFiscales",
                newName: "IX_RegimenesFiscales_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegimenesFiscales",
                table: "RegimenesFiscales",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsosCFDI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Clave = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsosCFDI", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RegimenesFiscales_Users_UserId",
                table: "RegimenesFiscales",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegimenesFiscales_Users_UserId",
                table: "RegimenesFiscales");

            migrationBuilder.DropTable(
                name: "UsosCFDI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegimenesFiscales",
                table: "RegimenesFiscales");

            migrationBuilder.RenameTable(
                name: "RegimenesFiscales",
                newName: "RegimenFiscal");

            migrationBuilder.RenameIndex(
                name: "IX_RegimenesFiscales_UserId",
                table: "RegimenFiscal",
                newName: "IX_RegimenFiscal_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegimenFiscal",
                table: "RegimenFiscal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegimenFiscal_Users_UserId",
                table: "RegimenFiscal",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
