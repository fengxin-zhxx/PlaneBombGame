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
        public static void Draw(Panel p, int PlacementX, int PlacementY)
        {
            Graphics g = p.CreateGraphics();                // 创建面板画布

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;


            int AccurateX = PlacementX * StandardSize.BlockWidth + StandardSize.toLeft; 
            int AccurateY = PlacementY * StandardSize.BlockWidth + StandardSize.toTop; 

            g.FillEllipse(new SolidBrush(Color.Blue), AccurateX, AccurateY, StandardSize.BlockWidth, StandardSize.BlockWidth);

        }

        public static void ReDraw(Panel p, int[,] CheckBoard)
        {
            
        }
    }
}
