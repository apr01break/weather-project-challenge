using Moq.Protected;
using Moq;
using System.Net;
using InterviewProject.DTOs;
using InterviewProject.Models.WeatherForecastApi;
using System.Text.Json;
using InterviewProject.Services;
using Microsoft.Extensions.Configuration;

namespace InterviewProject.Tests
{
    public class WeatherServiceTest
    {
        private IWeatherService _weatherService;
        private IConfiguration _configuration;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var inMemoryConfiguration = new Dictionary<string, string> {
                {"WeatherApi:Url", "https://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&appid={1}"},
                {"WeatherApi:Token", "735ec7b23b13524a667bb3ccce51c8a2"}
            };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfiguration).Build();
            _weatherService = new WeatherService(_configuration, new HttpClient(_httpMessageHandlerMock.Object));
        }

        [Test]
        public async Task GetWeatherForecastByText_SuccessfullResponse_ReturnsCorrectData()
        {
            var text = "Huacho";
            var mockWeatherResponse = new WeatherResponse
            {
                City = new WeatherCityInformation { Name = "Huacho", Country = "PE" },
                List = new List<WeatherItem>
                {
                    new WeatherItem
                    {
                        Dt_txt = "2024-10-03 00:00:00",
                        Main = new WeatherMainInformation { Temp = 14.0f },
                        Weather = new List<WeatherDescription>
                        {
                            new WeatherDescription { Main = "Cloudy", Description = "Overcast clouds" }
                        }
                    }
                }
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(mockWeatherResponse))
                });

            WeatherForecastDto result = await _weatherService.GetWeatherForecastByText(text);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.City, Is.EqualTo("Huacho"));
            Assert.That(result.Country, Is.EqualTo("PE"));
            Assert.That(result.Forecasts, Has.Count.EqualTo(1));
            Assert.That(result.Forecasts[0].Date, Is.EqualTo(new DateTime(2024, 10, 03, 00, 00, 00)));
            Assert.That(result.Forecasts[0].TemperatureC, Is.EqualTo(14));
            Assert.That(result.Forecasts[0].Summary, Is.EqualTo("Cloudy - Overcast clouds"));
        }

        [Test]
        public async Task GetWeatherForecastByText_FailureResponse_ReturnsEmptyData()
        {
            var text = "Abcdef";
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            WeatherForecastDto result = await _weatherService.GetWeatherForecastByText(text);

            Assert.That(result, Is.Null);
        }
    }
}