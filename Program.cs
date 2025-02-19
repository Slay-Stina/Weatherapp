using Weatherapp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Weatherapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RegExTester.TempToDB();
            TempEntity.PrintMoldRisk();
        }
    }
}
