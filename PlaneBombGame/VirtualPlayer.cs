using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class VirtualPlayer : Player
    {

        Plane[] planes { get; set; }

        ArrayList attackHistory = new ArrayList();

        Random r = new Random(); // 以当前时间为随机数种子


        public AttackPoint NextAttack()
        {
            /*TO DO*/
            // AI PART    
            return new AttackPoint(r.Next(1, 10), r.Next(1, 10));
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

        public void AddAttackPoint(AttackPoint attackPoint)
        {
            attackHistory.Add(attackPoint);
        }

        //用于随机生成三架飞机
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
