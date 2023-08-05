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
    public partial class Form11 : Form
    {
        public string strCountyName;
        public StringBuilder strSqlCommand = new StringBuilder("SELECT ID, County_Of_Res, CONVERT(CHAR(10), Queue_Check_DtTm, 101) as Queue_Check_Dt, CONVERT(CHAR(12), Queue_Check_DtTm, 108) as Queue_Check_Tm, Total_Agents_Online, Total_Agents_Avail, Num_Calls_In_Queue, Estimated_Wait_Tm, Active_Calls, QueueID FROM dbo.County_Code, dbo.Covered_CA_Queue_Stats", 5000);
        public Form11()
        {
            InitializeComponent();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            dateTimePicker1.MaxDate = DateTime.Now;
            dateTimePicker1.Text = DateTime.Now.ToString();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=148.92.136.117;Initial Catalog=ContactCalWINp;User ID=ContactCalWIN;Password=ContactCW2013!";
            cnn = new SqlConnection(connetionString);

            cnn.Open();

            SqlCommand command;
            String sql;

            sql = "SELECT dbo.County_Code.County_Name " +
                  "FROM [ContactCalWINp].[dbo].County_Code, [ContactCalWINp].[dbo].[County_Detail] " +
                  "WHERE [ContactCalWINp].[dbo].County_Code.County_Cd = [ContactCalWINp].[dbo].County_Detail.County_Cd AND " +
                  "[ContactCalWINp].[dbo].County_Detail.Calwin_County = '1' AND " +
                  "[ContactCalWINp].[dbo].County_Code.County_Name != 'EXECUTIVE SUMMARY ALERTS' ";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
            cnn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            strCountyName = comboBox1.Text;
            strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
            strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res = Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'") ;
            strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

            control_check();
                
            button1.Text = "call Analyzer";
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // int iCount = 0;
            button1.Text = "Querying The DB";
            button1.Enabled = false;
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=148.92.136.117;Initial Catalog=ContactCalWINp;User ID=ContactCalWIN;Password=ContactCW2013!";
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
            cnn.Close();

            button1.Text = "Displaying Result";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                control_check();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                control_check();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                control_check();
            }
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                control_check();
            }
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
               control_check();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
               control_check();             
        }

        public void control_check()
        {
            if (radioButton1.Checked == true)
            {
                if (radioButton3.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res = Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }

                if (radioButton4.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res = Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and ((Call_Disp_cd = \'18\') or (Call_Disp_Cd = \'19\'))");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }

                if (radioButton5.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res = Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and Call_Disp_cd != \'18\' and Call_Disp_Cd != \'19\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }
            }

            if (radioButton2.Checked == true)
            {
                if (radioButton3.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res != Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }

                if (radioButton4.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res != Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and ((Call_Disp_cd = \'18\') or (Call_Disp_Cd = \'19\'))");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }

                if (radioButton5.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res != Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and Call_Disp_cd != \'18\' and Call_Disp_Cd != \'19\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }
            }

            if (radioButton3.Checked == true)
            {
                if (radioButton1.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res = Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;

                }

                if (radioButton2.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res != Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;

                }
            }

            if (radioButton4.Checked == true)
            {
                if (radioButton1.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res = Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and ((Call_Disp_cd = \'18\') or (Call_Disp_Cd = \'19\'))");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;

                }

                if (radioButton2.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res != Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and ((Call_Disp_cd = \'18\') or (Call_Disp_Cd = \'19\'))");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }
            }

            if (radioButton5.Checked == true)
            {
                if (radioButton1.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res = Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and Call_Disp_cd != \'18\' and Call_Disp_Cd != \'19\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }

                if (radioButton2.Checked == true)
                {
                    strCountyName = comboBox1.Text;
                    strSqlCommand = new StringBuilder("SELECT Call_Key,Call_Track_ID,Cnty_of_Res,Lang_Cd,CONVERT(CHAR(10),Inbound_Call_dt,101),Inbound_Call_tm,SSC_Caller_ID,Answering_Cnty,Answering_Queue_Num,CONVERT(CHAR(10),Transfer_Call_Dt,101),Transfer_Call_Tm,Call_Disp_Cd,Call_Back_Num,Agent_Id_Num,Call_Stop_Dt,Call_Stop_Tm,Agent_Answer_Dt,Agent_Answer_Tm,Call_Disp_Dt,Call_Disp_Tm,UPD_USR_ID,Average_Call_Hndl_Tm,Transfer_Call_DateTime,Agent_Available,Call_Init_dt,Call_Init_tm,ININ_Call_ID,Test_Fl,Latency_Comp,Wrap_Up_End_Tm,TrunkNumber,LineNumber,VARFlag,SOFVarFlag,TransferLimitVal FROM ContactCalWINp.dbo.County_Code, ContactCalWINp.dbo.Covered_CA_Call_Transfer_Detail", 5000);
                    strSqlCommand.Append(@" WHERE dbo.County_Code.County_Name = " + "\'" + strCountyName + "\'" + " AND dbo.County_Code.County_Cd = dbo.Covered_CA_Call_Transfer_Detail.Cnty_of_Res and Cnty_of_Res != Answering_Cnty and Inbound_Call_dt = " + "\'" + dateTimePicker1.Text + "\'" + " and Call_Disp_cd != \'18\' and Call_Disp_Cd != \'19\'");
                    strSqlCommand.Append("ORDER BY dbo.Covered_CA_Call_Transfer_Detail.Call_Key DESC");

                    button1.Text = "call Analyzer";
                    button1.Enabled = true;
                }
            }
        }
    }
}
