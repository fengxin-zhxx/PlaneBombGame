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

        public Plane[] GetPlanes()
        {
            if (planes == null)
            {
                planes = new Plane[3];
            }
            return planes;
        }

        public void Init()
        {

        }

        public AttackPoint NextAttack()
        {
            throw new NotImplementedException();
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

        public void SetPlanes()
        {
            throw new NotImplementedException();
        }

        public void AddAttackPoint(AttackPoint attackPoint)
        {
            attackHistory.Add(attackPoint); 
        }
    }
}
