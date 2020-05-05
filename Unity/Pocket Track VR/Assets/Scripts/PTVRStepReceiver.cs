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
        private IPAddress _address;
        private int _receiverPort;
        private TcpListener _server;
        private Byte[] _receiverBuffer;

        public PTVRStepReceiver(int port) {
            _receiverPort = port;
            _stepsSinceLastCollection = 0;
            _hasStepsToCollect = false;
            InitializeTCPServer();
        }

        public int GetNumberOfStepsSinceLastCall() {
            ProcessTCPRequests();
            int stepsToReturn = _stepsSinceLastCollection;
            _stepsSinceLastCollection = 0;
            _hasStepsToCollect = false;
            return stepsToReturn;
        }

        public bool HasStepsToCollect() {
            return _hasStepsToCollect;
        }

        public string GetIPAddressAndPort() {
            return $"{_address}:{_receiverPort}";
        }

        private void InitializeTCPServer() {
            var hostName = Dns.GetHostName();
            
            // Find an IPv4 address to listen for steps
            _address = null;
            foreach (var addressEntry in Dns.GetHostEntry(hostName).AddressList) {
                if (addressEntry.AddressFamily == AddressFamily.InterNetwork) {
                    _address = addressEntry;
                    break;
                }
            }
            
            if (_address == null) {
                throw new Exception("[PTVR] Step receiver could not find an IPv4 address in the DNS host entry address list");
            }
            
            // Start TcpListener
            _server = new TcpListener(_address, _receiverPort);
            _server.Start();

            // Buffer for reading data
            _receiverBuffer = new Byte[256];
            
            Debug.Log("[PTVR] Step receiver listening for steps on " + _address + ":" + _receiverPort + ". Enter this address on the mobile app.");
        }

        // Code based on https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=netframework-4.8
        private void ProcessTCPRequests() {
            while (_server.Pending()) {
                // This call should be non-blocking because of _server.Pending() being true
                TcpClient client = _server.AcceptTcpClient();   

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                // If there's any data waiting to be read for the TCP connection
                if (stream.Read(_receiverBuffer, 0, 256) != 0) {   
                    _hasStepsToCollect = true;
                    _stepsSinceLastCollection++;     
                }

                // Shutdown and end connection
                client.Close();
            }
        }
    }
}