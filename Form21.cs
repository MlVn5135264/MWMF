using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;
using System.Threading;

namespace MWMF
{
    public partial class Form21 : Form
    {
        public System.Windows.Forms.Timer MyTimer1;
        public long dLength1 = 0, dLength2 = 0;
        public DateTime now1, now2;
        public string cMailTo = "narayananm@gainwelltechnologies.com;9162369772@txt.att.net";
        public char cEmailSent = 'N', cFirstIteration = 'Y';
        public int iCounter = 0;
        public int iTimer;
        public DirectoryInfo di;
        public FileInfo[] FilesInfo;
        public Form21()
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

            MyTimer1 = new System.Windows.Forms.Timer();

            if ((iTimer == 30) || (iTimer == 45))
            {
                MyTimer1.Interval = (iTimer * 1000);
            }
            else
            {
                MyTimer1.Interval = (iTimer * 60 * 1000);
            }
            MyTimer1.Tick += new EventHandler(MyTimer_Tick);
            MyTimer1.Start();
        }

        private void Form21_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Form Closing");

            MyTimer1.Stop();
            MyTimer1.Dispose();

            button3.Enabled = false;
            comboBox1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MyTimer1.Stop();
            MyTimer1.Dispose();

            // Disable relevant buttons
            comboBox1.Enabled = true;

            button3.Enabled = false;
        }

        private void Form21_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            MWMF.Form1.Sem.WaitAsync();

            comboBox1.Enabled = false;
            button3.Enabled = true;

            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // if the time is not between 6:00 AM and 10:00 PM don't process
            TimeSpan time = DateTime.Now.TimeOfDay;
            if (!((time > new TimeSpan(05, 59, 59)) && (time <= new TimeSpan(21, 59, 59))))
            {
                return;
            }

            try
            {
                // di = new DirectoryInfo(driveLetters[0] + @":\extractarea\consumer1\PAGE");
                di = new DirectoryInfo( @"D:\extractarea\consumer1\PAGE");
                FilesInfo = di.GetFiles();

                // if no file exists prompt that Page Extracts are not being created and leave
                if (FilesInfo.Length == 0)
                {
                    MessageBox.Show("Page Extracts are Missing.....");

                    return;
                }

                if (cFirstIteration == 'Y')
                {
                    foreach (FileInfo files in FilesInfo)
                    {
                        dLength1 = dLength1 + files.Length;
                    }

                    textBox1.Text = "Not in Scope Yet.....";
                    textBox2.Text = dLength1.ToString();
                    Application.DoEvents();
                }
                else
                {
                    // populate the file size for the prior run to textbox1
                    textBox1.Text = textBox2.Text;
                    textBox2.Text = "";
                    Application.DoEvents();

                    foreach (FileInfo files in FilesInfo)
                    {
                        dLength2 = dLength2 + files.Length;
                    }

                    textBox2.Text = dLength2.ToString();

                    Application.DoEvents();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }


            if (cFirstIteration == 'N')
            {
                if (dLength1 == dLength2)
                {
                    textBox3.Text = "Health Check Failed";
                    Application.DoEvents();
                }
                
                if(dLength2 > dLength1)
                {
                    textBox3.Text = "Health Check Passed";
                    Application.DoEvents();
                }

                if(dLength2 < dLength1)
                {
                   SendMail_AboutPageExtracts("Files Moved from D: to E:");
                    Application.DoEvents();
                }                
            }
            else
            {
                cFirstIteration = 'N';
            }

            iCounter++;

            now2 = DateTime.Now;
            textBox5.Text = now2.ToString();
            textBox4.Text = iCounter.ToString();
            dLength1 = dLength2;
            dLength2 = 0;

            Application.DoEvents();

            MWMF.Form1.Sem.Release();
        }

        private void SendMail_AboutPageExtracts(string strBody)
        {
            if (cEmailSent == 'N')
            {
                try
                {
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = cMailTo.ToString();
                    //mail.To = "narayananm@dxc.com;9162369772@tmomail.net;richard.chiu@dxc.com;9169470604@mms.att.net;vrajamreddygari@dxc.com;9165474216@txt.att.net";
                    // mail.To = "narayananm@dxc.com;9162369772@tmomail.net";
                    mail.Subject = strBody;
                    /*
                        mail.Body = "Action: Regular hours (8:00 AM till 6:15 PM):\n\n" +
                    */
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cEmailSent = 'N';
                }
            }

        }
    }
}
