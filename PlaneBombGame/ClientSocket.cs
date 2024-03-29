﻿using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;

namespace PlaneBombGame
{
    internal class ClientSocket
    {
        static Socket clientSocket;

        private static ClientSocket socket = null;

        public bool isConnected = false;

        private IPAddress ip = IPAddress.Parse("127.0.0.1");

        private int port = 8885;

        public string receiveStr = "";

        public string sendStr = "";

        ClientSocket()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public static ClientSocket getClientSocket()
        {
            if (socket == null)
            {
                socket = new ClientSocket();
            }
            return socket;
        }

        public void connectToServer()
        {
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8885));
                isConnected = true;
                Thread revServerThread = new Thread(revFromServer);
                revServerThread.Start();

                Thread sendServerThread = new Thread(sendToServer);
                sendServerThread.Start();
            }
            catch
            {
                Thread TryToConnect = new Thread(tryToConnectToServer);
                TryToConnect.Start();
            }

        }

        private void tryToConnectToServer()
        {
            bool tryToConnect = true;
            while (tryToConnect)
            {
                try
                {
                    clientSocket.Connect(new IPEndPoint(ip, 8885));
                    isConnected = true;
                    tryToConnect = false;

                    Thread revServerThread = new Thread(revFromServer);
                    revServerThread.Start();

                    Thread sendServerThread = new Thread(sendToServer);
                    sendServerThread.Start();
                }
                catch
                {

                }
            }
        }

        private void revFromServer()
        {
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    int receiveLength = clientSocket.Receive(result);
                    receiveStr = Encoding.ASCII.GetString(result, 0, receiveLength);

                }
                catch (Exception ex)
                {
                    isConnected = false;
                    Console.WriteLine(ex.Message);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
        }

        private void sendToServer()
        {
            while (true)
            {
                try
                {
                    if (sendStr != "")
                    {
                        clientSocket.Send(Encoding.ASCII.GetBytes(sendStr.ToString()));
                        sendStr = "";
                    }
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    Console.WriteLine(ex.Message);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
        }
    }
}
