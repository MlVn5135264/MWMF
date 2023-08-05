using System;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using EncryptDecrypt;
using Ini;
using System.Net.Mail;
using System.Net.Mime;

namespace MWMF
{
    public partial class Form29 : Form
    {
        public static string key = "\0";
        public ED ed = new ED();
        public IniFile inifile = new IniFile(@"c:\temp\config.ini");
        public string pr102PassWordE = "\0", pr102PassWordD = "\0";
        public string sMailTo = "'narayananm@gainwelltechnologies.com';'9162369772@text.att.net'";
        // public string sMailTo1 = "'narayananm@gainwelltechnologies.com';'richard.chiu@gainwelltechnologies.com';'pradeep.allaboyina@gainwelltechnologies.com'";
        // public string sMailTo2 = "'narayananm@gainwelltechnologies.com';'9162369772@text.att.net';'richard.chiu@gainwelltechnologies.com';'9169470604@text.att.net';'pradeep.allaboyina@gainwelltechnologies.com';'3093611299@mailmymobile.net'";
        public Form29()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder ef, gh;

            // create the file in c:\temp\ecm_metrics_command.txt
            string pathpr102 = @"c:\temp\ecm_metrics_command.txt";

            button1.Text = "Working.....";
            Application.DoEvents();

            // Delete the file if it exists.
            if (System.IO.File.Exists(pathpr102))
            {
                System.IO.File.Delete(pathpr102);
            }

            if (System.IO.File.Exists(@"c:\temp\alf_data.txt"))
            {
                System.IO.File.Delete(@"c:\temp\alf_data.txt");
            }

            if (System.IO.File.Exists(@"c:\ECM_METRICS\ECM_METRICS_TAB.txt"))
            {
                System.IO.File.Delete(@"c:\ECM_METRICS\ECM_METRICS_TAB.txt");
            }

            // write the command to be executed to the file ecm_metrics_commands.txt
            StreamWriter swpr102 = System.IO.File.CreateText(pathpr102);
            swpr102.Write(@". ~/.bashrc;" + "\n");
            swpr102.Write(@". ~/.bash_profile >/dev/null 2>&1;" + "\n");
            swpr102.Write(@"df -Plh | grep " + "\"alf_data\" " + "|" + " egrep -v \"contentstore\"" + " > " + "/tmp/alf_data.txt;");
            swpr102.Close();

            // get the password and decrypt it
            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
            pr102PassWordE = inifile.IniReadValue("usrnscwpr102_ecm_metrics", "Password");
            pr102PassWordD = ed.DecryptString(key, pr102PassWordE);

            ef = new StringBuilder("plink");
            gh = new StringBuilder(" -pw " + pr102PassWordD + " ecm_weblogic@172.22.0.6 " + @"-m c:\temp\ecm_metrics_command.txt");

            ProcessStartInfo psi = new ProcessStartInfo();
            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            richTextBox2.AppendText("Process File: Start Time: " + DateTime.Now.ToString() + "\n");
            richTextBox2.AppendText("ECM_PR102 Login....." + "\n\n");
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            richTextBox2.AppendText(s);
            richTextBox2.AppendText("Process File: End Time: " + DateTime.Now.ToString() + "\n");
            richTextBox2.AppendText("ECM_PR102 Logout....." + "\n\n");
            Application.DoEvents();

            // get the file created fromthe steps above
            try
            {
                ef = new StringBuilder("pscp");
                gh = new StringBuilder(" -pw " + pr102PassWordD + " ecm_weblogic@172.22.0.6:/tmp/alf_data.txt" + " " + @"C:\temp");
                psi = new ProcessStartInfo();
                psi.FileName = ef.ToString();
                psi.Arguments = gh.ToString();
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                richTextBox2.AppendText("\nProcess File: Start Time: " + DateTime.Now.ToString() + "\n");
                richTextBox2.AppendText("ECM_PR102 Login....." + "\n\n");
                proc = Process.Start(psi);
                s = proc.StandardOutput.ReadToEnd();
                richTextBox2.AppendText(s);
                richTextBox2.AppendText("\nProcess File: End Time: " + DateTime.Now.ToString() + "\n");
                richTextBox2.AppendText("ECM_PR102 Logout....." + "\n\n");
                button1.Enabled = true;
                System.Windows.Forms.Application.DoEvents();
                button1.Text = "Getting File from the Remote Server.....";
                
                Application.DoEvents();
                
                Thread.Sleep(20000);
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            // fire the Excel Macro now

            // Create the sorted Excel File Now. Sort in the ascending order of the Service name

            try
            {
                string batDir = string.Format("C:\\ECM_METRICS");
                proc = new Process();
                proc.StartInfo.WorkingDirectory = batDir;
                proc.StartInfo.FileName = "C:\\ECM_METRICS\\ECM_METRICS.bat";
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                // proc.WaitForExit();
                proc.Close();
                proc.Dispose();

                Thread.Sleep(15000);

                button1.Text = "Done. Closing.... Please Wait.....";

                System.Windows.Forms.Application.DoEvents();                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            // open the file ECM_METRICS_TAB.txt for further processing
            string[] lines = System.IO.File.ReadAllLines(@"C:\ECM_METRICS\ECM_METRICS_TAB.txt");
            StringBuilder sMailBody = new StringBuilder();
            StringBuilder sWarning = new StringBuilder();
            sMailBody.AppendLine("Time Left" + "\t\t" + "FILESYSTEM");

            int iLineCount = 0;

            foreach (string line in lines)
            {
                if ((iLineCount > 0) && (iLineCount <= 18))
                {
                    string[] ln = line.Split('\t');

                    sMailBody.AppendLine(ln[17] + "\t\t\t\t" + ln[0]);

                    if (Double.Parse(ln[17]) < 2.0)
                    {
                        string sCountyname = Path.GetFileName(ln[0]);
                        sWarning.AppendLine(sCountyname + " has less than 2 months of time, before SPACE runs out." + " Time left: " + ln[17] + " Month(s).");                    }              
                }
                else
                {
                    if (iLineCount > 18)
                    {
                        break;
                    }
                }

                // increment iLineCont by one
                iLineCount++;
            }

            if (sWarning.Length > 0)
            {
                // sSendWarningMail(sMailTo2, "WARNING: ECM Metrics ALF_DATA: Space Below Threshhod Value", sWarning);
                sSendWarningMail(sMailTo, "WARNING: ECM Metrics ALF_DATA: Space Below Threshhod Value", sWarning);
            }

            // sSendWarningMail(sMailTo1, "ECM Metrics ALF_DATA: Space Availability in Month(s)", sMailBody);          
               sSendWarningMail(sMailTo, "ECM Metrics ALF_DATA: Space Availability in Month(s)", sMailBody);
            // Close the application
            this.Close();
        }

        private void sSendWarningMail(string sMailTo, string sSubject, StringBuilder sBody)
        {
            // Send the report as an email
            try
            {
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = sMailTo.ToString();
                mail.Subject = sSubject;
                // mail.Body = sMailBody.ToString() + "\n";
                mail.Body = sBody.ToString() + "\n";
                mail.Importance = Outlook.OlImportance.olImportanceNormal;
                ((Outlook._MailItem)mail).Send();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
