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
        void SetLeftCount(int leftCount);

        int GetLeftCount();

        void SetAdversaryPlayer(Player player);
        Player GetAdversaryPlayer();
        void SetLocalPlayer(LocalPlayer player);
        LocalPlayer GetLocalPlayer();
        
        // 绘画LocalPlayer放置的飞机
        void DrawPlane(Panel panel);
        
        //绘画player的攻击点,local和adversay调用的都是这个
        void DrawPoint(Player player, Player adversaryPlayer, Panel panel);        
    }
}
