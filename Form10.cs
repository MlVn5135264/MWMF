using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MWMF
{
    public partial class Form10 : Form
    {
        public string strCountyName;
        public StringBuilder strSqlCommand = new StringBuilder("ID, County_Of_Res, CONVERT(CHAR(10), DATEADD(HOUR, -8, Queue_Check_DtTm), 101) as Queue_Check_Dt, CONVERT(CHAR(12), DATEADD(HOUR, -8, Queue_Check_DtTm), 108) as Queue_Check_Tm) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueID FROM dbo.County_Code, dbo.Queue_Stats", 5000);
        public string zoneId;
        public TimeZoneInfo zone;
        public char cPastDay = 'N';

        int iTotalDownTime = 0;
        // double dAddHours = 0;
        public int iCount = 0;
        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            dateTimePicker1.Value = dateTimePicker1.MaxDate = DateTime.Now;

            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=contact-calwin-db-us2ab-prod.cehxlb8w341l.us-west-2.rds.amazonaws.com;Initial Catalog=ConnectCalWIN;User ID=QueueStatsMonitoring;Password=sFNhx6qsW4XZKTuGgkVn";
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
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                // added by mohan
                Application.DoEvents();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    comboBox1.Items.Add(item: dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
                // dataGridView1.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
            cnn.Close();

            // Start Analyzing for Gaps
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {                             
            strCountyName = comboBox1.Text;
            //strSqlCommand = new StringBuilder("SELECT ID, County_Of_Res, CONVERT(CHAR(10), DATEADD(HOUR, -8, Queue_Check_DtTm), 101) as Queue_Check_Dt, CONVERT(CHAR(12), DATEADD(HOUR, -8, Queue_Check_DtTm), 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueId FROM dbo.County_Code, dbo.Queue_Stats", 5000);
            // strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Queue_Stats.County_Of_Res ");
            // strSqlCommand.Append(@"AND CONVERT(CHAR(10), Queue_Check_DtTm, 101) = " + "\'" + dateTimePicker1.Text + "\'");
            // strSqlCommand.Append(@"AND Queue_Check_DtTm between  " + "\'" + dateTimePicker1.Text + " 08:00:00.000000000\' AND " + "\'" + dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy") + " 06:59:59.999999999\'");
            // strSqlCommand.Append(" ORDER BY dbo.Queue_Stats.ID DESC");

            strCountyName = comboBox1.Text;
            strSqlCommand = new StringBuilder("SELECT ID, County_Of_Res, CONVERT(CHAR(10), DATEADD(HOUR, -7, Queue_Check_DtTm), 101) as Queue_Check_Dt, CONVERT(CHAR(12), DATEADD(HOUR, -7, Queue_Check_DtTm), 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueID FROM dbo.County_Code, dbo.Queue_Stats", 5000);
            strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Queue_Stats.County_Of_Res ");
            strSqlCommand.Append(@"AND Queue_Check_DtTm between  " + "\'" + dateTimePicker1.Text + " 07:00:00.000000000\' AND " + "\'" + dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy") + " 06:59:59.999999999\'");
            strSqlCommand.Append("ORDER BY dbo.Queue_Stats.ID DESC");

            button1.Enabled = true;

            button1.Enabled = true;
            dateTimePicker1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iCount = 0;
            iTotalDownTime = 0;
            button1.Text = "Querying The DB";
            button1.Enabled = false;
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=contact-calwin-db-us2ab-prod.cehxlb8w341l.us-west-2.rds.amazonaws.com;Initial Catalog=ConnectCalWIN;User ID=QueueStatsMonitoring;Password=sFNhx6qsW4XZKTuGgkVn";
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
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);

                // added by mohan
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
            cnn.Close();
             
            richTextBox1.Text = "";
            iTotalDownTime = 0;

            if(dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No Rows found");
                return;                
            }

            /*******************************************************************/

            /*******************************************************************/

            // if the date is not current, then check if there are any gaps at the beginning and at the end
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var utcNow = DateTime.UtcNow;            
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, zone);

            // if the date is not current and is in the past
            if (dataGridView1.Rows[0].Cells[2].Value.ToString() != currentDateTime.ToString("MM/dd/yyyy")) 
            {
                // get the row count from dataGridView1
                
                int iTotalRowCount = dataGridView1.RowCount;
                // MessageBox.Show("Past");
                cPastDay = 'Y';

                // To be added by Mohan.Add Condition for SLO county code != "40"
                if (dataGridView1.Rows[0].Cells[1].Value.ToString() != "40")
                {
                    // find out if there are is any gap at the start of the day 12:00 AM
                    vFindQueueStatsGapsAtTwelveAM();

                    // find queue stats gaps in the rest of the places
                    vFindQueueStatsGaps();

                    // find if there is an ongoing questats problem
                    if (cPastDay == 'N')
                    {
                        Process_ongoing_gaps();
                    }

                    string dttemp = dataGridView1.Rows[0].Cells[2].Value.ToString();

                    // DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime dt = currentDateTime.Date;

                    var datetime2 = DateTime.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString());
                    var DateTimeDiff = datetime2 - dt;


                    int cs = (int)DateTimeDiff.TotalSeconds;
                    int min = cs / 60;

                    if (cs > 175)
                    {
                        iTotalDownTime = iTotalDownTime + min;

                        // richTextBox1.AppendText(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  -  " + dt.ToString("HH:mm:ss") + ".000" + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                        richTextBox1.AppendText(dt.ToString("HH:mm:ss") + "  -  " + ".000" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                        iCount++;
                    }
                }
                else // this else is for SLO  and date is in the past
                {
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////
                    
                    // find out if there are is any gap at the start of the day 12:00 AM
                    vFindQueueStatsGapsAtTwelveAM();

                    // find queue stats gaps in the rest of the places
                    vFindQueueStatsGaps();

                    // find if there is an ongoing questats problem
                    if (cPastDay == 'N')
                    {
                        Process_ongoing_gaps();
                    }

                    string dttemp = dataGridView1.Rows[0].Cells[2].Value.ToString();

                    // DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime dt = currentDateTime.Date;

                    var datetime2 = DateTime.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString());
                    var DateTimeDiff = datetime2 - dt;

                    int cs = (int)DateTimeDiff.TotalSeconds;
                    int min = cs / 60;
                    
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////
                    
                    if (cs > 300)
                    {
                        iTotalDownTime = iTotalDownTime + min;

                        // richTextBox1.AppendText(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  -  " + dt.ToString("HH:mm:ss") + ".000" + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                        richTextBox1.AppendText(dt.ToString("HH:mm:ss") + "  -  " + ".000" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                        iCount++;
                    }
                } // this else is for SLO and date is in the past
            }
            else // if the date is current and is NOT in the past
            {
                if(dataGridView1.Rows[0].Cells[1].Value.ToString() != "40")
                {
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////

                    // find out if there are is any gap at the start of the day 12:00 AM
                    vFindQueueStatsGapsAtTwelveAM();

                    // find queue stats gaps in the rest of the places
                    vFindQueueStatsGaps();

                    // find if there is an ongoing questats problem
                    if (cPastDay == 'N')
                    {
                        Process_ongoing_gaps();
                    }

                    //string dttemp = dataGridView1.Rows[0].Cells[2].Value.ToString();

                    //// DateTime.Now.ToString("yyyy-MM-dd");
                    //DateTime dt = currentDateTime.Date;

                    //var datetime2 = DateTime.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString());
                    //var DateTimeDiff = datetime2 - dt;
                    //int cs = (int)DateTimeDiff.TotalSeconds;
                    //int min = cs / 60;

                    //if (cs > 175)
                    //{
                    //    iTotalDownTime = iTotalDownTime + min;

                    //    string[] dateSplit1 = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString().Split('/');
                    //    var date1 = new DateTime(int.Parse(dateSplit1[2]), int.Parse(dateSplit1[0]), int.Parse(dateSplit1[1]));

                    //    // richTextBox1.AppendText(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  -  " + dt.ToString("HH:mm:ss") + ".000" + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                    //    richTextBox1.AppendText(dt.ToString("HH:mm:ss") + "  -  " + ".000" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                    //    iCount++;
                    //}


                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        // Add condition or SLO i != 4
                        if ((i != 4) && (dataGridView1.Rows[0].Cells[1].Value.ToString() != "40"))
                        {
                            var datetime2 = DateTime.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                            var datetime1 = DateTime.Parse(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                            var DateTimeDiff = datetime2 - datetime1;
                            int cs = (int)DateTimeDiff.TotalSeconds;
                            int min = cs / 60;

                            if (cs > 175)
                            {
                                iTotalDownTime = iTotalDownTime + min;
                                // MessageBox.Show(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                                // richTextBox1.AppendText(dataGridView1.Rows[i].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: "+ min.ToString() + " " + "minutes" + "\n");
                                richTextBox1.AppendText(dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                                iCount++;
                            }
                        }
                        else
                        {
                            var datetime2 = DateTime.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                            var datetime1 = DateTime.Parse(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                            var DateTimeDiff = datetime2 - datetime1;
                            int cs = (int)DateTimeDiff.TotalSeconds;
                            int min = cs / 60;

                            if (cs > 300)
                            {
                                iTotalDownTime = iTotalDownTime + min;
                                // MessageBox.Show(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                                // richTextBox1.AppendText(dataGridView1.Rows[i].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: "+ min.ToString() + " " + "minutes" + "\n");
                                richTextBox1.AppendText(dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                                iCount++;
                            }
                        }
                    }

                }
                else
                {
                    /* cPastDay = 'N';
                    string dttemp = dataGridView1.Rows[0].Cells[2].Value.ToString();

                    // DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime dt = currentDateTime.Date;

                    var datetime2 = DateTime.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString());
                    var DateTimeDiff = datetime2 - dt;
                    int cs = (int)DateTimeDiff.TotalSeconds;
                    int min = cs / 60;

                    if (cs > 300)
                    {
                        iTotalDownTime = iTotalDownTime + min;

                        // richTextBox1.AppendText(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  -  " + dt.ToString("HH:mm:ss") + ".000" + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                        richTextBox1.AppendText(dt.ToString("HH:mm:ss") + "  -  " + ".000" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                        iCount++;
                    } */

                    //////////////////////////////////////////////////////////////////////////////////////////////

                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        // Add condition or SLO i != 4
                        if ((i != 4) && (dataGridView1.Rows[0].Cells[1].Value.ToString() != "40"))
                        {
                            var datetime2 = DateTime.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                            var datetime1 = DateTime.Parse(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                            var DateTimeDiff = datetime2 - datetime1;
                            int cs = (int)DateTimeDiff.TotalSeconds;
                            int min = cs / 60;

                            if (cs > 175)
                            {
                                iTotalDownTime = iTotalDownTime + min;
                                // MessageBox.Show(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                                // richTextBox1.AppendText(dataGridView1.Rows[i].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: "+ min.ToString() + " " + "minutes" + "\n");
                                richTextBox1.AppendText(dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                                iCount++;
                            }
                        }
                        else
                        {
                            var datetime2 = DateTime.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                            var datetime1 = DateTime.Parse(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                            var DateTimeDiff = datetime2 - datetime1;
                            int cs = (int)DateTimeDiff.TotalSeconds;
                            int min = cs / 60;

                            if (cs > 300)
                            {
                                iTotalDownTime = iTotalDownTime + min;
                                // MessageBox.Show(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                                // richTextBox1.AppendText(dataGridView1.Rows[i].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: "+ min.ToString() + " " + "minutes" + "\n");
                                richTextBox1.AppendText(dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                                iCount++;
                            }
                        }
                    }


                    //////////////////////////////////////////////////////////////////////////////////////////////

                }
            } // if the date is current and is NOT in the past

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                // Add condition or SLO i != 4
                if((i != 4) && (dataGridView1.Rows[0].Cells[1].Value.ToString() != "40"))
                {
                    var datetime2 = DateTime.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    var datetime1 = DateTime.Parse(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                    var DateTimeDiff = datetime2 - datetime1;
                    int cs = (int)DateTimeDiff.TotalSeconds;
                    int min = cs / 60;

                    if (cs > 175)
                    {
                        iTotalDownTime = iTotalDownTime + min;
                        // MessageBox.Show(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                        // richTextBox1.AppendText(dataGridView1.Rows[i].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: "+ min.ToString() + " " + "minutes" + "\n");
                        richTextBox1.AppendText(dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                        iCount++;
                    }
                }
                else
                {
                    var datetime2 = DateTime.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    var datetime1 = DateTime.Parse(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                    var DateTimeDiff = datetime2 - datetime1;
                    int cs = (int)DateTimeDiff.TotalSeconds;
                    int min = cs / 60;

                    if (cs > 300)
                    {
                        iTotalDownTime = iTotalDownTime + min;
                        // MessageBox.Show(dataGridView1.Rows[i + 1].Cells[3].Value.ToString());
                        // richTextBox1.AppendText(dataGridView1.Rows[i].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: "+ min.ToString() + " " + "minutes" + "\n");
                        richTextBox1.AppendText(dataGridView1.Rows[i + 1].Cells[3].Value.ToString() + "  -  " + dataGridView1.Rows[i].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                        iCount++;
                    }
                }
            }
            button1.Text = "Analyzed.....";

            // ********************************************************************** //
            
 
            // ********************************************************************** //


            richTextBox1.AppendText("\nTotal Queue Stats Down Time on " + dateTimePicker1.Text + " for " + strCountyName + " county: " + iTotalDownTime.ToString() + " minutes." + "\n");
            
            if(iCount == 0)
            {
                MessageBox.Show("No Gaps Found");
            }
            else
            {
                MessageBox.Show("Queue Stats Gap found in " + iCount.ToString() + " " + "Places");
            }

            dataGridView1.DataSource = null;
            button1.Text = "Gap Analyzer";
            button1.Enabled = false;
            dateTimePicker1.Enabled = false;
            comboBox1.Text = "*** Choose County ***";
            Application.DoEvents();

            for (int i = 0; i < 3; i++)
            {
                // Flash for County Drop Down
                comboBox1.Text = "";
                comboBox1.BackColor = System.Drawing.Color.Cyan;
                Application.DoEvents();
                System.Threading.Thread.Sleep(150);

                comboBox1.Text = "*** Choose County ***";
                comboBox1.BackColor = System.Drawing.Color.White;
                Application.DoEvents();
                System.Threading.Thread.Sleep(150);
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;

            for (int i = 0; i < 3; i++)
            {
                // Flash for County Drop Down
                comboBox1.Text = "";
                comboBox1.BackColor = System.Drawing.Color.Cyan;
                Application.DoEvents();
                System.Threading.Thread.Sleep(250);

                comboBox1.Text = "*** Choose County ***";
                comboBox1.BackColor = System.Drawing.Color.White;
                Application.DoEvents();
                System.Threading.Thread.Sleep(250);
            }
          
            strCountyName = comboBox1.Text;
            strSqlCommand = new StringBuilder("SELECT ID, County_Of_Res, CONVERT(CHAR(10), DATEADD(HOUR, -7, Queue_Check_DtTm), 101) as Queue_Check_Dt, CONVERT(CHAR(12), DATEADD(HOUR, -7, Queue_Check_DtTm), 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueID FROM dbo.County_Code, dbo.Queue_Stats", 5000);
            strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Queue_Stats.County_Of_Res ");
            strSqlCommand.Append(@"AND Queue_Check_DtTm between  " + "\'" + dateTimePicker1.Text + " 07:00:00.000000000\' AND " + "\'" + dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy") + " 06:59:59.999999999\'");
            strSqlCommand.Append("ORDER BY dbo.Queue_Stats.ID DESC");
            button1.Enabled = true;

            if (comboBox1.Text != "")
            {
                button1.Enabled = true;
            }
        }

        private void vFindQueueStatsGaps()
        {

            Application.DoEvents();
            int iTotalRowCount = dataGridView1.RowCount;

            // Check to see if there is a gap at the beeginning of the day (12:00 midnight)

            for(int iRowCount1 = iTotalRowCount - 2; iRowCount1 >= 0; iRowCount1--)
            {
                // just the date
                string datetime2 = dataGridView1.Rows[iRowCount1].Cells[2].Value.ToString();
                string datetime1 = dataGridView1.Rows[iRowCount1 + 1].Cells[2].Value.ToString();

                // just the time
                string datetime4 = dataGridView1.Rows[iRowCount1].Cells[3].Value.ToString();
                string datetime3 = dataGridView1.Rows[iRowCount1 + 1].Cells[3].Value.ToString();

                // compare the 2 dates
                if (datetime1 == datetime2)
                {
                    string[] date2Split = datetime2.Split('/');
                    string[] time4Split = datetime4.Split(':', ' ');
                    DateTime startDateTime = new DateTime(int.Parse(date2Split[2]), int.Parse(date2Split[0]), int.Parse(date2Split[1]), int.Parse(time4Split[0]), int.Parse(time4Split[1]), int.Parse(time4Split[2]));

                    string[] date1Split = (datetime1.Split('/'));
                    string[] time3Split = datetime3.Split(':', ' ');
                    DateTime endDateTime = new DateTime(int.Parse(date1Split[2]), int.Parse(date1Split[0]), int.Parse(date1Split[1]), int.Parse(time3Split[0]), int.Parse(time3Split[1]), int.Parse(time3Split[2]));

                    TimeSpan span = startDateTime.Subtract(endDateTime);

                    int Secondsdiff = span.Seconds;

                    int cs = Secondsdiff;
                    int min = cs / 60;

                    if (cs > 175)
                    {
                        iTotalDownTime = iTotalDownTime + min;

                        // richTextBox1.AppendText(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  -  " + dt.ToString("HH:mm:ss") + ".000" + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                        richTextBox1.AppendText(endDateTime.ToString("HH:mm:ss") + "  -  " + startDateTime.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

                        iCount++;
                    }

                }
                else
                {
                    continue;
                }

            }
            // New logic. Added by Mohan
        }

        private void vFindQueueStatsGapsAtTwelveAM()
        {
            Application.DoEvents();
            int iTotalRowCount = dataGridView1.RowCount;

            // just the date
            string datetime2 = dataGridView1.Rows[iTotalRowCount - 1].Cells[2].Value.ToString();

            // just the time
            string datetime4 = dataGridView1.Rows[iTotalRowCount - 1].Cells[3].Value.ToString();

            string[] date2Split = datetime2.Split('/');
            string[] time4Split = datetime4.Split(':', ' ');
            DateTime startDateTime = new DateTime(int.Parse(date2Split[2]), int.Parse(date2Split[0]), int.Parse(date2Split[1]), int.Parse(time4Split[0]), int.Parse(time4Split[1]), int.Parse(time4Split[2]));
            DateTime endDateTime = new DateTime(int.Parse(date2Split[2]), int.Parse(date2Split[0]), int.Parse(date2Split[1]), 0, 0, 0);

            // get the difference in seconds
            // TimeSpan span = startDateTime.Subtract(endDateTime);
            TimeSpan span = startDateTime - endDateTime;

            int Secondsdiff = (int)(span.TotalSeconds);

            // MessageBox.Show(Secondsdiff.ToString() + " Proceed !");

            int cs = Secondsdiff;
            int min = cs / 60;

            if (cs > 175)
            {
                iTotalDownTime = iTotalDownTime + min;

                // richTextBox1.AppendText(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  -  " + dt.ToString("HH:mm:ss") + ".000" + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                richTextBox1.AppendText(endDateTime.ToString("HH:mm:ss") + "  -  " + startDateTime.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                Application.DoEvents();

                iCount++;
            }
        }

        private void Process_ongoing_gaps()
        {
            Application.DoEvents();
            int iTotalRowCount = dataGridView1.RowCount;

            // get just the current date
            string datetime2 = DateTime.Now.ToString("MM/dd/yyyy");
            string[] date2Split = datetime2.Split('/');
            // get just the current time
            string datetime4 = DateTime.Now.ToString("HH:mm:ss");
            string[] time4Split = datetime4.Split(':', ' ');
            DateTime startDateTime = new DateTime(int.Parse(date2Split[2]), int.Parse(date2Split[0]), int.Parse(date2Split[1]), int.Parse(time4Split[0]), int.Parse(time4Split[1]), int.Parse(time4Split[2]));

            // get just the last reported date in the queue stats table
            datetime2 = dataGridView1.Rows[0].Cells[2].Value.ToString(); 
            date2Split = datetime2.Split('/');
            // get just the last reported time in the queue stats table
            datetime4 = dataGridView1.Rows[0].Cells[3].Value.ToString();
            time4Split = datetime4.Split(':', ' ');
            DateTime endDateTime = new DateTime(int.Parse(date2Split[2]), int.Parse(date2Split[0]), int.Parse(date2Split[1]), int.Parse(time4Split[0]), int.Parse(time4Split[1]), int.Parse(time4Split[2]));

            // get the difference in seconds
            // TimeSpan span = startDateTime.Subtract(endDateTime);
            TimeSpan span = startDateTime - endDateTime;

            int Secondsdiff = (int)(span.TotalSeconds);

            // MessageBox.Show(Secondsdiff.ToString() + " Proceed !");

            int cs = Secondsdiff;
            int min = cs / 60;

            if (cs > 175)
            {
                iTotalDownTime = iTotalDownTime + min;

                // richTextBox1.AppendText(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "  -  " + dt.ToString("HH:mm:ss") + ".000" + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                richTextBox1.AppendText(endDateTime.ToString("HH:mm:ss") + "  -  " + startDateTime.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
                Application.DoEvents();

                iCount++;
            }
        }
    }
}





// This was added for the end conditions.
/* if (cPastDay == 'Y')
{
    // To be added by Mohan.Add Condition for SLO county code != "40"
    if (dataGridView1.Rows[0].Cells[1].Value.ToString() != "40")
    {
        var datetime1 = DateTime.Parse(dataGridView1.Rows[0].Cells[3].Value.ToString());
        var datetime2 = datetime1.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        var DateTimeDiff = datetime2 - datetime1;
        int cs = (int)DateTimeDiff.TotalSeconds;
        int min = cs / 60;

        if (cs > 175)
        {
            iTotalDownTime = iTotalDownTime + min;

            // richTextBox1.AppendText(dataGridView1.Rows[0].Cells[3].Value.ToString() + "  -  " + datetime2.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
            richTextBox1.AppendText(datetime2.ToString() + "  -  " + dataGridView1.Rows[0].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

            iCount++;
        }
    }
    else
    {
        var datetime1 = DateTime.Parse(dataGridView1.Rows[0].Cells[3].Value.ToString());
        var datetime2 = datetime1.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        var DateTimeDiff = datetime2 - datetime1;
        int cs = (int)DateTimeDiff.TotalSeconds;
        int min = cs / 60;

        if (cs > 300)
        {
            iTotalDownTime = iTotalDownTime + min;

            // richTextBox1.AppendText(dataGridView1.Rows[0].Cells[3].Value.ToString() + "  -  " + datetime2.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");
            richTextBox1.AppendText(datetime2.ToString() + "  -  " + dataGridView1.Rows[0].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

            iCount++;
        }
    }
}
else 
{
    if(dataGridView1.Rows[0].Cells[1].Value.ToString() != "40")
    {
        var datetime1 = DateTime.Parse(dataGridView1.Rows[0].Cells[3].Value.ToString());
        // var datetime2 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
        var datetime2 = DateTime.UtcNow.AddHours(dAddHours);
        var DateTimeDiff = datetime2 - datetime1;
        int cs = (int)DateTimeDiff.TotalSeconds;
        int min = cs / 60;

        if (cs > 175)
        {
            iTotalDownTime = iTotalDownTime + min;

            richTextBox1.AppendText(datetime2.ToString() + "  -  " + dataGridView1.Rows[0].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

            iCount++;
        }
    }
    else
    {
        var datetime1 = DateTime.Parse(dataGridView1.Rows[0].Cells[3].Value.ToString());
        // var datetime2 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
        var datetime2 = DateTime.UtcNow.AddHours(dAddHours);
        var DateTimeDiff = datetime2 - datetime1;
        int cs = (int)DateTimeDiff.TotalSeconds;
        int min = cs / 60;

        if (cs > 300)
        {
            iTotalDownTime = iTotalDownTime + min;

            richTextBox1.AppendText(datetime2.ToString() + "  -  " + dataGridView1.Rows[0].Cells[3].Value.ToString() + "  ----->  " + "Queue Stats Down for: " + min.ToString() + " " + "minutes" + "\n");

            iCount++;
        }
    }
} */



// if (DateTime.Now.IsDaylightSavingTime())
// {
// dAddHours = -7;
// zoneId = "Pacific Daylight Time";
// zone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
// }
// else
// {
// dAddHours = -8;
// zoneId = "Pacific Standard Time";
// zone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
// }