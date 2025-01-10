using Refit;

namespace Client.Services;

public interface IMyApi
{
    [Get("/weatherforecast")]
    Task<string> GetWeatherForecastAsync();
}

public class RefitService
{
    public async Task Request(string host, int port)
    {
        var api = RestService.For<IMyApi>($"{host}:{port}");

        try
        {
            var responseContent = await api.GetWeatherForecastAsync();
            
            Console.WriteLine("Response:");
            Console.WriteLine(responseContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception:");
            Console.WriteLine(ex.Message);
        }
    }
}