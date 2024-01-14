using System.ComponentModel.DataAnnotations;

namespace SolarFix.Models;

public class WeatherData
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public float Temperature { get; set; }

    public float SnowFall { get; set; }

    public int CloudCover { get; set; }

    public float WindSpeed { get; set; }
    public float ShortwaveRadiation { get; set; }
    public float DirectRadiation { get; set; }
    public float DiffuseRadiation { get; set; }
    public float DirectNormalIrradiance { get; set; }
}
