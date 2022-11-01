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
        ArrayList al = new ArrayList();
        public ArrayList GetAttackHistory()
        {
            return al;
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

        public int[] NextAttack()
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

        public ArrayList GetResultHistory()
        {
            throw new NotImplementedException();
        }
    }
}
