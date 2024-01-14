using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarFix.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CurrentGridR",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CurrentGridS",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CurrentGridT",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FrequencyGridR",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FrequencyGridS",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FrequencyGridT",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HeatsinkTemperature",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PV1Current",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PV1InputPower",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PV1Voltage",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PV2Current",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PV2InputPower",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PV2Voltage",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalEnergy",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "VoltageGridR",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "VoltageGridS",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "VoltageGridT",
                table: "SolarProductions",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentGridR",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "CurrentGridS",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "CurrentGridT",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "FrequencyGridR",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "FrequencyGridS",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "FrequencyGridT",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "HeatsinkTemperature",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "PV1Current",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "PV1InputPower",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "PV1Voltage",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "PV2Current",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "PV2InputPower",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "PV2Voltage",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "TotalEnergy",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "VoltageGridR",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "VoltageGridS",
                table: "SolarProductions");

            migrationBuilder.DropColumn(
                name: "VoltageGridT",
                table: "SolarProductions");
        }
    }
}
