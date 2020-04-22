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
            // new Thread(new ThreadStart(startListeningForSteps)).Start();
            initializeTCPServer();
        }

        public int getNumberOfStepsSinceLastCall() {
            // Debug.Log("[PTVR] getNumberOfSteps called");
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
            
            IPAddress address = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            // TcpListener server = new TcpListener(port);
            _server = new TcpListener(address, _RECEIVER_PORT);

            // Start listening for client requests.
            _server.Start();

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
                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = _server.AcceptTcpClient();   

                Debug.Log("[PTVR] Connection established to Android app");

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                // Loop to receive all the steps sent by the client.
                while(stream.Read(_receiverBuffer, 0, 1) != 0) 
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