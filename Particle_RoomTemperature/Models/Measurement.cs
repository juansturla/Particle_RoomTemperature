using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_RoomTemperature.Models
{
    public class Measurement
    {
        public float Temperature { get; set;  }
        public float Humidity {  get; set; }
        public DateTime DateTime {  get; set; }
        public Variation TemperatureVariation {  get; set; }
        public Variation HumidityVariation {  get; set; }
        public override string ToString()
        {
            return Temperature.ToString() + "-" + Humidity.ToString() ;
        }
    }
}
