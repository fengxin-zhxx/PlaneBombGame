using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PlaneBombGame
{
    internal class BoomPlaneSocket 
    {
        static Socket clientSocket;
        
        static Socket serverSocket;

        private static BoomPlaneSocket socket = null;

        public bool isConnected = false;

        public IPAddress ip ;

        public int port;

        public string receiveStr = "";

        public string sendStr = "";

        BoomPlaneSocket(bool clientOrServer , IPAddress newIp , int newPort)
        {
            if (clientOrServer)
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ip = newIp;
                port = newPort;
            }
            else
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(newIp, newPort));
                serverSocket.Listen(1);
            }
        }

        public static BoomPlaneSocket getSocket(bool clientOrSocket, IPAddress newIp, int newPort)
        {
            if (socket == null)
            {
                socket = new BoomPlaneSocket(clientOrSocket,newIp,newPort);
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

                if(clientSocket != null)
                {
                    Thread receiveThread = new Thread(revFromClient);
                    receiveThread.Start();

                    Thread sendThred = new Thread(sendToClient);
                    sendThred.Start();
                }
            }
        }

        private void revFromClient()
        {
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    int receiveNumber = clientSocket.Receive(result);
                    receiveStr = Encoding.ASCII.GetString(result, 0, receiveNumber);
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

        private void sendToClient()
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
