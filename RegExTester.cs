using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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


            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);
            }
        }
    }
}
