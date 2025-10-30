using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maurice.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeNotNeededField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "RegimenesFiscales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "RegimenesFiscales",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
