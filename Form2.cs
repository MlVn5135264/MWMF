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
    public partial class Form2 : Form
    {
        string strInstanceName;

        public Form2()
        {
            InitializeComponent();
        }

        private void tmain_Load(object sender, EventArgs e)
        {
            cbInstanceName.Text = "Click the Drop Down";
            cbGroupName.Text = "Click From the Drop Down";
            cbGroupName.Enabled = false;
        }

        private void cbInstanceName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbInstanceName.Text == "p1a") || (cbInstanceName.Text == "p1b") || (cbInstanceName.Text == "p2a") || (cbInstanceName.Text == "p2b"))
            {
                rtbProductionServerName.Text = "usrnucwpr201";

                strInstanceName = "Y";
            }

            if ((cbInstanceName.Text == "p3a") || (cbInstanceName.Text == "p3b") || (cbInstanceName.Text == "p4a") || (cbInstanceName.Text == "p4b"))
            {
                rtbProductionServerName.Text = "usrnucwpr202";

                strInstanceName = "Y";
            }

            // First Clear all Items in the Combo Box
            cbGroupName.Items.Clear();

            // if tuxedo instance is p1a add SAC and SOL County Groups
            if (cbInstanceName.Text == "p1a")
            {
                cbGroupName.Items.Add(item: ("SAC34R"));
                cbGroupName.Items.Add(item: ("SAC34S"));
                cbGroupName.Items.Add(item: ("SAC34C"));
                cbGroupName.Items.Add(item: ("SOL48R"));
                cbGroupName.Items.Add(item: ("SOL48S"));
                cbGroupName.Items.Add(item: ("SOL48C"));
            }

            // if tuxedo instance is p1b add TUL and VEN County Groups
            if (cbInstanceName.Text == "p1b")
            {
                cbGroupName.Items.Add(item: ("TUL54R"));
                cbGroupName.Items.Add(item: ("TUL54S"));
                cbGroupName.Items.Add(item: ("TUL54C"));
                cbGroupName.Items.Add(item: ("VEN56R"));
                cbGroupName.Items.Add(item: ("VEN56S"));
                cbGroupName.Items.Add(item: ("VEN56C"));
            }

            // if tuxedo instance is p2a add PLA, SCZ and SCL County Groups
            if (cbInstanceName.Text == "p2a")
            {
                cbGroupName.Items.Add(item: ("PLA31R"));
                cbGroupName.Items.Add(item: ("PLA31S"));
                cbGroupName.Items.Add(item: ("PLA31C"));
                cbGroupName.Items.Add(item: ("SCZ44R"));
                cbGroupName.Items.Add(item: ("SCZ44S"));
                cbGroupName.Items.Add(item: ("SCZ44C"));
                cbGroupName.Items.Add(item: ("SCL43R"));
                cbGroupName.Items.Add(item: ("SCL43S"));
                cbGroupName.Items.Add(item: ("SCL43C"));
            }


            // if tuxedo instance is p2b add CCS and ORG County Groups
            if (cbInstanceName.Text == "p2b")
            {
                cbGroupName.Items.Add(item: ("CCS07R"));
                cbGroupName.Items.Add(item: ("CCS07S"));
                cbGroupName.Items.Add(item: ("CCS07C"));
                cbGroupName.Items.Add(item: ("ORG30R"));
                cbGroupName.Items.Add(item: ("ORG30S"));
                cbGroupName.Items.Add(item: ("ORG30C"));
            }

            // if tuxedo instance is p3a add SFO and SLO County Groups
            if (cbInstanceName.Text == "p3a")
            {
                cbGroupName.Items.Add(item: ("SFO38R"));
                cbGroupName.Items.Add(item: ("SFO38S"));
                cbGroupName.Items.Add(item: ("SFO38C"));
                cbGroupName.Items.Add(item: ("SLO40R"));
                cbGroupName.Items.Add(item: ("SLO40S"));
                cbGroupName.Items.Add(item: ("SLO40C"));
            }

            // if tuxedo instance is p3b add FRS, SMT and YOL County Groups
            if (cbInstanceName.Text == "p3b")
            {
                cbGroupName.Items.Add(item: ("FRS10R"));
                cbGroupName.Items.Add(item: ("FRS10S"));
                cbGroupName.Items.Add(item: ("FRS10C"));
                cbGroupName.Items.Add(item: ("SMT41R"));
                cbGroupName.Items.Add(item: ("SMT41S"));
                cbGroupName.Items.Add(item: ("SMT41C"));
                cbGroupName.Items.Add(item: ("YOL57R"));
                cbGroupName.Items.Add(item: ("YOL57S"));
                cbGroupName.Items.Add(item: ("YOL57C"));
            }

            // if tuxedo instance is p4a add SDG, SON and SBR County Groups
            if (cbInstanceName.Text == "p4a")
            {
                cbGroupName.Items.Add(item: ("SDG37R"));
                cbGroupName.Items.Add(item: ("SDG37S"));
                cbGroupName.Items.Add(item: ("SDG37C"));
                cbGroupName.Items.Add(item: ("SON49R"));
                cbGroupName.Items.Add(item: ("SON49S"));
                cbGroupName.Items.Add(item: ("SON49C"));
                cbGroupName.Items.Add(item: ("SBR42R"));
                cbGroupName.Items.Add(item: ("SBR42S"));
                cbGroupName.Items.Add(item: ("SBR42C"));
            }

            // if tuxedo instance is p4b add ALA County Group
            if (cbInstanceName.Text == "p4b")
            {
                cbGroupName.Items.Add(item: ("ALA01R"));
                cbGroupName.Items.Add(item: ("ALA01S"));
                cbGroupName.Items.Add(item: ("ALA01C"));
            }

            // Enable Combo Box
            cbGroupName.Enabled = true;
        }

        private void cbGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strInstanceName == "Y")
            {
                rtbServerID.Enabled = true;
            }
        }

        private void rtbProductionServerName_TextChanged(object sender, EventArgs e)
        {

        }

        /* private void button1_Click(object sender, EventArgs e)
         {
             ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
             psi.RedirectStandardInput = true;
             psi.RedirectStandardOutput = true;
             psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
             psi.UseShellExecute = false;
             psi.CreateNoWindow = false;

             Process process = Process.Start(psi);
             string cmdForTunnel = @"plink -pw W1ndingroad tuxedo@10.3.2.254 -m c:\temp\m.txt";
             process.StandardInput.WriteLine(cmdForTunnel);
             process.WaitForExit();
             //System.Threading.Thread.Sleep(10000);
             // process.StandardInput.WriteLine("logout");
             //System.Threading.Thread.Sleep(10000);

             if (process.HasExited)
             {
                 process.Close();
                 process.Dispose();
             }
         }*/

        private void button1_Click(object sender, EventArgs e)
        {
            // rTextBox.Text = "Start Process";

            // connect to ST700
            // ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw B0at2019 tuxedo@10.3.2.254 -m c:\\temp\\m.txt");

            // Connect to BD700
            // ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw Jan@2019 tuxedo@10.3.2.7 -m c:\\temp\\m.txt");

            // Connect to Tuxedo PR201
            ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\temp\\m.txt");

            // Connect to Tuxedo PR202
            // ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw N0v$mber tuxedo@148.92.137.11 -m c:\\temp\\m.txt");          

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            rTextBox.AppendText(s);
            // rTextBox.AppendText("End Process");
            
            /*psi = new ProcessStartInfo(@"plink", " -pw Jan@2019 tuxedo@10.3.2.7 -m c:\\temp\\n.txt");
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
            rTextBox.AppendText(s);
            rTextBox.AppendText("End Process Again");*/

        }

    }
}

