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



        void SetPlanes(); 
        // 对于VirtualPlayer: 生成对应的Planes
        // 对于SocketPlayer:  接收对方放置的Planes



        ArrayList GetAttackHistory();
        ArrayList GetResultHistory();
    }
}
