﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace PlaneBombGame
{
    public partial class Form1 : Form
    {

        private bool isEnemySetAllPlanes = false;

        private bool whoseTurn = false; // 初始值设置为false 代表为后手下棋

        private int chessDownCount = 0; // 后手方的chessDownCount应该始终比先手放的小一  在接收的时候应该用的上  ....  叭

        private State state;

        private ClientSocket socket;

        //private ServerSocket socket;



        bool start;
        int nowDir;
        string[] directions = { "→", "↓", "←", "↑" };
        //飞机方向显示
        string label1Text = "放置你的飞机 按右键切换机头朝向 当前朝向：";
        public Form1()
        {
            InitializeComponent();
        }

        //人机对战  采用随机生成飞机  随即落点的方式
        private void button1_Click(object sender, EventArgs e)
        {
            state  = new VirtualModeState();
            nowDir = 0;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label1.Text = label1Text + directions[nowDir];           
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();

            start = true;
            state.SetLeftCount(0);
            state.SetAdversaryPlayer(new VirtualPlayer());
            state.SetLocalPlayer(new LocalPlayer());

        }

        //人人对战  初始化 socket  Client 端  并生成state
        private void button2_Click(object sender, EventArgs e)
        {
            socket = ClientSocket.getClientSocket();
            socket.connectToServer();

            //socket = ServerSocket.getServerSocket();
            //socket.ListenClientConnect();   

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



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initialize();                      
            this.Width = StandardSize.FormWidth;       
            this.Height = StandardSize.FormHeight;     
            this.Location = new Point(400, 75);   
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

        private void ReDraw()
        {
            state.DrawPlane(panel3);
            state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), panel4);
            state.DrawPoint(state.GetAdversaryPlayer(), state.GetLocalPlayer(), panel3);
        }

        //绘制左侧棋盘
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            PlayingBoard.DrawCB(panel3);
            if(state != null) ReDraw();
        }

        //绘制右侧棋盘
        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            PlayingBoard.DrawCB(panel4);
            if(state != null) ReDraw();
        }

        //左侧棋盘 放置自己的飞机
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {

            if (state.GetLeftCount() == 3)
            {
                //飞机放置完毕后发送至server端
                string planesStr = "0 ";

                Plane[] planes = state.GetLocalPlayer().GetPlanes();
                for (int i = 0; i < planes.Length; i++)
                {
                    planesStr += planes[i].x + "," + planes[i].y + "," + planes[i].direction;
                    if (i != planes.Length - 1)
                    {
                        planesStr += " ";
                    }
                }

                socket.sendStr = planesStr;

                if (isEnemySetAllPlanes == false)
                {
                    MessageBox.Show("请等待对手放置完Ta的飞机", "提示");
                    return;
                }

                MessageBox.Show("对手已经放置完Ta的飞机", "提示");

                Plane[] showPlanes = state.GetAdversaryPlayer().GetPlanes();


                MessageBox.Show(showPlanes[0].x + " " + showPlanes[0].y + "  " + showPlanes[1].x + " " + showPlanes[1].y + "  " + showPlanes[2].x + " " + showPlanes[2].y, "对方放置飞机");

                MessageBox.Show("按确认开始对战", "提示");
                label1.Text = "点击右侧方格以攻击对手";
            }

            if (e.Button == MouseButtons.Right)
            {
                if (start)
                {
                    nowDir = (nowDir + 1) % 4;
                    label1.Text = label1Text + directions[nowDir];
                }
                else
                {
                    MessageBox.Show("右键:请先开始游戏！", "提示");      // 提示开始游戏
                }
                return;
            }
            if (start)
            {

                if(state.GetLeftCount() >= 3) return;
                if (!Judger.JudgeLegalMouseDown(e)) return;


                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop)  / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位

                try
                {                    

                    Plane plane = new Plane(PlacementX, PlacementY, nowDir);

                    if (!Judger.JudgeLegalPlanePlacement(state.GetLocalPlayer().GetPlanes(), plane))
                    {
                        MessageBox.Show("位置不合法, 请重新放置", "提示");
                        return;
                    }
                    MessageBox.Show("X=" + PlacementX + " Y=" + PlacementY + " DIR=" + nowDir);

                    state.GetLocalPlayer().SetOnePlane(plane,state.GetLeftCount());
                    state.DrawPlane(panel3); 

                    state.SetLeftCount(state.GetLeftCount() + 1);
                    
                }
                catch (Exception) { } // 防止崩溃

            }
            else
            {
                MessageBox.Show("请先开始游戏！", "提示");
            }
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
                if(whoseTurn == false)
                {
                    MessageBox.Show("请等待对手行棋！", "提示");
                    return;
                }


                //MessageBox.Show(e.X + " " + e.Y); 相对于当前panel
                if (!Judger.JudgeLegalMouseDown(e)) return;

                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop) / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位

                try
                {
                    if(!Judger.JudgeLegalPlacement(state.GetAdversaryPlayer(), PlacementX, PlacementY))
                    {
                        MessageBox.Show("位置不合法, 请重新放置", "提示");
                        return;
                    }

                    AttackPoint attackPoint = new AttackPoint(PlacementX, PlacementY); 
                    
                    state.GetLocalPlayer().AddAttackPoint(attackPoint); // 新的攻击点加入历史记录
                    
                    state.DrawPoint(state.GetLocalPlayer(), state.GetAdversaryPlayer(), panel4);//判断落子结果

                    string chessDownStr = "1" + " " + PlacementX + " " + PlacementY + " " + chessDownCount;

                    socket.sendStr = chessDownStr;

                    chessDownCount++;

                    whoseTurn = false;//在对手下完棋之前不会再下棋
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
                //如果接收到的消息不为空
                if (socket.receiveStr != "")
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
                            state.DrawPoint(state.GetAdversaryPlayer(), state.GetLocalPlayer(), panel3);
                            //允许下棋
                            whoseTurn = true;
                            break;
                    }
                    //消息翻译完毕字符串清空
                    socket.receiveStr = "";
                }
            }
        }

    }
}