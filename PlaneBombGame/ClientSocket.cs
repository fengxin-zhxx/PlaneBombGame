using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace PlaneBombGame
{
    internal class ClientSocket
    {
        static Socket clientSocket;

        private static ClientSocket socket = null;

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
            }
            catch
            {
                return;
            }
            Thread revServerThread = new Thread(revFromServer);
            revServerThread.Start();

            Thread sendServerThread = new Thread(sendToServer);
            sendServerThread.Start();

        }

        private void revFromServer()
        {
            while (true)
            {
                byte[] result = new byte[1024];
                int receiveLength = clientSocket.Receive(result);
                receiveStr = Encoding.ASCII.GetString(result, 0, receiveLength);    
                
            }
        }

        private void sendToServer()
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
