using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models;

public class WeatherViewModel
{
    public WeatherViewModel(List<WeatherInfo> weatherList)
    {
        WeatherList = weatherList;
    }
    
    public WeatherViewModel(List<WeatherInfo> weatherList, IEnumerable<int> yearsList, IEnumerable<string?> monthsList)
    {
        WeatherList = weatherList;
        Years = new SelectList(yearsList);
        Months = new SelectList(monthsList);
    }

    public List<WeatherInfo> WeatherList { get; set; }
    
    public SelectList? Years { get; set; }
    
    public SelectList? Months { get; set; }
    
}