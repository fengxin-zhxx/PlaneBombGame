using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneBombGame
{
    public partial class BaseInfoSet : Form
    {
        public string ipStr;
        public string portStr;
        public bool clientOrServer = false;
        private Form1 form1;

        public BaseInfoSet()
        {
            InitializeComponent();
            //checkedListBox1 = new CheckedListBox();

            checkedListBox1.Items.Add("Client");
            checkedListBox1.Items.Add("Server");
            checkedListBox1.CheckOnClick = true;

            form1 = Form1.getForm1();
        }

        public void changeBaseInfoSet()
        {
            this.Location = new Point(form1.Location.X +50 , form1.Location.Y + 60 + 60);
        }
         
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ipStr = textBox1.Text;
            portStr = textBox2.Text;
            DialogResult = DialogResult.OK;
        }

        private void BaseInfoSet_Load(object sender, EventArgs e)
        {
            changeBaseInfoSet();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(checkedListBox1.SelectedIndex == 0)
            {
                checkedListBox1.SetItemChecked(1, false);
                clientOrServer = true;
            }
            else
            {
                checkedListBox1.SetItemChecked(0, false);
                clientOrServer = false;
            }
        }
    }
}
