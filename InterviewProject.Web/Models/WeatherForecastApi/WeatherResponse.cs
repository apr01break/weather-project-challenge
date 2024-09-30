using System.Collections.Generic;

namespace InterviewProject.Models.WeatherForecastApi
{
    public class WeatherResponse
    {
        public string Cod { get; set; }
        public object Message { get; set; }
        public int Cnt { get; set; }
        public List<WeatherItem> List { get; set; }
        public WeatherCityInformation City { get; set; }
    }
}
