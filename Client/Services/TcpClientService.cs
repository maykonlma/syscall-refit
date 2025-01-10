using System.Net.Sockets;
using System.Text;
using Client.Tools;

namespace Client.Services
{
    public class TcpClientService
    {
        public void Request(string host, int port, string path)
        {
            try
            {
                using TcpClient tcpClient = new TcpClient(host, port);
                
                var request = $"GET /{path} HTTP/1.1\r\n" +
                              $"Host: {host}:{port}\r\n" +
                              "Accept: application/json\r\n" +
                              "Connection: close\r\n\r\n";
                
                using NetworkStream networkStream = tcpClient.GetStream();
                var requestBytes = Encoding.ASCII.GetBytes(request);
                networkStream.Write(requestBytes, 0, requestBytes.Length);

                var responseBuffer = new byte[4096];
                var responseContent = new StringBuilder();
                int bytesRead;

                while ((bytesRead = networkStream.Read(responseBuffer, 0, responseBuffer.Length)) > 0)
                {
                    responseContent.Append(Encoding.ASCII.GetString(responseBuffer, 0, bytesRead));
                }
                
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
}
