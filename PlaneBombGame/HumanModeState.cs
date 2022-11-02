using PlaneBombGame;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections;

internal class HumanModeState : State

{
    private int leftCount; // 已经放置的飞机数

    private LocalPlayer localPlayer;

    private Player adversaryPlayer;


    public void DrawPlane(Panel panel)
    {
        Plane[] planes = localPlayer.GetPlanes();
        foreach (Plane plane in planes)
        {
            if (plane != null)
                plane.Draw(panel);
        }
    }

    //第一个参数为攻击方  第二个参数为受击方 绘制第一个对第二个的伤害点
    public void DrawPoint(Player player, Player adversaryPlayer, Panel panel)
    {
        //遍历自身攻击过的点
        foreach (AttackPoint a in player.GetAttackHistory())
        {
            //判断攻击点对对手的伤害
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
}