﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    internal class Plane
    {

        public int x {get; set;}
        public int y {get; set;}

        public int direction { get; set; }
        public Plane(int x, int y,int direction)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
        }

        //加入飞机方向判断
        public void Draw(Panel p) {
            Graphics g = p.CreateGraphics();                // 创建面板画布

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;



            int AccurateX = x * StandardSize.BlockWidth + StandardSize.toLeft;
            int AccurateY = y * StandardSize.BlockWidth + StandardSize.toTop;

            int dx = StandardSize.BlockWidth;
            int dy = dx;
            Rectangle[] recs = new Rectangle[12];
            // Ellipse[] ells = new Ellipse[4];
            // 0
            recs[0] = new Rectangle(AccurateX, AccurateY - dy, dx, 3 * dy);  //长横
            recs[1] = new Rectangle(AccurateX - 2 * dx, AccurateY, dx, dy); //短横
            recs[2] = new Rectangle(AccurateX - 2 * dx, AccurateY, 3 * dx, dy); //一竖
            // 1
            recs[3] = new Rectangle(AccurateX - dx, AccurateY, 3 * dx, dy);
            recs[4] = new Rectangle(AccurateX, AccurateY - 2 * dy, dx, dy);
            recs[5] = new Rectangle(AccurateX, AccurateY - 2 * dy, dx, 3 * dy);
            // 2 
            recs[6] = new Rectangle(AccurateX, AccurateY - dy, dx, 3 * dy);
            recs[7] = new Rectangle(AccurateX + 2 * dx, AccurateY, dx, dy);
            recs[8] = new Rectangle(AccurateX, AccurateY, 3 * dx, dy);
            // 3
            recs[9] = new Rectangle(AccurateX - dx, AccurateY, 3 * dx, dy);
            recs[10] = new Rectangle(AccurateX, AccurateY + 2 * dy, dx, dy);
            recs[11] = new Rectangle(AccurateX, AccurateY, dx, 3 * dy);


            //direction = 3;
            /*Pen pen = new Pen(Color.Black);
            for (int i = 0; i < 3; i++) {
                g.DrawRectangle(pen, recs[direction * 3 + i]);
            }*/
            SolidBrush brush = new SolidBrush(Color.SteelBlue);
            for (int i = 0; i < 3; i++) {
                g.FillRectangle(brush, recs[direction * 3 + i]);
            }

            Rectangle[] rc = new Rectangle[20];

            //头
            rc[0] = new Rectangle(AccurateX, AccurateY, 2 * dx, dy);
            rc[5] = new Rectangle(AccurateX, AccurateY, dx, 2 * dy);
            rc[10] = new Rectangle(AccurateX - dx, AccurateY, 2 * dx, dy);
            rc[15] = new Rectangle(AccurateX, AccurateY - dy, dx, 2 * dy);

            //0
            rc[1] = new Rectangle(AccurateX, AccurateY - 2 * dy, dx, 2 * dy);
            rc[2] = new Rectangle(AccurateX, AccurateY + dy, dx, 2 * dy);
            rc[3] = new Rectangle(AccurateX - 2 * dx, AccurateY - dy, dx, 2 * dy);
            rc[4] = new Rectangle(AccurateX - 2 * dx, AccurateY, dx, 2 * dy);
            //1
            rc[6] = new Rectangle(AccurateX + dx, AccurateY, 2 * dx, dy);
            rc[7] = new Rectangle(AccurateX - 2 * dx, AccurateY, 2 * dx, dy);
            rc[8] = new Rectangle(AccurateX - dx, AccurateY - 2 * dy, 2 * dx, dy);
            rc[9] = new Rectangle(AccurateX, AccurateY - 2 * dy, 2 * dx, dy);
            //2
            rc[11] = rc[1];
            rc[12] = rc[2];
            rc[13] = new Rectangle(AccurateX + 2 * dx, AccurateY - dy, dx, 2 * dy);
            rc[14] = new Rectangle(AccurateX + 2 * dx, AccurateY, dx, 2 * dy);
            //3
            rc[16] = rc[6];
            rc[17] = rc[7];
            rc[18] = new Rectangle(AccurateX - dx, AccurateY + 2 * dy, 2 * dx, dy);
            rc[19] = new Rectangle(AccurateX, AccurateY + 2 * dy, 2 * dx, dy);




            SolidBrush brush2 = new SolidBrush(Color.Yellow);

            for (int i = 0; i < 5; i++) {
                g.FillEllipse(brush, rc[5 * direction + i]);
            }

            //Bitmap bt = new Bitmap("D:\\桌面\\大学\\大三上\\C#\\蓝色飞机.png");
            //TextureBrush tb = new TextureBrush(bt);
            //for(int i = 0; i < 3; i++) {
            //    g.FillRectangle(tb, recs[direction * 3 + i]);
            //}

            //g.FillEllipse(new SolidBrush(Color.Blue), AccurateX, AccurateY, StandardSize.BlockWidth, StandardSize.BlockWidth);
        }


    }
}