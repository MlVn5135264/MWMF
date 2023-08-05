using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using EncryptDecrypt;
using Ini;

namespace MWMF
{
    public partial class Form19 : Form
    {
        public System.Windows.Forms.Timer MyTimer1;
        // public string sMailTo = "'narayananm@dxc.com';'9162369772@text.att.net';'richard.chiu@dxc.com';'9169470604@text.att.net';'vrajamreddy2@dxc.com';'9165474216@vtext.com';'pradeep.allaboyina@dxc.com';'steven.deyo@dxc.com';'s.manoharan@dxc.com';'m216@dxc.com';'poornaviswanathan.manickam@dxc.com'";
        public string sMailTo = "'narayananm@dxc.com';'9162369772@text.att.net'";
        public int cs_PR101 = 0, cs_PR102 = 0;
        public char cBadAlertSent_PR101 = 'N', cGoodAlertSent_PR101 = 'Y';
        public char cBadAlertSent_PR102 = 'N', cGoodAlertSent_PR102 = 'Y';
        public int iTimer;
        public ED ed = new ED();
        public IniFile inifile = new IniFile(@"c:\temp\config.ini");
        public string pr101PassWordE, pr101PassWordD, pr102PassWordE, pr102PassWordD, key;

        public Form19()
        {
            InitializeComponent();
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
            button3.Enabled = false;
        }

        private void Form19_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Form Closing");

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

