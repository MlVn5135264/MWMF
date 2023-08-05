using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Office.Interop.Excel;
namespace MWMF
{
    public partial class Form24 : Form
    {        
        public long lLineCount = 0;
        public double SystemTransactionCount1 = 0, SystemTransactionCount2 = 0, SysTranDiff = 0;
        public int iFileCount = 0, iExcelLineCount, EDBCTransactionCount1 = 0, EDBCTransactionCount2 = 0, EDBCTranDiff = 0;
        public string modifiedTime;

        public Form24()
        {
            InitializeComponent();
        }

        private void Form24_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            dateTimePicker1.MaxDate = DateTime.Now;
            dateTimePicker1.Text = DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Running Health Check";
            button1.Enabled = false;

            // string[] FileNames = Directory.GetFiles(@"Y:\SW_Download_Area\archive_page_extracts\RUM_Extracts_202210");
            string[] FileNames = Directory.GetFiles(@"\\148.92.137.135\E$\SW_Download_Area\archive_page_extracts\RUM_Extracts_202210");
            // string[] FileNames = Directory.GetFiles(@"C:\RUM Health Check");
            // Iterate through each file and get the modified time
            foreach (string fileName in FileNames)
            {
                string startTime = dateTimePicker1.Text + " 05:59:59 AM";
                string endTime = dateTimePicker1.Text + " 21:59:59 PM";

                DateTime dt2 = new DateTime();
                dt2 = File.GetLastWriteTime(fileName);
                modifiedTime = dt2.ToString("MM/dd/yyyy HH:mm:ss");
                
                if (String.Compare(startTime, modifiedTime) < 0)
                {
                    if (String.Compare(modifiedTime, endTime) < 0)
                    {
                        iFileCount++;
                        process_csv_file(fileName, iFileCount);
                    }
                    // else
                    // {
                            // MessageBox.Show("Past Time Range");
                    // }
                }
            }

            textBox1.Text = iFileCount.ToString();

            System.Windows.Forms.Application.DoEvents();

            // Get the System and EDBC Transaction Counts
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            // Workbook wb = xlApp.Workbooks.Open(@"Y:\RUM_XS\Output\SLA_3.1.10_3.1.11_TransRespTime.xlsx");
             Workbook wb = xlApp.Workbooks.Open(@"\\148.92.137.135\E$\RUM_XS\Output\SLA_3.1.10_3.1.11_TransRespTime.xlsx");
            _Worksheet ws = wb.Sheets[1];

            // MessageBox.Show(Convert.ToString(ws.Cells[2, 3].Value));
            for (iExcelLineCount = 2; ws.Cells[iExcelLineCount, 3].Value != null; iExcelLineCount++);
            // MessageBox.Show(iExcelLineCount.ToString());
            SystemTransactionCount1 = (double) ws.Cells[(iExcelLineCount - 4), 3].Value;
            EDBCTransactionCount1 = (int) ws.Cells[(iExcelLineCount - 2), 3].Value;
            xlApp.Workbooks.Close();

            // MessageBox.Show(SystemTransactionCount1.ToString());
            // MessageBox.Show(EDBCTransactionCount1.ToString());

            // MessageBox.Show(Convert.ToString(ws.Cells[(iExcelLineCount - 4), 3].Value));
            // MessageBox.Show(Convert.ToString(ws.Cells[(iExcelLineCount - 2), 3].Value));

            // get the B4 file name
            // string[] B4FileNames = Directory.GetFiles(@"Y:\RUM_XS\Output");
            string[] B4FileNames = Directory.GetFiles(@"\\148.92.137.135\E$\RUM_XS\Output");
            foreach (string b4FileName in B4FileNames)
            {
                dateTimePicker1.CustomFormat = "yyyy/MM/dd";
                // dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd");

                string[] DateSplit = dateTimePicker1.Text.Split('/');
                if (b4FileName.Contains("B4_" + DateSplit[0] + DateSplit[1] + DateSplit[2]))
                {
                    // Get the System and EDBC Transaction Counts
                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                    wb = xlApp.Workbooks.Open(b4FileName);
                    ws = wb.Sheets[1];

                    // MessageBox.Show(Convert.ToString(ws.Cells[2, 3].Value));
                    for (iExcelLineCount = 2; ws.Cells[iExcelLineCount, 3].Value != null; iExcelLineCount++) ;
                    // MessageBox.Show(iExcelLineCount.ToString());
                    SystemTransactionCount2 = (double)ws.Cells[(iExcelLineCount - 4), 3].Value;
                    EDBCTransactionCount2 = (int)ws.Cells[(iExcelLineCount - 2), 3].Value;
                    xlApp.Workbooks.Close();

                    // MessageBox.Show(SystemTransactionCount2.ToString());
                    // MessageBox.Show(EDBCTransactionCount2.ToString());

                    SysTranDiff = SystemTransactionCount1 - SystemTransactionCount2;
                    EDBCTranDiff = EDBCTransactionCount1 - EDBCTransactionCount2;
                    textBox65.Text = SysTranDiff.ToString();
                    textBox66.Text = EDBCTranDiff.ToString();
                    System.Windows.Forms.Application.DoEvents();

                    // MessageBox.Show(b4FileName);
                    textBox67.Text = "Completed";

                    break;
                }
            }

