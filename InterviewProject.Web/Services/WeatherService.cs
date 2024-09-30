using InterviewProject.DTOs;
using InterviewProject.Models;
using InterviewProject.Models.WeatherForecastApi;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace InterviewProject.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private string WeatherApiUrl => _configuration.GetValue<string>("WeatherApi:Url");
        private string WeatherApiToken => _configuration.GetValue<string>("WeatherApi:Token");

        public async Task<WeatherForecastDto> GetWeatherForecastByText(string text)
        {
            WeatherForecastDto result = null;
            HttpResponseMessage jsonResult = await _httpClient.GetAsync(string.Format(WeatherApiUrl, text, WeatherApiToken));
            if (jsonResult.IsSuccessStatusCode)
            {
                var weatherResponse = await jsonResult.Content.ReadFromJsonAsync<WeatherResponse>();
                result = new WeatherForecastDto
                {
                    City = weatherResponse.City?.Name,
                    Country = weatherResponse.City?.Country,
                    Forecasts = weatherResponse.List
                        .Where(w => w.Dt_date.Hour == 00)
                        .Select(w => new WeatherForecast
                        {
                            Date = w.Dt_date,
                            TemperatureC = (int)w.Main?.Temp,
                            Summary = $"{w.Weather.FirstOrDefault()?.Main} - {w.Weather.FirstOrDefault()?.Description}"
                        })
                        .ToList()
                };
            }
            return result;
        }
    }
}
