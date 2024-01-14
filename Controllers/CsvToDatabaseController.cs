using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarFix.Data;
using SolarFix.Dto;
using SolarFix.Models;
using System.Globalization;

namespace SolarFix.Controllers;
public class CsvToDatabaseController : Controller
{
    private readonly ApplicationDbContext dbContext;
    private readonly ILogger<CsvToDatabaseController> logger;

    public CsvToDatabaseController(ApplicationDbContext dbContext, ILogger<CsvToDatabaseController> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var filesCount = 0;

        var directories = Directory.GetDirectories("csv/");

        foreach (var directory in directories)
        {
            var inverters = Directory.GetFiles($"{directory}/", "*.csv");

            foreach (var inverter in inverters)
            {
                filesCount++;
                logger.LogInformation(inverter);

                using (var reader = new StreamReader($"{inverter}"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<InverterCsvDto>();

                    float toAdd = 0, prev = 0, prevOrg = 0;
                    bool first = true;
                    foreach (var record in records)
                    {
                        if (first && record.DailyEnergy != 0)
                        {
                            first = false;
                            continue;
                        }
                        first = false;

                        if (record.Mode != "Normal") continue;

                        if (record.DailyEnergy == 0 && !first && record.DailyEnergy < prevOrg) toAdd += prev;

                        var solarProduction = new SolarProduction();

                        solarProduction.CurrentPower = record.TotalACPower;
                        solarProduction.DailyProducedEnergy = record.DailyEnergy + toAdd;
                        solarProduction.Date = DateTime.Parse(record.Time);
                        solarProduction.InverterId = Path.GetFileName(inverter);

                        solarProduction.Temperature = record.Temperature;
                        solarProduction.PV1Voltage = record.PV1Voltage;
                        solarProduction.PV2Voltage = record.PV2Voltage;
                        solarProduction.PV1Current = record.PV1Current;
                        solarProduction.PV2Current = record.PV2Current;
                        solarProduction.TotalEnergy = record.TotalEnergy;
                        solarProduction.PV1InputPower = record.PV1InputPower;
                        solarProduction.PV2InputPower = record.PV2InputPower;
                        solarProduction.HeatsinkTemperature = record.HeatsinkTemperature;
                        solarProduction.CurrentGridR = record.CurrentGridR;
                        solarProduction.VoltageGridR = record.VoltageGridR;
                        solarProduction.FrequencyGridR = record.FrequencyGridR;
                        solarProduction.CurrentGridS = record.CurrentGridS;
                        solarProduction.VoltageGridS = record.VoltageGridS;
                        solarProduction.FrequencyGridS = record.FrequencyGridS;
                        solarProduction.CurrentGridT = record.CurrentGridT;
                        solarProduction.VoltageGridT = record.VoltageGridT;
                        solarProduction.FrequencyGridT = record.FrequencyGridT;

                        dbContext.SolarProductions.Add(solarProduction);

                        prev = solarProduction.DailyProducedEnergy;
                        prevOrg = record.DailyEnergy;
                    }
                }
            }
        }

        await dbContext.SolarProductions.ExecuteDeleteAsync();

        var numberOfInsertedRecords = await dbContext.SaveChangesAsync();

        return Ok($"Inserted data count: {numberOfInsertedRecords}\nFound files: {filesCount}");
    }

    [HttpGet]
    public async Task<IActionResult> ToMlCsv()
    {
        logger.LogInformation("Collecting info for csv file...");

        DateTime startDate = DateTime.Parse("2015-01-01");
        DateTime endDate = DateTime.Parse("2016-12-31");

        var inverter = "T15135A997.csv";

        var weatherData = await dbContext.WeatherDatas
            .Where(b => b.Date >= startDate && b.Date <= endDate)
            .Select(x => new { x.CloudCover, x.SnowFall, x.Date })
            .ToListAsync();

        var solarData = await dbContext.SolarProductions
            .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter)
            .GroupBy(sp => new
            {
                sp.Date.Year,
                sp.Date.Month,
                sp.Date.Day,
                sp.Date.Hour,
                Minute = sp.Date.Minute / 60
            })
            .Select(g => new
            {
                Date = g.Min(sp => sp.Date),
                ProducedEnergy = g.Max(sp => sp.DailyProducedEnergy) - g.Min(sp => sp.DailyProducedEnergy)
            })
            .OrderBy(r => r.Date).ToListAsync();

        var producedEnergy = new List<float?>(weatherData.Count);

        for (int i = 1; i < weatherData.Count; i++)
        {
            float? producedEnergyPerHour = solarData
                .Where(x =>
                    x.Date.Year == weatherData[i].Date.Year
                    && x.Date.DayOfYear == weatherData[i].Date.DayOfYear
                    && x.Date.Hour == weatherData[i].Date.Hour)
                .Select(x => x.ProducedEnergy)
                .SingleOrDefault();

            producedEnergy.Add(producedEnergyPerHour ?? 0);
        }

        weatherData.RemoveAt(weatherData.Count - 1);

        var records = new List<MlCsv>(producedEnergy.Count);

        for (int i = 0; i < producedEnergy.Count; i++)
        {
            var record = new MlCsv();

            record.ProducedEnergy = producedEnergy[i];
            record.Cloudcover = weatherData[i].CloudCover;
            record.SnowFall = weatherData[i].SnowFall;
            record.Date = weatherData[i].Date;

            records.Add(record);
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter= "\t",
        };

        using (var writer = new StreamWriter($"csv/{startDate.Year}.{startDate.Month}.{startDate.Day}-{endDate.Year}.{endDate.Month}.{endDate.Day}-solar-{inverter}"))
        using (var csv = new CsvWriter(writer, config))
        {
            logger.LogInformation("Writing to csv...");
            csv.WriteRecords(records);
        }

        logger.LogInformation("Csv has been created.");

        return Ok($"Finished writing {records.Count} reords.");
    }
}
