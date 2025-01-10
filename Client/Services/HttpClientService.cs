namespace Client.Services;

public class HttpClientService
{
    public async Task Request(string host, int port, string path)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("accept", "application/json");

        try
        {
            var response = await client.GetAsync($"{host}:{port}/{path}");
    
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