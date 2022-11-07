using PlaneBombGame;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections;


namespace PlaneBombGame
{
    internal class HumanModeState : State

    {
        private int leftCount; // 已经放置的飞机数

        private LocalPlayer localPlayer;

        private Player adversaryPlayer;


        public void DrawPlane(Graphics g)
        {
            Plane[] planes = localPlayer.GetPlanes();
            foreach (Plane plane in planes)
            {
                if (plane != null)
                    plane.Draw(g);
            }
        }

        //第一个参数为攻击方  第二个参数为受击方 绘制第一个对第二个的伤害点
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
            return adversaryPlayer;
        }

        public int GetLeftCount()
        {
            return leftCount;
        }

        public LocalPlayer GetLocalPlayer()
        {
            return localPlayer;
        }

        public void SetAdversaryPlayer(Player player)
        {
            adversaryPlayer = player;
        }

        public void SetLeftCount(int leftCount)
        {
            this.leftCount = leftCount;
        }

        public void SetLocalPlayer(LocalPlayer player)
        {
            this.localPlayer = player;
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
            // TO DO
            return attackRes;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void setFirstAiPlayer(AiVirtualPlayer player)
        {
            throw new NotImplementedException();
        }

        public void setSecondAiPlayer(AiVirtualPlayer player)
        {
            throw new NotImplementedException();
        }

        public AiVirtualPlayer getFirstAiPlayer()
        {
            throw new NotImplementedException();
        }

        public AiVirtualPlayer getSecondAiPlayer()
        {
            throw new NotImplementedException();
        }

        public void DrawPlane(Graphics g, bool threeOrFour)
        {
            throw new NotImplementedException();
        }
    }
}