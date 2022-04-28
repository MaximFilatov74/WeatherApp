using System.Data.Entity;

namespace WebApplication1.Models;

public class WeatherContext : DbContext
{ 
    public DbSet<WeatherInfo> WeatherContextInfos { get; set; }
}