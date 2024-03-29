﻿using System;
using System.Collections;

namespace PlaneBombGame
{
    internal class RandomVirtualPlayer : Player
    {

        Plane[] planes { get; set; }

        ArrayList attackHistory = new ArrayList();

        Random r = new Random(); // 以当前时间为随机数种子


        public AttackPoint NextAttack()
        {
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

        public void AddAttackPoint(AttackPoint attackPoint, string res = "")
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

        public void Init()
        {
            throw new NotImplementedException();
        }

        public AiVirtualPlayer GetAiAssistantPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
