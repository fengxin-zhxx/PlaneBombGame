using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class HumanPlayer : Player
    {
        Plane[] planes { get; set; }

        ArrayList attackHistory = new ArrayList();

        public Plane[] GetPlanes()
        {
            return planes;
        }
        public AttackPoint NextAttack()
        {
            return new AttackPoint(1, 1);
        }

        public void SetPlanes(Plane[] planes)
        {
            this.planes = planes;
        }

        public void AddAttackPoint(AttackPoint attackPoint, string res = "")
        {
            attackHistory.Add(attackPoint);
        }

        public ArrayList GetAttackHistory()
        {
            return attackHistory;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public AiVirtualPlayer GetAiAssistantPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
