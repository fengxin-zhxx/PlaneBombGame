using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace PlaneBombGame
{
    public partial class Form1 : Form
    {
        public static Form1 form1 = null;

        public bool isEnemySetAllPlanes = false;

        private bool readyForOnePlane = false;

        public int lastX = -1, lastY = -1;
        
        public int nowDir;

        private bool whoseTurn = true; // 初始值设置为false 代表为后手下棋

        private int chessDownCount = 0; // 后手方的chessDownCount应该始终比先手放的小一  在接收的时候应该用的上  ....  叭

        internal State state;

        //private ClientSocket socket;

        internal ServerSocket socket;

        private MovePlane movePlaneForm;

        private Bitmap bitmap = new Bitmap(StandardSize.BoardWidth, StandardSize.BoardHeight);        

        private bool start;

        private string[] directions = { "→", "↓", "←", "↑" };
        //飞机方向显示
        private string label1Text = "放置你的飞机 按右键切换机头朝向 当前朝向：";

        internal static Form1 getForm1()
        {
            if (form1 == null)
            {
                form1 = new Form1();
            }
            return form1;
        }

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initialize();
            //this.FormBorderStyle = FormBorderStyle.None;
            this.Width = StandardSize.FormWidth;
            this.Height = StandardSize.FormHeight;
            this.Location = new Point(100, 10);
        }

        //人机对战  采用随机生成飞机  随即落点的方式
        private void button1_Click(object sender, EventArgs e)
        {
            BeginNewVirtualModeGame();
        }

        private void BeginNewVirtualModeGame()
        {
            state = new VirtualModeState();
            nowDir = 0;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label1.Text = label1Text + directions[nowDir];
            lastX = lastY = -1;
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();
            start = true;
            state.SetLeftCount(0);
            //state.SetAdversaryPlayer(new RandomVirtualPlayer());
            state.SetAdversaryPlayer(new AiVirtualPlayer());
            state.SetLocalPlayer(new LocalPlayer());
            paintLoaclPlane();

        }


        //人人对战  初始化 socket  Client 端  并生成state
        private void BeginNewHumenModeState()
        {
            //socket = ClientSocket.getClientSocket();
            //socket.connectToServer();

            lastX = lastY = -1;

            socket = ServerSocket.getServerSocket();
            socket.ListenClientConnect();

            //新建一个线程用于翻译接收到的信息
            Thread TransMessageThread = new Thread(transMessage);
            TransMessageThread.Start();

            state = new HumanModeState();
            nowDir = 0;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label1.Text = label1Text + directions[nowDir];
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();

            start = true;
            state.SetLeftCount(0);
            state.SetAdversaryPlayer(new HumanPlayer());
            state.SetLocalPlayer(new LocalPlayer());

            paintLoaclPlane();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BeginNewHumenModeState();
        }

        private void initialize()
        {
            start = false;
            label1.Text = "WelCome To Plane Bombbbb!!!";
            panel3.Width = StandardSize.BoardWidth;
            panel3.Height = StandardSize.BoardHeight;
            panel4.Width = StandardSize.BoardWidth;
            panel4.Height = StandardSize.BoardHeight;
            // TO DO
        }


        //绘制左侧棋盘
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            PlayingBoard.DrawCB(g);
            if (state != null)
            {
                state.DrawPlane(state.GetLocalPlayer(), g);
                state.DrawPoint(state.GetAdversaryPlayer(), state.GetLocalPlayer(), g);
                if (lastX != -1)
                {
                    state.GetLocalPlayer().GetPreviewPlane().Draw(g, true);
                }
            }
            panel3.CreateGraphics().DrawImage(bitmap, 0, 0);
        }

        //绘制右侧棋盘
        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            PlayingBoard.DrawCB(g);
            if (state != null)
            {
                state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), g);
            }
            panel4.CreateGraphics().DrawImage(bitmap, 0, 0);
        }

        //右侧棋盘 点击绘制落点并显示颜色
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            if (start)
            {

                if (state.GetLeftCount() != 3)
                {
                    MessageBox.Show("请先放置三个飞机！", "提示");
                    return;
                }

                //不是我的回合
                if (state is HumanModeState)
                {
                    if (whoseTurn == false)
                    {
                        MessageBox.Show("请等待对手行棋！", "提示");
                        return;
                    }
                }



                //MessageBox.Show(e.X + " " + e.Y); 相对于当前panel
                if (!Judger.JudgeLegalMouseDown(e.X, e.Y))
                {
                    MessageBox.Show("位置不合法, 请重新放置", "提示");
                    return;
                }

                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop) / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位

                try
                {
                    if (!Judger.JudgeLegalPlacement(state.GetLocalPlayer(), PlacementX, PlacementY))
                    {
                        MessageBox.Show("位置不合法, 请重新放置", "提示");
                        return;
                    }

                    AttackPoint attackPoint = new AttackPoint(PlacementX, PlacementY);

                    state.GetLocalPlayer().AddAttackPoint(attackPoint); // 新的攻击点加入历史记录

                    state.DrawLastPoint(attackPoint, state.GetAdversaryPlayer(), panel4.CreateGraphics());

                    if (Judger.JudgePlayerWin(state.GetLocalPlayer(), state.GetAdversaryPlayer()))
                    {
                        MessageBox.Show("You Won The Game!!");
                        BeginNewVirtualModeGame();
                        return;
                    }

                    /*TO DO Socket游戏结束时的信息传送*/


                    if (state is HumanModeState)
                    {
                        string chessDownStr = "1" + " " + PlacementX + " " + PlacementY + " " + chessDownCount;

                        socket.sendStr = chessDownStr;


                        chessDownCount++;

                        whoseTurn = false;//在对手下完棋之前不会再下棋

                        /*TO DO Socket游戏结束时的判断*/
                    }
                    else
                    {
                        Player player = state.GetAdversaryPlayer();
                        AttackPoint a = player.NextAttack();
                        string res = state.DrawLastPoint(a, state.GetLocalPlayer(), panel3.CreateGraphics());
                        player.AddAttackPoint(a, res);
                        if (Judger.JudgePlayerWin(state.GetAdversaryPlayer(),state.GetLocalPlayer()))
                        {
                            Graphics g = Graphics.FromImage(bitmap);
                            state.DrawPlane(state.GetAdversaryPlayer(), g);
                            state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), g);
                            panel4.CreateGraphics().DrawImage(bitmap, 0, 0);
                            MessageBox.Show("AI Won The Game!");
                            BeginNewVirtualModeGame();
                            return;
                        }
                    }


                }
                catch (Exception) { }
            }
            else
            {
                MessageBox.Show("请先开始游戏！", "提示");
            }
        }

        private void transMessage()
        {
            while (true)
            {

                if (PreviewPlane.getPrePlane().avaliable == true)
                {

                    string str = socket.receiveStr;
                    
                    MessageBox.Show(str, "提示");

                    string[] words = str.Split(' ');//按空格进行拆解

                    switch (int.Parse(words[0]))
                    {
                        case 0:
                            //对手飞机设置完毕，传入进行赋值   
                            // "SetPlanes 1,2,3 4,5,6 7,8,9"
                            // "   0         1    2     3 "
                            //可能会放置很多次，我们只取第一次
                            if (isEnemySetAllPlanes == false)
                            {
                                Plane[] planes = new Plane[3]; int index = -1;
                                for (int i = 1; i <= 3; i++)
                                {
                                    string[] planePlace = words[i].Split(',');
                                    Plane plane = new Plane(int.Parse(planePlace[0]), int.Parse(planePlace[1]), int.Parse(planePlace[2]));
                                    planes[++index] = plane;
                                }
                                state.GetAdversaryPlayer().SetPlanes(planes);
                                isEnemySetAllPlanes = true;
                            }
                            break;
                        case 1:
                            //"ChessDown 1 2 chessDownCount"
                            int chessDownX = int.Parse(words[1]);
                            int chessDownY = int.Parse(words[2]);
                            int enemyChessDownCount = int.Parse(words[3]);
                            AttackPoint attackPoint = new AttackPoint(chessDownX, chessDownY);
                            //加入对手落子历史
                            state.GetAdversaryPlayer().AddAttackPoint(attackPoint);
                            //显示弹窗
                            MessageBox.Show(attackPoint.x + " " + attackPoint.y, "对方落子");
                            //绘制棋盘
                            state.DrawLastPoint(attackPoint, state.GetLocalPlayer(), panel3.CreateGraphics());
                            //允许下棋
                            whoseTurn = true;
                            break;
                    }
                    //消息翻译完毕字符串清空
                    socket.receiveStr = "";
                }


                //如果接收到的消息不为空
                /* if (socket.receiveStr != "")
                 {

                     string str = socket.receiveStr;

                     MessageBox.Show(str, "提示");

                     string[] words = str.Split(' ');//按空格进行拆解

                     switch (int.Parse(words[0]))
                     {
                         case 0:
                             //对手飞机设置完毕，传入进行赋值   
                             // "SetPlanes 1,2,3 4,5,6 7,8,9"
                             // "   0         1    2     3 "
                             //可能会放置很多次，我们只取第一次
                             if (isEnemySetAllPlanes == false)
                             {
                                 Plane[] planes = new Plane[3]; int index = -1;
                                 for (int i = 1; i <= 3; i++)
                                 {
                                     string[] planePlace = words[i].Split(',');
                                     Plane plane = new Plane(int.Parse(planePlace[0]), int.Parse(planePlace[1]), int.Parse(planePlace[2]));
                                     planes[++index] = plane;
                                 }
                                 state.GetAdversaryPlayer().SetPlanes(planes);
                                 isEnemySetAllPlanes = true;
                             }
                             break;
                         case 1:
                             //"ChessDown 1 2 chessDownCount"
                             int chessDownX = int.Parse(words[1]);
                             int chessDownY = int.Parse(words[2]);
                             int enemyChessDownCount = int.Parse(words[3]);
                             AttackPoint attackPoint = new AttackPoint(chessDownX, chessDownY);
                             //加入对手落子历史
                             state.GetAdversaryPlayer().AddAttackPoint(attackPoint);
                             //显示弹窗
                             MessageBox.Show(attackPoint.x + " " + attackPoint.y, "对方落子");
                             //绘制棋盘
                             state.DrawLastPoint(state.GetAdversaryPlayer(), state.GetLocalPlayer(), panel3.CreateGraphics());
                             //允许下棋
                             whoseTurn = true;
                             break;
                     }
                     //消息翻译完毕字符串清空
                     socket.receiveStr = "";
                 }*/
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            state = new VirtualModeState();
            nowDir = 0;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label1.Text = label1Text + directions[nowDir];
            lastX = lastY = -1;
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();

            start = true;
            state.SetLeftCount(0);
            state.SetAdversaryPlayer(new RandomVirtualPlayer());
            state.SetLocalPlayer(new LocalPlayer());
            paintLoaclPlane();

        }
        private void paintLoaclPlane()
        {
            if (movePlaneForm != null) movePlaneForm.Close();
            movePlaneForm = new MovePlane();
            movePlaneForm.TransparencyKey = Color.Red;
            movePlaneForm.BackColor = Color.Red;            
            movePlaneForm.TopMost = true;
            movePlaneForm.Show();
            movePlaneForm.Invalidate();
        }

        public void changeLable1Msg(String str)
        {
            if(str == null)
            {
                label1.Text = label1Text + directions[nowDir];
            }
            else
            {
                label1.Text = str;  
            }
            
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {                        
            if(movePlaneForm != null)
            {
                movePlaneForm.reSetLocation(this.Location.X + 8, this.Location.Y + 30); 
            }

        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (start)
            {
                if(movePlaneForm.WindowState == FormWindowState.Minimized)
                {
                    movePlaneForm.WindowState = FormWindowState.Normal;
                }
            }
        }

        public void setLocalPlane()
        {
            state.DrawPlane(state.GetLocalPlayer(), panel3.CreateGraphics());
        }

    }
}
