namespace Client.Services;

public class HttpClientFactoryService
{
    private readonly HttpClient _httpClient;

    public HttpClientFactoryService(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task GetWeatherForecastAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("weatherforecast");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response:");
                Console.WriteLine(responseContent);
            }
            else
            {
                Console.WriteLine("Error while making the request:");
                Console.WriteLine(response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception:");
            Console.WriteLine(ex.Message);
        }
    }
}