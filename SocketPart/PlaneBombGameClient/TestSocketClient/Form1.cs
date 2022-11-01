using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestSocketClient
{
    public partial class Form1 : Form
    {
        private static byte[] result = new byte[1024];

        static Socket clientSocket;

        private int countNum;

        public Form1()
        {
            countNum = 0;
            InitializeComponent();            
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8885)); //配置服务器IP与端口
            }
            catch
            {
                return;
            }

            Thread revServerThread = new Thread(revServer);
            revServerThread.Start();

            //Thread sendServerThread = new Thread(sendServer);
            //sendServerThread.Start();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread sendToServerThread = new Thread(sendServer);
            sendToServerThread.Start(textBox2.Text);
        }


        private void sendServer(object str)
        {
            /*for (int i = 0; i < 10; i++)
            {
                try
                {
                    Thread.Sleep(1000);    //等待1秒钟
                    countNum++;
                    string sendMessage = countNum.ToString();
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }*/

            //string sendMessage = countNum.ToString();
            clientSocket.Send(Encoding.ASCII.GetBytes(str.ToString()));
        }

        private void revServer()
        {
            while (true)
            {
                if (textBox1.InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<string> actionDelegate = (x) => { this.textBox1.Text = x.ToString(); };
                    int receiveLength = Form1.clientSocket.Receive(result);
                    // Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
                    this.textBox1.Invoke(actionDelegate, Encoding.ASCII.GetString(result, 0, receiveLength));
                }
                else
                {
                    this.textBox1.Text = "2222";
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
