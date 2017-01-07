using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ToolsGenGkode.pages;

namespace ToolsGenGkode
{
    public partial class ProfileUserSetting : Form
    {
        public List<UserCommand> uc; 

        public ProfileUserSetting()
        {
            InitializeComponent();
        }

        private void ProfileUserSetting_Load(object sender, EventArgs e)
        {
            foreach (UserCommand VARIABLE in uc)
            {
                Panel pn = new Panel();
                pn.Name = "pn" + VARIABLE.Name;
                pn.Width = 250;
                pn.Height = 60;

                Label lb = new Label();
                lb.Name = "lb"+ VARIABLE.Name;
                lb.Text = VARIABLE.Description;

                NumericUpDown num = new NumericUpDown();
                num.Name = "num" + VARIABLE.Name;
                num.Top = 30;
                num.Minimum = -999999;
                num.Maximum = 999999;
                num.DecimalPlaces = 3;
                num.Value = (decimal)VARIABLE.Value;

                pn.Controls.Add(lb);
                pn.Controls.Add(num);

                flowLayoutPanel1.Controls.Add(pn);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int indx = 0;

            foreach (UserCommand VARIABLE in uc)
            {
                decimal mmm = ((System.Windows.Forms.NumericUpDown) (flowLayoutPanel1.Controls[indx].Controls[1])).Value;

                VARIABLE.Value = (double)mmm;

                indx++;
            }

        }
    }
}
