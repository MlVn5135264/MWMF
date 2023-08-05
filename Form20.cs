using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;
using EncryptDecrypt;
using Ini;

namespace MWMF
{
    public partial class Form20 : Form
    {
        public System.Windows.Forms.Timer MyTimer1;
        public int iCounter1, iCounter2;
        public int dCWA_PR360_File_Line_Count = 0, dCWA_PR370_File_Line_Count = 0;
        public int size_backup, size, ipvPR360 = 0, ipvPR370 = 0;
        public string backup_fileName, fileName;
        public string[] iFileLen;
        public string sFileName1ToProcess = "CWAPRD_Server01-cwa-diagnostic.log";
        public string sFileName2ToProcess = "CWAPRD_Server02-cwa-diagnostic.log";
        public int iTimer;
        // public string sMailTo = "'mike.wilhite@dxc.com';'narayananm@dxc.com';'steven.deyo@dxc.com';'9132263942@messaging.sprintpcs.com';'richard.chiu@dxc.com';'poornaviswanathan.manickam@dxc.com';'vrajamreddy2@dxc.com';'pradeep.allaboyina@dxc.com';'9165474216@vtext.com';'m216@dxc.com';'s.manoharan@dxc.com'";
        public string sMailTo = "'narayananm@dxc.com';'9162369772@text.att.net'";
        public ED ed = new ED();
        public IniFile inifile = new IniFile(@"c:\temp\config.ini");
        public string pr360PassWordE, pr360PassWordD, pr370PassWordE, pr370PassWordD, key;

        public Form20()
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

        private void Form20_Load(object sender, EventArgs e)
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

            // create the command file for pr360
            string pathpr360 = @"C:\CWA_Rotation\PR360\pr360.txt";
            // Delete the file if it exists.
            if (File.Exists(pathpr360))
            {
                File.Delete(pathpr360);
            }
            // create the file pr360.txt in C:\CWA_Rotation\PR360
            StreamWriter swpr360 = File.CreateText(pathpr360);
            swpr360.Write(@". ~/.bashrc;" + "\n");
            swpr360.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr360.Write(@"grep -e ""Error was: Unknown SMTP host:"" /WEBLOGIC/CWA_Domain_PRD/servers/CWAPRD_Server01/logs/" + sFileName1ToProcess + " > /tmp/out.log.mohan;" + "\n");
            swpr360.Write(@"ls -l /WEBLOGIC/CWA_Domain_PRD/servers/CWAPRD_Server01/logs/" + sFileName1ToProcess + " > /tmp/out.log.mohan.len;");
            swpr360.Close();

            // create the command file for pr370
            string pathpr370 = @"C:\CWA_Rotation\PR370\pr370.txt";
            // Delete the file if it exists.
            if (File.Exists(pathpr370))
            {
                File.Delete(pathpr370);
            }

            // create the file pr370.txt in C:\CWA_Rotation\PR370
            StreamWriter swpr370 = File.CreateText(pathpr370);
            swpr370.Write(@". ~/.bashrc;" + "\n");
            swpr370.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr370.Write(@"grep -e ""Error was: Unknown SMTP host:"" /WEBLOGIC/CWA_Domain_PRD/servers/CWAPRD_Server02/logs/" + sFileName2ToProcess + " > /tmp/out.log.mohan;" + "\n");
            swpr370.Write(@"ls -l /WEBLOGIC/CWA_Domain_PRD/servers/CWAPRD_Server02/logs/" + sFileName2ToProcess + " > /tmp/out.log.mohan.len;");
            swpr370.Close();

            Check_CWA_PR360();

            // Delete the file if it exists.
            if (File.Exists(pathpr360))
            {
                File.Delete(pathpr360);
            }

            // check for DPE log rotation 
            // Get the linecount to skip from the control if one exists
            if (File.Exists(@"C:\CWA_Rotation\PR360\out.log.mohan.len"))
            {
                foreach (string line360 in File.ReadLines(@"C:\CWA_Rotation\PR360\out.log.mohan.len"))
                {
                    iFileLen = line360.Split(' ');

                    size = Int32.Parse(iFileLen[4]);

                    // ipvPR360 = (int) size;

                    break;
                }
            }

