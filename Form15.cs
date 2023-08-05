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
    public partial class Form15 : Form
    {
        private StringBuilder ab;
        private StringBuilder cd;

        public Form15()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\Temp\\p1a.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\Temp\\p1b.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\Temp\\p2a.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\Temp\\p2b.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\Temp\\201gw1.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button5.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\Temp\\201gw2.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button6.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw N0v$mber tuxedo@148.92.137.11 -m c:\\Temp\\p3a.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button7.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw N0v$mber tuxedo@148.92.137.11 -m c:\\Temp\\p3b.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button8.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw N0v$mber tuxedo@148.92.137.11 -m c:\\Temp\\p4a.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button9.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw N0v$mber tuxedo@148.92.137.11 -m c:\\Temp\\p4b.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button10.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw N0v$mber tuxedo@148.92.137.11 -m c:\\Temp\\202gw1.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button11.Enabled = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Application.DoEvents();
            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw N0v$mber tuxedo@148.92.137.11 -m c:\\Temp\\202gw2.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.Text = s;
            Application.DoEvents();
            button12.Enabled = false;
        }
    }
}
