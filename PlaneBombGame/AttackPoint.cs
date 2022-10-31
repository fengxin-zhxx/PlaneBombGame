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
    internal class AttackPoint
    {
        public int x { get; set; }
        public int y { get; set; }

        public AttackPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Draw(Panel p, Color color)
        {
            Graphics g = p.CreateGraphics();                // 创建面板画布

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;


            int AccurateX = x * StandardSize.BlockWidth + StandardSize.toLeft;
            int AccurateY = y * StandardSize.BlockWidth + StandardSize.toTop;

            g.FillEllipse(new SolidBrush(color), AccurateX, AccurateY, StandardSize.BlockWidth, StandardSize.BlockWidth);

        }
    }
}