            if (size < ipvPR360)
            {
                if (File.Exists(@"C:\CWA_Rotation\PR360\ctrl_file_pr360.txt"))
                {
                    File.Delete(@"C:\CWA_Rotation\PR360\ctrl_file_pr360.txt");
                }

                // CWA logs Rotatated in PR360
                // Send Message
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo;
                    mail.Subject = "CWA Log Rotation: USTSCWWPR360";
                    mail.Body = "CWA Log Rotation in USTSCWWPR360 Completed SuccessFully\n" + "No Action is required\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ipvPR360 = (int)size;

            Process_CWA_PR360_Log_File();

            if (iCounter1 > 0)
            {
                richTextBox1.AppendText("Error: Unknown SMTP Host Count in CWA_PR360: " + iCounter1.ToString() + "\n\n");
                Application.DoEvents();

                // now process for the actual errors in CWA_PR360
                if (iCounter1 > 1)
                {
                    // create the file ctrl_file_pr360.txt
                    StreamWriter swPR360 = File.CreateText(@"C:\CWA_Rotation\PR360\ctrl_file_pr360.txt");
                    swPR360.Write(dCWA_PR360_File_Line_Count.ToString() + "\n");
                    swPR360.Close();

                    // send a text message
                    try
                    {
                        Outlook._Application _app = new Outlook.Application();
                        Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                        mail.To = sMailTo;
                        mail.Subject = "Error: Unknown SMTP Host received CWA_PR360";
                        // mail.Body = "Count: " + iCounter1.ToString() + "\n\n" + "Action: Bounce SMTP Instance in USTSCWWPR360 \n";
                        mail.Body = "Count: " + iCounter1.ToString() + "\n\n" + "Action: Bounce SMTP Instance in USTSCWWPR360\n\n" + "To test SMTP,  Run the Command below in USTSCWWPR360 and verify you receive an email:\n" + "echo \'Test Email for ustscwwpr360.production.calwin.org\' | mailx -v -r \'donotreply@calwin.org\' -s \'This is the subject ustscwwpr360.production.calwin.org\' -S \'smtp=CALWINSMTP01\'  calwinopsmiddleware@dxc.com";
                        mail.Importance = Outlook.OlImportance.olImportanceNormal;
                        ((Outlook._MailItem)mail).Send();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                richTextBox1.AppendText("Error: Unknown SMTP Host received CWA_PR360: " + iCounter1.ToString() + "\n\n");
                Application.DoEvents();

                // create the file ctrl_file_pr360.txt
                StreamWriter swPR360 = File.CreateText(@"C:\CWA_Rotation\PR360\ctrl_file_pr360.txt");
                swPR360.Write(dCWA_PR360_File_Line_Count.ToString() + "\n");
                swPR360.Close();
            }

            Check_CWA_PR370();

            // Delete the file if it exists.
            if (File.Exists(pathpr370))
            {
                File.Delete(pathpr370);
            }

            // check for CWA log rotation 
            // Get the linecount to skip from the control if one exists
            if (File.Exists(@"C:\CWA_Rotation\PR370\out.log.mohan.len"))
            {
                foreach (string line370 in File.ReadLines(@"C:\CWA_Rotation\PR370\out.log.mohan.len"))
                {
                    iFileLen = line370.Split(' ');

                    size = Int32.Parse(iFileLen[4]);

                    break;
                }
            }

            if (size < ipvPR370)
            {
                if (File.Exists(@"C:\CWA_Rotation\PR370\ctrl_file_pr370.txt"))
                {
                    File.Delete(@"C:\CWA_Rotation\PR370\ctrl_file_pr370.txt");
                }

                // CWA logs Rotatated in PR370
                // send a text message
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo;
                    mail.Subject = "CWA Log Rotation: USTACWWPR370";
                    mail.Body = "CWA Log Rotation in USTACWWPR370 Completed SuccessFully\n" + "No Action is required\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ipvPR370 = (int)size;

            Process_CWA_PR370_Log_File();

            if (iCounter2 > 0)
            {
                richTextBox2.AppendText("Error: Unknown SMTP Host received CWA_PR370: " + iCounter2.ToString() + "\n\n");
                Application.DoEvents();

                // now process for the actual errors in ECM_PR102
                if (iCounter2 > 1)
                {
                    // create the file ctrl_file_pr102.txt
                    StreamWriter swPR370 = File.CreateText(@"C:\CWA_Rotation\PR370\ctrl_file_pr370.txt");
                    swPR370.Write(dCWA_PR370_File_Line_Count.ToString() + "\n");
                    swPR370.Close();

                    // send a text message
                    try
                    {
                        Outlook._Application _app = new Outlook.Application();
                        Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                        mail.To = sMailTo;
                        mail.Subject = "Error: Unknown SMTP Host received CWA_PR370";
                        mail.Body = "Count: " + iCounter2.ToString() + "\n\n" + "Action: Bounce SMTP Instance in USTSCWWPR370\n\n" + "To Test SMTP, Run the Command below in USTSCWWPR370 and verify you receive an email:\n" + "echo \'Test Email for ustscwwpr370.production.calwin.org\' | mailx -v -r \'donotreply@calwin.org\' -s \'This is the subject ustscwwpr370.production.calwin.org\' -S \'smtp=CALWINSMTP01'  calwinopsmiddleware@dxc.com";
                        mail.Importance = Outlook.OlImportance.olImportanceNormal;
                        ((Outlook._MailItem)mail).Send();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                richTextBox2.AppendText("Error: Unknown SMTP Host received CWA_PR370: " + iCounter2.ToString() + "\n\n");
                Application.DoEvents();

                // create the file ctrl_file_pr102.txt
                StreamWriter swPR370 = File.CreateText(@"C:\CWA_Rotation\PR370\ctrl_file_pr370.txt");
                swPR370.Write(dCWA_PR370_File_Line_Count.ToString() + "\n");
                swPR370.Close();
            }

            MWMF.Form1.Sem.Release();
        }

        private void Check_CWA_PR360()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transder files over
            textBox1.Text = "CWA_PR360: Log File";
            textBox1.Refresh();
            button1.Enabled = false;
            button2.Enabled = false;
            richTextBox1.Clear();

            richTextBox1.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
            pr360PassWordE = inifile.IniReadValue("ustscwwpr360", "Password");
            pr360PassWordD = ed.DecryptString(key, pr360PassWordE);

            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw " + pr360PassWordD + " weblogic@148.92.137.120" + @" -m C:\CWA_Rotation\PR360\pr360.txt");
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
            richTextBox1.AppendText("CWA_PR360 Logout....." + "\n\n");

            richTextBox1.AppendText("Get File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            ef = new StringBuilder("pscp");
            // gh = new StringBuilder(" -pw Se8@2@2150 weblogic@148.92.137.120:/tmp/out.log.mohan*" + " " + @"C:\CWA_Rotation\PR360");
            gh = new StringBuilder(" -pw " + pr360PassWordD + " weblogic@148.92.137.120:/tmp/out.log.mohan*" + " " + @"C:\CWA_Rotation\PR360");

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
            richTextBox1.AppendText("CWA_PR360 Logout....." + "\n\n");

            button1.Enabled = true;
            button2.Enabled = true;
            textBox1.Text = "CWA_PR360: Done";
            Application.DoEvents();
        }

        private void Check_CWA_PR370()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transder files over
            textBox2.Text = "CWA_PR370: Log File";
            richTextBox2.Refresh();
            button2.Enabled = false;
            button1.Enabled = false;
            richTextBox2.Clear();

            richTextBox2.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
            pr370PassWordE = inifile.IniReadValue("ustscwwpr370", "Password");
            pr370PassWordD = ed.DecryptString(key, pr370PassWordE);

            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw " + pr370PassWordD + " weblogic@148.92.137.121" + @" -m C:\CWA_Rotation\PR370\pr370.txt");
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
            richTextBox2.AppendText("CWA_PR370 Logout....." + "\n\n");

            richTextBox2.AppendText("Get File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            ef = new StringBuilder("pscp");
            gh = new StringBuilder(" -pw " + pr370PassWordD + " weblogic@148.92.137.121:/tmp/out.log.mohan*" + " " + @"C:\CWA_Rotation\PR370");

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
            richTextBox2.AppendText("CWA_PR370 Logout....." + "\n\n");

            button2.Enabled = true;
            button1.Enabled = true;
            textBox2.Text = "CWA_PR370: Done";
            Application.DoEvents();
        }

        private void Process_CWA_PR360_Log_File()
        {
            Double dCWA_PR360_Skip_Line_Count = 0;
            iCounter1 = 0;
            dCWA_PR360_File_Line_Count = 0;
            string StrCWALogFileName = @"C:\CWA_Rotation\PR360\out.log.mohan";
            string StrctrlFileName = @"C:\CWA_Rotation\PR360\ctrl_file_pr360.txt";

            // Get the linecount to skip from the control if one exists
            if (File.Exists(StrctrlFileName))
            {
                foreach (string line101 in File.ReadLines(StrctrlFileName))
                {
                    dCWA_PR360_Skip_Line_Count = Convert.ToInt64(line101);

                    break;
                }
            }

            int iTemp = 0;
            foreach (string line in File.ReadLines(StrCWALogFileName))
            {
                if (iTemp < dCWA_PR360_Skip_Line_Count)
                {
                    iTemp++;

                    dCWA_PR360_File_Line_Count++;

                    continue;
                }

                dCWA_PR360_File_Line_Count++;

                if (line.Contains(@"Error was: Unknown SMTP host"))
                {
                    iCounter1++;
                }
            }
        }

        private void Process_CWA_PR370_Log_File()
        {
            Double dCWA_PR370_Skip_Line_Count = 0;
            iCounter2 = 0;
            dCWA_PR370_File_Line_Count = 0;
            string StrCWALogFileName = @"C:\CWA_Rotation\PR370\out.log.mohan";
            string StrctrlFileName = @"C:\CWA_Rotation\PR370\ctrl_file_pr370.txt";

            // Get the linecount to skip from the control if one exists
            if (File.Exists(StrctrlFileName))
            {
                foreach (string line102 in File.ReadLines(StrctrlFileName))
                {
                    dCWA_PR370_Skip_Line_Count = Convert.ToInt64(line102);

                    break;
                }
            }

            int iTemp = 0;
            foreach (string line in File.ReadLines(StrCWALogFileName))
            {
                if (iTemp < dCWA_PR370_Skip_Line_Count)
                {
                    iTemp++;

                    dCWA_PR370_File_Line_Count++;

                    continue;
                }

                dCWA_PR370_File_Line_Count++;

                if (line.Contains(@"Error was: Unknown SMTP host"))
                {
                    iCounter2++;
                }
            }
        }
    }
}
