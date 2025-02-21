using System;
using System.Linq;
using Weatherapp.Models;

namespace Weatherapp;

public class TemperatureSorter
{
    public static void ShowSortedTemps(bool sorteringVarmastFörst)
    {
        using (var db = new WeatherDbContext())
        {
            var averageTemperaturesByDay = db.TempEntities
                .GroupBy(t => t.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    AverageTemperature = g.Average(t => t.Temperature)
                })
                .ToList();

            if (sorteringVarmastFörst)
            {
                averageTemperaturesByDay = averageTemperaturesByDay
                    .OrderByDescending(d => d.AverageTemperature)
                    .ToList();
            }
            else
            {
                averageTemperaturesByDay = averageTemperaturesByDay
                    .OrderBy(d => d.AverageTemperature)
                    .ToList();
            }

            Console.WriteLine("Sortering av dagar baserat på medeltemperatur:");
            foreach (var data in averageTemperaturesByDay)
            {
                Console.WriteLine($"Datum: {data.Date.ToShortDateString()}, Medeltemp: {data.AverageTemperature:F1}");
            }
        }
    }
}