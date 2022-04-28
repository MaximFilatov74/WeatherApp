using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ArchiveController : Controller
{
    private readonly WeatherContext _dbContext = new();
    
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any, NoStore = false)] 
    public IActionResult ShowArchive(List<int>? years, List<string>? months)
    {
        var context = _dbContext.WeatherContextInfos.Select(x => x).ToList();
        var listYears = context.Select(x => x.Date.Year).Distinct();
        var listMonths = context.Select(x => x.Month).Distinct();
        
        if (years != null && years.Count > 0)
        {
            context = context.Where(x => years.Contains(x.Date.Year)).ToList();
        }
        
        if (months != null && months.Count > 0)
        {
            context = context.Where(x => x.Month != null && months.Contains(x.Month)).ToList();
        }
        
        return View(new WeatherViewModel(context, listYears, listMonths ?? throw new InvalidOperationException()));
    }
}