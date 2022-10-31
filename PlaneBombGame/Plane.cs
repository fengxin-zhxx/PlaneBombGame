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
        public static void Draw(Panel p, int PlacementX, int PlacementY) {
            Graphics g = p.CreateGraphics();                // 创建面板画布

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;


            int AccurateX = PlacementX * StandardSize.BlockWidth + StandardSize.toLeft;
            int AccurateY = PlacementY * StandardSize.BlockWidth + StandardSize.toTop;
            int dx = StandardSize.BlockWidth;
            int dy = dx;
            Rectangle rec1 = new Rectangle(AccurateX, AccurateY - 2 * dy, dx, 5 * dy);
            Rectangle rec2 = new Rectangle(AccurateX - 2 * dx, AccurateY - dy, dx, 3 * dy);
            Rectangle rec3 = new Rectangle(AccurateX - 2 * dx, AccurateY, 4 * dx, dy);

            Pen pen = new Pen(Color.Black);
            g.DrawRectangle(pen, rec1);
            g.DrawRectangle(pen, rec2);
            g.DrawRectangle(pen, rec3);

            SolidBrush brush = new SolidBrush(Color.Blue);
            g.FillRectangle(brush, rec1);
            g.FillRectangle(brush, rec2);
            g.FillRectangle(brush, rec3);

            g.FillEllipse(new SolidBrush(Color.Blue), AccurateX, AccurateY, StandardSize.BlockWidth, StandardSize.BlockWidth);

        }

        public static void ReDraw(Panel p, int[,] CheckBoard)
        {
            
        }
    }
}
