using System;
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

        public void Draw(Panel p)
        {
            Graphics g = p.CreateGraphics();                // 创建面板画布

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;



            int AccurateX = x * StandardSize.BlockWidth + StandardSize.toLeft;
            int AccurateY = y * StandardSize.BlockWidth + StandardSize.toTop;

            int dx = StandardSize.BlockWidth;
            int dy = dx;
            Rectangle[] recs = new Rectangle[12];
            // 0
            recs[0] = new Rectangle(AccurateX, AccurateY - 2 * dy, dx, 5 * dy);  //长横
            recs[1] = new Rectangle(AccurateX - 2 * dx, AccurateY - dy, dx, 3 * dy); //短横
            recs[2] = new Rectangle(AccurateX - 2 * dx, AccurateY, 4 * dx, dy); //一竖
            // 1
            recs[3] = new Rectangle(AccurateX - 2 * dx, AccurateY, 5 * dx, dy);
            recs[4] = new Rectangle(AccurateX - dx, AccurateY - 2 * dy, 3 * dx, dy);
            recs[5] = new Rectangle(AccurateX, AccurateY - 2 * dy, dx, 4 * dy);
            // 2 
            recs[6] = new Rectangle(AccurateX, AccurateY - 2 * dy, dx, 5 * dy);
            recs[7] = new Rectangle(AccurateX + 2 * dx, AccurateY - dy, dx, 3 * dy);
            recs[8] = new Rectangle(AccurateX - dx, AccurateY, 4 * dx, dy);
            // 3
            recs[9] = new Rectangle(AccurateX - 2 * dx, AccurateY, 5 * dx, dy);
            recs[10] = new Rectangle(AccurateX - dx, AccurateY + 2 * dy, 3 * dx, dy);
            recs[11] = new Rectangle(AccurateX, AccurateY - dy, dx, 4 * dy);



            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < 3; i++)
            {
                g.DrawRectangle(pen, recs[direction * 3 + i]);
            }
            //LinearGradientBrush brush=new LinearGradientBrush()
            SolidBrush brush = new SolidBrush(Color.Blue);
            for (int i = 0; i < 3; i++)
            {
                g.FillRectangle(brush, recs[direction * 3 + i]);
            }

            //g.FillEllipse(new SolidBrush(Color.Blue), AccurateX, AccurateY, StandardSize.BlockWidth, StandardSize.BlockWidth);
        }


    }
}
