using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Weatherapp.Models;
using System.IO;
using Weatherapp.Extensions;

namespace Weatherapp
{
    internal class RegExTester
    {
        public delegate float MoldRiskCalculator(TempEntity tempEntity);

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
                MoldRiskCalculator calcMold = MyDelegates.CalculateMoldRisk;

                foreach (Match match in matches)
                {
                    TempEntity newTemp = match.ToTempEntity(calcMold);

                    newTemp.PrintTemp();

                    db.Add(newTemp);
                }
                db.SaveChanges();
            }
        }
    }
}
