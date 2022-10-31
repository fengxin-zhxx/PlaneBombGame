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

        public static bool JudgeLegalPlanePlacement(Player player, int x, int y, int diraction)
        {
            return true;
        }
        public static bool JudgeLegalPlacement(Player player, int x, int y)
        {
            /*TO DO*/
            return true;
        }
    
    }
}
