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

        internal static int[] FindBest(int[,] cnt, int count)
        {
            int bx = 1, by = 1, Min = Math.Abs(cnt[1, 1] * 2 - count);
            for(int i = 1; i <= 10; i++)
            {
                for(int j = 1; j <= 10; j++)
                {
                    int delta = Math.Abs(cnt[i, j] * 2 - count);
                    if (delta < Min)
                    {
                        bx = i;
                        by = j;
                        Min = delta;
                    }
                }
            }
            Console.WriteLine(bx + " " + by + " " + Min);
            return new int[] { bx, by };
        }

        internal static AttackPoint[] GetPlaneHeads(Plane[] planes)
        {
            AttackPoint[] result = new AttackPoint[planes.Length]; 
            int[] cnt = new int[] { 0, 2, 0, 1 };
            for(int i = 0; i < planes.Length; i++)
            {
                Plane plane = planes[i];
                if (plane == null) continue;
                int px = plane.x, py = plane.y;
                switch (plane.direction)
                {
                    case 0:
                        result[i] = new AttackPoint(px + 1, py);
                        break;
                    case 1:
                        result[i] = new AttackPoint(px, py + 1);
                        break;
                    case 2:
                        result[i] = new AttackPoint(px + 1, py);
                        break;
                    case 3:
                        result[i] = new AttackPoint(px, py - 1);
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
