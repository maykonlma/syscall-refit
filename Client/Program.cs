using System.Runtime.InteropServices;
using Client.Services;
using ConsoleTools;
using Microsoft.Extensions.DependencyInjection;

const string HOST = "127.0.0.1";
const int PORT = 5000;
const string ROUTE = "weatherforecast";

var serviceCollection = new ServiceCollection();
serviceCollection.AddHttpClient<HttpClientFactoryService>(client =>
{
    client.BaseAddress = new Uri($"http://{HOST}:{PORT}");
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var menu = new ConsoleMenu(args, level: 1)
    .Add("SysCall for Linux", () =>
    {
        Console.WriteLine("You chose SysCall for Linux.");
        
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Console.WriteLine("This function is only allowed on Linux.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            
            return;    
        }
        
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var client = new SysCallLinuxService();
        client.Request(host: HOST, port: PORT, path: ROUTE);
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("SysCall for Windows", () =>
    {
        Console.WriteLine("You chose SysCall for Windows.");
        
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Console.WriteLine("This function is only allowed on Windows.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            
            return;    
        }
        
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");        
        
        var client = new SysCallWindowsService();
        client.Request(host: HOST, port: PORT, path: ROUTE);       

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("Socket", () =>
    {
        Console.WriteLine("You chose Socket.");
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var client = new SocketService();
        client.Request(host: HOST, port: PORT, path: ROUTE);
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("TCPClient", () =>
    {
        Console.WriteLine("You chose TCPClient.");
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var client = new TcpClientService();
        client.Request(host: HOST, port: PORT, path: ROUTE);
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("HTTPClient", () =>
    {
        Console.WriteLine("You chose HTTPClient.");
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var client = new HttpClientService();
        client.Request(host: $"http://{HOST}", port: 5000, path: "weatherforecast").Wait();
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("HTTPClientStatic", () =>
    {
        Console.WriteLine("You chose HTTPClientStatic.");
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var client = new HttpClientStaticService();
        client.Request(host: $"http://{HOST}", port: 5000, path: "weatherforecast").Wait();
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("HTTPClientFactory", () =>
    {
        Console.WriteLine("You chose HTTPClientFactory.");
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var httpClientService = serviceProvider.GetRequiredService<HttpClientFactoryService>(); 
        httpClientService.GetWeatherForecastAsync().Wait();
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("RestSharp", () =>
    {
        Console.WriteLine("You chose RestSharp.");
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var client = new RestSharpService();
        client.Request(host: $"http://{HOST}", port: 5000, path: "weatherforecast").Wait();
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("Refit", () =>
    {
        Console.WriteLine("You chose Refit.");
        Console.WriteLine($"Configuration: https://{HOST}:{PORT}/{ROUTE}");
        
        var client = new RefitService();
        client.Request(host: $"http://{HOST}", port: 5000).Wait();
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    })
    .Add("Exit", ConsoleMenu.Close)
    .Configure(config =>
    {
        config.Title = "Main Menu";
        config.EnableWriteTitle = true;
    });

menu.Show();