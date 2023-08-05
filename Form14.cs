using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MWMF
{
    public partial class Form14 : Form
    {
        public System.Windows.Forms.Timer MyTimer1;
        public int iCounter1, iCounter2;
        public int dECM_PR101_File__Line_Count = 0, dECM_PR102_File__Line_Count = 0;
        public int size_backup, size, ipvPR101= 0, ipvPR102 = 0;
        public string backup_fileName, fileName;
        public StringBuilder cMailTo = new StringBuilder("", 5000);
        public string[] iFileLen;
        public string sFileNameToProcess = "out.log";
        public int iTimer;

        // public string sMailTo = "'narayananm@dxc.com';'9162369772@mms.att.net';'steven.deyo@dxc.com';'9132263942@messaging.sprintpcs.com';'joe.mendez@dxc.com';'9162840731@vtext.com';'richard.chiu@dxc.com';'9169470604@mms.att.net';'poornaviswanathan.manickam@dxc.com';'velayutham@dxc.com';'vrajamreddy2@dxc.com';'9165474216@vtext.com';'m216@dxc.com';'s.manoharan@dxc.com'";
        public string sMailTo = "narayananm@dxc.com;9162369772@mms.att.net";

        public Form14()
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

        private void Form14_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
            button3.Enabled = false;

            // MyTimer1 = new System.Windows.Forms.Timer();
            // MyTimer1.Interval = ( 3 * 60 * 1000); // 5 Minutes
            // MyTimer1.Tick += new EventHandler(MyTimer1_Tick);
            // MyTimer1.Start();
            // MyTimer1.Dispose();
        }

        private void MyTimer1_Tick(object sender, EventArgs e)
        {
            MWMF.Form1.Sem.WaitAsync();
            size_backup = 0;
            size = 0;

            button3.Enabled = true;

            // create the command file for pr101
            string pathpr101 = @"C:\DPE_Rotation\PR101\pr101.txt";
            // Delete the file if it exists.
            if (File.Exists(pathpr101))
            {
                File.Delete(pathpr101);
            }
            // create the file pr101.txt in C:\DPE_Rotation\PR101
            StreamWriter swpr101 = File.CreateText(pathpr101);
            swpr101.Write(@". ~/.bashrc;" + "\n");
            swpr101.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr101.Write(@"grep -e ""Error code : 500 received from Alfresco"" /PRD/ECM/DPE/node_modules/oodebe/logs/" + sFileNameToProcess +" > /tmp/out.log.mohan;" + "\n");
            swpr101.Write(@"ls -l /PRD/ECM/DPE/node_modules/oodebe/logs/" + sFileNameToProcess + " > /tmp/out.log.mohan.len;");
            swpr101.Close();

            // create the command file for pr102
            string pathpr102 = @"C:\DPE_Rotation\PR102\pr102.txt";
            // Delete the file if it exists.
            if (File.Exists(pathpr102))
            {
                File.Delete(pathpr102);
            }
            // create the file pr102.txt in C:\DPE_Rotation\PR102
            StreamWriter swpr102 = File.CreateText(pathpr102);
            swpr102.Write(@". ~/.bashrc;" + "\n");
            swpr102.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr102.Write(@"grep -e ""Error code : 500 received from Alfresco"" /PRD/ECM/DPE/node_modules/oodebe/logs/" + sFileNameToProcess + " > /tmp/out.log.mohan;" + "\n");
            swpr102.Write(@"ls -l /PRD/ECM/DPE/node_modules/oodebe/logs/" + sFileNameToProcess + " > /tmp/out.log.mohan.len;");
            swpr102.Close();

            Check_ECM_PR0D1();

            // Delete the file if it exists.
            if (File.Exists(pathpr101))
            {
                File.Delete(pathpr101);
            }

            // check for DPE log rotation 
            // Get the linecount to skip from the control if one exists
            if (File.Exists(@"C:\DPE_Rotation\PR101\out.log.mohan.len"))
            {
                foreach (string line101 in File.ReadLines(@"C:\DPE_Rotation\PR101\out.log.mohan.len"))
                {
                    iFileLen = line101.Split(' ');

                    size = Int32.Parse(iFileLen[4]);

                    // ipvPR101 = (int) size;
                    
                    break;
                }
            }

            if (size < ipvPR101)
            {
                if (File.Exists(@"C:\DPE_Rotation\PR101\ctrl_file_pr101.txt"))
                {
                    File.Delete(@"C:\DPE_Rotation\PR101\ctrl_file_pr101.txt");
                }

                // DPE logs Rotatated in PR101
                // Send Message
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo;
                    mail.Subject = "DPE Log Rotation: USRNSCWPR101";
                    mail.Body = "DPE Log Rotation in USRNSCWPR101 Completed SuccessFully\n" + "No Action is required\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ipvPR101 = (int)size;

            Process_ECM_PROD1_Log_File();

            if (iCounter1 > 0)
            {
                richTextBox1.AppendText("500 Error Count in ECM_PR101: " + iCounter1.ToString() + "\n\n");
                Application.DoEvents();

                // now process for the actual errors in ECM_PR101
                if(iCounter1 > 9)
                {
                    // create the file ctrl_file_pr101.txt
                    StreamWriter swPR101 = File.CreateText(@"C:\DPE_Rotation\PR101\ctrl_file_pr101.txt");
                    swPR101.Write(dECM_PR101_File__Line_Count.ToString() + "\n");
                    swPR101.Close();

                    // send a text message
                    try
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
                }
            }
            else
            {
                richTextBox1.AppendText("500 Error Count in ECM_PR101: " + iCounter1.ToString() + "\n\n");
                Application.DoEvents();

                // create the file ctrl_file_pr101.txt
                StreamWriter swPR101 = File.CreateText(@"C:\DPE_Rotation\PR101\ctrl_file_pr101.txt");
                swPR101.Write(dECM_PR101_File__Line_Count.ToString() + "\n");
                swPR101.Close();
            }

            Check_ECM_PR0D2();

            // Delete the file if it exists.
            if (File.Exists(pathpr102))
            {
                File.Delete(pathpr102);
            }

            // check for DPE log rotation 
            // Get the linecount to skip from the control if one exists
            if (File.Exists(@"C:\DPE_Rotation\PR102\out.log.mohan.len"))
            {
                foreach (string line101 in File.ReadLines(@"C:\DPE_Rotation\PR102\out.log.mohan.len"))
                {
                    iFileLen = line101.Split(' ');

                    size = Int32.Parse(iFileLen[4]);
                
                    break;
                }
            }

            if (size < ipvPR102)
            {
                if (File.Exists(@"C:\DPE_Rotation\PR102\ctrl_file_pr102.txt"))
                {
                    File.Delete(@"C:\DPE_Rotation\PR102\ctrl_file_pr102.txt");
                }

                // DPE logs Rotatated in PR102
                // send a text message
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = sMailTo;
                    mail.Subject = "DPE Log Rotation: USRNSCWPR102";
                    mail.Body = "DPE Log Rotation in USRNSCWPR102 Completed SuccessFully\n" + "No Action is required\n";
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ipvPR102 = (int) size;

            Process_ECM_PROD2_Log_File();

            if (iCounter2 > 0)
            {
                richTextBox2.AppendText("500 Error Count in ECM_PR102: " + iCounter2.ToString() + "\n\n");
                Application.DoEvents();

                // now process for the actual errors in ECM_PR102
                if (iCounter2 > 9)
                {
                    // create the file ctrl_file_pr102.txt
                    StreamWriter swPR102 = File.CreateText(@"C:\DPE_Rotation\PR102\ctrl_file_pr102.txt");
                    swPR102.Write(dECM_PR102_File__Line_Count.ToString() + "\n");
                    swPR102.Close();

                    // send a text message
                    try
                    {
                        Outlook._Application _app = new Outlook.Application();
                        Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                        mail.To = sMailTo;
                        mail.Subject = "Error code : 500 received from Alfresco on ECM_PR102";
                        mail.Body = "Count: " + iCounter2.ToString() + "\n\n" + "Action: Bounce JVM ECMPRD02-Server01 in usrnscwpr102 (172.22.0.6)\n";
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
                richTextBox2.AppendText("500 Error Count in ECM_PR102: " + iCounter2.ToString() + "\n\n");
                Application.DoEvents();

                // create the file ctrl_file_pr102.txt
                StreamWriter swPR102 = File.CreateText(@"C:\DPE_Rotation\PR102\ctrl_file_pr102.txt");
                swPR102.Write(dECM_PR102_File__Line_Count.ToString() + "\n");
                swPR102.Close();
            }

            MWMF.Form1.Sem.Release();
        }

        private void Check_ECM_PR0D1()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transder files over
            textBox1.Text = "ECM_PR101: Log File";
            textBox1.Refresh();
            button1.Enabled = false;
            button2.Enabled = false;
            richTextBox1.Clear();

            richTextBox1.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();
            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw 4P'[wd(c) ecm@172.22.0.5" + @" -m C:\DPE_Rotation\PR101\pr101.txt");
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
 
            richTextBox1.AppendText("Get File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            ef = new StringBuilder("pscp");
            gh = new StringBuilder(" -pw 4P'[wd(c) ecm@172.22.0.5:/tmp/out.log.mohan*" + " " + @"C:\DPE_Rotation\PR101");

            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
            richTextBox1.AppendText(s);
            richTextBox1.AppendText("\n" +"Get File: End Time: " + DateTime.Now.ToString() + "\n");
            richTextBox1.AppendText("ECM_PR101 Logout....." + "\n\n");

            button1.Enabled = true;
            button2.Enabled = true;
            textBox1.Text = "ECM_PR101: Done";
            Application.DoEvents();
        }

        private void Check_ECM_PR0D2()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // transder files over
            textBox2.Text = "ECM_PR102: Log File";
            richTextBox2.Refresh();
            button2.Enabled = false;
            button1.Enabled = false;
            richTextBox2.Clear();

            richTextBox2.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();
            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw 4P'[wd(c) ecm@172.22.0.6" + @" -m C:\DPE_Rotation\PR102\pr102.txt");
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
            richTextBox2.AppendText("ECM_PR102 Logout....." + "\n\n");
 
            richTextBox2.AppendText("Get File: Start Time: " + DateTime.Now.ToString() + "\n");
            Application.DoEvents();

            ef = new StringBuilder("pscp");
            gh = new StringBuilder(" -pw 4P'[wd(c) ecm@172.22.0.6:/tmp/out.log.mohan*" + " " + @"C:\DPE_Rotation\PR102");
           
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

            button2.Enabled = true;
            button1.Enabled = true;
            textBox2.Text = "ECM_PR102: Done";
            Application.DoEvents();
        }

        private void Process_ECM_PROD1_Log_File()
        {
            Double dECM_PR101_Skip_Line_Count = 0;
            iCounter1 = 0;
            dECM_PR101_File__Line_Count = 0;
            string StrDPELogFileName = @"C:\DPE_Rotation\PR101\out.log.mohan";
            string StrctrlFileName = @"C:\DPE_Rotation\PR101\ctrl_file_pr101.txt";

            // Get the linecount to skip from the control if one exists
            if (File.Exists(StrctrlFileName))
            {
                foreach (string line101 in File.ReadLines(StrctrlFileName))
                {
                    dECM_PR101_Skip_Line_Count = Convert.ToInt64(line101);

                    break;
                }
            }

            int iTemp = 0;
            foreach (string line in File.ReadLines(StrDPELogFileName))
            {
                if (iTemp < dECM_PR101_Skip_Line_Count)
                {
                    iTemp++;

                    dECM_PR101_File__Line_Count++;

                    continue;
                }

                dECM_PR101_File__Line_Count++;

                if (line.Contains(@"Error code : 500 received from Alfresco"))
                {
                    iCounter1++;
                }
            }
        }

        private void Process_ECM_PROD2_Log_File()
        {
            Double dECM_PR102_Skip_Line_Count = 0;
            iCounter2 = 0;
            dECM_PR102_File__Line_Count = 0;
            string StrDPELogFileName = @"C:\DPE_Rotation\PR102\out.log.mohan";
            string StrctrlFileName = @"C:\DPE_Rotation\PR102\ctrl_file_pr102.txt";

            // Get the linecount to skip from the control if one exists
            if (File.Exists(StrctrlFileName))
            {
                foreach (string line102 in File.ReadLines(StrctrlFileName))
                {
                    dECM_PR102_Skip_Line_Count = Convert.ToInt64(line102);

                    break;
                }
            }
            
            int iTemp = 0;
            foreach (string line in File.ReadLines(StrDPELogFileName))
            {
                if(iTemp < dECM_PR102_Skip_Line_Count)
                {
                    iTemp++;

                    dECM_PR102_File__Line_Count++;

                    continue;
                }

                dECM_PR102_File__Line_Count++;

                if (line.Contains(@"Error code : 500 received from Alfresco"))
                {
                    iCounter2++;
                }
            }
        }
    }
}
