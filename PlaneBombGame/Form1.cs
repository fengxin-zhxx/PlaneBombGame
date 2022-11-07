using System;
using PlaneBombGame.Image;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Net.Sockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace PlaneBombGame
{
    public partial class Form1 : Form
    {
        public static Form1 form1 = null;

        public bool isEnemySetAllPlanes = false;

        public int lastX = -1, lastY = -1;
        



        public int nowDir;

        private bool whoseTurn = true;  // 初始值设置为false 代表为后手下棋

        private int chessDownCount = 0; // 后手方的chessDownCount应该始终比先手放的小一  在接收的时候应该用的上  ....  叭

        internal State state;

        private MovePlane movePlaneForm;

        internal BoomPlaneSocket socket;

        public bool isConnected = false;

        private Bitmap bitmap = new Bitmap(StandardSize.BoardWidth, StandardSize.BoardHeight);        

        private bool start;

        private string[] directions = { "→", "↓", "←", "↑" };

        //飞机方向显示
        private string label1Text = "放置你的飞机 按右键切换机头朝向 当前朝向：";

        private bool adverReadyForNewGame = false;

        public  Boolean aNewGameStart = false;

        public bool isEnemyReadyForGame = false;


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
        private void Form1_Load(object sender, EventArgs e)
        {
            initialize();
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
            state.SetAdversaryPlayer(new AiVirtualPlayer());
            state.SetLocalPlayer(new LocalPlayer());
            paintLoaclPlane();
            label4.Text = "正在进行人机对战...";
        }

        private void BeginNewHumanModeGame()
        {
            isEnemySetAllPlanes = false;

            aNewGameStart = false;            

/*            if (clientOrServer)
            {
                whoseTurn = false;
            }
            else
            {
                whoseTurn = true;
            }*/

            state = new HumanModeState();
            nowDir = 0;

            start = true;
            state.SetLeftCount(0);
            state.SetAdversaryPlayer(new HumanPlayer());
            state.SetLocalPlayer(new LocalPlayer());

            Thread TransMessageThread = new Thread(transMessage);
            TransMessageThread.IsBackground = true;
            TransMessageThread.Start();

            lastX = lastY = -1;
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();

            paintLoaclPlane();                                                         
        }

        //人人对战  初始化 socket  Client 端  并生成state
        private void button2_Click(object sender, EventArgs e)
        {
            string getNewIp = "";

            string getNewPort = "";

            bool clientOrServer = false;

            SetInfoDialog.Show(out getNewIp, out getNewPort, out clientOrServer);
            try
            {
                IPAddress ip = IPAddress.Parse(getNewIp);

                int port = int.Parse(getNewPort);

                if (clientOrServer)
                {
                    socket = BoomPlaneSocket.getSocket(clientOrServer, ip, port);
                    socket.connectToServer();
                    whoseTurn = false;
                }
                else
                {
                    socket = BoomPlaneSocket.getSocket(clientOrServer, ip, port);
                    socket.ListenClientConnect();
                    whoseTurn = true;
                }

                this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                label1.Text = label1Text + directions[nowDir];

                BeginNewHumanModeGame();

                button2.Enabled = false;
            }
            catch (ArgumentNullException)
            {

            
            }
        }
        
        //绘制左侧棋盘
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            PlayingBoard.DrawCB(g);
            if (state != null)
            {
                if(state is AIModeState)
                {
                    state.DrawPlane(g,true);
                    state.DrawPoint(state.getSecondAiPlayer(), state.getFirstAiPlayer(), g);
                }
                else
                {
                    state.DrawPlane(g);
                    state.DrawPoint(state.GetAdversaryPlayer(), state.GetLocalPlayer(), g);
                }
                
               /* if (lastX != -1)
                {
                    state.GetLocalPlayer().GetPreviewPlane().Draw(g, true);
                }*/
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
                if(state is AIModeState)
                {
                    state.DrawPoint(state.getFirstAiPlayer(), state.getSecondAiPlayer(), g);
                    state.DrawPlane(g,false);
                }
                else
                {
                    state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), g);
                }
               
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
                    MessageBox.Show("请先放置三个飞机！ 当前飞机数 ： " + state.GetLeftCount(), "提示");
                    return;
                }

                //不是我的回合
                if (state is HumanModeState)
                {
                    if(isConnected == false)
                    {
                        MessageBox.Show("请耐心等待云端接入！", "提示");
                        return;
                    }
                    if(isEnemySetAllPlanes == false)
                    {
                        MessageBox.Show("请耐心等待对手放置完Ta的飞机！", "提示");
                        return;
                    }
                    if (isEnemyReadyForGame == false)
                    {
                        MessageBox.Show("请等待对手做好新游戏准备！", "提示");
                        return;
                    }
                    if (whoseTurn == false)
                    {
                        MessageBox.Show("请等待对手行棋！", "提示");
                        label4.Text = "请等待对手行棋！";
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

                    if (state is HumanModeState)
                    {
                        string chessDownStr = "1" + " " + PlacementX + " " + PlacementY + " " + chessDownCount;
                        socket.sendStr = chessDownStr;
                        chessDownCount++;
                        label4.Text = "请等待对方落子...";
                        if (Judger.JudgePlayerWin(state.GetLocalPlayer(), state.GetAdversaryPlayer()))
                        {
                            state.DrawPlane(panel4.CreateGraphics(), false);
                            state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), panel4.CreateGraphics());
                            MessageBox.Show("You Won The Game!!");
                            label4.Text = "点击此处重新开始游戏 ...";
                            aNewGameStart = true;
                            return;
                        }
                        whoseTurn = false;
                    }
                    else
                    {
                        if (Judger.JudgePlayerWin(state.GetLocalPlayer(), state.GetAdversaryPlayer()))
                        {
                            state.DrawPlane(panel4.CreateGraphics(), false);
                            state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), panel4.CreateGraphics());
                            MessageBox.Show("You Won The Game!!");
                            BeginNewVirtualModeGame();                            
                            return;
                        }
                        Player player = state.GetAdversaryPlayer();
                        AttackPoint a = player.NextAttack();
                        string showMsgStr = "对方落子 : " + a.x + " " + a.y + "，请您行棋";
                        label4.Text = showMsgStr;
                        string res = state.DrawLastPoint(a, state.GetLocalPlayer(), panel3.CreateGraphics());
                        player.AddAttackPoint(a, res);
                        if (Judger.JudgePlayerWin(state.GetAdversaryPlayer(),state.GetLocalPlayer()))
                        {
                            state.DrawPlane(panel4.CreateGraphics(), false);
                            state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), panel4.CreateGraphics());
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
            while (true && !aNewGameStart)
            {
                if(socket.isConnected == false && isConnected == false)
                {
                    if (label4.InvokeRequired)
                    {
                        Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                        this.label4.Invoke(actionDelegate, "正在等待云端接入...");
                    }
                }

                if(socket.isConnected == true && isConnected == false)
                {
                    isConnected = true;
                    adverReadyForNewGame = true;
                    start = true;

                    if (label1.InvokeRequired)
                    {
                        Action<string> actionDelegate = (x) => { this.label1.Text = x.ToString(); };
                        //label1.Text = label1Text + directions[nowDir];
                        this.label1.Invoke(actionDelegate,label1Text + directions[nowDir]);
                    }

                    if (label4.InvokeRequired)
                    {
                        Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                        this.label4.Invoke(actionDelegate, "云端已接入，可以开始游戏啦");
                    }
                }

                if(socket.isConnected == false && isConnected == true)
                {
                    isConnected = false;                    
                    if (label4.InvokeRequired)
                    {
                        Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                        this.label4.Invoke(actionDelegate, "云端已下线，请重新开始游戏");                       
                    }            
                    
                }
                if(socket.isConnected == true && isConnected == true)
                {
                    if(isEnemySetAllPlanes == false)
                    {
                        if (label4.InvokeRequired)
                        {
                            Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                            this.label4.Invoke(actionDelegate, "对手正在放置Ta的飞机");
                        }
                    }
                }
                if(socket.isConnected == true && isEnemyReadyForGame == false)
                {
                    socket.sendStr = "2";
                  /* if (label1.InvokeRequired)
                    {
                        Action<string> actionDelegate = (x) => { this.label1.Text = x.ToString(); };
                        this.label1.Invoke(actionDelegate, "connected");
                    }*/
                }

                //如果接收到的消息不为空
                if (socket.receiveStr != "")
                {
                    string str = socket.receiveStr;
                    
                    string[] words = str.Split(' ');//按空格进行拆解

                    switch (int.Parse(words[0].Substring(0,1)))
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
                                if (label4.InvokeRequired)
                                {
                                    Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                                    this.label4.Invoke(actionDelegate, "对手已经放置完Ta的飞机，随时可以开始对战！");
                                }
                                if (label1.InvokeRequired)
                                {
                                    Action<string> actionDelegate = (x) => { this.label1.Text = x.ToString(); };
                                    this.label1.Invoke(actionDelegate, "点击右侧方格以进攻对手");
                                }

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
                            //判断胜负
                            if (Judger.JudgePlayerWin(state.GetAdversaryPlayer(), state.GetLocalPlayer()))
                            {
                                state.DrawPlane(panel4.CreateGraphics(), false);
                                state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), panel4.CreateGraphics());
                                MessageBox.Show("AdversaryPlayer Won The Game!");
                                if (label4.InvokeRequired)
                                {
                                    Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                                    this.label4.Invoke(actionDelegate, "点击此处重新开始游戏 ...");
                                }
                                aNewGameStart = true;
                            }
                            else
                            {
                                //显示文字
                                if (label4.InvokeRequired)
                                {
                                    Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                                    string showMsgStr = "对方落子 : " + attackPoint.x + " " + attackPoint.y + "，请您行棋";
                                    this.label4.Invoke(actionDelegate, showMsgStr);
                                }
                                //绘制棋盘
                                state.DrawLastPoint(attackPoint, state.GetLocalPlayer(), panel3.CreateGraphics());
                                //允许下棋
                                whoseTurn = true;
                            }
                            break;
                        case 2:
                            isEnemyReadyForGame = true;
                            socket.sendStr = "";
                            break;
                    }
                    //消息翻译完毕字符串清空
                    socket.receiveStr = "";
                }
            }
        }

        //自动对战
        private void button3_Click(object sender, EventArgs e)
        {
            state = new AIModeState();
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label1.Text = "比赛进行中...";
            label4.Text = "AI自动对战";                      
            
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();

            start = true;
            state.setFirstAiPlayer(new AiVirtualPlayer());
            state.setSecondAiPlayer(new AiVirtualPlayer());

            AiModePlayerInit();
        }


        private void AiModePlayerInit()
        {
            state.getFirstAiPlayer().SetPlanes(null);
            //避免两个机器人生成一样的飞机
            Thread.Sleep(20);
            state.getSecondAiPlayer().SetPlanes(null);
            state.DrawPlane(panel3.CreateGraphics(),true);            
            state.DrawPlane(panel4.CreateGraphics(),false);
            Thread AiModePlayBeiginThread = new Thread(AiModePlayerBegin);
            AiModePlayBeiginThread.IsBackground = true;
            AiModePlayBeiginThread.Start();
        }

        private void AiModePlayerBegin()
        {
            while (true)
            {                 
                //取一号机器人进攻点
                AttackPoint firstAttackPoint = state.getFirstAiPlayer().NextAttack();

                if (label4.InvokeRequired)
                {
                    Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                    this.label4.Invoke(actionDelegate, "1号落子：" + firstAttackPoint.x + " "+ firstAttackPoint.y);
                }

                //绘制在二号机器人棋盘上
                string res = state.DrawLastPoint(firstAttackPoint, state.getSecondAiPlayer(), panel4.CreateGraphics());                
                
                state.getFirstAiPlayer().AddAttackPoint(firstAttackPoint, res);
                
                //判断一号机器人是否胜利
                if (Judger.JudgePlayerWin(state.getFirstAiPlayer(), state.getSecondAiPlayer()))
                {
                    MessageBox.Show("一号机器人赢得了游戏 !");                    
                    break;
                }               

                Thread.Sleep(1000);

                AttackPoint secondAttackPoint = state.getSecondAiPlayer().NextAttack();
                
                if (label4.InvokeRequired)
                {
                    Action<string> actionDelegate = (x) => { this.label4.Text = x.ToString(); };
                    this.label4.Invoke(actionDelegate, "2号落子：" + secondAttackPoint.x + " " + secondAttackPoint.y);
                }
                
                res = state.DrawLastPoint(secondAttackPoint, state.getFirstAiPlayer(), panel3.CreateGraphics());
                
                state.getSecondAiPlayer().AddAttackPoint(secondAttackPoint, res);

                //判断二号机器人是否胜利
                if (Judger.JudgePlayerWin(state.getSecondAiPlayer(), state.getFirstAiPlayer()))
                {
                    MessageBox.Show("二号机器人赢得了游戏 !");
                    break;
                }

                Thread.Sleep(1000);
            }
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
        
        public void changeLable4Msg(String str)
        {
            label4.Text = str;
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
                if (state is AIModeState) return;
                if(movePlaneForm.WindowState == FormWindowState.Minimized)
                {
                    movePlaneForm.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           // System.Environment.Exit(0);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            isEnemyReadyForGame = false;
            reBeginNewHumanModeGame();            
        }

        public void setLocalPlane()
        {
            state.DrawPlane(panel3.CreateGraphics());
        }  

        private void reBeginNewHumanModeGame()
        {
            BeginNewHumanModeGame();            
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label1.Text = label1Text + directions[nowDir];
        }
    
    }
}
