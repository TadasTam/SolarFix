using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarFix.Migrations
{
    /// <inheritdoc />
    public partial class added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DiffuseRadiation",
                table: "WeatherDatas",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DirectNormalIrradiance",
                table: "WeatherDatas",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DirectRadiation",
                table: "WeatherDatas",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ShortwaveRadiation",
                table: "WeatherDatas",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiffuseRadiation",
                table: "WeatherDatas");

            migrationBuilder.DropColumn(
                name: "DirectNormalIrradiance",
                table: "WeatherDatas");

            migrationBuilder.DropColumn(
                name: "DirectRadiation",
                table: "WeatherDatas");

            migrationBuilder.DropColumn(
                name: "ShortwaveRadiation",
                table: "WeatherDatas");
        }
    }
}
