using System;
using System.Collections.Generic;
using System.Drawing;
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
        
        void DrawPoint(Player player, Player adversaryPlayer, Graphics g);
        //绘画player的攻击点, 以adversaryPlayer的Planes为结果判断依据

        string DrawLastPoint(AttackPoint a, Player adversaryPlayer, Graphics g);
        //绘画player的攻击点, 以adversaryPlayer的Planes为结果判断依据, 并返回攻击的结果

        void DrawPlane(Graphics g);
        // 绘画LocalPlayer放置的飞机
        
    }
}
