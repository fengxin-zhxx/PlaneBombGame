using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.AxHost;

namespace PlaneBombGame
{
    public partial class MovePlane : Form
    {
        private int lastX = -1;
        private int lastY = -1;
        private int nowDir = 0;

        private int countSet = 0;

        private State state;

        private Form1 form1;

        private Bitmap bitmap = new Bitmap(StandardSize.BoardWidth, StandardSize.BoardHeight);

        public MovePlane()
        {
            InitializeComponent();
            form1 = Form1.getForm1();
            state = form1.state;            
        }

        private void MovePlane_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Location = new Point(form1.Location.X + 8, form1.Location.Y + 30 + 60);
            this.ShowInTaskbar = false;
        }

        public void reSetLocation(int x, int y)
        {
            this.Location = new Point(x, y + 60);

        }

        private void movePlane_Paint(object sender, PaintEventArgs e)
        {            
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(this.BackColor);           
            if (state != null && state.GetLocalPlayer().GetPreviewPlane() != null)
            {
                //if (lastX != -1)
                //{
                    state.GetLocalPlayer().GetPreviewPlane().Draw(g, true);
                //}
            }
            this.CreateGraphics().DrawImage(bitmap, 0, 0);
        }


        private void form_MouseDown(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Right)
            {
                nowDir = (nowDir + 1) % 4;
                form1.nowDir = nowDir;
                form1.changeLable1Msg(null);
                lastX = lastY = -1;
                return;
            }

            if (state is HumanModeState)
            {
                if(form1.isConnected == false)
                {
                    MessageBox.Show("请等待云端接入！", "提示");
                    return;
                }
                if(form1.isEnemyReadyForGame == false)
                {
                    MessageBox.Show("请等待对手做好新游戏准备！", "提示");
                    return;
                }
                
            }

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
                
                state.GetLocalPlayer().SetOnePlane(plane, state.GetLeftCount());
                
                form1.setLocalPlane();
                
                state.SetLeftCount(state.GetLeftCount() + 1);

                lastX = lastY = -1;
                if (state.GetLeftCount() == 3)
                {
                    if (state is HumanModeState)
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

                        form1.socket.sendStr = planesStr;

                        /*if (form1.isConnected == false)
                        {
                            MessageBox.Show("请等待对手放置完Ta的飞机", "提示");
                            this.Close();
                            return;
                        }*/

                        if (form1.isEnemySetAllPlanes == false)
                        {
                            MessageBox.Show("请等待对手放置完Ta的飞机", "提示");
                            this.Close();
                            return;
                        }

                        MessageBox.Show("对手已经放置完Ta的飞机", "提示");

                        //Plane[] showPlanes = state.GetAdversaryPlayer().GetPlanes();
                        //MessageBox.Show(showPlanes[0].x + " " + showPlanes[0].y + "  " + showPlanes[1].x + " " + showPlanes[1].y + "  " + showPlanes[2].x + " " + showPlanes[2].y, "对方放置飞机");
                    }
                    else
                    {
                        state.GetAdversaryPlayer().SetPlanes(null);
                    }
                    MessageBox.Show("按确认开始对战", "提示");
                    form1.changeLable1Msg("点击右侧方格以攻击对手");
                    form1.changeLable4Msg("开始进攻 ! ");
                    this.Close();
                }
            }
            catch (Exception) { } // 防止崩溃
           

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(form1.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            
            //if (!Judger.JudgeLegalMouseDown(e.X, e.Y)) return;

            int PlacementX = (e.X - StandardSize.toLeft) / StandardSize.BlockWidth;      // 求鼠标点击的X方向的第几个点位
            int PlacementY = (e.Y - StandardSize.toTop) / StandardSize.BlockWidth;      // 求鼠标点击的Y方向的第几个点位
            
            if (PlacementX == lastX && PlacementY == lastY)
            {
                return;
            }            
            
            lastX = PlacementX;
            lastY = PlacementY;
            
            state.GetLocalPlayer().UpdatePreviewPlane(PlacementX, PlacementY, nowDir);

            try
            {
                if (!Judger.JudgeLegalPlanePlacement(state.GetLocalPlayer().GetPlanes(), state.GetLocalPlayer().GetPreviewPlane()))
                {
                    lastX = -1;
                    lastY = -1;
                }
                this.Invalidate();
            }
            catch (Exception)
            {

            }
        }
    }
}

