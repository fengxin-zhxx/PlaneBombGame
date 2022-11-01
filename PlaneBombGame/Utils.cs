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
        public static Plane[] Transform(ArrayList al)
        {
            Plane[] planes = new Plane[al.Count];
            for(int i = 0; i < al.Count; i++)
            {
                planes[i] = (Plane)al[i];
            }
            return planes;
        }
    }
}
