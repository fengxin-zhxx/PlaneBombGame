using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    internal class PlayingBoard
    {
        public static void DrawCB(Graphics g)  //传入画布对象
        {
            int BlockWidth = StandardSize.BlockWidth;                   // 格子宽度
            int BlockNum = StandardSize.BoardWidth / BlockWidth - 1;    // 地图格子数量

            int toLeft = StandardSize.toLeft;
            int toTop = StandardSize.toTop;

            g.Clear(Color.Bisque);                               // 清除画布、并用Bisque颜色填满画布
            Pen pen = new Pen(Color.FromArgb(192, 166, 107));    // 实例化画笔
            // 画棋盘
            for (int i = 0; i < BlockNum + 1; i++)
            {
                g.DrawLine(pen, new Point(toLeft, i * BlockWidth + toTop), new Point(BlockWidth * BlockNum + toLeft, i * BlockWidth + toTop)); // 横线
                g.DrawLine(pen, new Point(i * BlockWidth + toLeft, toTop), new Point(i * BlockWidth + toLeft, BlockWidth * BlockNum + toTop)); // 竖线
            }

            SolidBrush brush = new SolidBrush(Color.Black);

            Rectangle rec = new Rectangle(toLeft, toTop, BlockWidth, BlockWidth);
            Bitmap bt = global::PlaneBombGame.Properties.Resources.icon_已去底_;
            g.DrawImage(bt, rec);


            //g.DrawString("MISS", new System.Drawing.Font("Viner Hand ITC", 12F), brush, recf);
            for (int i = 1; i <= 10; i++) {
                RectangleF recf1 = new RectangleF(toLeft + i * BlockWidth + 18, toTop + 15, BlockWidth, BlockWidth);
                g.DrawString(i.ToString(), new System.Drawing.Font("Viner Hand ITC", 20F), brush, recf1);
                RectangleF recf2 = new RectangleF(toLeft + 20, toTop + i * BlockWidth + 15, BlockWidth, BlockWidth);
                g.DrawString(i.ToString(), new System.Drawing.Font("Viner Hand ITC", 20F), brush, recf2);
            }
        }
    }
}
