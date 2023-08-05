using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MWMF
{
    public partial class Form28 : Form
    {
        public System.Windows.Forms.Timer MyTimer1;
        public int iTimer;

        public Form28()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Is the Timing Correct?", "Important Question", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.No)
            {
                return;
            }

            iTimer = int.Parse(comboBox1.Text);

            MyTimer1 = new System.Windows.Forms.Timer();

            if ((iTimer == 30) || (iTimer == 45))
            {
                MyTimer1.Interval = (iTimer * 1000);
            }
            else
            {
                MyTimer1.Interval = (iTimer * 60 * 1000);
            }
            MyTimer1.Tick += new EventHandler(MyTimer_Tick);
            MyTimer1.Start();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Entering Event");
            // System.Diagnostics.Process.Start(@"c:\windows\system32/Cscript.exe //B //Nologo c:\Data_Cleansing\dc.vbs");

            Process scriptProc = new Process();
            scriptProc.StartInfo.FileName = @"cscript";
            scriptProc.StartInfo.WorkingDirectory = @"c:\Data_Cleansing"; //<---very important 
            scriptProc.StartInfo.Arguments = "//B //Nologo dc.vbs";
            scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //prevent console window from popping up
            scriptProc.Start();
            scriptProc.WaitForExit(); // <-- Optional if you want program running until your script exit
            scriptProc.Close();
            MessageBox.Show("Done Event");
        }
    }
}
