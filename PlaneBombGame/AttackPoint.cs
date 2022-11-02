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

            /*TO DO color*/
            int flag = 0;
            if (color == Color.Red) flag = 1;
            else if (color == Color.Green) flag = 2;

            Graphics g = p.CreateGraphics();                // 创建面板画布

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;



            int AccurateX = x * StandardSize.BlockWidth + StandardSize.toLeft;
            int AccurateY = y * StandardSize.BlockWidth + StandardSize.toTop;

            Bitmap bt1 = global::PlaneBombGame.Properties.Resources.击中;
            Bitmap bt2 = global::PlaneBombGame.Properties.Resources.爆炸;
            SolidBrush brush = new SolidBrush(Color.Black);

            //TextureBrush tb = new TextureBrush(bt);

            Rectangle rc = new Rectangle(AccurateX, AccurateY, StandardSize.BlockWidth, StandardSize.BlockWidth);


            if (flag == 2) g.DrawImage(bt1, rc);
            else if (flag == 1) g.DrawImage(bt2, rc);
            else {
                RectangleF recf = new RectangleF(AccurateX + 6, AccurateY + 18, StandardSize.BlockWidth - 10, StandardSize.BlockWidth - 10);
                g.DrawString("MISS", new System.Drawing.Font("Viner Hand ITC", 12F), brush, recf);
            }

            //g.FillEllipse(new SolidBrush(color), AccurateX, AccurateY, StandardSize.BlockWidth, StandardSize.BlockWidth);

        }
    }
}
