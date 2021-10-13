using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Particle_RoomTemperature.Models;

namespace Particle_RoomTemperature.Services
{
    public static class Particle_Service
    {
        public static ObservableCollection<Measurement> GetRoomMeasurements()
        {
            ObservableCollection<Measurement> measurements = new ObservableCollection<Measurement>();
            string[] tempMeasurements = GetTemperatureMeasurements().Split(';');
            string[] humidMeasurements = GetHumidityMeasurements().Split(';');
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddMinutes(-dateTime.Minute);
            dateTime = dateTime.AddSeconds(-dateTime.Second);
            Measurement lastMeasurement = null;
            for (int i = 0; i < tempMeasurements.Length; i++)
            {
                try { 
                Measurement newMeasurement = new Measurement();
                newMeasurement.Temperature = float.Parse(tempMeasurements[i]);
                newMeasurement.Humidity = float.Parse(humidMeasurements[i]);
                newMeasurement.DateTime = dateTime;
                dateTime = dateTime.AddHours(-1);
                if (lastMeasurement!=null)
                {
                    if (newMeasurement.Humidity > lastMeasurement.Humidity)
                        newMeasurement.HumidityVariation = Variation.HIGHER;
                    if (newMeasurement.Humidity < lastMeasurement.Humidity)
                        newMeasurement.HumidityVariation = Variation.LOWER;

                    if (newMeasurement.Temperature > lastMeasurement.Temperature)
                        newMeasurement.TemperatureVariation = Variation.HIGHER;
                    if (newMeasurement.Temperature < lastMeasurement.Temperature)
                        newMeasurement.TemperatureVariation = Variation.LOWER;
                }
                    lastMeasurement = newMeasurement;
                measurements.Add(newMeasurement);
                }
                catch (Exception) { }
            }
            return measurements;
        }

        static string GetTemperatureMeasurements()
        {
            return "24.0;24.0;23.0;24.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;24.0;24.0;24.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;22.0;22.0;22.0;22.0;22.0;22.0;22.0;23.0;22.0;23.0;23.0;23.0;23.0;23.0;23.0;24.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;22.0;22.0;22.0;22.0;22.0;22.0;22.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;23.0;24.0;24.0;24.0;23.0;24.0;24.0;24.0;25.0;25.0;24.0;24.0;24.0;24.0;24.0;24.0;24.0;24.0;23.0;24.0;24.0;24.0;24.0;";
        }
        static string GetHumidityMeasurements()
        {
            return "42.0;42.0;42.0;42.0;43.0;42.0;42.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;42.0;42.0;41.0;41.0;41.0;43.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;44.0;41.0;41.0;41.0;41.0;41.0;41.0;40.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;41.0;42.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;41.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;42.0;43.0;43.0;43.0;44.0;49.0;47.0;48.0;47.0;48.0;49.0;48.0;52.0;56.0;51.0;51.0;51.0;50.0;50.0;49.0;49.0;49.0;48.0;48.0;48.0;49.0;49.0;49.0;";
        }
    }
}
