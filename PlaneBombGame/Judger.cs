using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    internal class Judger
    {
        static string HIT = "HIT";
        static string MISS = "MISS";
        static string KILL = "KILL";
        public static string JudgeAttack(Player player, int x, int y)
        {
            Plane[] planes = player.GetPlanes();
            string msg = "ATTACK AT:" + x + " "+ y + "\n";
            for(int i = 0; i < planes.Length; i++)
            {
                //MessageBox.Show(planes[i].x + " " + planes[i].y);
                msg += "PLANE AT: " + planes[i].x + " " + planes[i].y + "\n";
                int dir = planes[i].direction;
                switch (dir)
                {
                    case 0:
                        if (x == planes[i].x && y >= planes[i].y - 2 && y <= planes[i].y + 2) return HIT;
                        if (x == planes[i].x - 1 && y == planes[i].y) return HIT;
                        if (x == planes[i].x + 1 && y == planes[i].y) return KILL;
                        if (x == planes[i].x - 2 && y >= planes[i].y - 1 && y <= planes[i].y + 1) return HIT;
                        break;
                    case 1:break;
                    case 2:break;
                    case 3:break;
                    default:break;
                }
            }
            //MessageBox.Show(msg);
            return MISS;  
        }
        public static bool JudgePlaneOverlap(Plane p1, Plane p2)
        {

            return false;
        }
        public static bool JudgeLegalPlanePlacement(Player player, Plane plane)
        {
            if (plane == null) return false;
            if (player == null) return false;
            foreach(Plane playerPlane in player.GetPlanes())
            {
                if (Judger.JudgePlaneOverlap(plane, playerPlane))
                {
                    return false;
                }
            }
            switch (plane.direction)
            {
                case 0:

                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
            return true;
        }
        public static bool JudgeLegalPlacement(Player player, int x, int y)
        {
            /*TO DO*/
            return true;
        }
    
        public static bool JudgeLegalMouseDown(MouseEventArgs e)
        {
            if (   e.Y < StandardSize.toTop 
                || e.X < StandardSize.toLeft 
                || e.Y >= StandardSize.toTop + (StandardSize.BlockNum + 1) * StandardSize.BlockWidth 
                || e.X >= StandardSize.toLeft + (StandardSize.BlockNum + 1) * StandardSize.BlockWidth
                ) return false;
            return true;
        }
    }
}
