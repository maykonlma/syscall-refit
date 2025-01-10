using System.Net;
using System.Net.Sockets;
using System.Text;
using Client.Tools;

namespace Client.Services;

public class SocketService
{
    public void Request(string host, int port, string path)
    {
        try
        {
            var ipAddress = IPAddress.Parse(host);
            
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(ipAddress, port));

            var request = $"GET /{path} HTTP/1.1\r\n" +
                          $"Host: {host}:{port}\r\n" +
                          "Accept: application/json\r\n" +
                          "Connection: close\r\n\r\n";
            
            var requestBytes = Encoding.ASCII.GetBytes(request);
            socket.Send(requestBytes);
            
            var responseBuffer = new byte[4096];
            int bytesRead;
            var responseContent = new StringBuilder();
            
            do
            {
                bytesRead = socket.Receive(responseBuffer);
                if (bytesRead > 0)
                {
                    responseContent.Append(Encoding.ASCII.GetString(responseBuffer, 0, bytesRead));
                }
            } while (bytesRead > 0);
            
            var json = JsonUtils.ExtractJsonFromResponse(responseContent.ToString());
            
            Console.WriteLine("Response:");
            Console.WriteLine(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception:");
            Console.WriteLine(ex.Message);
        }
    }
}