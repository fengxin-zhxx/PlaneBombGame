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

        void AddLeftCount();

        void SetAdversaryPlayer(Player player);
        Player GetAdversaryPlayer();
        void SetLocalPlayer(LocalPlayer player);
        LocalPlayer GetLocalPlayer();



        void DrawPlane(Panel panel);  // 绘画LocalPlayer放置的飞机
        
        void DrawPoint(Player player, Player adversaryPlayer, Panel panel);
        //绘画player的攻击点, 以adversaryPlayer的Planes为结果判断依据

    }
}
