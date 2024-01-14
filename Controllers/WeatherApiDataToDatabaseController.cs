using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SolarFix.Data;
using SolarFix.Dto;
using SolarFix.Models;
using System.Globalization;

namespace SolarFix.Controllers;

public class WeatherApiDataToDatabaseController : Controller
{
    private readonly HttpClient httpClient;
    private readonly ApplicationDbContext _dbContext;

    public WeatherApiDataToDatabaseController(HttpClient httpClient, ApplicationDbContext dbContext)
    {
        this.httpClient = httpClient;
        _dbContext = dbContext;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var deletionTask = _dbContext.WeatherDatas.ExecuteDeleteAsync();

        await deletionTask;

        
        string API = "https://archive-api.open-meteo.com/v1/archive?latitude=54.23&longitude=23.51&start_date=2015-01-01&end_date=2017-12-31&hourly=temperature_2m,snowfall,cloudcover,shortwave_radiation,direct_radiation,diffuse_radiation,direct_normal_irradiance,windspeed_10m";
        var response = await httpClient.GetAsync(API);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        WeatherDataDto data = JsonConvert.DeserializeObject<WeatherDataDto>(responseString);
        for(int i = 0; i < data.hourly.time.Count; i++)
        {

            var weatherData = new WeatherData();

            weatherData.Date = DateTime.ParseExact(data.hourly.time[i], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            weatherData.Temperature = data.hourly.temperature_2m[i];
            weatherData.SnowFall = data.hourly.snowfall[i];
            weatherData.CloudCover = data.hourly.cloudcover[i];

            weatherData.ShortwaveRadiation = data.hourly.shortwave_radiation[i];
            weatherData.DirectRadiation = data.hourly.direct_radiation[i];
            weatherData.DiffuseRadiation = data.hourly.diffuse_radiation[i];
            weatherData.DirectNormalIrradiance = data.hourly.direct_normal_irradiance[i];
            weatherData.WindSpeed = data.hourly.windspeed_10m[i];

            _dbContext.WeatherDatas.Add(weatherData);

        }
        
        var numberOfInsertedRecords = await _dbContext.SaveChangesAsync();

        return Ok($"added: {numberOfInsertedRecords}");
    }
}
