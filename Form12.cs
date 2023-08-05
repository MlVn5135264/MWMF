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
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=148.92.136.117;Initial Catalog=ContactCalWINp;User ID=ContactCalWIN;Password=ContactCW2013!";
            cnn = new SqlConnection(connetionString);

            cnn.Open();

            SqlCommand command;
            String sql;

            sql = "Select [Record_num],County_Name,[Total_Agents_Online],[Total_Agents_Avail],[Queue_Check_Dt],[Queue_Check_Tm] " +
                  "FROM[ContactCalWINp].[dbo].[Covered_CA_Queue_Status],[ContactCalWINp].[dbo].[County_Code] " +
                  "WHERE [ContactCalWINp].[dbo].[County_Code].County_Cd = [ContactCalWINp].[dbo].[Covered_CA_Queue_Status].County_of_Res AND " +
                  "[Total_Agents_Avail] > 0 AND " +
                  "[Queue_Check_Dt] = (cast((DATENAME(year, SYSDATETIME())) as CHAR(4)) + '-' + cast((DATEPART(MM, SYSDATETIME())) as CHAR(2)) + '-' + cast((DATEPART(dd, SYSDATETIME())) as CHAR(2))) AND " +
                  "datepart(hh,[Queue_Check_Tm]) = datepart(hh, getdate()) and(((datepart(hh, getdate())) - (datepart(hh,[Queue_Check_Tm])) = 0) or((datepart(hh, getdate())) - (datepart(hh,[Queue_Check_Tm])) = 1)) AND " +
                  "dateadd(mi, 10, datepart(mi,[Queue_Check_Tm])) >= datepart(mi, getdate()) " +
                  "Order By[Total_Agents_Avail] DESC";



       /*
       sql = "SELECT dbo.County_Code.County_Name " +
             "FROM [ContactCalWINp].[dbo].County_Code, [ContactCalWINp].[dbo].[County_Detail] " +
             "WHERE [ContactCalWINp].[dbo].County_Code.County_Cd = [ContactCalWINp].[dbo].County_Detail.County_Cd AND " +
             "[ContactCalWINp].[dbo].County_Detail.Calwin_County = '1' AND " +
             "[ContactCalWINp].[dbo].County_Code.County_Name != 'EXECUTIVE SUMMARY ALERTS' ";
       */
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No Agents are available to take calls in all the Counties");
            }

            command.Dispose();
            cnn.Close();

        }
    }
}
