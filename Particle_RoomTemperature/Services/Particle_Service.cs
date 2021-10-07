using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Particle_RoomTemperature.Models;

namespace Particle_RoomTemperature.Services
{
    public static class Particle_Service
    {
        public static IEnumerable<Measurement> GetRoomMeasurements()
        {
            ObservableCollection<Measurement> measurements = new ObservableCollection<Measurement>();
            string[] tempMeasurements = GetTemperatureMeasurements().Split(';');
            string[] humidMeasurements = GetHumidityMeasurements().Split(';');
            DateTime dateTime = new DateTime();
            dateTime = dateTime.AddMinutes(dateTime.Minute);
            dateTime = dateTime.AddSeconds(dateTime.Second);

            for (int i = 0; i < tempMeasurements.Length; i++)
            {
                Measurement newMeasurement = new Measurement();
                newMeasurement.Temperature = float.Parse(tempMeasurements[i]);
                newMeasurement.Humidity = float.Parse(humidMeasurements[i]);
                newMeasurement.DateTime = dateTime;
                dateTime = dateTime.AddHours(-1);
                if (i > 0)
                {
                    if (newMeasurement.Humidity > measurements[i - 1].Humidity)
                        newMeasurement.HumidityVariation = Variation.HIGHER;
                    if (newMeasurement.Humidity < measurements[i - 1].Humidity)
                        newMeasurement.HumidityVariation = Variation.LOWER;

                    if (newMeasurement.Temperature > measurements[i - 1].Temperature)
                        newMeasurement.HumidityVariation = Variation.HIGHER;
                    if (newMeasurement.Temperature < measurements[i - 1].Temperature)
                        newMeasurement.TemperatureVariation = Variation.LOWER;
                }
                measurements.Add(newMeasurement);
            }
            return measurements;
        }

        static string GetTemperatureMeasurements()
        {
#if DEBUG
            return "20.0;20.0;21.0;25.0;16.0;16.0;16.0";
#endif
            return Task.Run(() => GetParticleVariable("last_temps")).Result;
        }
        static string GetHumidityMeasurements()
        {
#if DEBUG
            return "20.0;20.0;21.0;25.0;16.0;16.0;16.0";
#endif
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
