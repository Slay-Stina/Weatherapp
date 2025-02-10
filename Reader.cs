using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weatherapp
{
    internal class Reader
    {
        string FilePath = "..\\..\\..\\Data\\tempdata5-med fel.txt";
        string text;

        public Reader()
        {
            text = File.ReadAllText(FilePath);
        }

        public void ReadFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