/* ******************* Create a File ******************* */
/*
    string path = @"c:\temp\m.txt";
    // Delete the file if it exists.
    if (File.Exists(path))
    {
        Note that no lock is put on the
        file and the possibility exists
        that another process could do
        something with it between
        the calls to Exists and Delete.
        File.Delete(path);
    }
    // Creates the file.
    FileStream fs = File.Create(path);
*/

/* 
    rTextBox.AppendText("Logging into bd700 Box.......................................");
    psi = new ProcessStartInfo(@"plink", " -pw Jan@2019 tuxedo@10.3.2.7 -m c:\\temp\\m.txt");
    psi.UseShellExecute = false;
    psi.RedirectStandardOutput = true;
    psi.CreateNoWindow = true;
    proc = Process.Start(psi);
    s = proc.StandardOutput.ReadToEnd();
    rTextBox.AppendText(s); 
    rTextBox.AppendText("End Process");
*/

/*
    ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw W1ndingroad tuxedo@10.3.2.254 -m c:\\temp\\a.txt");
    ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw Jan@2019 tuxedo@10.3.2.7 -m c:\\temp\\m.txt");
    ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw Rics6Od~ tuxedo@148.92.137.10 -m c:\\temp\\m.txt");
    ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw N0v$mber tuxedo@148.92.137.11 -m c:\\temp\\m.txt");          
*/
