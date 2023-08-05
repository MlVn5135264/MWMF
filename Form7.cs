using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MWMF
{
    public partial class Form7 : Form
    {
        public string strCountyName;
        // public StringBuilder strSqlCommand = new StringBuilder("SELECT ID, County_Of_Res, CONVERT(CHAR(10), Queue_Check_DtTm, 101) as Queue_Check_Dt, CONVERT(CHAR(12), Queue_Check_DtTm, 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueId FROM dbo.County_Code, dbo.Queue_Stats", 5000);
        public StringBuilder strSqlCommand = new StringBuilder("SELECT ID, County_Of_Res, CONVERT(CHAR(10), DATEADD(HOUR,-8,Queue_Check_DtTm), 101) as Queue_Check_Dt, CONVERT(CHAR(12), DATEADD(HOUR,-8,Queue_Check_DtTm), 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueId FROM dbo.County_Code, dbo.Queue_Stats");        
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
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

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    comboBox1.Items.Add(item: dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
                dataGridView1.DataSource = null;
                sda.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
            cnn.Close();
            cnn.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Querying";
            button1.Enabled = false;
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=contact-calwin-db-us2ab-prod.cehxlb8w341l.us-west-2.rds.amazonaws.com;Initial Catalog=ConnectCalWIN;User ID=QueueStatsMonitoring;Password=sFNhx6qsW4XZKTuGgkVn";

            cnn = new SqlConnection(connetionString);

            cnn.Open();

            SqlCommand command;
            String sql;

            cnn = new SqlConnection(connetionString);
            cnn.Open();

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
                sda.Dispose();

                // MessageBox.Show(dataGridView1.Rows.Count.ToString());

                textBox1.Text = dataGridView1.Rows.Count.ToString();
                Application.DoEvents();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
            cnn.Close();
            cnn.Dispose();

            button1.Text = "Connect to DB";
            button1.Enabled = true;
            
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No Queue Stats found for the County: " + strCountyName);
            }
            else
            {
                var datetime = DateTime.Parse(dataGridView1.Rows[0].Cells[3].Value.ToString());
                var currentDateTime = DateTime.Now;
                var DateTimeDiff = currentDateTime - datetime;
                long cs = (long)DateTimeDiff.TotalMilliseconds;
                // MessageBox.Show(currentDateTime.ToString() + "....." +cs.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            strCountyName = comboBox1.Text;
            strSqlCommand = new StringBuilder("SELECT ID, County_Of_Res, CONVERT(CHAR(10), DATEADD(HOUR, -7, Queue_Check_DtTm), 101) as Queue_Check_Dt, CONVERT(CHAR(12), DATEADD(HOUR, -7, Queue_Check_DtTm), 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueID FROM dbo.County_Code, dbo.Queue_Stats");
            strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Queue_Stats.County_Of_Res ");
            // strSqlCommand.Append(@"AND CONVERT(CHAR(10), Queue_Check_DtTm, 101) = CONVERT(CHAR(10), GETDATE(), 101) ");
            strSqlCommand.Append(@"AND Queue_Check_DtTm between  " + "\'" + DateTime.Now.ToString("MM/dd/yyyy") + " 07:00:00.000000000\' AND " + "\'" + DateTime.Now.AddDays(1).ToString("MM/dd/yyyy") + " 06:59:59.999999999\'");
            strSqlCommand.Append("ORDER BY dbo.Queue_Stats.ID DESC");
            button1.Enabled = true;           
        }
   }
}

