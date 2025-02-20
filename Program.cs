namespace Weatherapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
 Anton
            //Reader reader = new Reader();
            //reader.ReadFile();

            //Weather.FindWinter("..\\..\\..\\Data\\tempdata5-med fel.txt");

            //RegExTester.RegExTest();

            string filePath = "..\\..\\..\\Data\\tempdata5-med fel.txt";
            WeatherDataAnalyzer analyzer = new WeatherDataAnalyzer(filePath);

            Console.Write("Ange ett datum (YYYY-MM-DD) för att få medeltemperatur och luftfuktighet: ");
            string date = Console.ReadLine();
            analyzer.GetDailyAverageWeather(date);

            Console.Write("Ange ett datum (YYYY-MM-DD) för att få endast medeltemperatur: ");
            string tempDate = Console.ReadLine();
            analyzer.GetDailyAverageTemperature(tempDate);

            analyzer.SortDaysByHumidity();
=======
            RegExTester.TempToDB();
            TempEntity.PrintMoldRisk();
            TemperatureSorter.ShowSortedTemps(true);
 master
        }

        
    }
}
