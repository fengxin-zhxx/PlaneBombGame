using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class BlackBoard
    {
        public static BlackBoard blackboard = null;

        public static BlackBoard getBlackBoard()
        {
            if(blackboard == null)
            {
                blackboard = new BlackBoard();
            }
            return blackboard;
        }



    }
}
