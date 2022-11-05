using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame.Image
{
    internal class SetInfoDialog
    {
        public static DialogResult Show(out string getNewIp,out string getNewPort,out bool clientOrSocket)
        {
            BaseInfoSet inputDialog = new BaseInfoSet();
            DialogResult result = inputDialog.ShowDialog();
            //inputDialog.lable1
            getNewIp = inputDialog.ipStr;
            getNewPort = inputDialog.portStr;
            clientOrSocket = inputDialog.clientOrServer;
            return result;
        }
    }
}
