using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MWMF
{
    public partial class Form23 : Form
    {
        public System.Windows.Forms.Timer MyTimer1;
        public int iCounter1, iCounter2;
        public int dECM_PR101_File__Line_Count = 0, dECM_PR102_File__Line_Count = 0;
        public int size_backup, size, ipvPR101 = 0, ipvPR102 = 0;
        public string backup_fileName, fileName;
        public StringBuilder cMailTo = new StringBuilder("", 5000);
        public string[] iFileLen;
        public string sFileNameToProcess = "out.log";
        public int iTimer, iPR300Count = 0, iPR310Count = 0, iTempPR300Count = 0, iTempPR310Count = 0;

        // public string sMailTo = "'narayananm@dxc.com';'9162369772@mms.att.net';'steven.deyo@dxc.com';'9132263942@messaging.sprintpcs.com';'joe.mendez@dxc.com';'9162840731@vtext.com';'richard.chiu@dxc.com';'9169470604@mms.att.net';'poornaviswanathan.manickam@dxc.com';'velayutham@dxc.com';'vrajamreddy2@dxc.com';'9165474216@vtext.com';'m216@dxc.com';'s.manoharan@dxc.com'";
        public string sMailTo = "'narayananm@gainwelltechnologies.com';'9162369772@mms.att.net';'joe.mendez@gainwelltechnologies.com';'9162840731@tmomail.net';'3035326594@vtext.com';'karen.guyette@gainwelltechnologies.com';'9165474216@vtext.com';'vrajamreddy2@gainwelltechnologies.com'";
        public Form23()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
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

        private void Form23_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
            button3.Enabled = false;
        }

        private void MyTimer1_Tick(object sender, EventArgs e)
        {
            MWMF.Form1.Sem.WaitAsync();
            size_backup = 0;
            size = 0;

            button3.Enabled = true;

            // create the command file for pr101
            string pathpr300 = @"C:\AWS_Rotation\PR300\pr300.txt";
            // Delete the file if it exists.
            if (File.Exists(pathpr300))
            {
                File.Delete(pathpr300);
            }
            // create the file pr101.txt in C:\DPE_Rotation\PR101
            StreamWriter swpr101 = File.CreateText(pathpr300);
            swpr101.Write(@". ~/.bashrc;" + "\n");
            swpr101.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr101.Write(@"cd /PRD/Exstream_Online_Msgs;" + "\n");
            swpr101.Write(@"find exstream*/ -type f -mmin -120 | wc -l > /tmp/ctrl_file_pr300.txt;");
            swpr101.Close();

            // create the command file for pr102
            string pathpr310 = @"C:\AWS_Rotation\PR310\pr310.txt";
            // Delete the file if it exists.
            if (File.Exists(pathpr310))
            {
                File.Delete(pathpr310);
            }
            // create the file pr102.txt in C:\DPE_Rotation\PR102
            StreamWriter swpr102 = File.CreateText(pathpr310);
            swpr102.Write(@". ~/.bashrc;" + "\n");
            swpr102.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr102.Write(@"cd /PRD/Exstream_Online_Msgs;" + "\n");
            swpr102.Write(@"find exstream*/ -type f -mmin -120 | wc -l > /tmp/ctrl_file_pr310.txt;");
            swpr102.Close();

            Check_EXT_PR300();

            Check_EXT_PR310();

            StreamReader sr = new StreamReader(@"c:\AWS_Rotation\PR300\ctrl_file_pr300.txt");
            string line = sr.ReadLine();
            iTempPR300Count = (int) Convert.ToInt64(line);
            sr.Close();

            // MessageBox.Show(iTempPR300Count.ToString());

            if (iTempPR300Count > iPR300Count)
            {
                iPR300Count = iTempPR300Count;
                richTextBox1.AppendText("Alerts Received in PR300. Emails/Text Messages Sent.....");
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo;
                    mail.Subject = "Backup EXT Engines Alert on EXT_PR300";
                    mail.Body = "Count: " + (iTempPR300Count - iPR300Count).ToString() + "\n\n" + "Action: Email/Text sent to Joe Mendez\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            sr = new StreamReader(@"c:\AWS_Rotation\PR310\ctrl_file_pr310.txt");
            line = sr.ReadLine();
            iTempPR310Count = (int)Convert.ToInt64(line);
            sr.Close();

            // MessageBox.Show(iTempPR310Count.ToString());

            if (iTempPR310Count > iPR310Count)
            {
                iPR310Count = iTempPR310Count;
                richTextBox1.AppendText("Alerts Received in PR310. Emails/Text Messages Sent.....");
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo;
                    mail.Subject = "Backup EXT Engines Alert on EXT_PR310";
                    mail.Body = "Count: " + (iTempPR310Count - iPR310Count).ToString() + "\n\n" + "Action: Email/Text sent to Joe Mendez\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MWMF.Form1.Sem.Release();
        }

        private void Check_EXT_PR300()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transder files over
            textBox1.Text = "EXT_PR300: Log File";
            textBox1.Refresh();
            button1.Enabled = false;
            button2.Enabled = false;
            richTextBox1.Clear();

            richTextBox1.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();
            ef = new StringBuilder("plink");
            gh = new StringBuilder(@" -i C:\AWS\CalWIN_NC_PRD_moh_nar.ppk moh_nar@usawcwspr300.folsom.calwin.eds.com" + @" -m C:\AWS_Rotation\PR300\pr300.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();            
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox1.AppendText(s);
            richTextBox1.AppendText("Process File: End Time: " + DateTime.Now.ToString() + "\n");
            richTextBox1.AppendText("EXT_PR300 Logout....." + "\n\n");

            richTextBox1.AppendText("Get File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            string CurrentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(@"C:\AWS_Rotation\PR300");
            ef = new StringBuilder("pscp");
            gh = new StringBuilder(@" -i C:\AWS\CalWIN_NC_PRD_moh_nar.ppk moh_nar@usawcwspr300.folsom.calwin.eds.com:/tmp/ctrl_file_pr300.txt c:\AWS_Rotation\PR300\ctrl_file_pr300.txt");

            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
            richTextBox1.AppendText(s);
            richTextBox1.AppendText("\n" + "Get File: End Time: " + DateTime.Now.ToString() + "\n");
            richTextBox1.AppendText("EXT_PR300 Logout....." + "\n\n");

            button1.Enabled = true;
            button2.Enabled = true;
            textBox1.Text = "EXT_PR300: Done";

            Directory.SetCurrentDirectory(CurrentDir);
            Application.DoEvents();
        }

        private void Check_EXT_PR310()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transder files over
            textBox2.Text = "EXT_PR310: Log File";
            richTextBox2.Refresh();
            button2.Enabled = false;
            button1.Enabled = false;
            richTextBox2.Clear();

            richTextBox2.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();
            ef = new StringBuilder("plink");
            gh = new StringBuilder(@" -i C:\AWS\CalWIN_NC_PRD_moh_nar.ppk moh_nar@usawcwspr310.folsom.calwin.eds.com" + @" -m C:\AWS_Rotation\PR310\pr310.txt");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox2.AppendText(s);
            richTextBox2.AppendText("Process File: End Time: " + DateTime.Now.ToString() + "\n");
            richTextBox2.AppendText("EXT_PR310 Logout....." + "\n\n");

            richTextBox2.AppendText("Get File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            string CurrentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(@"C:\AWS_Rotation\PR310");

            ef = new StringBuilder("pscp");
            gh = new StringBuilder(@"-i C:\AWS\CalWIN_NC_PRD_moh_nar.ppk moh_nar@usawcwspr310.folsom.calwin.eds.com:/tmp/ctrl_file_pr310.txt c:\AWS_Rotation\PR310\ctrl_file_pr310.txt");

            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
            richTextBox2.AppendText(s);
            richTextBox2.AppendText("\n" + "Get File: End Time: " + DateTime.Now.ToString() + "\n");
            richTextBox2.AppendText("EXT_PR310 Logout....." + "\n\n");

            button2.Enabled = true;
            button1.Enabled = true;
            textBox2.Text = "EXT_PR310: Done";

            Directory.SetCurrentDirectory(CurrentDir);
            Application.DoEvents();            
        }
    }
}

/*  try
   {
       Outlook._Application _app = new Outlook.Application();
       Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
       mail.To = sMailTo;
       mail.Subject = "Error code : 500 received from Alfresco on ECM_PR101";
       mail.Body = "Count: " + iCounter1.ToString() + "\n\n" + "Action: Bounce JVM ECMPRD01-Server01 in usrnscwpr101 (172.22.0.5)\n";
       mail.Importance = Outlook.OlImportance.olImportanceNormal;
       ((Outlook._MailItem)mail).Send();
   }
   catch (Exception ex)
   {
       MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
}*/
