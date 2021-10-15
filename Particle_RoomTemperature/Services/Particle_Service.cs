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
            return Task.Run(() => GetParticleVariable("last_temps")).Result;
        }
        static string GetHumidityMeasurements()
        {
            return Task.Run(() => GetParticleVariable("last_humids")).Result;
        }
        static async Task<string> GetParticleVariable(string particleVariable)
        {
            string accessToken = "2c22a76b05aad4212fb61ccef05f41b60c211911"; //This is your Particle Cloud Access Token
            string deviceId = "32004f001051373331333230"; //This is your Particle Device Id

            HttpClient client = new HttpClient
            {
                BaseAddress =
                new Uri("https://api.particle.io/")
            };

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", accessToken),
            });

            var result = await client.GetStringAsync("v1/devices/" + deviceId + "/" + particleVariable + "?access_token=" + accessToken);
            var particleResponse = System.Text.Json.JsonSerializer.Deserialize<ParticleResponse>(result);
            return particleResponse.result;
        }
    }

    public class CoreInfo
    {
        public DateTime last_heard { get; set; }
        public bool connected { get; set; }
        public DateTime last_handshake_at { get; set; }
        public string deviceID { get; set; }
        public int product_id { get; set; }
    }

    public class ParticleResponse
    {
        public string cmd { get; set; }
        public string name { get; set; }
        public string result { get; set; }
        public CoreInfo coreInfo { get; set; }
    }
}
