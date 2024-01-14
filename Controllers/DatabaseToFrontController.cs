using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarFix.Data;

namespace SolarFix.Controllers;

public class DatabaseToFrontController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DatabaseToFrontController> _logger;

    public DatabaseToFrontController(ILogger<DatabaseToFrontController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetDataByDate([FromQuery(Name = "startDate")] DateTime startDate, [FromQuery(Name = "endDate")] DateTime endDate)
    {
        endDate = endDate.AddDays(1);
        var data = await _dbContext.SolarProductions.Where(b => b.Date >= startDate && b.Date <= endDate).ToListAsync();

        _logger.LogInformation(data.ToString());

        return Json(data);
    }
}
