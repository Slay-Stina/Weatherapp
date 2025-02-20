namespace Weatherapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RegExTester.TempToDB();
            TempEntity.PrintMoldRisk();
            TemperatureSorter.ShowSortedTemps(true);
        }

        
    }
}
