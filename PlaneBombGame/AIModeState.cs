using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class AIModeState : State
    {
        private AiVirtualPlayer AiPlayer_1;

        private AiVirtualPlayer AiPlayer_2;


        public void setFirstAiPlayer(AiVirtualPlayer player)
        {
            AiPlayer_1 = player;
            player.Init();
        }

        public void setSecondAiPlayer(AiVirtualPlayer player)
        {
            AiPlayer_2 = player;
            player.Init();
        }

        public AiVirtualPlayer getFirstAiPlayer()
        {
            return AiPlayer_1;
        }

        public AiVirtualPlayer getSecondAiPlayer()
        {
            return AiPlayer_2;
        }

        public string DrawLastPoint(AttackPoint a, Player adversaryPlayer, Graphics g)
        {
            string attackRes = Judger.JudgeAttack(adversaryPlayer, a);
            switch (attackRes)
            {
                case "HIT":
                    a.Draw(g, Color.Green);
                    break;
                case "KILL":
                    a.Draw(g, Color.Red);
                    break;
                case "MISS":
                    a.Draw(g, Color.Gray);
                    break;
            }
            return attackRes;
        }

        public void DrawPlane(Graphics g)
        {
            throw new NotImplementedException();
        }

        public void DrawPlane(Graphics g, bool threeOrFour)
        {
            if (threeOrFour)
            {
                Plane[] planes = AiPlayer_1.GetPlanes();
                foreach (Plane plane in planes)
                {
                    if (plane != null)
                        plane.Draw(g);
                }
            }
            else
            {
                Plane[] planes = AiPlayer_2.GetPlanes();
                foreach (Plane plane in planes)
                {
                    if (plane != null)
                        plane.Draw(g);
                }
            }
        }

        public void DrawPoint(Player player, Player adversaryPlayer, Graphics g)
        {
            //遍历自身攻击过的点
            foreach (AttackPoint a in player.GetAttackHistory())
            {
                //判断攻击点对对手的伤害
                string attackRes = Judger.JudgeAttack(adversaryPlayer, a);
                switch (attackRes)
                {
                    case "HIT":
                        a.Draw(g, Color.Green);
                        break;
                    case "KILL":
                        a.Draw(g, Color.Red);
                        break;
                    case "MISS":
                        a.Draw(g, Color.Gray);
                        break;
                }
            }
        }

        public Player GetAdversaryPlayer()
        {
            throw new NotImplementedException();
        }

        public int GetLeftCount()
        {
            throw new NotImplementedException();
        }

        public LocalPlayer GetLocalPlayer()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void SetAdversaryPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void SetLeftCount(int leftCount)
        {
            throw new NotImplementedException();
        }

        public void SetLocalPlayer(LocalPlayer player)
        {
            throw new NotImplementedException();
        }


    }
}
