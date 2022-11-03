using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //DateTime d1 = System.DateTime.Now;
            //new AiVirtualPlayer().Init(); 
            //DateTime d2 = System.DateTime.Now;
            //MessageBox.Show(d1.ToString("o") + "\n" + d2.ToString("o"));
            Application.Run(new Form1());
        }
    }
}
