using Newtonsoft.Json.Linq;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlGetWeatherHandler :
        IQueryBaseHandler<ApiAccessControlGetWeatherArguments, ApiAccessControlGetWeatherResult>
    {
        public class SentiloResponse
        {
            public class SentiloObservation
            {
                public decimal Value { get; set; }
            }

            public List<SentiloObservation> Observations { get; set; }
        }

        #region ExecuteAsync

        public async Task<ResultBase<ApiAccessControlGetWeatherResult>> ExecuteAsync(ApiAccessControlGetWeatherArguments arguments)
        {
            var temperature = ConfigurationManager.AppSettings["Sentilo_Temperature"];
            var humidity = ConfigurationManager.AppSettings["Sentilo_Humidity"];
            var windSpeed = ConfigurationManager.AppSettings["Sentilo_Wind_Speed"];
            var windDirection = ConfigurationManager.AppSettings["Sentilo_Wind_Direction"];
            var pressure = ConfigurationManager.AppSettings["Sentilo_Pressure"];
            var uvIndex = ConfigurationManager.AppSettings["Sentilo_UV_Index"];
            var radiation = ConfigurationManager.AppSettings["Sentilo_Radiation"];

            var temperatureValue = await GetSentiloData(temperature);
            var humidityValue = await GetSentiloData(humidity);
            var windSpeedValue = await GetSentiloData(windSpeed);
            var windDirectionValue = await GetSentiloData(windDirection);
            var pressureValue = await GetSentiloData(pressure);
            var uvIndexValue = await GetSentiloData(uvIndex);
            var radiationValue = await GetSentiloData(radiation);

            var result = new List<ApiAccessControlGetWeatherResult>() {
                new ApiAccessControlGetWeatherResult()
                {
                    Temperature = temperatureValue,
                    Humidity = humidityValue,
                    WindSpeed = windSpeedValue,
                    WindDirection = windDirectionValue,
                    Pressure = pressureValue,
                    UVIndex = uvIndexValue,
                    Radiation = radiationValue,
                    DateTime = DateTime.UtcNow,
                }
            };

            return new ResultBase<ApiAccessControlGetWeatherResult> { Data = result };
        }

        #endregion

        #region GetData

        private async Task<decimal> GetSentiloData(string sensor)
        {
            var host = ConfigurationManager.AppSettings["Sentilo_Host"];
            var provider = ConfigurationManager.AppSettings["Sentilo_Provider"];
            var identityKey = ConfigurationManager.AppSettings["Sentilo_Identity_Key"];

            using (var client = new HttpClient())
            {
                var uri = new Uri($"http://{host}/data/{provider}/{sensor}");

                client.DefaultRequestHeaders.Add("Host", host);
                client.DefaultRequestHeaders.Add("IDENTITY_KEY", identityKey);

                var response = (await client.GetAsync(uri));
                var responseText = (await response.Content.ReadAsStringAsync())
                    .FromJson<SentiloResponse>();

                return responseText.Observations.FirstOrDefault()?.Value ?? 0.0m;
            }
        }

        #endregion
    }
}
