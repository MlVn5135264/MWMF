using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MWMF
{
    public partial class Form18 : Form
    {
        public static string FLine;

        public Form18()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if there is a change initialize the values in the labels
            label10.Text = "";
            label13.Text = "";
            label15.Text = "";
            label17.Text = "";
            label34.Text = "";
            label32.Text = "";
            label30.Text = "";
            label28.Text = "";

            Application.DoEvents();

            if (comboBox1.Text.Contains("Sacramento"))
            {
                NGetIPInfo("Sacramento");
            }

            if (comboBox1.Text.Contains("Solano"))
            {
                NGetIPInfo("Solano");
            }

            if (comboBox1.Text.Contains("Tulare"))
            {
                NGetIPInfo("Tulare");
            }

            if (comboBox1.Text.Contains("Ventura"))
            {
                NGetIPInfo("Ventura");
            }

            if (comboBox1.Text.Contains("Placer"))
            {
                NGetIPInfo("Placer");
            }

            if (comboBox1.Text.Contains("Santa Cruz"))
            {
                NGetIPInfo("Santa Cruz");
            }

            if (comboBox1.Text.Contains("Santa Clara"))
            {
                NGetIPInfo("Santa Clara");
            }

            if (comboBox1.Text.Contains("Contra Costa"))
            {
                NGetIPInfo("Contra Costa");
            }

            if (comboBox1.Text.Contains("Orange"))
            {
                NGetIPInfo("Orange");
            }

            if (comboBox1.Text.Contains("San Francisco"))
            {
                NGetIPInfo("San Francisco");
            }

            if (comboBox1.Text.Contains("San Luis Obispo"))
            {
                NGetIPInfo("San Luis Obispo");
            }

            if (comboBox1.Text.Contains("Fresno"))
            {
                NGetIPInfo("Fresno");
            }

            if (comboBox1.Text.Contains("San Mateo"))
            {
                NGetIPInfo("San Mateo");
            }

            if (comboBox1.Text.Contains("Yolo"))
            {
                NGetIPInfo("Yolo");
            }

            if (comboBox1.Text.Contains("San Diego"))
            {
                NGetIPInfo("San Diego");
            }

            if (comboBox1.Text.Contains("Sonoma"))
            {
                NGetIPInfo("Sonoma");
            }

            if (comboBox1.Text.Contains("Santa Barbara"))
            {
                NGetIPInfo("Santa Barbara");
            }

            if (comboBox1.Text.Contains("Alameda"))
            {
                NGetIPInfo("Alameda");
            }
        }

        private void Form18_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "Choose a County Name";
        }

        public void NGetIPInfo(String strCountyName)
        {           
            // open c:\temp\out.txt and parse the file and get the access information
            StreamReader CountyIPInfo = new StreamReader(@"C:\Temp\County_CCW_MR_POP_GWS_IP.txt");

            FLine = CountyIPInfo.ReadLine();

           while ((FLine = CountyIPInfo.ReadLine()) != null)
            {
                if (FLine.Contains("*"))
                {
                    continue;
                }

                if (FLine.Contains("#"))
                {
                    continue;
                }

                if (!FLine.Contains(strCountyName))
                {
                    continue;
                }

                String[] details = FLine.Split(',');

                iPopScreen(FLine, details[2]);
            }

            Application.DoEvents();

            // Close the file
            CountyIPInfo.Close();
        }

        public void iPopScreen(string FLine, string details)
        {
            if (FLine.Contains("CTI"))
            {
                label10.Text = details;

                Application.DoEvents();
            }

            if (FLine.Contains("POP"))
            {
                label13.Text = details;

                Application.DoEvents();
            }

            if (FLine.Contains("MR"))
            {
                label15.Text = details;

                Application.DoEvents();
            }

            if (FLine.Contains("GWS"))
            {
                label17.Text = details;

                Application.DoEvents();
            }

            if (FLine.Contains("ACW1"))
            {
                label34.Text = details;

                Application.DoEvents();
            }

            if (FLine.Contains("ACW2"))
            {
                label32.Text = details;

                Application.DoEvents();
            }

            if (FLine.Contains("CCW1"))
            {
                label30.Text = details;

                Application.DoEvents();
            }

            if (FLine.Contains("CCW2"))
            {
                label28.Text = details;

                Application.DoEvents();
            }
        }
    }
}
