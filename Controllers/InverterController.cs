using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarFix.Data;

namespace SolarFix.Controllers;
public class InverterController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public InverterController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allInverters = await _dbContext.SolarProductions.Select(x => new { value = x.InverterId, label = x.InverterId }).Distinct().ToListAsync();

        return Json(allInverters);
    }
}
