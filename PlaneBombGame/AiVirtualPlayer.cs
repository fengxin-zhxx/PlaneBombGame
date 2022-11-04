using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PlaneBombGame
{
    internal class AiVirtualPlayer : Player
    {

        Plane[] planes;

        ArrayList attackHistory = new ArrayList();
        Random r = new Random(); // 以当前时间为随机数种子

        ArrayList VectorStore = new ArrayList();
        int[,] nowMap = new int[11, 11];
        int[,] nowCnt = new int[11, 11];
        // 0 UNKNOWN   1  MISS   2  HIT  3 KILL

        /*int Rest = 3;                                           //剩余飞机数量
        int[] X = new int[3], Y = new int[3], D = new int[3];   // X, Y, D 记录 DFS 过程中, 飞机存放的位置 
        int Cnt = 0;       // Cnt 表示合法的方案数 
        void DFS(int x, int y)
        {
            if (Rest == 0)
            {
                Cnt++;
                Plane[] planes = new Plane[3];
                for (int i = 0; i < 3; i++)
                {
                    planes[i] = new Plane(X[i], Y[i], D[i]);
                }
                VectorStore.Add(planes);
                return;
            }
            if (x == 10) return;
            for (int i = 0; i <= 3; i++)
            { 
                if (Judger.JudgeLegalPlanePlacement(X, Y, D, x, y, i))
                {
                    Rest--;
                    X[Rest] = x;
                    Y[Rest] = y;
                    D[Rest] = i;
                    if (y == 9)
                    {
                        DFS(x + 1, 1);
                    }
                    else
                    {
                        DFS(x, y + 1);
                    }
                    D[Rest] = -1;
                    Rest++;
                }
            }
            if (y == 9) DFS(x + 1, 1);
            else DFS(x, y + 1);
        }
*/
        public void Init()
        {
            //for (int i = 0; i < 3; i++) D[i] = -1;
            //DFS(1, 1);
            string str = global::PlaneBombGame.Properties.Resources.All;
            string[] lines = str.Split('\n');
            foreach(string line in lines)
            {
                string[] strs = line.Split(' ');
                if (strs.Length != 9) continue;
                int[] nums = Array.ConvertAll<string, int>(strs, s => int.Parse(s));
                Plane[] planes = new Plane[] { new Plane(nums[0], nums[1], nums[2]), new Plane(nums[3], nums[4], nums[5]), new Plane(nums[6], nums[7], nums[8]) };
                VectorStore.Add(planes);
            }
        }
        public AttackPoint NextAttack()
        {
            int cnt = attackHistory.Count;
            ArrayList ResVector = new ArrayList();
            // 剩余的飞机方案

            Utils.ClearInts(nowCnt, 10, 10);
            foreach(Plane[] vectorPlanes in VectorStore)
            {
                if (Judger.JudgeLegalPlanePlacement(nowMap, vectorPlanes)) // 如果当前方案符合
                {
                    ResVector.Add(vectorPlanes);
                    Utils.AddPlanesOnMap(nowCnt, vectorPlanes);
                }
            }
            VectorStore = ResVector;
            int count = VectorStore.Count;
            if(count == 1)
            {
                AttackPoint[] atks = Utils.GetPlaneHeads((Plane[])VectorStore[0]);
                foreach(AttackPoint atk in atks)
                {
                    if (nowMap[atk.x, atk.y] == 0)
                    {
                        return atk;
                    }
                }
                MessageBox.Show("AI has won the game.");
                throw new Exception("");
            }
            else
            {
                int[] res = Utils.FindBest(nowCnt, VectorStore.Count);
                return new AttackPoint(res[0], res[1]);
            }
        }

        public Plane[] GetPlanes()
        {
            return planes;
        }

        public ArrayList GetAttackHistory()
        {
            return attackHistory;
        }

        public void SetPlanes(Plane[] planes)
        {    
            this.planes = GeneratePlanes();
        }

        public void AddAttackPoint(AttackPoint attackPoint, string res = "")
        {
            attackHistory.Add(attackPoint);
            int resNum = 0;
            switch (res)
            {
                case "MISS":
                    resNum = 1;
                    break;
                case "HIT":
                    resNum = 2;
                    break;
                case "KILL":
                    resNum = 3;
                    break;
                default:
                    break;
            }
            nowMap[attackPoint.x, attackPoint.y] = resNum;
        }

        //TO DO 放置飞机策略(?) 随机是不是也可以?
        public Plane[] GeneratePlanes()
        {
            Plane[] res = new Plane[3];
            for (int i = 0; i < 3; i++)
            {
                Plane plane = new Plane(r.Next(2, 9), r.Next(2, 9), r.Next(0, 4));
                if (!Judger.JudgeLegalPlanePlacement(res, plane))
                {
                    i--;
                    continue;
                }
                res[i] = plane;
            }
            return res;
        }
    }
}
