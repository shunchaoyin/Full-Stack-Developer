using System.Net.Http.Json;
using FrontEndApp.Models;

namespace FrontEndApp.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherForecast[]?> GetWeatherForecastAsync(CancellationToken token = default)
    {
        try
        {
            Console.WriteLine($"正在调用 API: {_httpClient.BaseAddress}weatherforecast");
            var response = await _httpClient.GetFromJsonAsync<WeatherForecast[]>(
                "weatherforecast",
                token
            );
            Console.WriteLine($"成功获取到 {response?.Length ?? 0} 条天气数据");
            return response;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP 请求错误: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"获取天气数据时发生错误: {ex.Message}");
            Console.WriteLine($"错误类型: {ex.GetType().Name}");
            return null;
        }
    }
}

public class WeatherState
{
    public WeatherForecast[]? Weather { get; private set; }
    public event Action? OnChange;

    public void UpdateWeather(WeatherForecast[] newWeather)
    {
        Weather = newWeather;
        NotifyStateChanged();
    }
    private void NotifyStateChanged() => OnChange?.Invoke();
}
