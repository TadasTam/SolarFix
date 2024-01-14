using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarFix.Migrations
{
    /// <inheritdoc />
    public partial class AddedTemperatureColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Temperature",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "SolarProductions");
        }
    }
}