            button1.Text = "Completed Health Check";
            button1.Enabled = true;
            
            Thread.Sleep(10000);
            
            button1.Text = "Start Rum Health Check";
            button1.Enabled = true;

        }

        private void process_csv_file(string fileName, int iFileCount)
        {
            lLineCount = 0;
            lLineCount = File.ReadAllLines(fileName).Length;

            if (iFileCount == 1)
            {
                textBox2.Text = fileName;
                textBox3.Text = modifiedTime;
                textBox4.Text = lLineCount.ToString();
            }

            if (iFileCount == 2)
            {
                textBox7.Text = fileName;
                textBox6.Text = modifiedTime;
                textBox5.Text = lLineCount.ToString();
            }

            if (iFileCount == 3)
            {
                textBox10.Text = fileName;
                textBox9.Text = modifiedTime;
                textBox8.Text = lLineCount.ToString();
            }

            if (iFileCount == 4)
            {
                textBox13.Text = fileName;
                textBox12.Text = modifiedTime;
                textBox11.Text = lLineCount.ToString();
            }

            if (iFileCount == 5)
            {
                textBox16.Text = fileName;
                textBox15.Text = modifiedTime;
                textBox14.Text = lLineCount.ToString();
            }

            if (iFileCount == 6)
            {
                textBox19.Text = fileName;
                textBox18.Text = modifiedTime;
                textBox17.Text = lLineCount.ToString();
            }

            if (iFileCount == 7)
            {
                textBox22.Text = fileName;
                textBox21.Text = modifiedTime;
                textBox20.Text = lLineCount.ToString();
            }

            if (iFileCount == 8)
            {
                textBox25.Text = fileName;
                textBox24.Text = modifiedTime;
                textBox23.Text = lLineCount.ToString();
            }

            if (iFileCount == 9)
            {
                textBox28.Text = fileName;
                textBox27.Text = modifiedTime;
                textBox26.Text = lLineCount.ToString();
            }

            if (iFileCount == 10)
            {
                textBox31.Text = fileName;
                textBox30.Text = modifiedTime;
                textBox29.Text = lLineCount.ToString();
            }

            if (iFileCount == 11)
            {
                textBox34.Text = fileName;
                textBox33.Text = modifiedTime;
                textBox32.Text = lLineCount.ToString();
            }

            if (iFileCount == 12)
            {
                textBox37.Text = fileName;
                textBox36.Text = modifiedTime;
                textBox35.Text = lLineCount.ToString();
            }

            if (iFileCount == 13)
            {
                textBox40.Text = fileName;
                textBox39.Text = modifiedTime;
                textBox38.Text = lLineCount.ToString();
            }

            if (iFileCount == 14)
            {
                textBox43.Text = fileName;
                textBox42.Text = modifiedTime;
                textBox41.Text = lLineCount.ToString();
            }

            if (iFileCount == 15)
            {
                textBox46.Text = fileName;
                textBox45.Text = modifiedTime;
                textBox44.Text = lLineCount.ToString();
            }

            if (iFileCount == 16)
            {
                textBox49.Text = fileName;
                textBox48.Text = modifiedTime;
                textBox47.Text = lLineCount.ToString();
            }

            if (iFileCount == 17)
            {
                textBox52.Text = fileName;
                textBox51.Text = modifiedTime;
                textBox50.Text = lLineCount.ToString();
            }

            if (iFileCount == 18)
            {
                textBox55.Text = fileName;
                textBox54.Text = modifiedTime;
                textBox53.Text = lLineCount.ToString();
            }

            if (iFileCount == 19)
            {
                textBox58.Text = fileName;
                textBox57.Text = modifiedTime;
                textBox56.Text = lLineCount.ToString();
            }

            if (iFileCount == 20)
            {
                textBox61.Text = fileName;
                textBox60.Text = modifiedTime;
                textBox59.Text = lLineCount.ToString();
            }

            if (iFileCount == 21)
            {
                textBox64.Text = fileName;
                textBox63.Text = modifiedTime;
                textBox62.Text = lLineCount.ToString();
            }

            System.Windows.Forms.Application.DoEvents();
        }
    }
}
