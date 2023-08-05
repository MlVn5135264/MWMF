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
using EncryptDecrypt;
using Ini;

namespace MWMF
{
    public partial class Form6 : Form
    {
        public string[] times, StartTime, EndTime, strMonthDay;
        public string strStartTime, strEndTime, strFileName, cCurrentDate, pr201PassWordE, pr201PassWordD, pr202PassWordE, pr202PassWordD, key;
        public char cProcessPr201, cProcessPr202;

        public ED ed = new ED();
        public IniFile inifile = new IniFile(@"c:\temp\config.ini");

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= (checkedListBox2.Items.Count - 1); i++)
            {
                checkedListBox1.SetItemCheckState(i, CheckState.Checked);
            }

            for (int i = 0; i <= (checkedListBox2.Items.Count - 1); i++)
            {
                checkedListBox2.SetItemCheckState(i, CheckState.Checked);
            }

            for (int i = 0; i <= (checkedListBox4.Items.Count - 1); i++)
            {
                checkedListBox4.SetItemCheckState(i, CheckState.Checked);
            }

            for (int i = 0; i <= (checkedListBox3.Items.Count - 1); i++)
            {
                checkedListBox3.SetItemCheckState(i, CheckState.Checked);
            }

            for (int i = 0; i <= (checkedListBox8.Items.Count - 1); i++)
            {
                checkedListBox8.SetItemCheckState(i, CheckState.Checked);
            }

            for (int i = 0; i <= (checkedListBox7.Items.Count - 1); i++)
            {
                checkedListBox7.SetItemCheckState(i, CheckState.Checked);
            }

            for (int i = 0; i <= (checkedListBox6.Items.Count - 1); i++)
            {
                checkedListBox6.SetItemCheckState(i, CheckState.Checked);
            }

            for (int i = 0; i <= (checkedListBox5.Items.Count - 1); i++)
            {
                checkedListBox5.SetItemCheckState(i, CheckState.Checked);
            }

            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
            {
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
            }

            for (int i = 0; i <= (checkedListBox2.Items.Count - 1); i++)
            {
                checkedListBox2.SetItemCheckState(i, CheckState.Unchecked);
            }

            for (int i = 0; i <= (checkedListBox4.Items.Count - 1); i++)
            {
                checkedListBox4.SetItemCheckState(i, CheckState.Unchecked);
            }

            for (int i = 0; i <= (checkedListBox3.Items.Count - 1); i++)
            {
                checkedListBox3.SetItemCheckState(i, CheckState.Unchecked);
            }

            for (int i = 0; i <= (checkedListBox8.Items.Count - 1); i++)
            {
                checkedListBox8.SetItemCheckState(i, CheckState.Unchecked);
            }

            for (int i = 0; i <= (checkedListBox7.Items.Count - 1); i++)
            {
                checkedListBox7.SetItemCheckState(i, CheckState.Unchecked);
            }

            for (int i = 0; i <= (checkedListBox6.Items.Count - 1); i++)
            {
                checkedListBox6.SetItemCheckState(i, CheckState.Unchecked);
            }

            for (int i = 0; i <= (checkedListBox5.Items.Count - 1); i++)
            {
                checkedListBox5.SetItemCheckState(i, CheckState.Unchecked);
            }

