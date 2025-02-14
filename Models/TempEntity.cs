using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weatherapp.Models
{
    internal class TempEntity
    {
        public int Id { get; set; }
        public float Temperature { get; set; }

        public DateTime Date { get; set; }

        public bool IsIndoor { get; set; }

        public int Humidity { get; set; }
        public float MoldRisk { get; set; }

        public static void PrintMoldRisk()
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

        public float CalculateMoldRisk()
        {
            if (Humidity < 80 || Temperature <= 5)
            {
                return 0; // Ingen mögelrisk om luftfuktighet < 80% eller T <= 5°C
            }

            // Definiera referensvärdena för min- och maxrisk
            float minRiskTemp = 50.0f;
            float maxRiskTemp = 5.0f;
            float minRiskHumidity = 80.0f;
            float maxRiskHumidity = 100.0f;

            // Normalisera temperatur och luftfuktighet till intervallet [0, 1]
            float normalizedTemp = (Temperature - minRiskTemp) / (maxRiskTemp - minRiskTemp);
            float normalizedHumidity = (Humidity - minRiskHumidity) / (maxRiskHumidity - minRiskHumidity);

            float riskIndex = normalizedTemp * normalizedHumidity * 100.0f;

            return riskIndex;
        }

        public string ClassifyMoldRisk(float moldRisk)
        {
            if (moldRisk == 0)
            {
                return "Ingen risk" + 
                    (Temperature < 5 ? " - För kallt" : "") +
                    (Humidity < 80 ? " - För låg LF": "");
            }
            else if (moldRisk <= 20)
            {
                return "Låg risk";
            }
            else if (moldRisk <= 50)
            {
                return "Måttlig risk";
            }
            else
            {
                return "Hög risk";
            }
        }
    }
}
