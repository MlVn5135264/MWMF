using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EncryptDecrypt;
using Ini;

namespace MWMF
{
    public partial class Form16 : Form
    {
        public int iTimer;
        public ED ed = new ED();
        public IniFile inifile = new IniFile(@"c:\temp\config.ini");
        public string pr103PassWordE, pr103PassWordD, pr104PassWordE, pr104PassWordD, key;
        public System.Windows.Forms.Timer MyTimer1;
        public Form16()
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

            if ((iTimer == 30) || (iTimer == 45))
            {
                label1.Text = "Event will fire automatically every " + comboBox1.Text + " seconds";
                label2.Text = "Event will fire automatically every " + comboBox1.Text + " seconds";
            }
            else
            {
                if (iTimer == 1)
                {
                    label1.Text = "Event will fire automatically every " + comboBox1.Text + " minute";
                    label2.Text = "Event will fire automatically every " + comboBox1.Text + " minute";
                }
                else
                {
                    label1.Text = "Event will fire automatically every " + comboBox1.Text + " minutes";
                    label2.Text = "Event will fire automatically every " + comboBox1.Text + " minutes";
                }
            }

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            label1.Enabled = true;
            label2.Enabled = true;
            richTextBox1.Enabled = true;
            richTextBox2.Enabled = true;
            comboBox1.Enabled = false;

            MyTimer1 = new System.Windows.Forms.Timer();

            if ((iTimer == 30) || (iTimer == 45))
            {
                MyTimer1.Interval = (iTimer * 1000);
            }
            else
            {
                MyTimer1.Interval = (iTimer * 60 * 1000);
            }
            MyTimer1.Tick += new EventHandler(MyTimer1_Tick);
            MyTimer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MyTimer1.Stop();
            MyTimer1.Dispose();

            // Disable relevant buttons
            button1.Enabled = false;
            button2.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            richTextBox1.Clear();
            richTextBox2.Clear();
            Application.DoEvents();
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
            button3.Enabled = false;
            comboBox1.Enabled = true;
        }

        private void Form16_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
        }

        private void MyTimer1_Tick(object sender, EventArgs e)
        {
            MWMF.Form1.Sem.WaitAsync();
            
            // check pr103
            Check_EXT_PR0D103();

            // check pr104
            Check_EXT_PR0D104();

            button3.Enabled = true;

            MWMF.Form1.Sem.Release(1);
        }
        private void Check_EXT_PR0D103()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transfer files over
            button1.Enabled = false;
            button2.Enabled = false;
            richTextBox1.Clear();
            richTextBox1.Update();
            Application.DoEvents();

            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
            pr103PassWordE = inifile.IniReadValue("usrnscwpr103", "Password");
            pr103PassWordD = ed.DecryptString(key, pr103PassWordE);

            ef = new StringBuilder("plink");
            // gh = new StringBuilder(" -pw Oct@2@21 ecm_weblogic@172.22.0.15" + @" -m C:\temp\pr103.txt");
            gh = new StringBuilder(" -pw " + pr103PassWordD + " ecm_weblogic@172.22.0.15" + @" -m C:\temp\pr103.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.AppendText(s);
            richTextBox1.Update();

            ef = new StringBuilder("pscp");
            // gh = new StringBuilder(" -pw Oct@2@21 ecm_weblogic@172.22.0.15:/tmp/pr103_pid.txt" + @" C:\temp\pr103_getpid.txt");
            gh = new StringBuilder(" -pw " + pr103PassWordD + " ecm_weblogic@172.22.0.15:/tmp/pr103_pid.txt" + @" C:\temp\pr103_getpid.txt");
            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();            
            richTextBox1.AppendText(s);
            label6.Text = DateTime.Now.ToString();

            Application.DoEvents();
        }

        private void Check_EXT_PR0D104()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transder files over
            button1.Enabled = false;
            button2.Enabled = false;
            richTextBox2.Clear();
            richTextBox2.Update();
            Application.DoEvents();

            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
            pr104PassWordE = inifile.IniReadValue("usrnscwpr104", "Password");
            pr104PassWordD = ed.DecryptString(key, pr104PassWordE);

            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw " + pr104PassWordD + " ecm_weblogic@172.22.0.16" + @" -m C:\temp\pr104.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox2.AppendText(s);
            richTextBox2.Update();
            
            ef = new StringBuilder("pscp");
            gh = new StringBuilder(" -pw " + pr104PassWordD + " ecm_weblogic@172.22.0.16:/tmp/pr104_getpid.txt" + @" C:\temp\pr104_getpid.txt");
            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
            richTextBox2.AppendText(s);
            label7.Text = DateTime.Now.ToString();

            Application.DoEvents();
        }
    }
}
