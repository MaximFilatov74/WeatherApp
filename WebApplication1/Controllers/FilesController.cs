using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using static System.Int32;

namespace WebApplication1.Controllers;

public class FilesController : Controller
{
    private readonly WeatherContext _dbContext = new();

    [HttpPost]
    public IActionResult Import(IFormFileCollection excelFiles)
    {
        var listWeatherInfo = new List<WeatherInfo>();

        foreach (var excelFile in excelFiles)
        {
            if (!ModelState.IsValid) return View(new WeatherViewModel(listWeatherInfo));
            using var workbook = new XLWorkbook(excelFile.OpenReadStream(), XLEventTracking.Disabled);
            foreach (var worksheet in workbook.Worksheets)
            {
                foreach (var row in worksheet.RowsUsed().Skip(4))
                {
                    var weatherInfo = new WeatherInfo()
                    {
                        FileName = excelFile.FileName,
                        Month = Regex.Replace(worksheet.Name, @"\s[0-9]+", "", RegexOptions.IgnoreCase),
                        Date = GetDate(row, worksheet),
                        Temperature = GetTemperature(row, worksheet),
                        AirHumidity = GetAirHumidity(row, worksheet),
                        DewPoint = GetDewPoint(row, worksheet),
                        Pressure = GetPressure(row, worksheet),
                        DirectionOfTheWind = GetDirectionOfTheWind(row, worksheet),
                        WindSpeed = GetWindSpeed(row, worksheet),
                        Cloudiness = GetCloudiness(row, worksheet),
                        CloudBase = GetCloudBase(row, worksheet),
                        HorizontalVisibility = GetHorizontalVisibility(row, worksheet),
                        WeatherConditions = GetWeatherConditions(row, worksheet)
                    };

                    listWeatherInfo.Add(weatherInfo);
                    _dbContext.WeatherContextInfos?.Add(weatherInfo);
                }
            }
        }

        _dbContext.SaveChanges();
        return View(new WeatherViewModel(listWeatherInfo));
    }

    private DateTime GetDate(IXLRow row, IXLWorksheet worksheet)
    {
        return DateTime.TryParse(string.Concat(row.Cell(worksheet.Row(3).CellsUsed()
                    .FirstOrDefault(x => x.Value.ToString() == "Дата")?.Address.ColumnNumber ?? 1)?.Value
                ?.ToString() ?? string.Empty,
            " ",
            row.Cell(worksheet.Row(3).CellsUsed()
                    .FirstOrDefault(x => x.Value.ToString() == "Время")?.Address.ColumnNumber ?? 2)?.Value
                ?.ToString() ?? string.Empty), out var date)
            ? date
            : DateTime.Now;

    }

    private static double? GetTemperature(IXLRow row, IXLWorksheet worksheet)
    {
        return double.TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(x => x.Value.ToString() == "Т")?.Address.ColumnNumber ?? 3).Value
            ?.ToString(), out var temperature)
            ? temperature
            : null;
    }

    private static double? GetAirHumidity(IXLRow row, IXLWorksheet worksheet)
    {
        return double.TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(x => x.Value.ToString() == "Отн. влажность")?.Address.ColumnNumber ?? 4)
            .Value?.ToString(), out var airHumidity)
            ? airHumidity
            : null;
    }

    private static double? GetDewPoint(IXLRow row, IXLWorksheet worksheet)
    {
        return double.TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(x => x.Value.ToString() == "Td")?.Address.ColumnNumber ?? 5).Value
            ?.ToString(), out var dewPoint)
            ? dewPoint
            : null;
    }

    private static double? GetPressure(IXLRow row, IXLWorksheet worksheet)
    {
        return double.TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(x => x.Value.ToString() == "Атм. давление,")?.Address.ColumnNumber ?? 6)
            .Value?.ToString(), out var pressure)
            ? pressure
            : null;
    }

    private static string? GetDirectionOfTheWind(IXLRow row, IXLWorksheet worksheet)
    {
        return row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(x => x.Value.ToString() == "Направление")?.Address.ColumnNumber ?? 7)
            .Value?.ToString() ?? null;
    }

    private static int? GetWindSpeed(IXLRow row, IXLWorksheet worksheet)
    {
        return TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(x => x.Value.ToString() == "Скорость")?.Address.ColumnNumber ?? 8).Value
            ?.ToString(), out var windSpeed)
            ? windSpeed
            : null;
    }

    private static int? GetCloudiness(IXLRow row, IXLWorksheet worksheet)
    {
        return TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(x => x.Value.ToString() == "Облачность,")?.Address.ColumnNumber ?? 9)
            .Value
            ?.ToString(), out var cloudiness)
            ? cloudiness
            : null;
    }

    private static int? GetCloudBase(IXLRow row, IXLWorksheet worksheet)
    {
        return TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(xlCell => xlCell.Value.ToString() == "h")?.Address.ColumnNumber ?? 10)
            .Value
            ?.ToString(), out var cloudBase)
            ? cloudBase
            : null;
    }

    private static int? GetHorizontalVisibility(IXLRow row, IXLWorksheet worksheet)
    {
        return TryParse(row.Cell(worksheet.CellsUsed()
                .FirstOrDefault(xlCell => xlCell.Value.ToString() == "VV")?.Address.ColumnNumber ?? 11)
            .Value?.ToString(), out var horizontalVisibility)
            ? horizontalVisibility
            : null;
    }

    private static string? GetWeatherConditions(IXLRow row, IXLWorksheet worksheet)
    {
        return row.Cell(worksheet.CellsUsed()
            .FirstOrDefault(xlCell => xlCell.Value.ToString() == "Погодные явления")?.Address
            .ColumnNumber ?? 12).Value?.ToString() ?? null;
    }
}