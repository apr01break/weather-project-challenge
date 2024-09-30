using System.Collections.Generic;
using System;

namespace InterviewProject.Models.WeatherForecastApi
{
    public class WeatherItem
    {
        public WeatherMainInformation Main { get; set; }
        public List<WeatherDescription> Weather { get; set; }
        public string Dt_txt { get; set; }
        public DateTime Dt_date
        {
            get
            {
                DateTime.TryParse(Dt_txt, out DateTime date);
                return date;
            }
        }
    }
}
