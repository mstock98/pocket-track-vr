using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace PTVR
{
    public class PTVRStepReceiver
    {
        private int _stepsSinceLastCollection;
        private bool _hasStepsToCollect;
        private const int _RECEIVER_PORT = 1422;
        private TcpListener _server;
        private Byte[] _receiverBuffer;

        public PTVRStepReceiver() {
            _stepsSinceLastCollection = 0;
            _hasStepsToCollect = false;
            initializeTCPServer();
        }

        public int getNumberOfStepsSinceLastCall() {
            processTCPRequests();
            int stepsToReturn = _stepsSinceLastCollection;
            _stepsSinceLastCollection = 0;
            _hasStepsToCollect = false;
            return stepsToReturn;
        }

        public bool hasStepsToCollect() {
            return _hasStepsToCollect;
        }

        private void initializeTCPServer() {
            Debug.Log("[PTVR] Setting up TCP server...");
            
            var hostName = Dns.GetHostName();
            Debug.Log("[PTVR] Host name: " + hostName);
            
            IPAddress address = Dns.GetHostEntry(hostName).AddressList[1];

            // TcpListener server = new TcpListener(port);
            Debug.Log("[PTVR] Creating TcpListener...");
            _server = new TcpListener(address, _RECEIVER_PORT);

            // Start listening for client requests.
            Debug.Log("[PTVR] Starting TcpListener...");
            _server.Start();

            // address = address.MapToIPv4();
            
            Debug.Log("[PTVR] Step receiver listening for data on " + address + ":" + _RECEIVER_PORT);

            // Buffer for reading data
            _receiverBuffer = new Byte[256];
        }

        // Code based on https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=netframework-4.8
        private void processTCPRequests() 
        {
            // Debug.Log("[PTVR] Checking for TCP connection requests");

            while (_server.Pending())
            {
                Debug.Log("[PTVR] Connection pending...");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = _server.AcceptTcpClient();   

                Debug.Log("[PTVR] Connection established to Android app");

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                if (stream.Read(_receiverBuffer, 0, 256) != 0) 
                {   
                    Debug.Log("[PTVR] Received step");
                    _hasStepsToCollect = true;
                    _stepsSinceLastCollection++;     
                }

                // Shutdown and end connection
                client.Close();
            }
        }
    }
}