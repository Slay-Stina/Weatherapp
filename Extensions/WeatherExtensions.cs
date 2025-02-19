using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Weatherapp.Models;

namespace Weatherapp.Extensions;

internal static class WeatherExtensions
{
    public static string ClassifyMoldRisk(this TempEntity tempEnt)
    {
        if (tempEnt.MoldRisk == 0)
        {
            return "Ingen risk" +
                (tempEnt.Temperature < 5 ? " - För kallt" : "") +
                (tempEnt.Humidity < 80 ? " - För låg LF" : "");
        }
        else if (tempEnt.MoldRisk <= 1)
        {
            return "Låg risk";
        }
        else if (tempEnt.MoldRisk <= 3)
        {
            return "Måttlig risk";
        }
        else
        {
            return "Hög risk";
        }
    }
    public static TempEntity ToTempEntity(this Match match)
    {
        try
        {
            TempEntity newTempEntity = new TempEntity();

            newTempEntity.Date = DateTime.Parse(match.Groups[1].Value);
            newTempEntity.IsIndoor = match.Groups[2].Value.ToLower() == "inne" ? true : false;
            newTempEntity.Temperature = float.Parse(match.Groups[3].Value.Replace('.', ','));
            newTempEntity.Humidity = int.Parse(match.Groups[4].Value);

            return newTempEntity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException);
        }
        return null;
    }
}