        private void MyTimer1_Tick(object sender, EventArgs e)
        {
            MWMF.Form1.Sem.WaitAsync();

            button3.Enabled = true;

            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // create the command file for pr101
            string pathpr101 = @"C:\temp\dpe_pr101_tail.txt";

            // Delete the file if it exists.
            if (File.Exists(pathpr101))
            {
                File.Delete(pathpr101);
            }

            // create the file dpe_pr102_tail.txt in C:\temp
            StreamWriter swpr101 = File.CreateText(pathpr101);
            swpr101.Write(@". ~/.bashrc;" + "\n");
            swpr101.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr101.Write(@"tail -2 /PRD/ECM/DPE/node_modules/oodebe/logs/out.log > /tmp/dpe_pr101_tail.out" + "\n");
            swpr101.Close();

            // create the command file for pr101
            string pathpr102 = @"C:\temp\dpe_pr102_tail.txt";

            // Delete the file if it exists.
            if (File.Exists(pathpr102))
            {
                File.Delete(pathpr102);
            }

            // create the file dpe_pr102_tail.txt in C:\temp
            StreamWriter swpr102 = File.CreateText(pathpr102);
            swpr102.Write(@". ~/.bashrc;" + "\n");
            swpr102.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr102.Write(@"tail -2 /PRD/ECM/DPE/node_modules/oodebe/logs/out.log > /tmp/dpe_pr102_tail.out" + "\n");
            swpr102.Close();

            // Head over to PR101 and create the tail output
            richTextBox1.Clear();
            Application.DoEvents();
            richTextBox1.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
            pr101PassWordE = inifile.IniReadValue("usrnscwpr101", "Password");
            pr101PassWordD = ed.DecryptString(key, pr101PassWordE);

            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw " + pr101PassWordD + " ecm@172.22.0.5" + @" -m C:\temp\dpe_pr101_tail.txt");
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
            richTextBox1.AppendText("ECM_PR101 Logout....." + "\n\n");
            Application.DoEvents();

            
            // get the file created fromthe steps above
            ef = new StringBuilder("pscp");
            gh = new StringBuilder(" -pw " + pr101PassWordD + " ecm@172.22.0.5:/tmp/dpe_pr101_tail.out" + " " + @"C:\temp");
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
            richTextBox1.AppendText("ECM_PR101 Logout....." + "\n\n");
            button1.Enabled = true;
            button2.Enabled = true;
            textBox1.Text = "ECM_PR101: Done";
            Application.DoEvents();
            

            // Head over to PR102 and create the tail output
            richTextBox2.Clear();
            Application.DoEvents();
            richTextBox2.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
            pr102PassWordE = inifile.IniReadValue("usrnscwpr102", "Password");
            pr102PassWordD = ed.DecryptString(key, pr102PassWordE);

            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw " + pr102PassWordD + " ecm@172.22.0.6" + @" -m C:\temp\dpe_pr102_tail.txt");
            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
            richTextBox2.AppendText(s);
            richTextBox2.AppendText("Process File: End Time: " + DateTime.Now.ToString() + "\n");
            richTextBox2.AppendText("ECM_PR102 Logout....." + "\n\n");
            Application.DoEvents();

            // get the file created fromthe steps above
            ef = new StringBuilder("pscp");
            // gh = new StringBuilder(" -pw 4P'[wd(c) ecm@172.22.0.6:/tmp/dpe_pr102_tail.out" + " " + @"C:\temp");
            gh = new StringBuilder(" -pw " + pr102PassWordD + " ecm@172.22.0.6:/tmp/dpe_pr102_tail.out" + " " + @"C:\temp");
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
            richTextBox2.AppendText("ECM_PR102 Logout....." + "\n\n");
            button1.Enabled = true;
            button2.Enabled = true;
            textBox2.Text = "ECM_PR102: Done";
            Application.DoEvents();

            // Process file from PR101
            string StrPR101DPETailFileName = @"c:\temp\dpe_pr101_tail.out";
            StreamReader PR101File = new System.IO.StreamReader(StrPR101DPETailFileName);
            String PR101String = PR101File.ReadLine();
            String [] PR101StringTemp = PR101String.Split('|');
            String[] PR101StringDT = PR101StringTemp[0].Split('-');
            var datetime_PR101 = PR101StringDT[3] + ":" + PR101StringDT[4] + ":" + PR101StringDT[5];
            var datetime_PR101_Temp = DateTime.Parse(datetime_PR101);
            var paczone_PR101 = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var gmtutcNow_PR101 = DateTime.UtcNow;
            var currentDateTime_PR101 = TimeZoneInfo.ConvertTimeFromUtc(gmtutcNow_PR101, paczone_PR101);
            // var currentDateTime = DateTime.UtcNow.AddHours(dAddHours);
            var DateTimeDiff_PR101 = currentDateTime_PR101 - datetime_PR101_Temp;
            cs_PR101 = (int)DateTimeDiff_PR101.TotalSeconds;
            richTextBox1.AppendText("Current System Time: " + currentDateTime_PR101.ToString() + "\n");
            richTextBox1.AppendText("DPE Time: " + datetime_PR101_Temp.ToString() + "\n");
            richTextBox1.AppendText("Time Difference: " + cs_PR101.ToString() + "\n\n");

            //send bad alert
            if(cs_PR101 >= 120)
            { 
                SendMail_DPE_Log_Gap_PR101();

                if ((cBadAlertSent_PR101 == 'N') && (cGoodAlertSent_PR101 == 'Y'))
                {
                    richTextBox1.AppendText("DPE_PR101: Bad Alert and Text Message sent" + "\n");
                    cBadAlertSent_PR101 = 'Y';
                    cGoodAlertSent_PR101 = 'N';
                }
            }

            // send good alert
            if (cs_PR101 < 120)
            {
                SendMail_DPE_Log_No_Gap_PR101();

                if ((cGoodAlertSent_PR101 == 'N') && (cBadAlertSent_PR101 == 'Y'))
                {
                    richTextBox1.AppendText("DPE_PR101: Good Alert and Text Message sent" + "\n");
                    cGoodAlertSent_PR101 = 'Y';
                    cBadAlertSent_PR101 = 'N';
                }
            }

            // Process file from PR102
            string StrPR102DPETailFileName = @"c:\temp\dpe_pr102_tail.out";
            StreamReader PR102File = new System.IO.StreamReader(StrPR102DPETailFileName);
            String PR102String = PR102File.ReadLine();
            String[] PR102StringTemp = PR102String.Split('|');
            String[] PR102StringDT = PR102StringTemp[0].Split('-');
            var datetime_PR102 = PR102StringDT[3] + ":" + PR102StringDT[4] + ":" + PR102StringDT[5];
            var datetime_PR102_Temp = DateTime.Parse(datetime_PR102);
            var paczone_PR102 = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var gmtutcNow_PR102 = DateTime.UtcNow;
            var currentDateTime_PR102 = TimeZoneInfo.ConvertTimeFromUtc(gmtutcNow_PR102, paczone_PR102);
            // var currentDateTime = DateTime.UtcNow.AddHours(dAddHours);
            var DateTimeDiff_PR102 = currentDateTime_PR102 - datetime_PR102_Temp;
            cs_PR102 = (int)DateTimeDiff_PR102.TotalSeconds;
            richTextBox2.AppendText("Current System Time: " + currentDateTime_PR102.ToString() + "\n");
            richTextBox2.AppendText("DPE Time: " + datetime_PR102_Temp.ToString() + "\n");
            richTextBox2.AppendText("Time Difference: " + cs_PR102.ToString() + "\n\n");

            //send bad alert
            if (cs_PR102 >= 120)
            {
                SendMail_DPE_Log_Gap_PR102();
                if ((cBadAlertSent_PR102 == 'N') && (cGoodAlertSent_PR102 == 'Y'))
                {
                    richTextBox2.AppendText("DPE_PR102: Bad Alert and Text Message sent" + "\n");
                    cBadAlertSent_PR102 = 'Y';
                    cGoodAlertSent_PR102 = 'N';
                }
            }

            // send good alert
            if (cs_PR102 < 120)
            {
                SendMail_DPE_Log_No_Gap_PR102();

                if ((cGoodAlertSent_PR102 == 'N') && (cBadAlertSent_PR102 == 'Y'))
                {
                    richTextBox2.AppendText("DPE_PR102: Good Alert and Text Message sent" + "\n");
                    cGoodAlertSent_PR102 = 'Y';
                    cBadAlertSent_PR102 = 'N';
                }
            }

            MWMF.Form1.Sem.Release();
        }

        private void SendMail_DPE_Log_Gap_PR101()
        {
            if(cBadAlertSent_PR101 == 'N') 
            {
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo.ToString();
                    mail.Subject = "ECM PR101: Gap Found in DPE Log";
                    mail.Body = "Action: Bounce DPE in ECM PR101\n\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SendMail_DPE_Log_Gap_PR102()
        {
            if (cBadAlertSent_PR102 == 'N')
            {
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo.ToString();
                    mail.Subject = "ECM PR102: Gap Found in DPE Log";
                    mail.Body = "Action: Bounce DPE in ECM PR102\n\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SendMail_DPE_Log_No_Gap_PR101()
        {
            if (cGoodAlertSent_PR101 == 'N')
            {
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo.ToString();
                    mail.Subject = "ECM PR101: No Gaps Found in DPE Log";
                    mail.Body = "Action: None\n\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SendMail_DPE_Log_No_Gap_PR102()
        {
            if (cGoodAlertSent_PR102 == 'N')
            {
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo.ToString();
                    mail.Subject = "ECM PR102: No Gaps Found in DPE Log";
                    mail.Body = "Action: None\n\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
