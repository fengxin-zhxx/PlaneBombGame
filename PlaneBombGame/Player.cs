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

        //void Init();
        AttackPoint NextAttack();

        Plane[] GetPlanes();

        //void SetPlanes();

        void SetPlanes(Plane[] planes);

        // 对于VirtualPlayer: 生成对应的Planes
        // 对于SocketPlayer:  接收对方放置的Planes

        void AddAttackPoint(AttackPoint attackPoint);
        ArrayList GetAttackHistory();
    }
}
