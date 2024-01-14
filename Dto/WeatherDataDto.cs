namespace SolarFix.Dto;

public class Hourly
{
    public List<string> time { get; set; }
    public List<float> temperature_2m { get; set; }
    public List<float> snowfall { get; set; }
    public List<int> cloudcover { get; set; }
    public List<float> shortwave_radiation { get; set; }
    public List<float> direct_radiation { get; set; }
    public List<float> diffuse_radiation { get; set; }
    public List<float> direct_normal_irradiance { get; set; }
    public List<float> windspeed_10m { get; set; }
}

//public class HourlyUnits
//{
//    public string time { get; set; }
//    public string temperature_2m { get; set; }
//    public string snowfall { get; set; }
//    public string cloudcover { get; set; }
//    public string windspeed_10m { get; set; }
//}

public class WeatherDataDto
{
    //public double latitude { get; set; }
    //public double longitude { get; set; }
    //public double generationtime_ms { get; set; }
    //public int utc_offset_seconds { get; set; }
    //public string timezone { get; set; }
    //public string timezone_abbreviation { get; set; }
    //public double elevation { get; set; }
    //public HourlyUnits hourly_units { get; set; }
    public Hourly hourly { get; set; }
}
