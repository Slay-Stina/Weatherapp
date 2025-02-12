using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Weatherapp
{
    internal class WeatherDataAnalyzer
    {
        private Dictionary<DateTime, List<double>> dailyTemperatures = new Dictionary<DateTime, List<double>>();
        private Dictionary<DateTime, List<int>> dailyHumidity = new Dictionary<DateTime, List<int>>();

        public WeatherDataAnalyzer(string filePath)
        {
            LoadData(filePath);
        }

        private void LoadData(string filePath)
        {
            string pattern = @"(\d{4}-\d{2}-\d{2}) (?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d,\w+,(-?\d{1,2}\.\d),(\d+)";
            string[] ignoredMonths = { "2016-05", "2017-01" };

            string text = File.ReadAllText(filePath);
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(text);

            foreach (Match match in matches)
            {
                string dateString = match.Groups[1].Value;
                if (ignoredMonths.Any(m => dateString.StartsWith(m))) continue;

                if (!double.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double temp))
                    continue;
                if (!int.TryParse(match.Groups[3].Value, out int humidity)) continue;

                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    if (!dailyTemperatures.ContainsKey(date))
                    {
                        dailyTemperatures[date] = new List<double>();
                        dailyHumidity[date] = new List<int>();
                    }

                    dailyTemperatures[date].Add(temp);
                    dailyHumidity[date].Add(humidity);
                }
                else
                {
                    
                    Console.WriteLine($"Invalid date found: {dateString}");
                }
            }
        }

        public void GetDailyAverageWeather(string inputDate)
        {
            if (!DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
            {
                Console.WriteLine("Fel: Ange datum i formatet YYYY-MM-DD.");
                return;
            }

            if (dailyTemperatures.ContainsKey(searchDate))
            {
                double avgTemp = dailyTemperatures[searchDate].Average();
                double avgHumidity = dailyHumidity[searchDate].Average();

                Console.WriteLine($"Datum: {searchDate:yyyy-MM-dd}");
                Console.WriteLine($"Medeltemperatur: {avgTemp:F1}°C");
                Console.WriteLine($"Medelluftfuktighet: {avgHumidity:F1}%");
            }
            else
            {
                Console.WriteLine($"Ingen data tillgänglig för {searchDate:yyyy-MM-dd}.");
            }
        }

        public void GetDailyAverageTemperature(string inputDate)
        {
            if (!DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
            {
                Console.WriteLine("Fel: Ange datum i formatet YYYY-MM-DD.");
                return;
            }

            if (dailyTemperatures.ContainsKey(searchDate))
            {
                double avgTemp = dailyTemperatures[searchDate].Average();
                Console.WriteLine($"Datum: {searchDate:yyyy-MM-dd}");
                Console.WriteLine($"Medeltemperatur: {avgTemp:F1}°C");
            }
            else
            {
                Console.WriteLine($"Ingen temperaturdata tillgänglig för {searchDate:yyyy-MM-dd}.");
            }
        }

        public void SortDaysByHumidity()
        {
            var sortedDays = dailyHumidity
                .OrderBy(kvp => kvp.Value.Average()) // Sorterar från torrast till fuktigast
                .ToList();

            Console.WriteLine("\nSorterad lista över dagar från torrast till fuktigast:");
            foreach (var entry in sortedDays)
            {
                Console.WriteLine($"{entry.Key:yyyy-MM-dd} - Medelluftfuktighet: {entry.Value.Average():F1}%");
            }
        }
    }
}
