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
        ArrayList PlanesTmp = new ArrayList();
        Plane[] planes { get; set; }
        ArrayList al = new ArrayList();
        public ArrayList GetAttackHistory()
        {
            return al;
        }

        public Plane[] GetPlanes()
        {
            return planes;
        }

        public void Init()
        {

        }

        public int[] NextAttack()
        {
            throw new NotImplementedException();
        }

        public void SetPlanes(Plane[] planes)
        {
            this.planes = planes;
        }
        public ArrayList GetPlaneTmp()
        {
            return PlanesTmp;
        }
        public void AddPlane(Plane plane)
        {
            PlanesTmp.Add(plane);
        }
        
    }
}
