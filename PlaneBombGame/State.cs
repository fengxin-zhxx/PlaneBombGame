using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    internal interface State
    {
        void Init();

        void SetLeftCount(int leftCount);
        int GetLeftCount();

        void SetAdversaryPlayer(Player player);
        Player GetAdversaryPlayer();
        void SetLocalPlayer(LocalPlayer player);
        LocalPlayer GetLocalPlayer();
        void Draw(Panel panel); // 绘画LocalPlayer放置的飞机
    }
}
