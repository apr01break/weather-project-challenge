using System.Collections.Generic;
using InterviewProject.Models;

namespace InterviewProject.DTOs
{
    public class WeatherForecastDto
    {
        public string City { get; set; }
        public string Country { get; set; }
        public IList<WeatherForecast> Forecasts { get; set; }
    }
}
