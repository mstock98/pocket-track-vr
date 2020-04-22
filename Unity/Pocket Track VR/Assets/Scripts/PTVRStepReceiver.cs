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

        public PTVRStepReceiver() {
            _stepsSinceLastCollection = 0;
            _hasStepsToCollect = false;
            // new Thread(new ThreadStart(startListeningForSteps)).Start();
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

        // Code based on https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=netframework-4.8
        IEnumerator processTCPRequests() 
        {
            Debug.Log("[PTVR] Setting up TCP server...");

            TcpListener server=null;   
            
            IPAddress address = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(address, _RECEIVER_PORT);

            // Start listening for client requests.
            server.Start();

            Debug.Log("[PTVR] Step receiver listening for data on " + address + ":" + _RECEIVER_PORT);

            // Buffer for reading data
            Byte[] bytes = new Byte[256];

            // Enter the listening loop.
            while(true) 
            {
                Debug.Log("[PTVR] Checking for TCP connection requests");

                while (server.Pending())
                {
                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();            
                    Debug.Log("[PTVR] Connection established to Android app");
 
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    // Loop to receive all the steps sent by the client.
                    while(stream.Read(bytes, 0, 1) != 0) 
                    {   
                        Debug.Log("[PTVR] Received step");

                        _hasStepsToCollect = true;

                        _stepsSinceLastCollection++;     
                    }

                    // Shutdown and end connection
                    client.Close();
                }

                yield return null;
            }
        }
    }
}