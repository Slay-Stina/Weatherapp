using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Weatherapp.Models;
using System.IO;

namespace Weatherapp
{
    internal class RegExTester
    {
        public static void TempToDB()
        {
            Console.WriteLine("TEMPERATURER");
            string filepath = "..\\..\\..\\Data\\tempdata5-med fel.txt";
            Task<string> t = File.ReadAllTextAsync(filepath);

            string text = t.Result;

            string pattern = @"(\d{4}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12]\d|3[01]) (?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d),(\w+),(-?\d{1,2}\.\d),(\d+)";

            Regex temps = new Regex(pattern);


            MatchCollection matches = temps.Matches(text);

            using (var db = new WeatherDbContext())
            {
                foreach (Match match in matches)
                {
                    TempEntity newTempEntity = new TempEntity();

                    newTempEntity.Date = DateTime.Parse(match.Groups[1].Value);
                    newTempEntity.IsIndoor = match.Groups[2].Value.ToLower() == "inne" ? true : false;
                    newTempEntity.Temperature = float.Parse(match.Groups[3].Value.Replace('.', ','));
                    newTempEntity.Humidity = int.Parse(match.Groups[4].Value);

                    Console.WriteLine(newTempEntity.Date + "\t" + newTempEntity.Temperature + "\t" + newTempEntity.IsIndoor + "\t" + newTempEntity.Humidity);

                    db.Add(newTempEntity);

                }
                db.SaveChanges();
            }
        }
    }
}
