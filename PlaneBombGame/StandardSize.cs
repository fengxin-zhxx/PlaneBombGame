using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class StandardSize
    {
        public static int FormWidth { get { return 1500; } }
        public static int FormHeight { get { return 1000; } }

        // 地图大小
        public static int BoardWidth { get { return 743; } }
        public static int BoardHeight { get { return 901; } }

        // 格子宽度
        public static int BlockNum { get { return 10; } }
        public static int BlockWidth { get { return 60; } }


        public static int toLeft { get { return 40; } }
        public static int toTop { get { return 150; } }

    }
}
