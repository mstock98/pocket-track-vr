using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class PTVRMovePlayerForwardOnStepInPlace : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[PTVR] Step in place script starting...");
        
        // Establish socket with Android app
        // https://docs.microsoft.com/en-us/dotnet/framework/network-programming/listening-with-sockets
        var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = ipHostInfo.AddressList[0];
        var localEndPoint = new IPEndPoint(ipAddress, 1422); 

        var listener = new Socket(ipAddress.AddressFamily,
                                  SocketType.Stream, 
                                  ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        Debug.Log("[PTVR] Listening for socket connections on " + localEndPoint.Address + ":" + localEndPoint.Port + "...");  
        listener.Listen(100);
        
        var receiveBuffer = new byte[64];
        while (true) 
        {
            var connectionSocket = listener.Accept();
            Debug.Log("[PTVR] Socket connection established");

            while (true)
            {
                connectionSocket.Receive(receiveBuffer);
                string data = Encoding.UTF8.GetString(receiveBuffer, 0, receiveBuffer.Length);

                if (data == "exit")
                {
                    break;
                }

                // Move player here
                Debug.Log("[PTVR] Step received: " + data);
            }
        }
    }
}