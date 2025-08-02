using System.Net.Http.Json;

namespace FrontEndApp.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherForecast[]?> GetWeatherForecastAsync()
    {
        try
        {
            Console.WriteLine($"正在调用 API: {_httpClient.BaseAddress}weatherforecast");
            var response = await _httpClient.GetFromJsonAsync<WeatherForecast[]>("weatherforecast");
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

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
