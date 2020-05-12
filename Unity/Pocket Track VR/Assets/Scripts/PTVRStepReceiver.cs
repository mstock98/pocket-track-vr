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
    /**
        PTVR class for receiving and counting steps from the step recording app.
        After the constructor is called, call GetNumberOfStepsSinceLastCall() to
        monitor the number of steps received from the recording app.
    */
    public class PTVRStepReceiver
    {
        private int _stepsSinceLastCollection;
        private bool _hasStepsToCollect;
        private IPAddress _address;
        private int _receiverPort;
        private TcpListener _server;
        private Byte[] _receiverBuffer;

        /**
            Constructor for the PTVRStepReceiver class.
            Sets the provided port and initializes the TCP listener.

            :param port: The port that the TCP listener will listen for incoming connections
        */
        public PTVRStepReceiver(int port) {
            _receiverPort = port;
            _stepsSinceLastCollection = 0;
            _hasStepsToCollect = false;
            InitializeTCPServer();
        }

        /**
            Gets the number of steps sent to the step receiver since this
            method was called last.

            :return: The number of steps recorded since this method was called.
        */
        public int GetNumberOfStepsSinceLastCall() {
            ProcessTCPRequests();
            int stepsToReturn = _stepsSinceLastCollection;
            _stepsSinceLastCollection = 0;
            _hasStepsToCollect = false;
            return stepsToReturn;
        }

        /**
            Determines if the step receiver has steps that haven't been
            returned in a GetNumberOfStepsSinceLastCall() method call.

            :return: false if GetNumberOfStepsSinceLastCall() will return 0,
                     true otherwise
        */
        public bool HasStepsToCollect() {
            return _hasStepsToCollect;
        }

        /**
            Gets the IPv4 address and port that the step receiver is listening
            for steps on, formatted in a string

            :return: IPv4 address and port formatted as "#.#.#.#:<port number>"
        */
        public string GetIPAddressAndPort() {
            return $"{_address}:{_receiverPort}";
        }

        /**
            Sets up and starts a TCP server/listener that listens for connections on
            the step receiver's port and a IPv4 address
        */
        private void InitializeTCPServer() {
            var hostName = Dns.GetHostName();
            
            // Find an IPv4 address to listen for incoming connections
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

        /**
            Processes all pending TCP requests from the TCP server/listener.
            For every pending TCP request, the value of _stepsSinceLastCollection will be incremented by 1.
        */
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