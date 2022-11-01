using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    public partial class Form1 : Form
    {

        private State state;
        bool start;
        int nowDir;
        string[] directions = { "→", "↓", "←", "↑" };
        //飞机方向显示
        string label1Text = "放置你的飞机 按右键切换机头朝向 当前朝向：";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            state  = new VirtualModeState();
            state.Init();
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
            // 判断游戏是否开始
            if (start)
            {

                if(state.GetLeftCount() >= 3)
                {
                    return;
                }
                if (!Judger.JudgeLegalMouseDown(e)) return;
                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop)  / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位

                try
                {
                    Plane plane = new Plane(PlacementX, PlacementY, nowDir);
                    // TODO 判断此位置是否可以放置 (不可以重叠)
                    if (!Judger.JudgeLegalPlanePlacement(state.GetLocalPlayer().GetPlanes(), plane))
                    {
                        MessageBox.Show("位置不合法, 请重新放置", "提示");
                        return;
                    }
                    MessageBox.Show("X=" + PlacementX + " Y=" + PlacementY + " DIR=" + nowDir);

                    state.GetLocalPlayer().SetOnePlane(plane,state.GetLeftCount());
                    state.Draw(panel3); 
                    state.SetLeftCount(state.GetLeftCount() + 1);
                    if(state.GetLeftCount() == 3)
                    {
                        state.GetAdversaryPlayer().SetPlanes();
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

                if (state.GetLeftCount() != 3)
                {
                    MessageBox.Show("请先放置三个飞机！", "提示");    
                    return;
                }

                //MessageBox.Show(e.X + " " + e.Y); 相对于当前panel
                if (!Judger.JudgeLegalMouseDown(e)) return;

                int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = (e.Y - StandardSize.toTop) / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位
                string attackRes;
                Color color;
                try
                {
                    // TODO 判断此位置是否可以放置 (不可以和之前的重复)
                    if(!Judger.JudgeLegalPlacement(state.GetAdversaryPlayer(), PlacementX, PlacementY))
                    {
                        MessageBox.Show("位置不合法, 请重新放置", "提示");
                        return;
                    }
                    attackRes = Judger.JudgeAttack(state.GetAdversaryPlayer(), PlacementX, PlacementY);
                    color = attackRes == "HIT" ? Color.Green : (attackRes == "KILL" ? Color.Red : Color.Gray);
                    new AttackPoint(PlacementX, PlacementY).Draw(panel4, color);
                

                    int[] res = state.GetAdversaryPlayer().NextAttack();
                    attackRes = Judger.JudgeAttack(state.GetLocalPlayer(), res[0], res[1]);
                    color = attackRes == "HIT" ? Color.Green : (attackRes == "KILL" ? Color.Red : Color.Gray);
                    MessageBox.Show(res[0] + " " + res[1],"对方落子");
                    new AttackPoint(res[0], res[1]).Draw(panel3,color);
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
