using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    public partial class Form1 : Form
    {
        private bool start;     // 游戏是否开始

        private bool ChessCheck = true;     // 白子黑子回合

        private const int size = 11;     // 棋盘大小

        private int[,] CheckBoard = new int[size, size];     // 棋盘点位数组

        private int LeftCount; // 已经放置的飞机数
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start = true;                             
            ChessCheck = true;                        
            label1.Text = "放置你的飞机";
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();
            LeftCount = 0;
        }
        private void initialize()
        {

            LeftCount = 0; 

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    CheckBoard[i, j] = 0;
                }
            }
            start = false; 
            label1.Text = "WelCome To Plane Bombbbb!!!";


            panel3.Width = StandardSize.BoardWidth;
            panel3.Height = StandardSize.BoardHeight;
            panel4.Width = StandardSize.BoardWidth;
            panel4.Height = StandardSize.BoardHeight;
            // TO DO
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initialize();                      // 调用初始化游戏
            this.Width = StandardSize.FormWidth;       // 设置窗口宽度
            this.Height = StandardSize.FormHeight;     // 设置窗口高度
            this.Location = new Point(400, 75);     // 设置窗口位置
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel3.CreateGraphics();      // 创建面板画布
            PlayingBoard.DrawCB(g);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = panel4.CreateGraphics();      // 创建面板画布
            PlayingBoard.DrawCB(g);
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            // 判断游戏是否开始
            if (start)
            {

                if(LeftCount >= 3)
                {
                    return;
                }

                if (e.Y < StandardSize.toTop || e.X < StandardSize.toLeft || e.Y >= StandardSize.toTop + 660 || e.X >= StandardSize.toLeft + 660) return;
                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop)  / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位

                try
                {
                    // TODO 判断此位置是否可以放置 (不可以重叠)
                
                    Plane.Draw(panel3,PlacementX, PlacementY);  // 画飞机
                    LeftCount++;
                    if(LeftCount == 3)
                    {
                        MessageBox.Show("按确认开始对战", "提示");
                        label1.Text = "点击右侧方格以攻击对手";
                    }

                }
                catch (Exception) { }                            // 防止鼠标点击边界，导致数组越界

            }
            else
            {
                MessageBox.Show("请先开始游戏！", "提示");      // 提示开始游戏
            }
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            // 判断游戏是否开始
            if (start)
            {

                if (LeftCount != 3)
                {

                    MessageBox.Show("请先放置三个飞机！", "提示");    
                    return;
                }

                //MessageBox.Show(e.X + " " + e.Y); 相对于当前panel
                if (e.Y < StandardSize.toTop || e.X < StandardSize.toLeft || e.Y >= StandardSize.toTop + 660 || e.X >= StandardSize.toLeft + 660) return;
                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop) / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位

                try
                {
                    // TODO 判断此位置是否可以放置 (不可以和之前的重复)

                    Plane.Draw(panel4, PlacementX, PlacementY);  // 画飞机
                    

                }
                catch (Exception) { }                            // 防止鼠标点击边界，导致数组越界

            }
            else
            {
                MessageBox.Show("请先开始游戏！", "提示");      // 提示开始游戏
            }
        }
    }
}
