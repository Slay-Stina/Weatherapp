using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Weatherapp
{
    internal class Weather
    {
        public static void FindWinter(string filepath)
        {
            Console.WriteLine("TEMPERATURER");
            // string filepath = "..\\..\\..\\Data\\tempdata5-med fel.txt";

            Task<string> t = File.ReadAllTextAsync(filepath);

            string text = t.Result;

            string pattern = @"(\d{4}-\d{2}-\d{2}) (?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d,\w+,(-?\d{1,2}\.\d),\d+";
            string[] ignoredMonths = { "2016-05", "2017-01" };
            Regex temps = new Regex(pattern);
            MatchCollection matches = temps.Matches(text);
            Dictionary<string, List<double>> dailyTemps = new Dictionary<string, List<double>>();

            foreach (Match match in matches)
            {
                string dateString = match.Groups[1].Value;
                double temp = double.Parse(match.Groups[2].Value.Replace('.', ','));
                if (ignoredMonths.Any(m => dateString.StartsWith(m)))
                    continue;

                if (!double.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out temp))
                    continue;

                DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!dailyTemps.ContainsKey(dateString))
                    dailyTemps[dateString] = new List<double>();

                dailyTemps[dateString].Add(temp);
            }
            // Beräkna dygnsmedeltemperaturer
            Dictionary<string, double> dailyAvgTemps = dailyTemps
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Average());

            // Hitta meteorologisk höst och vinter
            string autumnStart = FindSeasonStart(dailyAvgTemps, 10.0);
            string winterStart = FindSeasonStart(dailyAvgTemps, 0.0);

            Console.WriteLine($"Meteorologisk höst börjar: {autumnStart}");
            Console.WriteLine($"Meteorologisk vinter börjar: {winterStart}");

        }
        private static string FindSeasonStart(Dictionary<string, double> dailyAvgTemps, double threshold)
        {
            int count = 0;
            string startDate = null;

            foreach (var entry in dailyAvgTemps.OrderBy(kvp => kvp.Key))
            {
                if (entry.Value < threshold)
                {
                    count++;
                    if (count == 1) startDate = entry.Key;
                    if (count >= 5) return startDate; // Fem sammanhängande dagar funna
                }
                else
                {
                    count = 0;
                    startDate = null;
                }
            }
            return "Ej funnen";
        }
    }
}
