using Weatherapp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Weatherapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RegExTester.TempToDB();

            using (var db = new WeatherDbContext())
            {
                foreach (var entity in db.TempEntities.OrderByDescending(t => t.MoldRisk))
                {
                    //point.MoldRisk = point.CalculateMoldRisk();
                    string riskLevel = entity.ClassifyMoldRisk(entity.MoldRisk);

                    Console.WriteLine($"{entity.Date}: {(entity.IsIndoor ? "Inomhus" : "Utomhus")}, Temp: {entity.Temperature}°C, LF: {entity.Humidity}%");
                    Console.WriteLine($"Mögelrisk: {entity.MoldRisk:F2}, Risknivå: {riskLevel}");
                    Console.WriteLine();
                }
                //db.SaveChanges();
            }
        }
    }
}
