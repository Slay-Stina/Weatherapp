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
        public static void RegExTest()
        {
            Console.WriteLine("TEMPERATURER");
            string filepath = "..\\..\\..\\Data\\tempdata5-med fel.txt";
            Task<string> t = File.ReadAllTextAsync(filepath);

            string text = t.Result;

            string pattern = @"(((\d{4})-(\d{2})-(\d{2})) ((\d{2}):(\d{2}):(\d{2}))),(\w+),\s*?(\d{1,2}.\d),(\d+)";

            Regex temps = new Regex(pattern);


            MatchCollection matches = temps.Matches(text);

            TempEntity tempEntity = new TempEntity();

            tempEntity.Date = DateTime.Parse(matches[0].Groups[1].Value);
            tempEntity.IsIndoor = matches[0].Groups[10].Value.ToLower() == "inne" ? true : false;
            tempEntity.Temperature = float.Parse(matches[0].Groups[11].Value.Replace('.', ','));
            tempEntity.Humidity = int.Parse(matches[0].Groups[12].Value);

            Console.WriteLine(tempEntity.Date + "\t" + tempEntity.Temperature + "\t" + tempEntity.IsIndoor + "\t" + tempEntity.Humidity);
            Console.ReadKey();

            //using(var db = new WeatherDbContext())
            //{
            //    foreach (Match match in matches)
            //    {
            //        TempEntity tempEntity = new TempEntity();

            //        tempEntity.Date = DateTime.Parse(match.Groups[1].Value);
            //        tempEntity.IsIndoor = match.Groups[10].Value.ToLower() == "inne" ? true : false;
            //        tempEntity.Temperature = float.Parse(match.Groups[11].Value.Replace('.', ','));
            //        tempEntity.Humidity = int.Parse(match.Groups[12].Value);

            //        db.Add(tempEntity);

            //    }
            //    db.SaveChanges();
            //}
        }
    }
}
