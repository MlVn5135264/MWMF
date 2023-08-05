using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MWMF
{
    public partial class Form1 : Form
    {
        public char cStopTimerEvent = 'N';
        
        public static SemaphoreSlim Sem = new SemaphoreSlim(1, 1);
           
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            // Load the Tuxedo Maintenance Form here.....
            Form2 f2 = new Form2();
            // f2.ShowDialog(this);
            f2.Show();
        }

        private void MachineName_Click(object sender, EventArgs e)
        {
            // Load the Tuxedo Maintenance Form here.....
            Form3 f3 = new Form3();
            // f3.ShowDialog(this);     
            f3.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            // f4.ShowDialog(this);
            f4.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            // f5.ShowDialog(this);
            f5.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            // f6.ShowDialog(this);
            f6.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer()
            {
                Interval = 60000
            };

            if (cStopTimerEvent == 'N')
            {
                timer1.Enabled = true;
            }
            
            timer1.Tick += new EventHandler(OnTimerEvent);

            StringBuilder ab = new StringBuilder();
            StringBuilder cd = new StringBuilder();
            ProcessStartInfo psi = new ProcessStartInfo();

            ab = new StringBuilder("ping");
            cd = new StringBuilder(" 10.3.2.254");          
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();
 
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();

            // check if string usrncwust700 is present
            string[] strHostName = s.Split('\n');
            //iCompare = string.Compare(strHostName[0], "usrncwust700");
            int iCompare = -2;
            iCompare += 1;

            int iPingCount = strHostName.Count();
            for(int i = 0; i < iPingCount; i++)
            {
                if (strHostName[i].Contains("Reply from 10.3.2.254"))
                {
                    iCompare = 0;
                    break;
                }
            }

            if(iCompare == -1 )
            {
                // commented for DR
                MessageBox.Show("Connect to Calwin Network and Try again.....");
                this.Close();
            }
        }

        private void OnTimerEvent(object sender, EventArgs e)
        {
            // call the garbage collector
            long memory = GC.GetTotalMemory(true);
            // MessageBox.Show("Memory Used: " + memory.ToString());

            if (cStopTimerEvent == 'N')
            {
                StringBuilder ab = new StringBuilder();
                StringBuilder cd = new StringBuilder();
                ProcessStartInfo psi = new ProcessStartInfo();

                ab = new StringBuilder("ping");
                cd = new StringBuilder(" 10.3.2.254");
                psi.FileName = ab.ToString();
                psi.Arguments = cd.ToString();

                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                var proc = Process.Start(psi);
                string s = proc.StandardOutput.ReadToEnd();

                // check if string usrncwust700 is present
                string[] strHostName = s.Split('\n');
                int iCompare = -2;
                iCompare += 1;

                int iPingCount = strHostName.Count();
                for (int i = 0; i < iPingCount; i++)
                {
                    if (strHostName[i].Contains("Reply from 10.3.2.254"))
                    {
                        iCompare = 0;
                        break;
                    }
                }

                if (iCompare == -1)
                {
                  DialogResult iResult;
                  MessageBoxButtons buttons = MessageBoxButtons.OK;
                  cStopTimerEvent = 'Y';
                  iResult = MessageBox.Show("Connect to Calwin Network and Try again.....", "NetWork", buttons);
                  if (iResult == System.Windows.Forms.DialogResult.OK)
                  {
                    // Stop Timer Event.
                    cStopTimerEvent = 'N'; 
                  }                   
                }
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.Show();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Form10 f10 = new Form10();
            f10.Show();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11();
            f11.Show();
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"MiddleWare Maintenance FrameWork: V.0.0.1" + "\n" + "All Rights Reserved");
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            Form12 f12 = new Form12();
            f12.Show();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            Form13 f13 = new Form13();
            f13.Show();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            Form14 f14 = new Form14();
            f14.Show();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            Form15 f15 = new Form15();
            f15.Show();
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            Form16 f16 = new Form16();
            f16.Show();
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            Form17 f17 = new Form17();
            f17.Show();
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            Form18 f18 = new Form18();
            f18.Show();
        }

        private void toolStripMenuItem5_Click_1(object sender, EventArgs e)
        {
            Form19 f19 = new Form19();
            f19.Show();
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            Form20 f20 = new Form20();
            f20.Show();
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            Form21 f21 = new Form21();
            f21.Show();
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            Form22 f22 = new Form22();
            f22.Show();
        }

        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            Form23 f23 = new Form23();
            f23.Show();
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            Form24 f24 = new Form24();
            f24.Show();
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            Form25 f25 = new Form25();
            f25.Show();
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            Form26 f26 = new Form26();
            f26.Show();
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            Form27 f27 = new Form27();
            f27.Show();
        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            Form28 f28 = new Form28();
            f28.Show();
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            Form29 f29 = new Form29();
            f29.Show();
        }
    }
}
