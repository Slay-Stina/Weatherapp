using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weatherapp.Models;

internal class MyDelegates
{
    public static float CalculateMoldRisk(TempEntity tempEntity)
    {
        if (tempEntity.Humidity < 80 || tempEntity.Temperature <= 5)
        {
            return 0; // Ingen mögelrisk om luftfuktighet < 80% eller T <= 5°C
        }

        // Definiera referensvärdena för min- och maxrisk
        float minRiskTemp = 50.0f;
        float maxRiskTemp = 5.0f;
        float minRiskHumidity = 80.0f;
        float maxRiskHumidity = 100.0f;

        // Normalisera temperatur och luftfuktighet till intervallet [0, 1]
        float normalizedTemp = (tempEntity.Temperature - minRiskTemp) / (maxRiskTemp - minRiskTemp);
        float normalizedHumidity = (tempEntity.Humidity - minRiskHumidity) / (maxRiskHumidity - minRiskHumidity);

        float riskIndex = normalizedTemp * normalizedHumidity * 100.0f;

        return riskIndex;
    }
}
