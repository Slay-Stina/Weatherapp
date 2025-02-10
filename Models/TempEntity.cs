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



        

    }
}
