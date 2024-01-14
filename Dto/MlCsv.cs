namespace SolarFix.Dto;

public class MlCsv
{
    public DateTime Date { get; set; }

    public float? ProducedEnergy { get; set; }

    public int Cloudcover { get; set; }

    public double SnowFall { get; set; }
}
