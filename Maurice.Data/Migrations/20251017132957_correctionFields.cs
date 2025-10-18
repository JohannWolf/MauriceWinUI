using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maurice.Data.Migrations
{
    /// <inheritdoc />
    public partial class correctionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "Nominas");

            migrationBuilder.RenameColumn(
                name: "Descuento",
                table: "Facturas",
                newName: "RetencionImpuesto");

            migrationBuilder.AddColumn<int>(
                name: "TipoDeTransaccion",
                table: "Nominas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoDeTransaccion",
                table: "Facturas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDeTransaccion",
                table: "Nominas");

            migrationBuilder.DropColumn(
                name: "TipoDeTransaccion",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "RetencionImpuesto",
                table: "Facturas",
                newName: "Descuento");

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "Nominas",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
