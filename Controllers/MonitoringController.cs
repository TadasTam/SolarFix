using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarFix.Data;
using SolarFix.Dto;

namespace SolarFix.Controllers;

public class MonitoringController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public MonitoringController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> CloudCoverAndCurrentPower(DateTime startDate, DateTime endDate, string aggregateType, string inverter)
    {
        if (aggregateType == "hour")
        {
            endDate = endDate.AddDays(1);
            var weatherData = await _dbContext.WeatherDatas.Where(b => b.Date >= startDate && b.Date <= endDate).Select(x => new { x.CloudCover, x.Date }).ToListAsync();

            var solarData = await _dbContext.SolarProductions
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

            var weatherAndSolarData = weatherData.Select(x => new { x.Date, x.CloudCover, CurrentPower = solarData.Where(z => z.Date.Year == x.Date.Year && z.Date.DayOfYear == x.Date.DayOfYear && z.Date.Hour == x.Date.Hour).Select(z => z.ProducedEnergy).FirstOrDefault() }).ToList();

            return Json(new { CurrentPower = weatherAndSolarData.Select(x => x.CurrentPower).ToList(), CloudCover = weatherAndSolarData.Select(x => x.CloudCover).ToList(), Date = weatherAndSolarData.Select(x => x.Date).ToList(), Type = "Hour" });
        }
        else if (aggregateType == "day")
        {
            endDate = endDate.AddDays(1);

            var weatherData = await _dbContext.WeatherDatas
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.Date.Hour >= 10 && b.Date.Hour <= 18)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    CloudCoverAverage = g.Average(sp => sp.CloudCover)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            var solarData = await _dbContext.SolarProductions
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    ProducedEnergy = g.Max(sp => sp.DailyProducedEnergy) - g.Min(sp => sp.DailyProducedEnergy)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            var WeatherDays = new List<SimilarWeatherDayDto>(solarData.Count);

            for (int i = 0; i < solarData.Count; i++)
            {
                WeatherDays.Add(new SimilarWeatherDayDto() { Date = solarData[i].Date, CloudCover = (int)weatherData.Where(x => x.Date.Date == solarData[i].Date.Date).Select(x => x.CloudCoverAverage).First(), CurrentPower = solarData[i].ProducedEnergy });
            }

            return Json(new { Date = WeatherDays.Select(x => x.Date).ToList(), CloudCover = WeatherDays.Select(x => x.CloudCover).ToList(), CurrentPower = WeatherDays.Select(x => x.CurrentPower).ToList(), Type = "Day" });
        }
        else if (aggregateType == "week")
        {
            int daysNumber = endDate.Subtract(startDate).Days + 1;
            int addDays = (7 - ((daysNumber - 1) % 7 + 1));
            endDate = endDate.AddDays(addDays + 1);

            var weatherData = await _dbContext.WeatherDatas
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.Date.Hour >= 10 && b.Date.Hour <= 18)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    CloudCoverAverage = g.Average(sp => sp.CloudCover)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            var solarData = await _dbContext.SolarProductions
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    ProducedEnergy = g.Max(sp => sp.DailyProducedEnergy) - g.Min(sp => sp.DailyProducedEnergy)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            var WeatherDays = new List<SimilarWeatherDayDto>(solarData.Count);

            for (int i = 0; i < solarData.Count; i++)
            {
                int week = (weatherData[i].Date.Subtract(startDate.Date).Days) / 7;
                WeatherDays.Add(new SimilarWeatherDayDto() { Date = solarData[i].Date, CloudCover = (int)weatherData.Where(x => x.Date.Date == solarData[i].Date.Date).Select(x => x.CloudCoverAverage).First(), CurrentPower = solarData[i].ProducedEnergy });
            }

            var WeatherAndSolarDataGroupedByWeek = WeatherDays
                .GroupBy(x => x.Date.Subtract(startDate.Date).Days / 7)
                .Select(g => new { Date = g.Min(x => x.Date), CloudCoverWeightedAverage = g.Sum(x => x.CloudCover * (g.Sum(x => x.CurrentPower) != 0 ? x.CurrentPower / g.Sum(x => x.CurrentPower) : 1.0 / (9 * 7))), ProducedEnergy = g.Sum(x => x.CurrentPower) })
                .ToList();

            return Json(new { Date = WeatherAndSolarDataGroupedByWeek.Select(x => x.Date).ToList(), CloudCover = WeatherAndSolarDataGroupedByWeek.Select(x => x.CloudCoverWeightedAverage).ToList(), CurrentPower = WeatherAndSolarDataGroupedByWeek.Select(x => x.ProducedEnergy).ToList(), Type = "Week" });
        }
        else if (aggregateType == "month")
        {
            startDate = new DateTime(startDate.Year, startDate.Month, 1);
            endDate = endDate.Month == 12 ? new DateTime(endDate.Year, 12, 31) : new DateTime(endDate.Year, endDate.Month + 1, 1).AddDays(-1);

            var weatherData = await _dbContext.WeatherDatas
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.Date.Hour >= 10 && b.Date.Hour <= 18)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    CloudCoverAverage = g.Average(sp => sp.CloudCover)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            var solarData = await _dbContext.SolarProductions
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    ProducedEnergy = g.Max(sp => sp.DailyProducedEnergy) - g.Min(sp => sp.DailyProducedEnergy)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            var weatherAndSolarData = new List<SimilarWeatherDayDto>(solarData.Count);

            for (int i = 0; i < solarData.Count; i++)
            {
                weatherAndSolarData.Add(new SimilarWeatherDayDto() { Date = solarData[i].Date, CloudCover = (int)weatherData.Where(x => x.Date.Date == solarData[i].Date.Date).Select(x => x.CloudCoverAverage).First(), CurrentPower = solarData[i].ProducedEnergy });
            }

            var WeatherDaysGroupedByMonth = weatherAndSolarData
                .GroupBy(x => new { x.Date.Year, x.Date.Month })
                .Select(g => new { Date = g.Min(x => x.Date), WeightedCloudCoverAverage = g.Sum(x => x.CloudCover * (g.Sum(x => x.CurrentPower) != 0 ? x.CurrentPower / g.Sum(x => x.CurrentPower) : 1.0 / (30 * 9))), ProducedEnergy = g.Sum(x => x.CurrentPower) })
                .ToList();

            return Json(new { Date = WeatherDaysGroupedByMonth.Select(x => x.Date).ToList(), CloudCover = WeatherDaysGroupedByMonth.Select(x => x.WeightedCloudCoverAverage).ToList(), CurrentPower = WeatherDaysGroupedByMonth.Select(x => x.ProducedEnergy).ToList(), Type = "Month" });
        }

        return Json(new { });
    }

    [HttpGet]
    public async Task<IActionResult> CloudCoverAverageAndCurrentPower(DateTime selectedDate)
    {
        var selectedDateWeatherDate = await _dbContext.WeatherDatas.Where(b => b.Date.Date == selectedDate.Date).Select(x => new { x.CloudCover, x.Date }).ToListAsync();

        var selectedDateCurrentPower = await _dbContext.SolarProductions.Where(b => b.Date.Date == selectedDate.Date).ToListAsync();

        var selectedDayAggregatedCurrentPower = new List<int>(selectedDateWeatherDate.Count);

        for (int i = 1; i <= selectedDateWeatherDate.Count; i++)
        {
            var periodData = selectedDateCurrentPower.Where(x => x.Date >= selectedDateWeatherDate[i - 1].Date && x.Date <= selectedDateWeatherDate[i].Date).Select(x => x.CurrentPower);
            var aggregatedData = periodData.Count() != 0 ? (int)periodData.Average() : 0;

            selectedDayAggregatedCurrentPower.Add(aggregatedData);
        }

        var similarWeatherData = await _dbContext.WeatherDatas.Where(b => Math.Abs(b.Date.DayOfYear - selectedDate.DayOfYear) < 5).Select(x => new { x.CloudCover, x.Date }).ToListAsync();

        var similarDateCurrentPower = await _dbContext.SolarProductions.Where(b => Math.Abs(b.Date.DayOfYear - selectedDate.DayOfYear) < 5).ToListAsync();

        var similarAggregatedCurrentPower = new List<int>(similarWeatherData.Count);

        for (int i = 1; i <= similarWeatherData.Count; i++)
        {
            var periodData = similarDateCurrentPower.Where(x => x.Date >= similarWeatherData[i - 1].Date && x.Date <= similarWeatherData[i].Date).Select(x => x.CurrentPower);
            var aggregatedData = periodData.Count() != 0 ? (int)periodData.Average() : 0;

            similarAggregatedCurrentPower.Add(aggregatedData);
        }

        var similarWeatherDays = new List<SimilarWeatherDayDto>(similarWeatherData.Count);

        for (int i = 0; i < similarWeatherData.Count; i++)
        {
            similarWeatherDays.Add(new SimilarWeatherDayDto() { Date = similarWeatherData[i].Date, CloudCover = similarWeatherData[i].CloudCover, CurrentPower = similarAggregatedCurrentPower[i] });
        }

        var similarWeatherDaysGroupedByDay = similarWeatherDays
            .GroupBy(x => x.Date.Date)
            .Select(g => new { Date = g.Min(x => x.Date), WeightedCloudCoverDayAverage = g.Sum(x => x.CloudCover * (x.CurrentPower / g.Sum(x => x.CurrentPower))), TotalDayPower = g.Sum(x => x.CurrentPower) })
            .Where(x => x.TotalDayPower != 0)
            .OrderBy(x => x.Date)
            .ToList();

        var selectedDateWeightedCloudCoverDayAverage = similarWeatherDaysGroupedByDay.Where(x => x.Date == selectedDate).Select(x => x.WeightedCloudCoverDayAverage).FirstOrDefault();

        var similarDaysByWeather = similarWeatherDaysGroupedByDay.Where(x => Math.Abs(x.WeightedCloudCoverDayAverage - selectedDateWeightedCloudCoverDayAverage) <= 15);

        var energyAverage = similarDaysByWeather.Select(x => x.TotalDayPower).Average();

        return Json(new { date = similarDaysByWeather.Select(x => x.Date).ToList(), weightedCloudCoverDayAverage = similarDaysByWeather.Select(x => x.WeightedCloudCoverDayAverage).ToList(), totalDayPower = similarDaysByWeather.Select(x => new { value = x.TotalDayPower, itemStyle = new { color = x.Date == selectedDate ? "yellow" : "blue" } }).ToList(), energyAverage = Enumerable.Repeat(energyAverage, similarDaysByWeather.Count()) });
    }

    [HttpGet]
    public async Task<IActionResult> CloudCoverAndCurrentPowerInverters(DateTime startDate, DateTime endDate, string aggregateType)
    {
        var allInverters = await _dbContext.SolarProductions.Select(x => new { inverter = x.InverterId }).Distinct().ToListAsync();
        List<object> Jsons = new List<object>();

        if (aggregateType == "hour")
        {
            endDate = endDate.AddDays(1);
            var weatherData = await _dbContext.WeatherDatas.Where(b => b.Date >= startDate && b.Date <= endDate).Select(x => new { x.CloudCover, x.Date }).ToListAsync();

            foreach (var inverter in allInverters)
            {
                var solarData = await _dbContext.SolarProductions
            .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter.inverter)
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

                var weatherAndSolarData = weatherData.Select(x => new { x.Date, CurrentPower = solarData.Where(z => z.Date.Year == x.Date.Year && z.Date.DayOfYear == x.Date.DayOfYear && z.Date.Hour == x.Date.Hour).Select(z => z.ProducedEnergy).FirstOrDefault() }).ToList();

                Jsons.Add(new { InverterID = inverter.inverter, CurrentPower = weatherAndSolarData.Select(x => x.CurrentPower).ToList() });
            }

            return Json(new { CurrentPowers = Jsons, CloudCover = weatherData.Select(x => x.CloudCover).ToList(), Date = weatherData.Select(x => x.Date).ToList(), Type = "Hour" });

        }
        else if (aggregateType == "day")
        {
            endDate = endDate.AddDays(1);

            var weatherData = await _dbContext.WeatherDatas
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.Date.Hour >= 10 && b.Date.Hour <= 18)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    CloudCoverAverage = g.Average(sp => sp.CloudCover)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            foreach (var inverter in allInverters)
            {
                var solarData = await _dbContext.SolarProductions
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter.inverter)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    ProducedEnergy = g.Max(sp => sp.DailyProducedEnergy) - g.Min(sp => sp.DailyProducedEnergy)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

                var weatherAndSolarData = weatherData.Select(x => new { x.Date, CurrentPower = solarData.Where(z => z.Date.Year == x.Date.Year && z.Date.DayOfYear == x.Date.DayOfYear).Select(z => z.ProducedEnergy).FirstOrDefault() }).ToList();

                Jsons.Add(new { InverterID = inverter.inverter, CurrentPower = weatherAndSolarData.Select(x => x.CurrentPower).ToList() });
            }

            return Json(new { Date = weatherData.Select(x => x.Date).ToList(), CloudCover = weatherData.Select(x => x.CloudCoverAverage).ToList(), CurrentPowers = Jsons, Type = "Day" });
        }
        else if (aggregateType == "week")
        {
            int daysNumber = endDate.Subtract(startDate).Days + 1;
            int addDays = (7 - ((daysNumber - 1) % 7 + 1));
            endDate = endDate.AddDays(addDays + 1);

            var weatherData = await _dbContext.WeatherDatas
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.Date.Hour >= 10 && b.Date.Hour <= 18)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    CloudCoverAverage = g.Average(sp => sp.CloudCover)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            foreach (var inverter in allInverters)
            {
                var solarData = await _dbContext.SolarProductions
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter.inverter)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    ProducedEnergy = g.Max(sp => sp.DailyProducedEnergy) - g.Min(sp => sp.DailyProducedEnergy)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

                var WeekDays = new List<SimilarWeatherDayDto>(weatherData.Count);
                for (int i = 0; i < weatherData.Count; i++)
                {
                    int week = (weatherData[i].Date.Subtract(startDate.Date).Days) / 7;
                    var list = solarData.Where(x => x.Date.Date == weatherData[i].Date.Date).Select(x => x.ProducedEnergy).ToList();
                    float curPow = list.Count != 0 ? list[0] : 0;

                    WeekDays.Add(new SimilarWeatherDayDto() { Date = weatherData[i].Date, CurrentPower = curPow });
                }

                Jsons.Add(new { InverterID = inverter.inverter, CurrentPower = WeekDays.GroupBy(x => x.Date.Subtract(startDate.Date).Days / 7).Select(g => g.Sum(x => x.CurrentPower)).ToList() });
            }

            var WeatherDays = new List<SimilarWeatherDayDto>(weatherData.Count);

            for (int i = 0; i < weatherData.Count; i++)
            {
                int week = (weatherData[i].Date.Subtract(startDate.Date).Days) / 7;
                WeatherDays.Add(new SimilarWeatherDayDto() { Date = weatherData[i].Date, CloudCover = (int)weatherData.Where(x => x.Date.Date == weatherData[i].Date.Date).Select(x => x.CloudCoverAverage).First() });
            }

            var WeatherDataGroupedByWeek = WeatherDays
                .GroupBy(x => x.Date.Subtract(startDate.Date).Days / 7)
                .Select(g => new { Date = g.Min(x => x.Date), CloudCoverAverage = g.Sum(x => x.CloudCover) * 1.0 / (7) })
                .ToList();

            return Json(new { Date = WeatherDataGroupedByWeek.Select(x => x.Date).ToList(), CloudCover = WeatherDataGroupedByWeek.Select(x => x.CloudCoverAverage).ToList(), CurrentPowers = Jsons, Type = "Week" });
        }
        else if (aggregateType == "month")
        {
            startDate = new DateTime(startDate.Year, startDate.Month, 1);
            endDate = endDate.Month == 12 ? new DateTime(endDate.Year, 12, 31) : new DateTime(endDate.Year, endDate.Month + 1, 1).AddDays(-1);

            var weatherData = await _dbContext.WeatherDatas
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.Date.Hour >= 10 && b.Date.Hour <= 18)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    CloudCoverAverage = g.Average(sp => sp.CloudCover)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            foreach (var inverter in allInverters)
            {
                var solarData = await _dbContext.SolarProductions
                .Where(b => b.Date >= startDate && b.Date <= endDate && b.InverterId == inverter.inverter)
                .GroupBy(sp => new
                {
                    sp.Date.Year,
                    sp.Date.Month,
                    sp.Date.Day,
                })
                .Select(g => new
                {
                    Date = g.Min(sp => sp.Date),
                    ProducedEnergy = g.Max(sp => sp.DailyProducedEnergy) - g.Min(sp => sp.DailyProducedEnergy)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

                var SolarDataDate = new List<SimilarWeatherDayDto>(weatherData.Count);
                for (int i = 0; i < weatherData.Count; i++)
                {
                    var list = solarData.Where(x => x.Date.Date == weatherData[i].Date.Date).Select(x => x.ProducedEnergy).ToList();
                    float curPow = list.Count != 0 ? list[0] : 0;
                    SolarDataDate.Add(new SimilarWeatherDayDto() { Date = weatherData[i].Date, CurrentPower = curPow });
                }

                var DaysGroupedByMonth = SolarDataDate
                .GroupBy(x => new { x.Date.Year, x.Date.Month })
                .Select(g => new { Date = g.Min(x => x.Date), CurrentPower = g.Sum(x => x.CurrentPower) })
                .ToList();

                Jsons.Add(new { InverterID = inverter.inverter, CurrentPower = DaysGroupedByMonth.Select(x => x.CurrentPower).ToList() });
            }

            var WeatherDaysGroupedByMonth = weatherData
                .GroupBy(x => new { x.Date.Year, x.Date.Month })
                .Select(g => new { Date = g.Min(x => x.Date), CloudCoverAverage = g.Sum(x => x.CloudCoverAverage) * (1.0 / (30)) })
                .ToList();

            return Json(new { Date = WeatherDaysGroupedByMonth.Select(x => x.Date).ToList(), CloudCover = WeatherDaysGroupedByMonth.Select(x => x.CloudCoverAverage).ToList(), CurrentPowers = Jsons, Type = "Month" });
        }

        return Json(new { });
    }

    [HttpGet]
    public async Task<IActionResult> DayProducedEnergyInverters(DateTime date)
    {
        var anomalyDates = new List<DateTime>();
        var allInverters = await _dbContext.SolarProductions.Select(x => new { inverter = x.InverterId }).Distinct().ToListAsync();
        List<object> Jsons = new List<object>();
        var endDate = date.AddDays(1);

        var timeLimits = await _dbContext.SolarProductions.Where(b => b.Date >= date && b.Date <= endDate).Select(x => x.Date).ToListAsync();
        DateTime min = timeLimits.Min(), max = timeLimits.Max();

        List<DateTime> Times = new List<DateTime>();
        for (int h = min.Hour; h <= 23; h++)
            for (int m = 0; m <= 55; m += 5)
            {
                DateTime s = new DateTime(date.Year, date.Month, date.Day, h, m, 0);
                if (s >= min)
                    Times.Add(s);
                if (s > max) { h = 24; break; }
            }

        foreach (var inverter in allInverters)
        {
            var solarData = await _dbContext.SolarProductions
            .Where(b => b.Date >= date && b.Date <= endDate && b.InverterId == inverter.inverter)
            .Select(g => new { g.Date, g.DailyProducedEnergy })
            .OrderBy(r => r.Date)
            .ToListAsync();

            List<float> Produced = new List<float>();
            foreach (DateTime time in Times)
            {
                var l = solarData.Where(x => x.Date <= time).Select(x => x.DailyProducedEnergy).ToList();
                float p = l.Count != 0 ? l.Last() : 0;
                Produced.Add(p);
            }

            Jsons.Add(new { InverterID = inverter.inverter, Produced = Produced });
        }

        var previousProportion = 0;

        for (int i = 10; i < Times.Count; i++)
        {

        }

        return Json(new { Date = Times, Inverters = Jsons, AnomalyDates = anomalyDates });
    }
}
