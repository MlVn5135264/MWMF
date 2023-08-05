using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Mail;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace MWMF
{
    public partial class Form26 : Form
    {
        public StringBuilder cMailTo = new StringBuilder("", 5000);
        public string strMailTo = "Dave.Ahnell@gainwelltechnologies.com;vikas.mohindra@gainwelltechnologies.com;sharad.buisreddy@gainwelltechnologies.com;narayananm@gainwelltechnologies.com;9162369772@txt.att.net";
        // public string strMailTo = "narayananm@gainwelltechnologies.com;9162369772@txt.att.net";

        public Form26()
        {
            InitializeComponent();
        }

        private void Form26_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.button1, "Sends Email Notification a day prior");
            // toolTip1.SetToolTip(this.label2, "Check the file Schedule for Correspondence_in_Print_Queue list.csv");
            // toolTip1.SetToolTip(this.label3, "Check the file Schedule for non-Correspondence lists.csv");

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            dateTimePicker1.MaxDate = DateTime.Now;
            dateTimePicker1.Text = DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Checking.....";
            Application.DoEvents();

            using (StreamReader file = new StreamReader(@"C:\Data_Cleansing_Schedule\Schedule for non-Correspondence lists.txt"))
            {
                string ln;

                // ignore the first 3 lines
                ln = file.ReadLine();
                ln = file.ReadLine();
                ln = file.ReadLine();

                while ((ln = file.ReadLine()) != null)
                {
                    string[] ln_split = ln.Split('\t', '\"');

                    // compare the date contained in ln_split[3] as it contains the date
                    // when the Master Lists should be downloaded from Share Point.
                    // that morning inforning the various stake older about the download 
                    // from Share Point.

                    string date_today = DateTime.Now.ToString("yyyy/MM/dd");
                    int iCompare = String.Compare(ln_split[3], date_today);
                    if (iCompare > 0)
                    {
                        break;
                    }
                    else
                    {
                        if (iCompare == 0)
                        {
                            Send_Email_Text_Alert(@"Download Data Cleansing Master List files for non - Correspondence file refresh", @"Download Data Cleansing Master List files for non - Correspondence file refresh after COB today");
                            break;
                        }
                    }

                    iCompare = String.Compare(ln_split[5], date_today);
                    if (iCompare > 0)
                    {
                        break;
                    }
                    else
                    {
                        if (iCompare == 0)
                        {
                            Send_Email_Text_Alert(@"Upload Data Cleansing Master List and all non-Correspondence data list files", @"Upload Data Cleansing Master List and all non-Correspondence data list files after COB today");
                            break;
                        }
                    }

                }
                file.Close();
            }

            Application.DoEvents();

            using (StreamReader file = new StreamReader(@"C:\Data_Cleansing_Schedule\Schedule for Correspondence_in_Print_Queue list.txt"))
            {
                string ln;

                // ignore the first 3 lines
                ln = file.ReadLine();
                ln = file.ReadLine();
                ln = file.ReadLine();

                while ((ln = file.ReadLine()) != null)
                {
                    string[] ln_split = ln.Split('\t', '\"');

                    // compare the date contained in ln_split[3] as it contains the date
                    // when the Master Lists should be downloaded from Share Point.
                    // that morning inforning the various stake older about the download 
                    // from Share Point.

                    string date_today = DateTime.Now.ToString("yyyy/MM/dd");
                    int iCompare = String.Compare(ln_split[3], date_today);
                    if (iCompare > 0)
                    {
                        break;
                    }
                    else
                    {
                        if (iCompare == 0)
                        {
                            Send_Email_Text_Alert(@"Download Data Cleansing Master List files for Correspondence file refresh", @"Download Data Cleansing Master List files for Correspondence file refresh after COB today");
                            break;
                        }
                    }

                    iCompare = String.Compare(ln_split[5], date_today);
                    if (iCompare > 0)
                    {
                        break;
                    }
                    else
                    {
                        if (iCompare == 0)
                        {
                            Send_Email_Text_Alert(@"Upload Data Cleansing Master List and Correspondence data list files", @"Upload Data Cleansing Master List and Correspondence data list files after COB today");
                            break;
                        }
                    }
                }

                file.Close();
            }

            button1.Text = @"Click Here to Check Email Reminder for the Day";

            Application.DoEvents();
        }
        private void Send_Email_Text_Alert(string subject, string body)
        {

            try
            {
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = cMailTo.ToString();
                // mail.To = "narayananm@dxc
                mail.To = strMailTo;
                mail.Subject = subject;
                // Add image attachment from local disk
                mail.Body = body;
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
