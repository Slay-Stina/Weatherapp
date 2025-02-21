using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weatherapp.Extensions;

namespace Weatherapp.Models;

internal class TempEntity
{
    public int Id { get; set; }
    public float Temperature { get; set; }
    public DateTime Date { get; set; }
    public bool IsIndoor { get; set; }
    public int Humidity { get; set; }
    public float MoldRisk { get; set; }

    public void PrintTemp()
    {
        Console.WriteLine($"{Date}\t{Temperature}\t{IsIndoor}\t{Humidity}\t{MoldRisk:P1}");
    }
    public static void PrintMoldRisk()
    {
        //RegExTester.TempToDB();

        using (var db = new WeatherDbContext())
        {
            foreach (var entity in db.TempEntities
                                    .GroupBy(g => g.Date.Date)
                                    .Select(e => new TempEntity
                                    {
                                        Date = e.Key,
                                        MoldRisk = e.Average(g => g.MoldRisk)
                                    })
                                    .OrderByDescending(t => t.MoldRisk))
            {
                //Console.WriteLine($"{entity.Date}: {(entity.IsIndoor ? "Inomhus" : "Utomhus")}, Temp: {entity.Temperature}°C, LF: {entity.Humidity}%");
                //Console.WriteLine($"Mögelrisk: {entity.MoldRisk:F2}, Risknivå: {entity.ClassifyMoldRisk()}");
                Console.WriteLine($"{entity.Date:d}\t" +
                   $"{entity.MoldRisk:F2}%\t" +
                   $"{entity.ClassifyMoldRisk()}");
            }
            //db.SaveChanges();
        }
    }
}
