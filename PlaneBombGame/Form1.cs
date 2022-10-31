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

        private int LeftCount; // 已经放置的飞机数

        private LocalPlayer localPlayer;
        private Player AdversaryPlayer;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start = true;                            
            label1.Text = "放置你的飞机";
            panel1.Invalidate();
            panel3.Invalidate();
            panel4.Invalidate();
            LeftCount = 0;
            AdversaryPlayer = new VirtualPlayer();
            localPlayer = new LocalPlayer();
            LeftCount = 0;
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

                if (e.Y < StandardSize.toTop || e.X < StandardSize.toLeft || e.Y >= StandardSize.toTop + (StandardSize.BlockNum +1) * StandardSize.BlockWidth || e.X >= StandardSize.toLeft + (StandardSize.BlockNum + 1) * StandardSize.BlockWidth) return;
                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop)  / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位

                try
                {
                    // TODO 判断此位置是否可以放置 (不可以重叠)
                    if (!Judger.JudgeLegalPlanePlacement(AdversaryPlayer, PlacementX, PlacementY, 0))
                    {
                        MessageBox.Show("位置不合法, 请重新放置", "提示");
                        return;
                    }
                    localPlayer.AddPlane(new Plane(PlacementX, PlacementY, 0));
                    new Plane(PlacementX, PlacementY,0).Draw(panel3);  // 画飞机
                    LeftCount++;
                    if(LeftCount == 3)
                    {
                        AdversaryPlayer.SetPlanes(null);
                        localPlayer.SetPlanes(Utils.Transform(localPlayer.GetPlaneTmp()));
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
                string attackRes;
                Color color;
                try
                {
                    // TODO 判断此位置是否可以放置 (不可以和之前的重复)
                    if(!Judger.JudgeLegalPlacement(AdversaryPlayer, PlacementX, PlacementY))
                    {
                        MessageBox.Show("位置不合法, 请重新放置", "提示");
                        return;
                    }
                    attackRes = Judger.JudgeAttack(AdversaryPlayer, PlacementX, PlacementY);
                    color = attackRes == "HIT" ? Color.Green : (attackRes == "KILL" ? Color.Red : Color.Gray);
                    new AttackPoint(PlacementX, PlacementY).Draw(panel4, color);  
                    

                }
                catch (Exception) { }                            // 防止鼠标点击边界，导致数组越界

                int[] res = AdversaryPlayer.NextAttack();
                attackRes = Judger.JudgeAttack(localPlayer, res[0], res[1]);
                color = attackRes == "HIT" ? Color.Green : (attackRes == "KILL" ? Color.Red : Color.Gray);
                MessageBox.Show(res[0] + " " + res[1],"对方落子");
                new AttackPoint(res[0], res[1]).Draw(panel3,color);
                
            }
            else
            {
                MessageBox.Show("请先开始游戏！", "提示");      // 提示开始游戏
            }
        }
    }
}
