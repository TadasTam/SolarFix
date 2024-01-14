using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace SolarFix.Models;

public class SolarProduction : IEntityTypeConfiguration<SolarProduction>
{
    [Key]
    public int Id { get; set; }
    public string InverterId { get; set; } = null!;

    public DateTime Date { get; set; }

    public int CurrentPower { get; set; }

    public float DailyProducedEnergy { get; set; }

    public float Temperature { get; set; }

    public float PV1Voltage { get; set; }

    public float PV2Voltage { get; set; }

    public float PV1Current { get; set; }

    public float PV2Current { get; set; }

    public float TotalEnergy { get; set; }

    public float PV1InputPower { get; set; }

    public float PV2InputPower { get; set; }

    public float HeatsinkTemperature { get; set; }

    public float CurrentGridR { get; set; }

    public float VoltageGridR { get; set; }

    public float FrequencyGridR { get; set; }

    public float CurrentGridS { get; set; }

    public float VoltageGridS { get; set; }

    public float FrequencyGridS { get; set; }

    public float CurrentGridT { get; set; }

    public float VoltageGridT { get; set; }

    public float FrequencyGridT { get; set; }

    public void Configure(EntityTypeBuilder<SolarProduction> builder)
    {
        builder.Property(x => x.InverterId).HasMaxLength(256);
    }
}
