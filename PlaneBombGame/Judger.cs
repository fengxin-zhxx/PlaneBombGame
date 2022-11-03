using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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
        public static string JudgeAttack(Player player, AttackPoint attackPoint)
        {
            int x = attackPoint.x;
            int y = attackPoint.y;
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
                    case 1:
                        if (y == planes[i].y && x >= planes[i].x - 2 && x <= planes[i].x + 2) return HIT;
                        if (y == planes[i].y - 1 && x == planes[i].x) return HIT;
                        if (y == planes[i].y + 1 && x == planes[i].x) return KILL;
                        if (y == planes[i].y - 2 && x >= planes[i].x - 1 && x <= planes[i].x + 1) return HIT;
                        break;
                    case 2:
                        if (x == planes[i].x && y >= planes[i].y - 2 && y <= planes[i].y + 2) return HIT;
                        if (x == planes[i].x + 1 && y == planes[i].y) return HIT;
                        if (x == planes[i].x - 1 && y == planes[i].y) return KILL;
                        if (x == planes[i].x + 2 && y >= planes[i].y - 1 && y <= planes[i].y + 1) return HIT;
                        break;
                    case 3:
                        if (y == planes[i].y && x >= planes[i].x - 2 && x <= planes[i].x + 2) return HIT;
                        if (y == planes[i].y + 1 && x == planes[i].x) return HIT;
                        if (y == planes[i].y - 1 && x == planes[i].x) return KILL;
                        if (y == planes[i].y + 2 && x >= planes[i].x - 1 && x <= planes[i].x + 1) return HIT;
                        break;
                    default:break;
                }
            }
            //MessageBox.Show(msg);
            return MISS;  
        }
        public static bool JudgeLegalPlaneCoordinate(Plane plane)
        {
            switch (plane.direction)
            {
                case 0:
                    if (plane.x - 2 < 1 || plane.x + 1 > StandardSize.BlockNum) return false;
                    if (plane.y - 2 < 1 || plane.y + 2 > StandardSize.BlockNum) return false;
                    break;
                case 1:
                    if (plane.x - 2 < 1 || plane.x + 2 > StandardSize.BlockNum) return false;
                    if (plane.y - 2 < 1 || plane.y + 1 > StandardSize.BlockNum) return false;
                    break;
                case 2:
                    if (plane.x - 1 < 1 || plane.x + 2 > StandardSize.BlockNum) return false;
                    if (plane.y - 2 < 1 || plane.y + 2 > StandardSize.BlockNum) return false;
                    break;
                case 3:
                    if (plane.x - 2 < 1 || plane.x + 2 > StandardSize.BlockNum) return false;
                    if (plane.y - 1 < 1 || plane.y + 2 > StandardSize.BlockNum) return false;
                    break;
                default:
                    break;
            }
            return true;
        }
        public static bool JudgePlaneOverlap(Plane[] planes, Plane plane)
        {
            int[,] map = new int[11, 11];
            int[] cnt = new int[] { 0, 2, 0, 1 };
            ArrayList al = new ArrayList();
            foreach(Plane p in planes)
            {
                if (p != null)
                {
                    al.Add(p);
                }
            }
            al.Add(plane);
            foreach(Plane playerPlane in al)
            {
                if (playerPlane == null) break;
                int px = playerPlane.x, py = playerPlane.y;
                switch (playerPlane.direction)
                {
                    case 0:
                        for (int i = px + 1, j = 0; i >= px - 2; i--, j++)
                        {
                            for (int k = py - cnt[j]; k <= py + cnt[j]; k++)
                            {
                                map[i, k]++;
                            }
                        }
                        break;
                    case 1:
                        for (int i = py + 1, j = 0; i >= py - 2; i--, j++)
                        {
                            for (int k = px - cnt[j]; k <= px + cnt[j]; k++)
                            {
                                map[k, i]++;
                            }
                        }
                        break;
                    case 2:
                        for (int i = px - 1, j = 0; i <= px + 2; i++, j++)
                        {
                            for (int k = py - cnt[j]; k <= py + cnt[j]; k++)
                            {
                                map[i, k]++;
                            }
                        }
                        break;
                    case 3:
                        for(int i = py - 1, j = 0; i <= py + 2; i++, j++)
                        {
                            for (int k = px - cnt[j]; k <= px + cnt[j]; k++)
                            {
                                map[k, i]++;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            for(int i = 0; i <= StandardSize.BlockNum; i++)
            {   
                for(int j = 0; j <= StandardSize.BlockNum; j++)
                {
                    if (map[i, j] > 1) return false;
                }
            }
            return true;
        }
        public static bool JudgeLegalPlanePlacement(Plane[] planes, Plane plane)
        {
            if (plane == null) return false;
            if (planes == null) return false;

            if (!JudgeLegalPlaneCoordinate(plane)) return false;
            if (!JudgePlaneOverlap(planes, plane)) return false;
            return true;
        }
        public static bool JudgeLegalPlacement(Player player, int x, int y)
        {
            foreach(AttackPoint a in player.GetAttackHistory())
            {
                if (a.x == x && a.y == y) return false;
            }
            return true;
        }
    

        public static bool JudgeLegalMouseDown(int X, int Y)
        {
            if (Y < StandardSize.toTop + +StandardSize.BlockWidth
                || X < StandardSize.toLeft + +StandardSize.BlockWidth
                || Y >= StandardSize.toTop + (StandardSize.BlockNum + 1) * StandardSize.BlockWidth
                || X >= StandardSize.toLeft + (StandardSize.BlockNum + 1) * StandardSize.BlockWidth
                ) return false;
            return true;
        }
    }
}
