using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class Utils
    {

        //将可迭代的planes中每一个plane加入map地图中
        public static void AddPlanesOnMap(int[,] map, IEnumerable planes)
        {
            int[] cnt = new int[] { 0, 2, 0, 1 };
            foreach (Plane plane in planes)
            {
                if (plane == null) continue;
                int px = plane.x, py = plane.y;
                switch (plane.direction)
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
                        for (int i = py - 1, j = 0; i <= py + 2; i++, j++)
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
        }


        //将可迭代的planes中每一个plane的head加入map地图中
        public static void AddPlanesHeadsOnMap(int[,] map, IEnumerable planes)
        {
            foreach(Plane plane in planes)
            {
                AttackPoint atk = GetPlaneHead(plane);
                map[atk.x, atk.y]++;
            }
        }

        public static void ClearInts(int[,] ints, int v1, int v2)
        {
            for(int i=0; i<= v1; i++)
            {
                for(int j = 0; j <= v2; j++)
                {
                    ints[i, j] = 0;
                }
            }
        }

        public static int CalculateValue(int hit,int head,int empty)
        {
            return hit * (head + empty) + head * (hit + empty) + empty * (hit + head);
        }
        // HitCnt 包含 HeadCnt, count = HitCnt + EmptyCnt
        public static int[] FindBest(int[,] HitCnt, int[,] HeadCnt, int count)
        {
            int hitCnt = HitCnt[1, 1] - HeadCnt[1, 1];
            int headCnt = HeadCnt[1, 1];
            int emptyCnt = count - HitCnt[1, 1];

            int bx = 1, by = 1;
            long Max = CalculateValue(hitCnt, headCnt, emptyCnt);
            for(int i = 1; i <= 10; i++)
            {
                for(int j = 1; j <= 10; j++)
                {

                    hitCnt = HitCnt[i, j] - HeadCnt[i, j];
                    headCnt = HeadCnt[i, j];
                    emptyCnt = count - HitCnt[i, j];
                    long delta = CalculateValue(hitCnt, headCnt, emptyCnt);
                    if (delta > Max)
                    {
                        bx = i;
                        by = j;
                        Max = delta;
                    }
                }
            }
            Console.WriteLine(bx + " " + by + " " + Max);
            return new int[] { bx, by };
        }
        public static AttackPoint GetPlaneHead(Plane plane)
        {
            int px = plane.x, py = plane.y;
            switch (plane.direction)
            {
                case 0:
                    return new AttackPoint(px + 1, py);
                case 1:
                    return new AttackPoint(px, py + 1);
                case 2:
                    return new AttackPoint(px - 1, py);
                case 3:
                    return new AttackPoint(px, py - 1);
                default:
                    break;
            }
            return null;
        }
        public static AttackPoint[] GetPlanesHeads(Plane[] planes)
        {
            AttackPoint[] result = new AttackPoint[planes.Length]; 
            int[] cnt = new int[] { 0, 2, 0, 1 };
            for(int i = 0; i < planes.Length; i++)
            {
                Plane plane = planes[i];
                if (plane == null) continue;
                result[i] = GetPlaneHead(plane);
            }
            return result;
        }
    }
}
