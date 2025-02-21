using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Weatherapp;

internal class Moist
{
    public static void Moisturiser()
    {
        Console.WriteLine("TEMPERATURER");
        string filepath = "..\\..\\..\\Data\\tempdata5-med fel.txt";
        Task<string> t = File.ReadAllTextAsync(filepath);
        string text = t.Result;

        Console.WriteLine("\nVälj en funktion:");
        Console.WriteLine("1: Medeltemperatur och luftfuktighet per dag för valt datum");
        Console.WriteLine("2: Sortering av dagar från torrast till fuktigast");
        Console.WriteLine("3: Datum för meteorologisk vinter (mild vinter)");
        Console.WriteLine("4: Datum för meteorologisk höst");
        Console.WriteLine("0: Avsluta");

        int choice;
        do
        {
            Console.Write("\nAnge ditt val: ");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        GetAverageForDate(text);
                        break;
                    case 2:
                        SortByHumidity(text);
                        break;
                    case 3:
                        GetMeteorologicalWinter(text);
                        break;
                    case 4:
                        GetMeteorologicalAutumn(text);
                        break;
                    case 0:
                        Console.WriteLine("Programmet avslutas.");
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Felaktig inmatning, försök igen.");
            }
        } while (choice != 0);
    }

    // 1.Medeltemperatur och luftfuktighet per dag för valt datum
    private static void GetAverageForDate(string text)
    {
        Console.Write("\nAnge ett datum (YYYY-MM-DD): ");
        string inputDate = Console.ReadLine();

        if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
        {
            string pattern = $@"({inputDate} \d{{2}}:\d{{2}}:\d{{2}}),(\w+),(-?\d{{1,2}}\.\d),(\d+)";
            Regex regex = new Regex(pattern);

            var matches = regex.Matches(text)
                .Select(m => new
                {
                    Temperature = float.Parse(m.Groups[3].Value.Replace('.', ',')),
                    Humidity = int.Parse(m.Groups[4].Value)
                }).ToList();

            if (matches.Any())
            {
                Console.WriteLine($"\nDatum: {inputDate}");
                Console.WriteLine($"Medeltemperatur: {matches.Average(x => x.Temperature):F2} °C");
                Console.WriteLine($"Medelluftfuktighet: {matches.Average(x => x.Humidity):F2} %");
            }
            else
            {
                Console.WriteLine("\nIngen data hittades för det angivna datumet.");
            }
        }
        else
        {
            Console.WriteLine("\nFelaktigt datumformat. Använd formatet YYYY-MM-DD.");
        }
    }

    // 2.Sortering av dagar från torrast till fuktigast
    private static void SortByHumidity(string text)
    {
        string pattern = @"(\d{4}-\d{2}-\d{2}) \d{2}:\d{2}:\d{2},\w+,(-?\d{1,2}\.\d),(\d+)";
        Regex regex = new Regex(pattern);

        var dailyHumidity = regex.Matches(text)
            .GroupBy(m => m.Groups[1].Value)
            .Select(g => new
            {
                Date = DateTime.TryParseExact(g.Key, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : (DateTime?)null,
                AvgHumidity = g.Average(x => int.Parse(x.Groups[3].Value))
            })
            .Where(g => g.Date.HasValue)
            .OrderBy(g => g.AvgHumidity)
            .ToList();

        Console.WriteLine("\nDagar sorterade från torrast till fuktigast:");
        foreach (var day in dailyHumidity)
        {
            Console.WriteLine($"{day.Date:yyyy-MM-dd} - Medelluftfuktighet: {day.AvgHumidity:F2}%");
        }
    }

    // 3.Datum för meteorologisk vinter (mild vinter)
    private static void GetMeteorologicalWinter(string text)
    {
        string pattern = @"(\d{4}-\d{2}-\d{2}) \d{2}:\d{2}:\d{2},\w+,(-?\d{1,2}\.\d),\d+";
        Regex regex = new Regex(pattern);

        var winterStart = regex.Matches(text)
            .GroupBy(m => m.Groups[1].Value)
            .Select(g => new
            {
                DateString = g.Key,
                Date = DateTime.TryParseExact(g.Key, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : (DateTime?)null,
                AvgTemperature = g.Average(x => float.Parse(x.Groups[2].Value.Replace('.', ',')))
            })
            .Where(g => g.Date.HasValue && g.AvgTemperature < 0)
            .OrderBy(g => g.Date)
            .FirstOrDefault();

        if (winterStart != null)
        {
            Console.WriteLine($"\nMeteorologisk vinter (mild vinter) startade den: {winterStart.Date:yyyy-MM-dd}");
        }
        else
        {
            Console.WriteLine("\nIngen meteorologisk vinter identifierades.");
        }
    }

    // 4.Datum för meteorologisk höst
    private static void GetMeteorologicalAutumn(string text)
    {
        string pattern = @"(\d{4}-\d{2}-\d{2}) \d{2}:\d{2}:\d{2},\w+,(-?\d{1,2}\.\d),\d+";
        Regex regex = new Regex(pattern);

        var autumnStart = regex.Matches(text)
            .GroupBy(m => m.Groups[1].Value)
            .Select(g => new
            {
                Date = DateTime.TryParseExact(g.Key, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : (DateTime?)null,
                AvgTemperature = g.Average(x => float.Parse(x.Groups[2].Value.Replace('.', ',')))
            })
            .Where(g => g.Date.HasValue && g.AvgTemperature <= 10)
            .OrderBy(g => g.Date)
            .FirstOrDefault();

        if (autumnStart != null)
        {
            Console.WriteLine($"\nMeteorologisk höst startade den: {autumnStart.Date:yyyy-MM-dd}");
        }
        else
        {
            Console.WriteLine("\nIngen meteorologisk höst identifierades.");
        }
    }
}
