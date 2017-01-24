using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ToolsGenGkode
{
    public partial class SelectColor3D : Form
    {
        private Preview_Vectors pvf;
        public SelectColor3D(Preview_Vectors fff)
        {
            InitializeComponent();
            pvf = fff;

            btBackColor.BackColor = Color.FromArgb(pvf.Color_3DBack.iR, pvf.Color_3DBack.iG, pvf.Color_3DBack.iB);
            btBackColor.ForeColor = Color.FromArgb(255-pvf.Color_3DBack.iR, 0, 0);

            button2.BackColor = Color.FromArgb(pvf.Color_3DContur.iR, pvf.Color_3DContur.iG, pvf.Color_3DContur.iB);
            button3.BackColor = Color.FromArgb(pvf.Color_Grid.iR, pvf.Color_Grid.iG, pvf.Color_Grid.iB);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btBackColor.BackColor = colorDialog1.Color;

                pvf.Color_3DBack = new FColor(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

                pvf.openGLControl1.Refresh();
                pvf.openGLControl1.Refresh();
            }


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button2.BackColor = colorDialog1.Color;

                pvf.Color_3DContur = new FColor(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

                pvf.openGLControl1.Refresh();
                pvf.openGLControl1.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button3.BackColor = colorDialog1.Color;

                pvf.Color_Grid = new FColor(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

                pvf.openGLControl1.Refresh();
                pvf.openGLControl1.Refresh();
            }
        }

        private void SelectColor3D_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pvf.Color_3DBack = new FColor(120, 120, 120);
            pvf.Color_3DContur = new FColor(0, 255, 0);
            pvf.Color_Grid = new FColor(100, 0, 204);
        }
    }
}
