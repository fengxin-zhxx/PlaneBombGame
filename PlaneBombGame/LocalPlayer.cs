using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class LocalPlayer : Player
    {
        Plane[] planes { get; set; }

        ArrayList attackHistory = new ArrayList();
        public ArrayList GetAttackHistory()
        {
            return attackHistory;
        }
        public AttackPoint NextAttack()
        {
            return new AttackPoint(1, 1);
        }

        public Plane[] GetPlanes()
        {
            if (planes == null)
            {
                planes = new Plane[3];
            }
            return planes;
        }

        public void SetOnePlane(Plane plane, int index)
        {
            if(planes == null)
            {
                planes = new Plane[3];
                planes[0] = plane;
                return;
            }
            
            planes[index] = plane;
        }

        public void AddAttackPoint(AttackPoint attackPoint)
        {
            attackHistory.Add(attackPoint); 
        }


        public void SetPlanes(Plane[] planes)
        {
        }

    }
}
