using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MWMF
{
    public partial class Form4 : Form
    {
        public string[] times, StartTime, EndTime;
        public string strStartTime, strEndTime, strFileName;
        StringBuilder sb = new StringBuilder("Tuxedo Help:", 5000);
        char cFoundErr = 'n';

        public Form4()
        {
            InitializeComponent();
        }

        // if Submit button is Clicked
        private void b_Submit_Click(object sender, EventArgs e)
        {
            int iCompare, iCompare1, iCompare2;
            char cContains = 'N';
            string StrUlogFileName;

            // Clear Results window first
            rtb_log.Text = "";

            StrUlogFileName = @"c:\temp\" + "ulog" + "." + System.DateTime.Today.ToString("MMddyy");
            if (cb_date.Checked == true && cb_pattern.Checked == true)
            {
                iCompare = string.Compare(strStartTime, strEndTime);

                #if DEBUG
                //MessageBox.Show(strStartTime);
                //MessageBox.Show(strEndTime);
                #endif

                // start the ulog processing now
                // open the ulog file first

                if (iCompare <= 0)
                {
                    // Read in lines from file.
                    foreach (string line in File.ReadLines(StrUlogFileName))
                    {
                        string[] ulogEntry = line.Split('.');
                        string[] pattern = tb_pattern.Text.Split(',');
                        int iCount = pattern.Count();

                        iCompare1 = string.Compare(ulogEntry[0], strStartTime);
                        iCompare2 = string.Compare(ulogEntry[0], strEndTime);

                        if (iCompare1 >= 0 && iCompare2 <= 0)
                        {
                            for (int i = 0; i < iCount; i++)
                            {
                                if(line.Contains(pattern[i]))
                                {
                                    cContains = 'Y';
                                }
                            }

                            if (cContains == 'Y')
                            {
                                rtb_log.AppendText(line);
                                rtb_log.AppendText("\n");
                                rtb_log.Enabled = true;
                                button1.Enabled = true;
                                cContains = 'N';
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Start time is greater that end time", "Error in Time Entry");
                }                
            }

            if (cb_date.Checked == true && cb_pattern.Checked == false)
            {
                iCompare = string.Compare(strStartTime, strEndTime);

                #if DEBUG
                //MessageBox.Show(strStartTime);
                //MessageBox.Show(strEndTime);
                #endif

                // start the ulog processing now
                // open the ulog file first

                if (iCompare <= 0)
                {
                    // Read in lines from file.
                    foreach (string line in File.ReadLines(StrUlogFileName))
                    {
                        string[] ulogEntry = line.Split('.');
                        string[] pattern = tb_pattern.Text.Split(',');
                        int iCount = pattern.Count();

                        iCompare1 = string.Compare(ulogEntry[0], strStartTime);
                        iCompare2 = string.Compare(ulogEntry[0], strEndTime);

                        if (iCompare1 >= 0 && iCompare2 <= 0)
                        {
                            rtb_log.AppendText(line);
                            rtb_log.AppendText("\n");
                            rtb_log.Enabled = true;
                            button1.Enabled = true;
                        }                   
                    }
                }
                else
                {
                    MessageBox.Show("Start time is greater that end time", "Error in Time Entry");
                }
            }


            if (cb_date.Checked == false && cb_pattern.Checked == true)
            {
                // start the ulog processing now
                // open the ulog file first

                // Read in lines from file.
                foreach (string line in File.ReadLines(StrUlogFileName))
                {
                    string[] pattern = tb_pattern.Text.Split(',');
                    int iCount = pattern.Count();

                    for (int i = 0; i < iCount; i++)
                    {
                        if (line.Contains(pattern[i]))
                        {
                            cContains = 'Y';
                        }
                    }

                    if (cContains == 'Y')
                    {
                        rtb_log.AppendText(line);
                        rtb_log.AppendText("\n");
                        cContains = 'N';
                        rtb_log.Enabled = true;
                        button1.Enabled = true;
                    }
                }
                rtb_log.Copy();
            }
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            if(cb_date.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                dateTimePicker4.Enabled = true;
                b_Submit.Enabled = true;
            }
            else
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                dateTimePicker4.Enabled = false;
                if(cb_pattern.Checked == false)
                {
                    b_Submit.Enabled = false;
                }
            }
        }

        private void cb_pattern_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_pattern.Checked == true)
            {
                tb_pattern.Text = "";
                tb_pattern.Enabled = true;
                b_Submit.Enabled = true;
            }
            else
            {
                tb_pattern.Text = "Enter Text Comma Separated";
                tb_pattern.Enabled = false;
                if(cb_date.Checked == false)
                {
                    b_Submit.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtb_log.Text = null;
            rtb_log.Enabled = false;
        }

        private void cb_instancename_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder ab = new StringBuilder();
            StringBuilder cd = new StringBuilder();
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            string instancenameSYS = "r2stg,r1stg,r2sysa,r2sysb,r1sysa,r1sysb,mresys,rgtsys,pprsys,pstg,ustg";
            if ( instancenameSYS.Contains(cb_InstanceName.Text))
            {
                tb_ServerName.Text = "usrncwust700-10.3.2.254";
                cd = new StringBuilder(" -pw Apr@2@2! tuxedo@10.3.2.254 -m c:\\temp\\m.txt");
            }

            string instancenameTRN = "trn3a,trn3b,trn3c";
            if (instancenameTRN.Contains(cb_InstanceName.Text))
            {
                tb_ServerName.Text = "usrncwutr700-10.2.2.4";                                      
                cd = new StringBuilder(" -pw Sonoran1 tuxedo@10.2.2.4 -m c:\\temp\\m.txt");
            }

            string instancenameUAT = "uat1a,uat1b,uat2a,uat2b";
            if (instancenameUAT.Contains(cb_InstanceName.Text))
            {
                tb_ServerName.Text = "usrncwuuaa1-10.2.2.254";
                cd = new StringBuilder(" -pw March@2019 tuxedo@10.2.2.254 -m c:\\temp\\m.txt");
            }

            string instancenamePROD1 = "p1a,p1b,p2a,p2b";
            if (instancenamePROD1.Contains(cb_InstanceName.Text))
            {
                tb_ServerName.Text = "usrnucwpr201-148.92.137.10";
                cd = new StringBuilder(" -pw D$c2@2! tuxedo@148.92.137.10 -m c:\\temp\\m.txt");
            }

            string instancenamePROD2 = "p3a,p3b,p4a,p4b";
            if (instancenamePROD2.Contains(cb_InstanceName.Text))
            {
                tb_ServerName.Text = "usrnucwpr202-148.92.137.11";
                cd = new StringBuilder(" -pw D$c2@2! tuxedo@148.92.137.11 -m c:\\temp\\m.txt");
            }

            // get the ulog path by pinging the instance in the server listed.
            ab = new StringBuilder("plink");
            ef = new StringBuilder("pscp");

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            // Creates the file.
            // create the command file m.txt
            string path = @"c:\temp\m.txt";
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            StreamWriter sw = File.CreateText(path);
            sw.WriteLine(". ~/.kshrc;");
            sw.WriteLine(". ~/.profile >/dev/null 2>&1;");
            sw.Write(". set_tuxenv ");
            sw.Write(cb_InstanceName.Text);
            sw.WriteLine(";");
            sw.WriteLine("echo $ULOGPFX 1>1.txt 2>2.txt;");
            sw.WriteLine("cat 1.txt;");
            sw.WriteLine("cat 2.txt;");
            sw.Close();

            // Connect to BD700
            // ProcessStartInfo psi = new ProcessStartInfo(@"plink", " -pw Jan@2019 tuxedo@10.3.2.7 -m c:\\temp\\m.txt");         

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();

            // get ulog full path and display
            string[] ulogPath = s.Split('\n');
            tb_logfilepath.Text = ulogPath[0] + "." + System.DateTime.Today.ToString("MMddyy");

            // ************************************************************************************************************

            if (instancenameSYS.Contains(cb_InstanceName.Text))
            {
                gh = new StringBuilder(" -pw Oct@ber$2019 tuxedo@10.3.2.254:" + tb_logfilepath.Text + " " + "c:\\temp");
            }

            if (instancenameTRN.Contains(cb_InstanceName.Text))
            {
                gh = new StringBuilder(" -pw Sonoran1 tuxedo@10.2.2.4:" + tb_logfilepath.Text + " " + "c:\\temp");
            }

            if (instancenameUAT.Contains(cb_InstanceName.Text))
            {
                gh = new StringBuilder(" -pw March@2019 tuxedo@10.2.2.254:" + tb_logfilepath.Text + " " + "c:\\temp");
            }

            if (instancenamePROD1.Contains(cb_InstanceName.Text))
            {
                gh = new StringBuilder(" -pw D$c2@2! tuxedo@148.92.137.10:" + tb_logfilepath.Text + " " + "c:\\temp");
            }

            if (instancenamePROD2.Contains(cb_InstanceName.Text))
            {
                gh = new StringBuilder(" -pw D$c2@2! tuxedo@148.92.137.11:" + tb_logfilepath.Text + " " + "c:\\temp");
            }

            if (b_Submit.Enabled == true)
            {
                b_Submit.Enabled = false;
            }
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
            rtb_log.Text = s;
            String[] FileCopy = s.Split('|');
            int  iMemberCount = FileCopy.Count();
            for(int i = 0; i < iMemberCount; i++)
            {
                if(FileCopy[i].Contains("100%"))
                {                    
                    b_Submit.Enabled = true;
                }
            }
            
            // ************************************************************************************************************
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtb_log.SelectedText);
            rtb_log.SelectedText = string.Empty;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            Clipboard.SetText(rtb_log.SelectedText);
            string[] tuxhelp = Clipboard.GetText().Split(':');
            int iCount = tuxhelp.Count();

            for(int i = 0; i < iCount; iCount++)
            {
                if(tuxhelp[i].Contains("TMADMIN_CAT"))
                {
                    strFileName = "c:\\temp\\tmadmin_cat.txt";

                    break;
                }

                if (tuxhelp[i].Contains("CMDTUX_CAT"))
                {
                    strFileName = "c:\\temp\\cmdtux_cat.txt";

                    break;
                }

                if (tuxhelp[i].Contains("LIBTUX_CAT"))
                {
                    strFileName = "c:\\temp\\libtux_cat.txt";

                    break;
                }
            }

            foreach (string line in File.ReadLines(strFileName))
            {
                bool IsNumber = false;
                int iCompare = -1;
                string[] linecontent = line.Split(' ');
                
                iCompare = string.Compare(linecontent[0], "");
                if((iCompare == 0) && (cFoundErr == 'y'))
                {
                    sb.Append(Environment.NewLine);
                    continue;
                }

                IsNumber =  IsNumeric(linecontent[0]);

                if ((IsNumber == true) && (cFoundErr == 'y'))
                {
                    MessageBox.Show(sb.ToString());

                    break;
                }

                if ((IsNumber == true) && (cFoundErr == 'n'))
                {
                    iCompare = string.Compare(linecontent[0], tuxhelp[1]);

                    if(iCompare == 0)
                    {
                        cFoundErr = 'y';
                        sb.Append(Environment.NewLine);
                        sb.Append(line);
                        sb.Append(Environment.NewLine);
                    }
                }
                else
                {
                    if((cFoundErr == 'y'))
                    {
                        sb.Append(line);
                        sb.Append(Environment.NewLine);
                    }
                }
            }
        }

        private void rtb_log_TextChanged(object sender, EventArgs e)
        {

        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.ShowUpDown = true;

            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.ShowUpDown = true;

            string[] times = dateTimePicker2.Text.Split(' ');
            string[] StartTime = times[0].Split(':');

            times = dateTimePicker4.Text.Split(' ');
            string[] EndTime = times[0].Split(':');

            strStartTime = StartTime[0] + StartTime[1] + StartTime[2];
            strEndTime = EndTime[0] + EndTime[1] + EndTime[2];
       }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            times = dateTimePicker2.Text.Split(' ');
            StartTime = times[0].Split(':');
            strStartTime = StartTime[0] + StartTime[1] + StartTime[2];

            times = dateTimePicker4.Text.Split(' ');
            EndTime = times[0].Split(':');
            strEndTime = EndTime[0] + EndTime[1] + EndTime[2];
        }
        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            times = dateTimePicker2.Text.Split(' ');
            StartTime = times[0].Split(':');
            strStartTime = StartTime[0] + StartTime[1] + StartTime[2];

            times = dateTimePicker4.Text.Split(' ');
            EndTime = times[0].Split(':');
            strEndTime = EndTime[0] + EndTime[1] + EndTime[2];
        }
    }
}
