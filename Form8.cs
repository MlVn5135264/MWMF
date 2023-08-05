using System;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Outlook = Microsoft.Office.Interop.Outlook;
using EncryptDecrypt;
using Ini;
using System.Web.UI.WebControls;

namespace MWMF
{
    public partial class Form8 : Form
    {
        public long green = (long)150, orange = (long) 175;
        public string strCountyName;
        public string ala = "n", frs = "n", sac = "n",sfo = "n", slo = "n", son = "n";
        public StringBuilder cMailTo = new StringBuilder("", 5000);
        public StringBuilder cMailToBackUp = new StringBuilder("", 5000);
        public int cs = 0;
        public string zoneId;
        public TimeZoneInfo zone;
        public char cDayLightSavingsTime = 'N';
        public int iPlus = 0;
        public ED ed = new ED();
        public IniFile inifile = new IniFile(@"c:\temp\config.ini");
        public string DataSourceE, DataSourceD, InitialCatalogE, InitialCatalogD, UserIdE, UserIdD, SqlServerPasswordE, SqlServerPasswordD, key;

        private void Vinay_Text_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
            }
        }

        private void Vinay_Email_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
            }
        }

        private void Poorna_Email_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
            }
        }

        private void Venkat_Email_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
            }
        }

        private void Mohan_Text_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
            }
        }

        private void Mohan_Email_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
            }
        }

        private void Richard_Text_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
                
            }
        }

        private void Richard_Email_CheckedChanged(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            { 
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Enabled == true)
            {
                cComposeMailToString();
                button2.Enabled = false;
            }
        }

        public string setColorChange = "n";
        public StringBuilder strSqlCommand = new StringBuilder();
        public int questatsbadcount = 0;
        public System.Windows.Forms.Timer MyTimer_ColorChange = new System.Windows.Forms.Timer();

        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;
            cComposeMailToString();

            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";

            DataSourceE = inifile.IniReadValue("Contact CalWIN", "Data Source");
            DataSourceD = ed.DecryptString(key, DataSourceE);

            InitialCatalogE = inifile.IniReadValue("Contact CalWIN", "Initial Catalog");
            InitialCatalogD = ed.DecryptString(key, InitialCatalogE);

            UserIdE = inifile.IniReadValue("Contact CalWIN", "User ID");
            UserIdD = ed.DecryptString(key, UserIdE);

            SqlServerPasswordE = inifile.IniReadValue("Contact CalWIN", "Password");
            SqlServerPasswordD = ed.DecryptString(key, SqlServerPasswordE);

            // connetionString = @"Data Source=148.92.136.117;Initial Catalog=ContactCalWINp;User ID=ContactCalWIN;Password=ContactCW2013!";
            connetionString = "Data Source=" + DataSourceD + ";" + "Initial Catalog=" + InitialCatalogD + ";" + "User ID=" + UserIdD + ";" + "Password=" + SqlServerPasswordD;


            cnn = new SqlConnection(connetionString);

            cnn.Open();

            SqlCommand command;
            String sql;

            sql = "SELECT dbo.County_Code.County_Name " +
                  "FROM [ConnectCalWIN].[dbo].County_Code, [ConnectCalWIN].[dbo].[County_Details] " +
                  "WHERE [ConnectCalWIN].[dbo].County_Code.County_Cd = [ConnectCalWIN].[dbo].County_Details.County_Cd AND " +
                  "[ConnectCalWIN].[dbo].County_Details.Calwin_County = '1' AND " +
                  "[ConnectCalWIN].[dbo].County_Code.County_Name != 'EXECUTIVE SUMMARY ALERTS' ";

            command = new SqlCommand(sql, cnn);

            try
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource
                {
                    DataSource = dbdataset
                };
                dataGridView1.DataSource = bSource;
                sda.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            command.Dispose();
            cnn.Close();
            cnn.Dispose();

            // uncomment the the 4 lines below witout fail. Mohan
            System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
            MyTimer.Interval = (20 * 1000); // 20 Secs
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();     
            // MyTimer.Dispose();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            key = "djkdlmvnkw23l%ihl#hjvenk+4mkdvhw";

            DataSourceE = inifile.IniReadValue("Contact CalWIN", "Data Source");
            DataSourceD = ed.DecryptString(key, DataSourceE);

            InitialCatalogE = inifile.IniReadValue("Contact CalWIN", "Initial Catalog");
            InitialCatalogD = ed.DecryptString(key, InitialCatalogE);

            UserIdE = inifile.IniReadValue("Contact CalWIN", "User ID");
            UserIdD = ed.DecryptString(key, UserIdE);

            SqlServerPasswordE = inifile.IniReadValue("Contact CalWIN", "Password");
            SqlServerPasswordD = ed.DecryptString(key, SqlServerPasswordE);

            try
            {
                MWMF.Form1.Sem.WaitAsync();
            }
            catch (Exception E)
            {
                MessageBox.Show("Queue Stats Monitor: " + E.Message.ToString());
            }

            textBox22.Text = (++iPlus).ToString();

            checkBox1.Checked = false;
            checkBox3.Checked = false;
            checkBox6.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;

            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                strCountyName = dataGridView1.Rows[i].Cells[0].Value.ToString();
                strSqlCommand = new StringBuilder("SELECT Top 1 ID, County_Of_Res, CONVERT(CHAR(10), DATEADD(HOUR,-7,Queue_Check_DtTm), 101) as Queue_Check_Dt, CONVERT(CHAR(12), DATEADD(HOUR,-7,Queue_Check_DtTm), 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueID FROM dbo.County_Code, dbo.Queue_Stats", 5000);
                strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Queue_Stats.County_Of_Res ");
                strSqlCommand.Append(@"AND CONVERT(CHAR(10), Queue_Check_DtTm, 101) = CONVERT(CHAR(10), GETDATE(), 101) ");
                strSqlCommand.Append("ORDER BY dbo.Queue_Stats.ID DESC");

                string connetionString;
                SqlConnection cnn;
                // connetionString = @"Data Source=148.92.136.117;Initial Catalog=ContactCalWINp;User ID=ContactCalWIN;Password=ContactCW2013!";
                connetionString = "Data Source=" + DataSourceD + ";" + "Initial Catalog=" + InitialCatalogD + ";" + "User ID=" + UserIdD + ";" + "Password=" + SqlServerPasswordD;

                cnn = new SqlConnection(connetionString);

                cnn.Open();

                SqlCommand command;
                String sql;

                sql = strSqlCommand.ToString();

                command = new SqlCommand(sql, cnn);

                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = command;
                    DataTable dbdataset = new DataTable();
                    sda.Fill(dbdataset);
                    BindingSource bSource = new BindingSource();

                    bSource.DataSource = dbdataset;
                    dataGridView2.DataSource = bSource;
                    sda.Update(dbdataset);
                    sda.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                command.Dispose();

                cnn.Close();
                cnn.Dispose();

                // if no rows are retured go back to the for loop
                if (dataGridView2.Rows.Count == 0)
                {
                    continue;
                }

                // get the username
                string strUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                // MessageBox.Show("Username: " + strUserName);

                /*******************************************************************/

                // var zone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                // var utcNow = DateTime.UtcNow;
                // var pacificNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, zone);
                var datetime = DateTime.Parse(dataGridView2.Rows[0].Cells[3].Value.ToString());

                if (dataGridView2.Rows.Count == 1)
                {
                    var paczone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    var gmtutcNow = DateTime.UtcNow;
                    var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(gmtutcNow, paczone);
                    // var currentDateTime = DateTime.UtcNow.AddHours(dAddHours);
                    var DateTimeDiff = currentDateTime - datetime;
                    cs = (int)DateTimeDiff.TotalSeconds;
                    dataGridView2.DataSource = null;
                    textBox20.Text = currentDateTime.ToString();

                    // Added this for the time difference in the user laptop and the prodcution servers
                    if (cs < 0)
                    {
                        cs = 0;
                    }
                }
                else
                {
                    string dttemp = DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime dt = new DateTime(Int32.Parse(dttemp.Substring(0, 4)), Int32.Parse(dttemp.Substring(5, 2)), Int32.Parse(dttemp.Substring(8, 2)), 0, 0, 0);
                    var paczone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    var gmtutcNow = DateTime.UtcNow;
                    var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(gmtutcNow, paczone);
                    var DateTimeDiff = currentDateTime - dt;
                    cs = (int)DateTimeDiff.TotalSeconds;

                    // Added this for the time difference in the user laptop and the prodcution servers
                    if (cs < 0)
                    {
                        cs = 0;
                    }
                }

                if (i == 0) // ala - Good
                {
                    textBox1.Text = cs.ToString();

                    checkBox1.Checked = true;

                    if ((cs >= 0) && (cs <= green))
                    {
                        label40.BackColor = System.Drawing.Color.DarkGreen;
                        if (ala.Contains("y"))
                        {
                            SendMail_Good_Queue_Stats();
                            ala = "n";
                            questatsbadcount--;
                        }
                    }
                    if ((cs > green) && (cs <= orange))
                    {
                        label40.BackColor = System.Drawing.Color.Orange;
                    }
                    if (cs > orange)
                    {
                        label40.BackColor = System.Drawing.Color.DarkRed;
                        if (ala.Contains("n"))
                        {
                            SendMail_NoQueue_Stats();
                            ala = "y";
                            questatsbadcount++;
                        }
                    }
                }

                if (i == 1) // frs - Good
                {
                    textBox3.Text = cs.ToString();
                    checkBox3.Checked = true;
                    if ((cs >= 0) && (cs <= green))
                    {
                        label42.BackColor = System.Drawing.Color.DarkGreen;
                        if (frs.Contains("y"))
                        {
                            SendMail_Good_Queue_Stats();
                            frs = "n";
                            questatsbadcount--;
                        }
                    }
                    if ((cs > green) && (cs <= orange))
                    {
                        label42.BackColor = System.Drawing.Color.Orange;
                    }
                    if (cs > orange)
                    {
                        label42.BackColor = System.Drawing.Color.DarkRed;
                        if (frs.Contains("n"))
                        {
                            SendMail_NoQueue_Stats();
                            frs = "y";
                            questatsbadcount++;
                        }
                    }
                }

                if (i == 2) // sac - Good
                {
                    textBox6.Text = cs.ToString();
                    checkBox6.Checked = true;
                    if ((cs >= 0) && (cs <= green))
                    {
                        label45.BackColor = System.Drawing.Color.DarkGreen;
                        if (sac.Contains("y"))
                        {
                            SendMail_Good_Queue_Stats();
                            sac = "n";
                            questatsbadcount--;
                        }
                    }
                    if ((cs > green) && (cs <= orange))
                    {
                        label45.BackColor = System.Drawing.Color.Orange;
                    }
                    if (cs > orange)
                    {
                        label45.BackColor = System.Drawing.Color.DarkRed;
                        if (sac.Contains("n"))
                        {
                            SendMail_NoQueue_Stats();
                            sac = "y";
                            questatsbadcount++;
                        }
                    }
                }

                if (i == 3) // sfo - Good
                {
                    checkBox8.Checked = true;

                    textBox8.Text = cs.ToString();
                    checkBox8.Checked = true;
                    if ((cs >= 0) && (cs <= green))
                    {
                        label47.BackColor = System.Drawing.Color.DarkGreen;
                        if (sfo.Contains("y"))
                        {
                            SendMail_Good_Queue_Stats();
                            sfo = "n";
                            questatsbadcount--;
                        }
                    }
                    if ((cs > green) && (cs <= orange))
                    {
                        label47.BackColor = System.Drawing.Color.Orange;
                    }
                    if (cs > orange)
                    {
                        label47.BackColor = System.Drawing.Color.DarkRed;
                        if (sfo.Contains("n"))
                        {
                            SendMail_NoQueue_Stats();
                            sfo = "y";
                            questatsbadcount++;
                        }
                    }

                }

                if (i == 4) // slo - Good
                {
                    textBox9.Text = cs.ToString();
                    checkBox9.Checked = true;

                    if ((cs >= 0) && (cs <= green))
                    {
                        label48.BackColor = System.Drawing.Color.DarkGreen;
                        if (slo.Contains("y"))
                        {
                            SendMail_Good_Queue_Stats();
                            slo = "n";
                            questatsbadcount--;
                        }
                    }
                    if ((cs > green) && (cs <= orange))
                    {
                        label48.BackColor = System.Drawing.Color.Orange;
                    }
                    if (cs > orange)
                    {
                        label48.BackColor = System.Drawing.Color.DarkRed;
                        if (slo.Contains("n"))
                        {
                            SendMail_NoQueue_Stats();
                            slo = "y";
                            questatsbadcount++;
                        }
                    }
                }

                if (i == 5) // son
                {
                    textBox15.Text = cs.ToString();
                    checkBox15.Checked = true;
                    if ((cs >= 0) && (cs <= green))
                    {
                        label54.BackColor = System.Drawing.Color.DarkGreen;
                        if (son.Contains("y"))
                        {
                            SendMail_Good_Queue_Stats();
                            son = "n";
                            questatsbadcount--;
                        }
                    }
                    if ((cs > green) && (cs <= orange))
                    {
                        label54.BackColor = System.Drawing.Color.Orange;
                    }
                    if (cs > orange)
                    {
                        label54.BackColor = System.Drawing.Color.DarkRed;
                        if (son.Contains("n"))
                        {
                            SendMail_NoQueue_Stats();
                            son = "y";
                            questatsbadcount++;
                        }
                    }
                }

                if (((ala == "y") || (frs == "y") || (sac == "y") || (sfo == "y") || (slo == "y") || (son == "y")) && ((setColorChange == "n") && (questatsbadcount == 1)))
                {
                    MyTimer_ColorChange.Interval = (100); // 10 Secs
                    MyTimer_ColorChange.Tick += new EventHandler(MyTimer_ColorChange_Tick);
                    MyTimer_ColorChange.Start();
                    MyTimer_ColorChange.Enabled = true;
                    setColorChange = "y";
                }

                if ((ala == "n") && (frs == "n") && (sac == "n") && (sfo == "n") && (slo == "n") && (son == "n") && (setColorChange == "y") && (questatsbadcount == 0))
                {
                    MyTimer_ColorChange.Enabled = false;
                    MyTimer_ColorChange.Stop();
                    setColorChange = "n";
                }
           }

            long memory = GC.GetTotalMemory(true);
            textBox21.Text = memory.ToString();
            
            /*
                 * Process currentProc = Process.GetCurrentProcess();
                 * memory = currentProc.PrivateMemorySize64;
                 * MessageBox.Show(memory.ToString());
            */

            MWMF.Form1.Sem.Release(1);
        }

        private void MyTimer_ColorChange_Tick(object sender, EventArgs e)
        {
            Random labelcolor = new Random();
            int R = labelcolor.Next(0, 255);
            int G = labelcolor.Next(0, 255);
            int B = labelcolor.Next(0, 255);
            int A = labelcolor.Next(0, 255);

            if (ala == "y")
            {
                label40.BackColor = System.Drawing.Color.FromArgb(R, G, B, A);
            }

            if (frs == "y")
            {
                label42.BackColor = System.Drawing.Color.FromArgb(R, G, B, A);
            }

            if ( sac == "y")
            {
                label45.BackColor = System.Drawing.Color.FromArgb(R, G, B, A);
            }

            if ( sfo == "y")
            {
                label47.BackColor = System.Drawing.Color.FromArgb(R, G, B, A);
            }

            if ( slo == "y")
            {
                label48.BackColor = System.Drawing.Color.FromArgb(R, G, B, A);
            }

            if (son == "y")
            {
                label54.BackColor = System.Drawing.Color.FromArgb(R, G, B, A);
            }
        }

        private void SendMail_Good_Queue_Stats()
        {
            try
            {
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = cMailTo.ToString();
                // mail.To = "narayananm@gainwelltechnologies.com;9162369772@tmomail.net;richard.chiu@gainwelltechnologies.com;9169470604@mms.att.net;vrajamreddygari@gainwelltechnologies.com;9165474216@txt.att.net";
                // mail.To = "narayananm@gainwelltechnologies.com;9162369772@tmomail.net";
                mail.Subject = "Queue Stats Good for County: " + strCountyName;
                mail.Body = "Valid Queue Stats found for the County: " + strCountyName;
                mail.Importance = Outlook.OlImportance.olImportanceNormal;
                ((Outlook._MailItem)mail).Send();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           // cMailTo.Clear();
        }

        private void SendMail_NoQueue_Stats()
        {
            try
            {
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = cMailTo.ToString();
                //mail.To = "narayananm@gainwelltechnologies.com;9162369772@tmomail.net;richard.chiu@gainwelltechnologies.com;9169470604@mms.att.net;vrajamreddygari@gainwelltechnologies.com;9165474216@txt.att.net";
                // mail.To = "narayananm@gainwelltechnologies.com;9162369772@tmomail.net";
                mail.Subject = "No Queue Stats for County: " + strCountyName;
                mail.Body = "Action: Regular hours (8:00 AM till 6:15 PM):\n\n" +
                "1. Create SR\n" +
                "2. Start Incident Call\n" +
                "3. Engage Aaron Lisker (510-923-9205) of Streamwrite. Send Aaron Lisker the incident call Teams link.\n" +
                "4. Otherwise: Send an eamil to techsupport@streamwrite.com explaining issue. Include incident call Teams link\n\n" +
                "Action to be taken weekend & non regular hours:\n\n" +
                "1. Create SR\n" +
                "2. Start Incident Call\n" +
                "3. Engage Streamwrite. Call off-hour Streamwrite Support line: (800)333-8394 (Option 4)\n" +
                "4. Issue Priority: URGENT\n" +
                "5. When technician calls you, explain issue and email incident call Teams link\n\n";
                mail.Importance = Outlook.OlImportance.olImportanceNormal;
                ((Outlook._MailItem)mail).Send();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cComposeMailToString()
        {
            cMailTo.Clear();

            cMailTo.Append("narayananm@gainwelltechnologies.com;");

            if (Mohan_Email.Checked == true)
            {
                // cMailTo.Append("narayananm@gainwelltechnologies.com;");
            }

            if (Mohan_Text.Checked == true)
            {
                // cMailTo.Append("9162369772@txt.att.net;");
            }            
            
            if (Satish_Email.Checked == true)
            {
                // cMailTo.Append("s.manoharan@gainwelltechnologies.com" + ";");
            }
            if (Satish_Text.Checked == true)
            {
                // cMailTo.Append(";");
            }

            if (Richard_Email.Checked == true)
            {
                // cMailTo.Append("richard.chiu@gainwelltechnologies.com;");
            }
            if (Richard_Text.Checked == true)
            {
                // cMailTo.Append("9169470604@mms.att.net;");
            }

            if(Vinay_Email.Checked == true)
            {
                // cMailTo.Append("vrajamreddy2@gainwelltechnologies.com;");              
            }

            if (Vinay_Text.Checked == true)
            {
                cMailTo.Append("9165474216@vtext.com;");
            }

            if (Sakthi_Email.Checked == true)
            {
                // cMailTo.Append("sakthi-vadivel.sagadevan@gainwelltechnologies.com;");
            }

            if (Sakthi_Text.Checked == true)
            {
                // cMailTo.Append("2814608683@txt.att.net;");
            }

            if(Gopi_Email.Checked == true)
            {
                // cMailTo.Append("velayutham@gainwelltechnologies.com;");
            }

            if (Poorna_Email.Checked == true)
            {
                // cMailTo.Append("poornaviswanathan.manickam@gainwelltechnologies.com;");
            }

            if (Pradeep_Email.Checked == true)
            {
                // cMailTo.Append("pradeep.allaboyina@gainwelltechnologies.com;");
            }

            if (Pradeep_Text.Checked == true)
            {
                // cMailTo.Append("pradeep.allaboyina@gainwelltechnologies.com;");
            }

            if (Gopi_Email.Checked == true)
            {
                // cMailTo.Append("gsundharam@gainwelltechnologies.com;");
            }

            if (Gopi_Text.Checked == true)
            {
                // cMailTo.Append("s.manoharan@gainwelltechnologies.com;");
            }
        }
    }
}
