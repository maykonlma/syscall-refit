using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Client.Tools;

namespace Client.Services;

public class SysCallLinuxService
{
    private const int AF_INET = 2;
    private const int SOCK_STREAM = 1;
    private const int IPPROTO_TCP = 6;

    [DllImport("libc", SetLastError = true)]
    private static extern int socket(int domain, int type, int protocol);

    [DllImport("libc", SetLastError = true)]
    private static extern int connect(int sockfd, ref SockAddrIn addr, int addrlen);

    [DllImport("libc", SetLastError = true)]
    private static extern int send(int sockfd, byte[] buffer, int length, int flags);

    [DllImport("libc", SetLastError = true)]
    private static extern int recv(int sockfd, byte[] buffer, int length, int flags);

    [DllImport("libc", SetLastError = true)]
    private static extern int close(int sockfd);

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr strerror(int errnum);

    [StructLayout(LayoutKind.Sequential)]
    private struct SockAddrIn
    {
        public short sinFamily;
        public ushort sinPort;
        public uint sinAddr;
        public ulong sinZero;
    }

    public void Request(string host, int port, string path)
    {
        try
        {
            var ip = IPAddress.Parse(host);
            var ipBinary = BitConverter.ToUInt32(ip.GetAddressBytes(), 0);
            
            var serverAddress = new SockAddrIn
            {
                sinFamily = AF_INET,
                sinPort = (ushort)IPAddress.HostToNetworkOrder((short)port),
                sinAddr = ipBinary,
                sinZero = 0
            };
            
            var sockfd = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
            if (sockfd < 0)
            {
                Console.WriteLine($"Error while creating socket.");
                
                return;
            }
            
            var connStatus = connect(sockfd, ref serverAddress, Marshal.SizeOf(typeof(SockAddrIn)));
            if (connStatus < 0)
            {
                Console.WriteLine($"Error while connecting to the server.");
                
                close(sockfd);
                return;
            }
            
            var request = $"GET /{path} HTTP/1.1\r\n" + $"Host: {host}:{port}\r\n" +
                          "Accept: application/json\r\n" + "Connection: close\r\n\r\n";
            
            var requestBytes = Encoding.ASCII.GetBytes(request);
            send(sockfd, requestBytes, requestBytes.Length, 0);
            
            var responseBuffer = new byte[4096];
            var bytesRead = recv(sockfd, responseBuffer, responseBuffer.Length, 0);

            if (bytesRead <= 0)
            {
                Console.WriteLine("Error while receiving the response");
            }
            else
            {
                var responseContent = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
                var json = JsonUtils.ExtractJsonFromResponse(responseContent.ToString());
            
                Console.WriteLine("Response:");
                Console.WriteLine(json);
            }

            close(sockfd);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception:");
            Console.WriteLine(ex.Message);
        }
    }
}