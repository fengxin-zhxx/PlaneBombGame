using System;
using System.Collections.Generic;
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

        public void Draw(Panel panel)
        {
            Plane[] planes = localPlayer.GetPlanes();
            foreach(Plane plane in planes)
            {
                if(plane != null)   
                    plane.Draw(panel);
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
