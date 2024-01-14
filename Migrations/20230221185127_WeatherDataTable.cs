using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarFix.Migrations;

/// <inheritdoc />
public partial class WeatherDataTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "WeatherDatas",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Temperature = table.Column<float>(type: "real", nullable: false),
                SnowFall = table.Column<float>(type: "real", nullable: false),
                CloudCover = table.Column<int>(type: "int", nullable: false),
                WindSpeed = table.Column<float>(type: "real", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WeatherDatas", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "WeatherDatas");
    }
}
