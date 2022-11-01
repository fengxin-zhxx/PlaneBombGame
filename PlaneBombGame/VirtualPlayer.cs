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
        ArrayList al = new ArrayList();
        Random r = new Random(); // 以当前时间为随机数种子


        public int[] NextAttack()
        {
            int[] res = new int[2];
            res[0] = r.Next(1,10);
            res[1] = r.Next(1,10);
            return res;
        }

        public Plane[] GeneratePlanes()
        {
            Plane[] res = new Plane[3];
            for(int i = 0; i < 3; i++)
            {
                res[i] = new Plane(r.Next(1, 10), r.Next(1, 10), 0);
            }
            // TODO 
            return res;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public Plane[] GetPlanes()
        {
            return planes;
        }

        public ArrayList GetAttackHistory()
        {
            return al;
        }

        public void SetPlanes()
        {
            this.planes = GeneratePlanes();
        }

        public ArrayList GetResultHistory()
        {
            throw new NotImplementedException();
        }
    }
}
