using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace MWMF
{
    public partial class Form27 : Form
    {
        public StreamWriter writer1;

        public Form27()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // rename Results.csv to Results.txt
            System.IO.File.Move(@"C:\Service Now\Results.csv", @"C:\Service Now\Results.txt");

            writer1 = new StreamWriter(@"C:\Service Now\Results.csv");
            
            button1.Text = "Modifying.....";
            Application.DoEvents();
            
            using (StreamReader file = new StreamReader(@"C:\Service Now\Results.txt"))
            {
                string ln;
                String[] Service_Now;

                // ignore the first  line          
                ln = file.ReadLine();
                writer1.WriteLine(ln);

                while ((ln = file.ReadLine()) != null)
                {
                    // now split based on comma
                    Service_Now = ln.Split(',');
                    int i = Service_Now.Length;

                    // Scan for "NULLS" and replace with "EMPTY"
                    // This must be in the entire file
                    for (int k = 0; k < i; k++)
                    {
                        if (Service_Now[k] == "NULL")
                        {
                            Service_Now[k] = "EMPTY";
                        }
                    }

                    // Compose the line that should be written to the CSV file.
                    if (Service_Now[2] == "I")
                    {
                        Service_Now[2] = "Incident";
                    }

                    if (Service_Now[2] == "P")
                    {
                        Service_Now[2] = "Problem";
                    }

                    if (Service_Now[2] == "R")
                    {
                        Service_Now[2] = "Request";
                    }

                    // Compose the line that should be written to the CSV file.      
                    if (Service_Now[9] == "Urgent")
                    {
                        Service_Now[9] = "1-Urgent";
                    }

                    if (Service_Now[9] == "High")
                    {
                        Service_Now[9] = "2-High";
                    }

                    if (Service_Now[9] == "Medium")
                    {
                        Service_Now[9] = "3-Medium";
                    }

                    if (Service_Now[9] == "Low")
                    {
                        Service_Now[9] = "4-Low";
                    }

                    // Service_Now Compose
                    int j = 0;
                    for (; j < i; j++)
                    {
                        if (j < i - 1)
                        {
                            writer1.Write(Service_Now[j] + ",");
                        }
                        else
                        {
                            writer1.Write(Service_Now[j] + "\n");
                        }
                        //if (j == i)
                        //{
                        //writer1.WriteLine("\n");
                        //}
                    }
                    for(j = 0; j < i; j++)
                    {
                        Service_Now[j] = null;
                    }
                }

                file.Close();
                writer1.Close();
                button1.Text = "Done.....";
                Application.DoEvents();

                // Close the application
                Thread.Sleep(5000);
                this.Close();
            }
        }
    }
}