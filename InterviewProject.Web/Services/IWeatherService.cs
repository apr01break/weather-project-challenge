using InterviewProject.DTOs;
using System.Threading.Tasks;

namespace InterviewProject.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecastDto> GetWeatherForecastByText(string text);
    }
}
