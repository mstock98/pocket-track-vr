import socket
from socket import *
import os, sys

if len(sys.argv) > 1:
    try:
        serverPort = int(sys.argv[1])
    except:
        print("Error: " + sys.argv[1] + " is not a valid port number")
        exit()
    if serverPort < 0 or serverPort > 65535:
        print("Error: " + str(serverPort) + " is not a valid port number")
        exit()
else:
    serverPort = 1422
serverSocket = socket(AF_INET, SOCK_STREAM)
serverSocket.bind(('', serverPort))
serverSocket.listen(1)
hostName = gethostname()
ipAddr = gethostbyname(hostName)
print("Server ready to receive on " + str(ipAddr) + ":" + str(serverPort) + " ...")
while True:
    connectionSocket, addr = serverSocket.accept()
    print("Connection established: ", addr)
    while True:
        command = connectionSocket.recv(2048).decode()
        print("Command recieved: ", command)