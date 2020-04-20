using UnityEngine;
using System.Net.Sockets;

public class MovePlayerForwardOnStepInPlace : MonoBehaviour
{
    void Start()
    {
        // Establish socket with Android app
        // https://docs.microsoft.com/en-us/dotnet/framework/network-programming/listening-with-sockets
        var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = ipHostInfo.AddressList[0];
        var localEndPoint = new IPEndPoint(ipAddress, 1422); 

        var listener = new Socket(ipAddress.AddressFamily,
                                  SocketType.Stream, 
                                  ProtocolType.Tcp);
        listener.Bind(localEndPoint);  
        listener.Listen(100);
        
        var receiveBuffer = new Byte[64];
        while (true) 
        {
            connectionSocket = listener.Accept();

            while (true)
            {
                connectionSocket.Receive(receiveBuffer);
                string data = Encoding.UTF8.GetString(receiveBuffer, 0, receiveBuffer.Length);

                if (data == "exit")
                {
                    break;
                }

                // Move player here
            }
        }
    }
}