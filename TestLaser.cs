// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
    public partial class TestLaser : Form
    {
        public TestLaser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("%");



            int CurrentX = 0;
            int CurrentY = 0;

            int LastX = -999999;
            int LastY = -999999;


            int currPower = (int)powerStart.Value;
            if (powerStep.Value == 0) powerEnd.Value = powerStart.Value;

            int currDuration = (int) durationStart.Value;
            if (durationStep.Value == 0) durationEnd.Value = durationStart.Value;


            while (currPower <= (int)powerEnd.Value && powerStep.Value != 0)
            {

                while (currDuration <= (int)durationEnd.Value && durationStep.Value != 0)//    int duration = (int)durationStart.Value; duration <= ; duration += (int)durationStep.Value)
                {
                    string ssrt = "M5 ";

                    if (LastX != CurrentX)
                    {
                        //нужно вывести значение по оси Х
                        LastX = CurrentX;

                        ssrt += " X" + CurrentX.ToString("#0.###");
                    }

                    if (LastY != CurrentY)
                    {
                        //нужно вывести значение по оси Х
                        LastY = CurrentY;
                        ssrt += " Y" + CurrentY.ToString("#0.###");
                    }


                    sb.AppendLine(ssrt.Trim());
                    sb.AppendLine("M3 S" + currPower.ToString("0000") + " G4 P" + (((decimal)currDuration / 1000).ToString("###0.####")));
                    sb.AppendLine("M5");

                    CurrentY += (int) distance.Value;

                    currDuration += (int) durationStep.Value;


                }
                CurrentX += (int)distance.Value;
                CurrentY = 0;

                currPower += (int) powerStep.Value;
            }

            sb.Replace(',', '.');

            sb.AppendLine("%");

            textBox1.Text = sb.ToString();
        }

        private void TestLaser_Load(object sender, EventArgs e)
        {

        }
    }
}
