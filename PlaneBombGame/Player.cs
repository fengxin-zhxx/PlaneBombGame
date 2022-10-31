using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal interface Player
    {

        void Init();
        int[] NextAttack();

        Plane[] GetPlanes();
        void SetPlanes(Plane[] planes);

        ArrayList GetAttackHistory();

    }
}
