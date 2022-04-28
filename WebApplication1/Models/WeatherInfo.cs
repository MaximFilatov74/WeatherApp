using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class WeatherInfo
{
    [Key]
    public int Id { get; set; }
    
    public string? Month { get; init; }
    
    public string? FileName { get; init; }
    
    public DateTime Date { get; init; }

    public double? Temperature { get; init; }
    
    public double? AirHumidity { get; init; }
    
    public double? DewPoint { get; init; }
    
    public double? Pressure { get; init; }
    
    public string? DirectionOfTheWind { get; init; }
    
    public int? WindSpeed { get; init; }
    
    public int? Cloudiness { get; init; }
    
    public int? CloudBase { get; init; }
    
    public int? HorizontalVisibility { get; init; }

    public string? WeatherConditions { get; init; }
}