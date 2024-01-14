using CsvHelper.Configuration.Attributes;

namespace SolarFix.Dto;

[Delimiter(",")]
public class InverterCsvDto
{
    [Name("Time")]
    public string Time { get; set; }

    [Name("Mode")]
    public string Mode { get; set; }

    [Name("Total AC Power[W]")]
    public int TotalACPower { get; set; }

    [Name("Daily Energy[KW.Hr]")]
    public float DailyEnergy { get; set; }


    [Name("Temperature")]
    public float Temperature { get; set; }

    [Name("PV1 Voltage[V]")]
    public float PV1Voltage { get; set; }

    [Name("PV2 Voltage[V]")]
    public float PV2Voltage { get; set; }

    [Name("PV1 Current[A]")]
    public float PV1Current { get; set; }

    [Name("PV2 Current[A]")]
    public float PV2Current { get; set; }

    [Name("Total energy[KW.Hr]")]
    public float TotalEnergy { get; set; }

    [Name("PV1 Input Power[W]")]
    public float PV1InputPower { get; set; }

    [Name("PV2 Input Power[W]")]
    public float PV2InputPower { get; set; }

    [Name("Heatsink Temperature")]
    public float HeatsinkTemperature { get; set; }



    [Name("Current to grid (R Phase)[A]")]
    public float CurrentGridR { get; set; }

    [Name("Grid voltage(R Phase)[V]")]
    public float VoltageGridR { get; set; }

    [Name("Grid frequency(R Phase)[Hz]")]
    public float FrequencyGridR { get; set; }


    [Name("Current to grid (S Phase)[A]")]
    public float CurrentGridS { get; set; }

    [Name("Grid voltage(S Phase)[V]")]
    public float VoltageGridS { get; set; }

    [Name("Grid frequency(S Phase)[Hz]")]
    public float FrequencyGridS { get; set; }


    [Name("Current to grid (T Phase)[A]")]
    public float CurrentGridT { get; set; }

    [Name("Grid voltage(T Phase)[V]")]
    public float VoltageGridT { get; set; }

    [Name("Grid frequency(T Phase)[Hz]")]
    public float FrequencyGridT { get; set; }
}
