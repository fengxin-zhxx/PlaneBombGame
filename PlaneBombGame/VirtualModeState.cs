using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    internal class VirtualModeState : State
    {
        private bool start;     // 游戏是否开始

        private int leftCount; // 已经放置的飞机数

        private LocalPlayer localPlayer;

        private Player adversaryPlayer;

        public void AddLeftCount()
        {
            throw new NotImplementedException();
        }

        public void DrawLastPoint(Player player, Player adversaryPlayer, Panel panel)
        {
            ArrayList ah = player.GetAttackHistory();
            AttackPoint a = (AttackPoint)ah[ah.Count - 1];
            string attackRes = Judger.JudgeAttack(adversaryPlayer, a);
            switch (attackRes)
            {
                case "HIT":
                    a.Draw(panel, Color.Green);
                    break;
                case "KILL":
                    a.Draw(panel, Color.Red);
                    break;
                case "MISS":
                    a.Draw(panel, Color.Gray);
                    break;
            }
        }

        public void DrawPlane(Panel panel)
        {
            Plane[] planes = localPlayer.GetPlanes();
            foreach(Plane plane in planes)
            {
                if(plane != null)   
                    plane.Draw(panel);
            }

        }
        public void DrawPoint(Player player, Player adversaryPlayer, Panel panel)
        {
            foreach(AttackPoint a in player.GetAttackHistory())
            {
                string attackRes = Judger.JudgeAttack(adversaryPlayer, a);
                switch (attackRes)
                {
                    case "HIT":
                        a.Draw(panel, Color.Green);
                        break;
                    case "KILL":
                        a.Draw(panel, Color.Red);
                        break;
                    case "MISS":
                        a.Draw(panel, Color.Gray);
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

        public void Init()
        {

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
    }
}
