using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using System.Drawing;
using System.Windows.Forms;

namespace PlaneBombGame
{
    internal class ServerSocket
    {
        static Socket serverSocket;

        static Socket clientSocket;

        private static ServerSocket socket = null;

        public bool isConnected = false;

        private IPAddress ip = IPAddress.Parse("127.0.0.1");

        private int port = 8885;

        public string receiveStr = "";

        public string sendStr = "";

        ServerSocket()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, port));
            serverSocket.Listen(1);
        }

        public static ServerSocket getServerSocket()
        {
            if (socket == null)
            {
                socket = new ServerSocket();
            }
            return socket;
        }

        public void ListenClientConnect()
        {
            Thread goListenThread = new Thread(keepListening);
            goListenThread.Start();
        }

        public void keepListening()
        {
            while (true)
            {
                clientSocket = serverSocket.Accept();

                isConnected = true;

                Thread receiveThread = new Thread(revFromClient);
                receiveThread.Start();

                Thread sendThred = new Thread(sendToClient);
                sendThred.Start();
            }
        }

        private void revFromClient()
        {
            while (true)
            {
                byte[] result = new byte[1024];
                int receiveNumber = clientSocket.Receive(result);
                receiveStr = Encoding.ASCII.GetString(result, 0, receiveNumber);
            }
        }

        private void sendToClient()
        {
            while (true)
            {
                if (sendStr != "")
                {
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendStr.ToString()));
                    sendStr = "";
                }
            }
        }
    }
}