            button2.Enabled = true;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            button4.Enabled = false;
        }

        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // local variables
            StringBuilder ab = new StringBuilder();
            StringBuilder cd = new StringBuilder();
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();
            string[] cName_cCountCd;
            char cPath;

            button4.Text = "Clear Messages";
            button4.Enabled = true;


            if (dateTimePicker1.Value == dateTimePicker1.MaxDate)
            {
                cCurrentDate = "y";
            }
            else
            {
                cCurrentDate = "n";
            }

            button1.Text = "Running TXRPT. Please Wait.....";
            button1.Enabled = false;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            // Display the date as "Mon 27 Feb 2012".  
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";

            // declare path to the command file
            string path = @"c:\Temp\pr201.txt";
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // create the file pr201.txt in c:Temp
            StreamWriter sw = File.CreateText(path);
            sw.WriteLine(". ~/.kshrc;");
            sw.WriteLine(". ~/.profile >/dev/null 2>&1;");
            sw.WriteLine();

            // The main processing Starts here
            cPath = 'N';
            cProcessPr201 = 'N';

            foreach (string s in checkedListBox1.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox3.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox3.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr201 = 'Y';
                }
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }

            // The main processing Starts here
            cPath = 'N';
            foreach (string s in checkedListBox2.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox4.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox4.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr201 = 'Y';
                }
                
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }

            // The main processing Starts here
            cPath = 'N';
            foreach (string s in checkedListBox4.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox6.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox6.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr201 = 'Y';
                }
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }

            // The main processing Starts here
            cPath = 'N';
            foreach (string s in checkedListBox3.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox5.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox5.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr201 = 'Y';
                }
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }

            sw.Close();

            if (cProcessPr201 == 'Y')
            {
                // get the encrypted password from config.ini file and decrypt it
                key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
                pr201PassWordE = inifile.IniReadValue("usrnucwpr201", "Password");
                pr201PassWordD = ed.DecryptString(key, pr201PassWordE);

                richTextBox1.Text = "Getting Ready to extract the Tuxedo Response Time Report in PR201\n\n";
                Application.DoEvents();
                ab = new StringBuilder("plink");
                cd = new StringBuilder(" -pw " + pr201PassWordD + " tuxedo@148.92.137.10 -m c:\\Temp\\pr201.txt");
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = ab.ToString();
                psi.Arguments = cd.ToString();

                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                var proc = Process.Start(psi);
                string s = proc.StandardOutput.ReadToEnd();
                richTextBox1.AppendText(s);
                richTextBox1.AppendText("\nExtracted the Tuxedo Response Time Report from PR201, Successfully\n");
                Application.DoEvents();

                path = @"c:\Temp\" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // transder files over
                richTextBox1.AppendText("\nTransferring the Tuxedo Response Time Report from PR201\n");
                Application.DoEvents();
                ef = new StringBuilder("pscp");
                gh = new StringBuilder(" -pw " + pr201PassWordD + " tuxedo@148.92.137.10:" + "/home/tuxedo/???.txt" + " " + path);
                psi = new ProcessStartInfo();
                psi.FileName = ef.ToString();
                psi.Arguments = gh.ToString();
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                proc = Process.Start(psi);
                s = proc.StandardOutput.ReadToEnd();
                richTextBox1.AppendText(s);
                button4.Enabled = true;
                richTextBox1.AppendText("\nTransfered the Tuxedo Response Time Report from PR201 successfully\n");
                Application.DoEvents();

                // delete the text files created in pr201
                ab = new StringBuilder("plink");
                cd = new StringBuilder(" -pw " + pr201PassWordD + " tuxedo@148.92.137.10 -m c:\\Temp\\pr201_del.txt");
                psi = new ProcessStartInfo();
                psi.FileName = ab.ToString();
                psi.Arguments = cd.ToString();

                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                proc = Process.Start(psi);
                s = proc.StandardOutput.ReadToEnd();
                richTextBox1.AppendText(s + "\n");
                Application.DoEvents();
            }

            // declare path to the command file
            path = @"c:\Temp\pr202.txt";
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // create the file pr202.txt in c:Temp
            sw = File.CreateText(path);
            sw.WriteLine(". ~/.kshrc;");
            sw.WriteLine(". ~/.profile >/dev/null 2>&1;");
            sw.WriteLine();

            // The main processing Starts here
            cPath = 'N';
            cProcessPr202 = 'N';
            // The main processing Starts here
            foreach (string s in checkedListBox8.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox10.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox10.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr202 = 'Y';
                }
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }

            // The main processing Starts here
            cPath = 'N';
            foreach (string s in checkedListBox7.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox9.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox9.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr202 = 'Y';
                }
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }

            // The main processing Starts here
            cPath = 'N';
            foreach (string s in checkedListBox6.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox8.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox8.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr202 = 'Y';
                }
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }

            // The main processing Starts here
            cPath = 'N';
            foreach (string s in checkedListBox5.CheckedItems)
            {
                cName_cCountCd = s.Split(' ');
                strMonthDay = dateTimePicker1.Text.Split('/');

                // cd/PRD/tuxedo/p1a/spool_ar;
                if (cPath == 'N')
                {
                    if (cCurrentDate == "n")
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox7.Text + "/" + "spool_ar;");
                    }
                    else
                    {
                        sw.WriteLine("cd /PRD/tuxedo/" + groupBox7.Text + "/" + "spool;");
                    }

                    cPath = 'Y';
                    cProcessPr202 = 'Y';
                }
                // gunzip stderr34_reg_04292019.gz;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gunzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ".gz;");
                }

                //txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d 04/29 -s17:00:00 -e18:59:59 < stderr34_reg_04292019 > /home/tuxedo/sac.txt;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }
                else
                {
                    sw.WriteLine("txrpt -t -nED0006U,IQV101F,IQ0006F,IC0001F,GS0008F,IN0001F,INC050U,WU0010F,AA0009F,IQ0031F,AU0001U,WUV001F,CC0027F,AA0011U,CC0015F,CC0100U,SE0503F,INO005F,GSA003F,AA0601F,AAW200F -d" + strMonthDay[0] + "/" + strMonthDay[1] + " -s" + dateTimePicker2.Text + " -e" + dateTimePicker4.Text + " < stderr" + cName_cCountCd[1] + "_reg" + " > /home/tuxedo/" + cName_cCountCd[0] + ".txt;");
                }

                // gzip stderr48_reg_04292019;
                if (cCurrentDate == "n")
                {
                    sw.WriteLine("gzip stderr" + cName_cCountCd[1] + "_reg_" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2] + ";");
                }

                sw.WriteLine();
            }
            sw.Close();

            if (cProcessPr202 == 'Y')
            {
                // get the encrypted password from config.ini file and decrypt it
                key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";
                pr202PassWordE = inifile.IniReadValue("usrnucwpr202", "Password");
                pr202PassWordD = ed.DecryptString(key, pr202PassWordE);

                ab = new StringBuilder("plink");
                cd = new StringBuilder(" -pw " + pr202PassWordD + " tuxedo@148.92.137.11 -m c:\\Temp\\pr202.txt");
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = ab.ToString();
                psi.Arguments = cd.ToString();

                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                var proc = Process.Start(psi);
                string s = proc.StandardOutput.ReadToEnd();

                if (cProcessPr201 == 'Y')
                {
                    richTextBox1.AppendText(s);
                }
                else
                {
                    richTextBox1.Text = s;
                }

                path = @"c:\Temp\" + strMonthDay[0] + strMonthDay[1] + strMonthDay[2];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // transder files over
                ef = new StringBuilder("pscp");
                gh = new StringBuilder(" -pw " + pr202PassWordD + " tuxedo@148.92.137.11:" + "/home/tuxedo/???.txt" + " " + path);
                psi = new ProcessStartInfo();
                psi.FileName = ef.ToString();
                psi.Arguments = gh.ToString();
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                proc = Process.Start(psi);
                s = proc.StandardOutput.ReadToEnd();
                richTextBox1.AppendText(s);
                Application.DoEvents();
                button4.Enabled = true;

                // delete the Temp files created in pr202
                ab = new StringBuilder("plink");
                cd = new StringBuilder(" -pw " + pr202PassWordD + " tuxedo@148.92.137.11 -m c:\\Temp\\pr202_del.txt");
                psi = new ProcessStartInfo();
                psi.FileName = ab.ToString();
                psi.Arguments = cd.ToString();

                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                proc = Process.Start(psi);
                s = proc.StandardOutput.ReadToEnd();
                richTextBox1.AppendText(s);

                Application.DoEvents();
            }

            button1.Text = "Click Here to Generate Tuxedo Report";
            button1.Enabled = true;
        }
        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            int iCompare = -1;

            times = dateTimePicker4.Text.Split(' ');
            string[] EndTime = times[0].Split(':');

            strEndTime = EndTime[0] + EndTime[1] + EndTime[2];

            iCompare = string.Compare(strStartTime, strEndTime);

            if (iCompare > 0)
            {
                button1.Enabled = false;
                button1.Text = "Enter Valid Date, Start Time and End Time and Click";
                MessageBox.Show("Start time is greater than end time", "Error in Time Entry");
            }
            else
            {
                button1.Enabled = true;
                button1.Text = "Click Here to Generate Tuxedo Report";
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            int iCompare = -1;

            string[] times = dateTimePicker2.Text.Split(' ');
            string[] StartTime = times[0].Split(':');

            strStartTime = StartTime[0] + StartTime[1] + StartTime[2];

            iCompare = string.Compare(strStartTime, strEndTime);

            if (iCompare > 0)
            {
                button1.Enabled = false;
                button1.Text = "Enter Valid Date, Start Time and End Time and Click";
                MessageBox.Show("Start time is greater than end time", "Error in Time Entry");
            }
            else
            {
                button1.Enabled = true;
                button1.Text = "Click Here to Generate Tuxedo Report";
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int iCompare = -1;
            iCompare = string.Compare(strStartTime, strEndTime);

            if (iCompare > 0)
            {
                MessageBox.Show("Start time is greater that end time", "Error in Time Entry");
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            // set the MaxDatedate today by default
            dateTimePicker1.MaxDate = DateTime.Now;
            // set Mindate to MaxDate-90 Days
            dateTimePicker1.MinDate = DateTime.Now.AddDays(-90);

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
            dateTimePicker1.Value = dateTimePicker1.MaxDate;

            checkedListBox2.Enabled = false;
            checkedListBox3.Enabled = false;
        }
    }
}
