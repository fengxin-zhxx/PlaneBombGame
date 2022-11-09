using System;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class AiAssistant
    {
        AiVirtualPlayer aiPlayer;
        public AiAssistant()
        {
            aiPlayer = new AiVirtualPlayer();
            aiPlayer.Init();
        }
        public AiVirtualPlayer GetAiVirtualPlayer()
        {
            return aiPlayer;
        }
        public void AddAttackPoint(AttackPoint attackPoint, string res = "")
        {
            aiPlayer.AddAttackPoint(attackPoint,res);
        }

        //随机返回一种当前可行方案
        public Plane[] GetOneLegalPlacement()
        {
            return null;
        }

        

    }
}
