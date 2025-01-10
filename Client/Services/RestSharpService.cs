using RestSharp;

namespace Client.Services;

public class RestSharpService
{
    public async Task Request(string host, int port, string path)
    {
        try
        {
            var response = await new RestClient($"{host}:{port}")
                .ExecuteAsync(new RestRequest(path, Method.Get));

            if (response.IsSuccessful)
            {
                Console.WriteLine("Response:");
                Console.WriteLine(response.Content);
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