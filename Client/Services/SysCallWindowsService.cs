using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Client.Tools;

namespace Client.Services
{
    public class SysCallWindowsService
    {
        private const int AF_INET = 2;
        private const int SOCK_STREAM = 1;
        private const int IPPROTO_TCP = 6;

        // Inicialização do WinSock
        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int WSAStartup(ushort version, ref WSAData wsaData);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int WSACleanup();

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int socket(int af, int type, int protocol);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int connect(int sockfd, ref SockAddrIn addr, int addrlen);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int send(int sockfd, byte[] buf, int len, int flags);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int recv(int sockfd, byte[] buf, int len, int flags);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int closesocket(int sockfd);

        [StructLayout(LayoutKind.Sequential)]
        private struct SockAddrIn
        {
            public short sinFamily;
            public ushort sinPort;
            public uint sinAddr;
            public ulong sinZero;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WSAData
        {
            public ushort wVersion;
            public ushort wHighVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)]
            public byte[] szDescription;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
            public byte[] szSystemStatus;
            public ushort iMaxSockets;
            public ushort iMaxUdpDg;
            public IntPtr lpVendorInfo;
        }

        public void Request(string host, int port, string path)
        {
            try
            {
                // Inicializando o WinSock
                WSAData wsaData = new WSAData();
                int wsInitResult = WSAStartup(0x0202, ref wsaData); // Versão 2.2
                if (wsInitResult != 0)
                {
                    Console.WriteLine("WSAStartup failed!");
                    return;
                }

                // Resolve o IP
                var ip = IPAddress.Parse(host);
                var ipBinary = BitConverter.ToUInt32(ip.GetAddressBytes(), 0);

                var serverAddress = new SockAddrIn
                {
                    sinFamily = AF_INET,
                    sinPort = (ushort)IPAddress.HostToNetworkOrder((short)port),
                    sinAddr = ipBinary,
                    sinZero = 0
                };

                // Criando o socket
                var sockfd = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
                if (sockfd == -1)
                {
                    Console.WriteLine($"Error while creating socket:");
                    Console.WriteLine(Marshal.GetLastWin32Error());
                    WSACleanup();
                    return;
                }

                // Conectando ao servidor
                int connStatus = connect(sockfd, ref serverAddress, Marshal.SizeOf(typeof(SockAddrIn)));
                if (connStatus == -1)
                {
                    Console.WriteLine($"Error while connecting to the server:");
                    Console.WriteLine(Marshal.GetLastWin32Error());
                    closesocket(sockfd);
                    WSACleanup();
                    return;
                }

                // Enviando o request
                var request = $"GET /{path} HTTP/1.1\r\n" +
                              $"Host: {host}:{port}\r\n" +
                              "Accept: application/json\r\n" +
                              "Connection: close\r\n\r\n";
                var requestBytes = Encoding.ASCII.GetBytes(request);
                int sendStatus = send(sockfd, requestBytes, requestBytes.Length, 0);
                if (sendStatus == -1)
                {
                    Console.WriteLine("Error while sending request:");
                    Console.WriteLine(Marshal.GetLastWin32Error());
                    closesocket(sockfd);
                    WSACleanup();
                    return;
                }

                // Recebendo a resposta
                var responseBuffer = new byte[4096];
                int bytesRead = recv(sockfd, responseBuffer, responseBuffer.Length, 0);
                if (bytesRead <= 0)
                {
                    Console.WriteLine("Error while receiving the response");
                }
                else
                {
                    var responseContent = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
                    var json = JsonUtils.ExtractJsonFromResponse(responseContent);

                    Console.WriteLine("Response:");
                    Console.WriteLine(json);
                }

                // Fechando o socket e limpando o WinSock
                closesocket(sockfd);
                WSACleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}