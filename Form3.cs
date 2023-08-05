using System;
using System.Windows.Forms;


namespace MWMF
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string line;
            string[] lineElements = null;
            int myOutput = 0;

            tb_environment.Text = "Working...";
            tb_virtual.Text = "Working...";
            tb_osclass.Text = "Working...";
            tb_osversion.Text = "Working...";
            tb_systemdescription.Text = "Working...";
            tb_systemstatus.Text = "Working...";
            tb_systemtype.Text = "Working...";
            tb_dcname.Text = "Working...";
            // tb_lastinventoryconnect.Text = "Working...";

            // open the lines below for excel
           /*
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(@"c:\temp\Machines.xlsx");
                _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                for (i = 2; (xlRange.Cells[i, 1].Value2 != null); i++)
                {
                    myOutput = 0;

                    myOutput = string.Compare(cb_systemname.Text, xlRange.Cells[i, 1].Value2.ToString());

                    if(myOutput == 0)
                    {
                        break;
                    }
                }
                // print the rest of the information
                tb_environment.Text = xlRange.Cells[i, 2].Value2.ToString();
                tb_virtual.Text = xlRange.Cells[i, 3].Value2.ToString();
                tb_osclass.Text = xlRange.Cells[i, 4].Value2.ToString();
                tb_osversion.Text = xlRange.Cells[i, 5].Value2.ToString();
                tb_systemdescription.Text = xlRange.Cells[i, 6].Value2.ToString();
                tb_systemstatus.Text = xlRange.Cells[i, 7].Value2.ToString();
                tb_systemtype.Text = xlRange.Cells[i, 8].Value2.ToString();
                tb_dcname.Text = xlRange.Cells[i, 9].Value2.ToString();
                // tb_lastinventoryconnect.Text = xlRange.Cells[i, 10].Value2;

                xlApp.Workbooks.Close();
            */
            System.IO.StreamReader file =    new System.IO.StreamReader(@"c:\temp\Machines.txt");

            // skip the first line
            line = file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                lineElements = line.Split(',');
                
                myOutput = string.Compare(cb_systemname.Text, lineElements[0]);

                if (myOutput == 0)
                {
                    break;
                }
            }
            file.Close();
            
            tb_environment.Text = lineElements[1];
            tb_virtual.Text = lineElements[2];
            tb_osclass.Text = lineElements[3];
            tb_osversion.Text = lineElements[4];
            tb_systemdescription.Text = lineElements[5];
            tb_systemstatus.Text = lineElements[6];
            tb_systemtype.Text = lineElements[7];
            tb_dcname.Text = lineElements[8];
            comboBox1.Text = lineElements[10];
                        
            if(lineElements[9] == "")
            {
                tb_lastinventoryconnect.Text = "Data Not Available";
            }
            else
            {
                tb_lastinventoryconnect.Text = lineElements[9];
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            int iCount = 0;
            string line;
            string[] lineElements;

            System.IO.StreamReader file = new System.IO.StreamReader(@"c:\temp\Machines.txt");
            // skip the first line
            line = file.ReadLine();

            while ((line = file.ReadLine()) != null)
            {
                lineElements = line.Split(',');
                cb_systemname.Items.Add(item: (lineElements[0].ToString()));
                iCount++;
            }

            // rewind file pointer to the beginning
            file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

            // skip the first line
            line = file.ReadLine();

            iCount = 0;

            while ((line = file.ReadLine()) != null)
            {
                lineElements = line.Split(',');
                comboBox1.Items.Add(item: (lineElements[10].ToString()));
                iCount++;
            }

            file.Close();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string line;
            string[] lineElements = null;
            int myOutput = 0;

            tb_environment.Text = "Working...";
            tb_virtual.Text = "Working...";
            tb_osclass.Text = "Working...";
            tb_osversion.Text = "Working...";
            tb_systemdescription.Text = "Working...";
            tb_systemstatus.Text = "Working...";
            tb_systemtype.Text = "Working...";
            tb_dcname.Text = "Working...";

            System.IO.StreamReader file = new System.IO.StreamReader(@"c:\temp\Machines.txt");

            // skip the first line
            line = file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                lineElements = line.Split(',');

                myOutput = string.Compare(comboBox1.Text, lineElements[10]);

                if (myOutput == 0)
                {
                    break;
                }
            }
            file.Close();

            tb_environment.Text = lineElements[1];
            tb_virtual.Text = lineElements[2];
            tb_osclass.Text = lineElements[3];
            tb_osversion.Text = lineElements[4];
            tb_systemdescription.Text = lineElements[5];
            tb_systemstatus.Text = lineElements[6];
            tb_systemtype.Text = lineElements[7];
            tb_dcname.Text = lineElements[8];
            cb_systemname.Text = lineElements[0];

            if (lineElements[9] == "")
            {
                tb_lastinventoryconnect.Text = "Data Not Available";
            }
            else
            {
                tb_lastinventoryconnect.Text = lineElements[9];
            }

        }
    }
}
