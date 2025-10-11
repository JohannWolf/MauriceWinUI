using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maurice.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaveProdServ = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    Base = table.Column<decimal>(type: "TEXT", nullable: false),
                    Tasa = table.Column<string>(type: "TEXT", nullable: false),
                    ImporteImpuesto = table.Column<decimal>(type: "TEXT", nullable: false),
                    TipoDeDocumento = table.Column<string>(type: "TEXT", nullable: false),
                    Folio = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UUID = table.Column<string>(type: "TEXT", nullable: false),
                    RfcEmisor = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    NombreEmisor = table.Column<string>(type: "TEXT", nullable: false),
                    RfcReceptor = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    SubTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Descuento = table.Column<decimal>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nominas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TotalGravado = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalExento = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPercepciones = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalImpuestosRetenidos = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalOtrasDeducciones = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalDeducciones = table.Column<decimal>(type: "TEXT", nullable: false),
                    TipoDeDocumento = table.Column<string>(type: "TEXT", nullable: false),
                    Folio = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UUID = table.Column<string>(type: "TEXT", nullable: false),
                    RfcEmisor = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    NombreEmisor = table.Column<string>(type: "TEXT", nullable: false),
                    RfcReceptor = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    SubTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Descuento = table.Column<decimal>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Rfc = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UUID",
                table: "Facturas",
                column: "UUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nominas_UUID",
                table: "Nominas",
                column: "UUID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Nominas");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
