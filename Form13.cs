using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MWMF
{
    public partial class Form13 : Form
    {
        #region - variable declarations
        public string strUATDSSFileName, smallestCountyName_UAT1, smallestCountyName_UAT2;
        public string smallestCountyXMLFileName_UAT1, smallestCountyXMLFileName_UAT2;
        public string strUATDSSDETFileName, strWEB1, strWEB2, strTUX1, strTUX2;
        public string s;
        public char cDataSourceRead = 'N';
        public string strLogFileName = @"C:\UATDSS\log\UATDSS_log_" + DateTime.Now.ToString("MM_dd_yyyy_") + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + ".txt";
        public StreamWriter log_writer;
        String[] dataSourceDetailsUAT1;
        String[] dataSourceOrderingUAT1;
        String[] dataSourceDetailsUAT2;
        String[] dataSourceOrderingUAT2;
        public String[] getCountyName;
        public String[] getUATMapping;
        StreamWriter swConfigFile;
        public string strConfigXMLNewFile;
        #endregion - variable declarations;
        string strConfigXMLPath;

        public Form13()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            DataTable table = new DataTable();

            #region Write to log
            log_writer = File.CreateText(strLogFileName);
            log_writer.WriteLine("Welcome to UAT Data Sources Swapping Log file" + "\n");
            log_writer.WriteLine("User Name: " + Environment.UserName + "\n");
            log_writer.WriteLine("Start Time: " + DateTime.Now.ToString("MM/dd/yyyy") + ": " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "\n");
            log_writer.WriteLine("County MIGRATION details:");
            #endregion

            #region - Find Smallest County
            strUATDSSFileName = @"C:\UATDSS\UAT_Mapping.txt";
            table.Columns.Add(new DataColumn("No.", typeof(string)));
            table.Columns.Add(new DataColumn("County Name", typeof(string)));
            table.Columns.Add(new DataColumn("Moving From", typeof(string)));
            table.Columns.Add(new DataColumn("Moving To", typeof(string)));
            table.Columns.Add(new DataColumn("Small County", typeof(string)));

            // find the smallest county in UAT1 and UAT2
            foreach (string line in File.ReadLines(strUATDSSFileName))
            {
                // check if the line countains smallest county information
                string[] DSSEntry = line.Split(',');
                if (line.Contains("Yes"))
                {
                    // if the smallest county remains stationary
                    if (DSSEntry[3].Contains("ST"))
                    {
                        if (DSSEntry[2].Contains("UAT1"))
                        {
                            smallestCountyName_UAT1 = DSSEntry[1];
                        }
                        else
                        {
                            smallestCountyName_UAT2 = DSSEntry[1];
                        }
                    }
                    else
                    {
                        if (DSSEntry[3].Contains("UAT1"))
                        {
                            smallestCountyName_UAT1 = DSSEntry[1];
                        }
                        else
                        {
                            smallestCountyName_UAT2 = DSSEntry[1];
                        }
                    }
                }

                // write the log for county migration
                #region - Write to log
                if (DSSEntry[3].Contains("ST"))
                {
                    log_writer.WriteLine("County " + DSSEntry[1] + " " + "remains STATIONARY in " + DSSEntry[2]);
                }
                else
                {
                    log_writer.WriteLine("County " + DSSEntry[1] + " " + "moves from " + DSSEntry[2] + " to " + DSSEntry[3]);
                }
                #endregion - Write to log

                table.Rows.Add(DSSEntry[0], DSSEntry[1], DSSEntry[2], DSSEntry[3], DSSEntry[4]);

            } // find the smallest county in UAT1 and UAT2

            // write the log for smallest counties
            #region - Write to log
            log_writer.WriteLine("\n" + "SMALLEST Counties Details:");
            log_writer.WriteLine("Smallest County in UAT1: " + smallestCountyName_UAT1);
            log_writer.WriteLine("Smallest County in UAT2: " + smallestCountyName_UAT2 + "\n");
            #endregion

            dataGridView1.DataSource = table;
            dataGridView1.Update();

            DialogResult result1 = MessageBox.Show("Is the UAT Mapping Correct?", "Important Question", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.No)
            {
                // close the file first
                log_writer.Close();

                // delete the file
                File.Delete(strLogFileName);

                return;
            }

            #endregion - Find Smallest County 

            #region - Find the names of the xml files associated with the smallest counties
            // find the XML files associated with the small counties in UAT1 and UAT2
            strUATDSSDETFileName = @"C:\UATDSS\UAT_Data_Sources_Details.txt";
            foreach (string line in File.ReadLines(strUATDSSDETFileName))
            {
                if (line.Contains("WEB1"))
                {
                    string[] DSSDETEntry = line.Split(',');
                    strWEB1 = DSSDETEntry[2];
                }

                if (line.Contains("WEB2"))
                {
                    string[] DSSDETEntry = line.Split(',');
                    strWEB2 = DSSDETEntry[2];
                }

                if (line.Contains("TUX1"))
                {
                    string[] DSSDETEntry = line.Split(',');
                    strTUX1 = DSSDETEntry[2];
                }

                if (line.Contains("TUX2"))
                {
                    string[] DSSDETEntry = line.Split(',');
                    strTUX2 = DSSDETEntry[2];
                }

                if (line.Contains(smallestCountyName_UAT1))
                {
                    string[] DSSDETEntry = line.Split(',');
                    smallestCountyXMLFileName_UAT1 = DSSDETEntry[2];
                }

                if (line.Contains(smallestCountyName_UAT2))
                {
                    string[] DSSDETEntry = line.Split(',');
                    smallestCountyXMLFileName_UAT2 = DSSDETEntry[2];
                }
            }
            #endregion - Find the names of the xml files associated with the smallest counties

            #region - Write to log
            log_writer.WriteLine("WEBLOGIC and TUXEDO Multi Data Source XML File Names:");
            log_writer.WriteLine("WEBLOGIC UAT1 Multi Data Source XML File Name: " + strWEB1);
            log_writer.WriteLine("WEBLOGIC UAT2 Multi Data Source XML File Name: " + strWEB2);
            log_writer.WriteLine("TUXEDO UAT1 Multi Data Source XML File Name: " + strTUX1);
            log_writer.WriteLine("TUXEDO UAT2 Multi Data Source XML File Name: " + strTUX2);
            #endregion

            #region - backup files in the UAT box
            // backup the files in the remote box
            backup_files(strWEB1, strWEB2, strTUX1, strTUX2);
            #endregion - backup files in the UAT box

            #region - copy files from the UAT box locally on the Windows Box
            // copy the files over to the local machine
            log_writer.WriteLine("Start copying File " + strWEB1 + " locally on the Windows Box");
            copy_files(strWEB1);
            log_writer.WriteLine("Successfully Copied File " + strWEB1 + " locally on the Windows Box");

            log_writer.WriteLine("Start copying File " + strWEB2 + " locally on the Windows Box");
            copy_files(strWEB2);
            log_writer.WriteLine("Successfully Copied File " + strWEB2 + " locally on the Windows Box");

            log_writer.WriteLine("Start copying File " + strTUX1 + " locally on the Windows Box");
            copy_files(strTUX1);
            log_writer.WriteLine("Successfully Copied File " + strTUX1 + " locally on the Windows Box"); 

            log_writer.WriteLine("Start copying File " + strTUX2 + " locally on the Windows Box");
            copy_files(strTUX2);
            log_writer.WriteLine("Successfully Copied File " + strTUX2 + " locally on the Windows Box");

            log_writer.WriteLine("Start copying File " + smallestCountyXMLFileName_UAT1 + " locally on the Windows Box");
            copy_files(smallestCountyXMLFileName_UAT1);
            log_writer.WriteLine("Successfully Copied File " + smallestCountyXMLFileName_UAT1 + " locally on the Windows Box");

            log_writer.WriteLine("Start copying File " + smallestCountyXMLFileName_UAT2 + " locally on the Windows Box");
            copy_files(smallestCountyXMLFileName_UAT2);
            log_writer.WriteLine("Successfully Copied File " + smallestCountyXMLFileName_UAT2 + " locally on the Windows Box");

            log_writer.WriteLine("Start copying File config.xml on the host");
            copy_files("config.xml");
            log_writer.WriteLine("Successfully Copied File config.xml on the host");

            // Back the config.xml file first
            log_writer.WriteLine("Start Backing up config.xml locally in the Windows Box");
            config_xml_copy();
            log_writer.WriteLine("Backed up config.xml locally in the Windows Box");
            #endregion - copy files from the UAT box locally on the Windows Boxprocess_config_xml_file

            #region - Get SMALLEST Counties Data Source ordering in UAT and UAT2
            Get_smallest_counties_DataSource_Ordering();
            #endregion - Get SMALLEST Counties Data Source ordering in UAT and UAT2

            #region - Update xml files for Weblogic and Tuxedo DataSources for UAT1
            update_Weblogic_Tuxedo_XML_files_for_UAT1_region();
            #endregion - Update xml files for Weblogic and Tuxedo DataSources for UAT1

            #region - Update xml files for Weblogic and Tuxedo DataSources for UAT2
            update_Weblogic_Tuxedo_XML_files_for_UAT2_region();
            #endregion - Update xml files for Weblogic and Tuxedo DataSources for UAT2

            // start processing the config.xml 
            #region - Write to log
            log_writer.WriteLine("\nStart updating the config.xml file");
            #endregion

            process_config_xml_file();

            #region - Write to log
            log_writer.WriteLine("End updating the config.xml file");
            #endregion

            log_writer.WriteLine("\nEnd Time: " + DateTime.Now.ToString("MM/dd/yyyy") + ": " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "\n");

            // copy the xml files modified to UT100 in the concerned locations in UT100
            copy_modified_XML_files_to_UT100();

            log_writer.Close();
        }

        private void backup_files(string strWEB1, string strWEB2, string strTUX1, string strTUX2)
        {
            StringBuilder ab = new StringBuilder();
            StringBuilder cd = new StringBuilder();

            // declare path to the command file
            string path = @"C:\UATDSS\ut100.txt";
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // create the file pr201.txt in c:Temp
            StreamWriter sw = File.CreateText(path);
            sw.WriteLine(". ~/.kshrc;");
            sw.WriteLine(". ~/.profile >/dev/null 2>&1;");
            sw.WriteLine();
            sw.WriteLine("cd /WEBLOGIC_MDA2/UAT12C_domain/config/jdbc");

            string cksumStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            sw.Write("cp -p "); sw.Write(strWEB1); sw.Write(" "); sw.Write(strWEB1); sw.Write("_"); sw.WriteLine(cksumStr);
            sw.Write("cksum "); sw.Write(strWEB1); sw.Write(" "); sw.Write(strWEB1); sw.Write("_"); sw.WriteLine(cksumStr);

            sw.Write("cp -p "); sw.Write(strWEB2); sw.Write(" "); sw.Write(strWEB2); sw.Write("_"); sw.WriteLine(cksumStr);
            sw.Write("cksum "); sw.Write(strWEB2); sw.Write(" "); sw.Write(strWEB2); sw.Write("_"); sw.WriteLine(cksumStr);

            sw.Write("cp -p "); sw.Write(strTUX1); sw.Write(" "); sw.Write(strTUX1); sw.Write("_"); sw.WriteLine(cksumStr);
            sw.Write("cksum "); sw.Write(strTUX1); sw.Write(" "); sw.Write(strTUX1); sw.Write("_"); sw.WriteLine(cksumStr);

            sw.Write("cp -p "); sw.Write(strTUX2); sw.Write(" "); sw.Write(strTUX2); sw.Write("_"); sw.WriteLine(cksumStr);
            sw.Write("cksum "); sw.Write(strTUX2); sw.Write(" "); sw.Write(strTUX2); sw.Write("_"); sw.WriteLine(cksumStr);

            sw.WriteLine("cd /WEBLOGIC_MDA2/UAT12C_domain/config");

            sw.Write("cp -p "); sw.Write("config.xml"); sw.Write(" "); sw.Write("config.xml"); sw.Write("_"); sw.WriteLine(cksumStr);
            sw.Write("cksum "); sw.Write("config.xml"); sw.Write(" "); sw.Write("config.xml"); sw.Write("_"); sw.WriteLine(cksumStr);

            sw.Close();

            richTextBox1.AppendText("Backing up the following files: " + "\n");
            richTextBox1.AppendText("\t" + strWEB1 + "\n");
            richTextBox1.AppendText("\t" + strWEB2 + "\n");
            richTextBox1.AppendText("\t" + strTUX1 + "\n");
            richTextBox1.AppendText("\t" + strTUX2 + "\n");
            richTextBox1.AppendText("\t" + "config.xml" + "\n");
            richTextBox1.AppendText("\n" + "Running cksum on the files backed up");

            #region - Write to log
            log_writer.WriteLine("\nBackup and Cksum Operations started.....");
            #endregion

            ab = new StringBuilder("plink");
            cd = new StringBuilder(" -pw 2$7u]WsG weblogic@10.2.2.35 -m c:\\UATDSS\\ut100.txt");

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ab.ToString();
            psi.Arguments = cd.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            string s = proc.StandardOutput.ReadToEnd();
            // MOHAN
            richTextBox1.AppendText("\n" + s);
            richTextBox1.Update();

            #region - Write to log
            log_writer.Write(richTextBox1.Text);
            #endregion

            #region - Write to log
            log_writer.WriteLine("Backup and Cksum Operations completed....." + "\n");
            #endregion
        }

        private void copy_files(string strFileName)
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // Transfer the file over
            ef = new StringBuilder("pscp");

            if (strFileName.Contains("config.xml"))
            {
                gh = new StringBuilder(" -pw 2$7u]WsG weblogic@10.2.2.35:" + "/WEBLOGIC_MDA2/UAT12C_domain/config/" + strFileName + " C:\\UATDSS");
            }
            else
            {
                gh = new StringBuilder(" -pw 2$7u]WsG weblogic@10.2.2.35:" + "/WEBLOGIC_MDA2/UAT12C_domain/config/jdbc/" + strFileName + " C:\\UATDSS");
            }

            if (strFileName.Contains("MULTI_CMWADS-2055-jdbc"))
            {
                richTextBox1.AppendText("\n" + "Transferring file " + strFileName + "\n");
                richTextBox1.Update();
            }
            else
            {
                richTextBox1.AppendText("Transferring file " + strFileName + "\n");
                richTextBox1.Update();
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            s = proc.StandardOutput.ReadToEnd();
        }

        private void config_xml_copy()
        {
            string path = @"C:\UATDSS\config_xml_backup\config.xml";
            strConfigXMLPath = @"C:\UATDSS\config.xml";

            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // create the file pr201.txt in c:Temp
            StreamWriter sw = File.CreateText(path);

            foreach (string line in File.ReadLines(strConfigXMLPath))
            {
                sw.Write(line + "\n");
            }

            sw.Close();
        }

        private void Form13_Load(object sender, EventArgs e)
        {

        }

        private void Get_smallest_counties_DataSource_Ordering()
        {
            // smallestCountyXMLFileName_UAT1        
            // smallestCountyXMLFileName_UAT2

            foreach (string line in File.ReadLines(@"C:\UATDSS\" + smallestCountyXMLFileName_UAT1))
            {
                if (line.Contains("data-source-list"))
                {
                    dataSourceDetailsUAT1 = line.Split('>');
                    dataSourceOrderingUAT1 = dataSourceDetailsUAT1[1].Split('<');

                    #region - Write to log
                    log_writer.WriteLine("\nThe Data Source ordering for UAT1 smallest county " + " is " + dataSourceOrderingUAT1[0]);
                    #endregion

                    break;
                }
            }

            foreach (string line in File.ReadLines(@"C:\UATDSS\" + smallestCountyXMLFileName_UAT2))
            {
                if (line.Contains("data-source-list"))
                {
                    dataSourceDetailsUAT2 = line.Split('>');
                    dataSourceOrderingUAT2 = dataSourceDetailsUAT2[1].Split('<');

                    #region - Write to log
                    log_writer.WriteLine("The Data Source ordering for UAT2 smallest county " + " is " + dataSourceOrderingUAT2[0] + "\n");
                    #endregion

                    break;
                }
            }
        }

        private void update_Weblogic_Tuxedo_XML_files_for_UAT1_region()
        {
            string strWeb1XMLFile = @"C:\UATDSS\" + strWEB1;
            // string strWeb1XMLNewFile = @"C:\UATDSS\" + strWEB1 + "." + DateTime.Now.ToString("yyyyMMdd");
            string strWeb1XMLNewFile = @"C:\UATDSS\intransit\jdbc\" + strWEB1;

            string strTux1XMLFile = @"C:\UATDSS\" + strTUX1;
            // string strTux1XMLNewFile = @"C:\UATDSS\" + strTUX1 + "." + DateTime.Now.ToString("yyyyMMdd");
            string strTux1XMLNewFile = @"C:\UATDSS\intransit\jdbc\" + strTUX1;

            #region - Write to log
            log_writer.WriteLine("Start creating the file " + strWeb1XMLNewFile + ". This will have the updated information for Weblogic Multi Data Source UAT1 region");
            #endregion

            StreamWriter sw = File.CreateText(strWeb1XMLNewFile);
            foreach (string line in File.ReadLines(strWeb1XMLFile))
            {
                if (line.Contains("data-source-list"))
                {
                    sw.Write("<data-source-list>");
                    sw.Write(dataSourceOrderingUAT1[0]);
                    sw.Write("</data-source-list>" + "\n");
                }
                else
                {
                    sw.Write(line + "\n");
                }
            }
            sw.Close();

            #region - Write to log
            log_writer.WriteLine("Successful created the file " + strWeb1XMLNewFile + ". This has the updated information for Weblogic Multi Data Source UAT1 region");
            #endregion

            #region - Write to log
            log_writer.WriteLine("Start creating the file " + strTux1XMLNewFile + ". This will have the updated information for Tuxedo Multi Data Source UAT1 region");
            #endregion

            sw = File.CreateText(strTux1XMLNewFile);
            foreach (string line in File.ReadLines(strTux1XMLFile))
            {
                if (line.Contains("data-source-list"))
                {
                    sw.Write("<data-source-list>");
                    sw.Write(dataSourceOrderingUAT1[0]);
                    sw.Write("</data-source-list>" + "\n");
                }
                else
                {
                    sw.Write(line + "\n");
                }
            }
            sw.Close();

            #region - Write to log
            log_writer.WriteLine("Successfully created the file " + strTux1XMLNewFile + ". This has the updated information for Tuxedo Multi Data Source UAT1 region");
            #endregion
        }
        private void update_Weblogic_Tuxedo_XML_files_for_UAT2_region()
        {
            string strWeb2XMLFile = @"C:\UATDSS\" + strWEB2;
            // string strWeb2XMLNewFile = @"C:\UATDSS\" + strWEB2 + "." + DateTime.Now.ToString("yyyyMMdd");
            string strWeb2XMLNewFile = @"C:\UATDSS\intransit\jdbc\" + strWEB2;

            string strTux2XMLFile = @"C:\UATDSS\" + strTUX2;
            // string strTux2XMLNewFile = @"C:\UATDSS\" + strTUX2 + "." + DateTime.Now.ToString("yyyyMMdd");
            string strTux2XMLNewFile = @"C:\UATDSS\intransit\jdbc\" + strTUX2;

            #region - Write to log
            log_writer.WriteLine("Start creating the file " + strWeb2XMLNewFile + ". This will have the updated information for Weblogic Multi Data Source UAT2 region");
            #endregion

            StreamWriter sw = File.CreateText(strWeb2XMLNewFile);
            foreach (string line in File.ReadLines(strWeb2XMLFile))
            {
                if (line.Contains("data-source-list"))
                {
                    sw.Write("<data-source-list>");
                    sw.Write(dataSourceOrderingUAT2[0]);
                    sw.Write("</data-source-list>" + "\n");
                }
                else
                {
                    sw.Write(line + "\n");
                }
            }
            sw.Close();
            #region - Write to log
            log_writer.WriteLine("Successful created the file " + strWeb2XMLNewFile + ". This has the updated information for Weblogic Multi Data Source UAT2 region");
            #endregion

            #region - Write to log
            log_writer.WriteLine("Start creating the file " + strTux2XMLNewFile + ". This will have the updated information for Tuxedo Multi Data Source UAT2 region");
            #endregion

            sw = File.CreateText(strTux2XMLNewFile);
            foreach (string line in File.ReadLines(strTux2XMLFile))
            {
                if (line.Contains("data-source-list"))
                {
                    sw.Write("<data-source-list>");
                    sw.Write(dataSourceOrderingUAT2[0]);
                    sw.Write("</data-source-list>" + "\n");
                }
                else
                {
                    sw.Write(line + "\n");
                }
            }
            sw.Close();

            #region - Write to log
            log_writer.WriteLine("Successfully created the file " + strTux2XMLNewFile + ". This has the updated information for Tuxedo Multi Data Source UAT2 region");
            #endregion
        }

        private void process_config_xml_file()
        {
            string strConfigXMLFile = @"C:\UATDSS\config.xml";
            // strConfigXMLNewFile = @"C:\UATDSS\intransit\config\config.xml." + DateTime.Now.ToString("yyyyMMdd");
            // strConfigXMLNewFile = @"C:\UATDSS\intransit\config\config.xml" + "_" + DateTime.Now.ToString("MMddyyyyHHmmss");
            strConfigXMLNewFile = @"C:\UATDSS\intransit\config\config.xml";

            char CWrote = 'N';

            // Delete the file if it exists.
            if (File.Exists(strConfigXMLNewFile))
            {
                File.Delete(strConfigXMLNewFile);
            }
            // create the new config.xml.yyyyMMdd file
            swConfigFile = File.CreateText(strConfigXMLNewFile);

            foreach (string line in File.ReadLines(strConfigXMLFile)) // main config.xml file
            {
                if (cDataSourceRead == 'Y')
                {cDataSourceRead = 'N';

                    continue;
                }

                CWrote = 'N';
                if (line.Contains("<name>ALA01U1_CMWA01DS"))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("ALA01U1_CMWA01DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>ALA01U2_CMWA01DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("ALA01U2_CMWA01DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>FRS10U1_CMWA10DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("FRS10U1_CMWA10DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>FRS10U2_CMWA10DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("FRS10U2_CMWA10DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SAC34U1_CMWA31DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SAC34U1_CMWA31DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SAC34U2_CMWA31DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SAC34U2_CMWA31DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SFO38U1_CMWA38DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SFO38U1_CMWA38DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SFO38U2_CMWA38DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SFO38U2_CMWA38DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SLO40U1_CMWA40DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SLO40U1_CMWA40DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SLO40U2_CMWA40DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SLO40U2_CMWA40DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SON49U1_CMWA49DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SON49U1_CMWA49DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>SON49U2_CMWA49DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("SON49U2_CMWA49DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>MULTI_ALA_CMWA01DS_UAT1")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("MULTI_ALA_CMWA01DS_UAT1"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>MULTI_FRS_CMWA10DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("MULTI_FRS_CMWA10DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>MULTI_SAC_CMWA34DS-UAT1")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("MULTI_SAC_CMWA34DS-UAT1"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>MULTI_SFO_CMWA38DS-UAT2")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("MULTI_SFO_CMWA38DS-UAT2"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>MULTI_SLO_CMWA40DS-UAT2")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("MULTI_SLO_CMWA40DS-UAT2"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if ((line.Contains("<name>MULTI_SON_CMWA49DS")))
                {
                    swConfigFile.Write(line + "\n");

                    foreach (string line1 in File.ReadLines(@"c:\UATDSS\UAT_Data_Sources_Details.txt"))
                    {
                        if (line1.Contains("MULTI_SON_CMWA49DS"))
                        {
                            getCountyName = line1.Split(',');
                            foreach (string line2 in File.ReadLines(@"c:\UATDSS\UAT_Mapping.txt"))
                            {
                                if (line2.Contains(getCountyName[0]))
                                {
                                    getUATMapping = line2.Split(',');
                                    if (getUATMapping[3].Contains("UAT1"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT1-Server01,CWEUAT1-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                    if (getUATMapping[3].Contains("UAT2"))
                                    {
                                        File.ReadLines(strConfigXMLFile);

                                        swConfigFile.Write("    <target>CWEUAT2-Server01,CWEUAT2-Server02</target>" + "\n");
                                        cDataSourceRead = 'Y';
                                    }
                                }
                            }
                        }
                    }

                    CWrote = 'Y';
                }

                if (CWrote == 'N')
                {
                    swConfigFile.Write(line + "\n");
                    /*
                    #region - Write to log
                    log_writer.Write("County " + getCountyName[0] + " " + "remains STATIONARY in " + getUATMapping[2]);
                    if(getUATMapping[2].Contains("UAT1"))
                    {
                        log_writer.WriteLine(". Target -> CWEUAT1-Server01,CWEUAT1-Server02");
                    }
                    if (getUATMapping[2].Contains("UAT2"))
                    {
                        log_writer.WriteLine(". Target -> CWEUAT2-Server01,CWEUAT2-Server02");
                    }
                    #endregion - Write to log
                    */
                }

                if (CWrote == 'Y')
                {
                    #region - Write to log
                    if (getUATMapping[3].Contains("UAT1"))
                    {
                        log_writer.Write("County " + getCountyName[0] + " moves from " + getUATMapping[2] + " to " + getUATMapping[3]);
                        log_writer.WriteLine(". Target changed to -> CWEUAT1-Server01,CWEUAT1-Server02");
                    }
                    if (getUATMapping[3].Contains("UAT2"))
                    {
                        log_writer.Write("County " + getCountyName[0] + " moves from " + getUATMapping[2] + " to " + getUATMapping[3]);
                        log_writer.WriteLine(". Target changed to -> CWEUAT2-Server01,CWEUAT2-Server02");
                    }
                    #endregion - Write to log
                }
            }

            swConfigFile.Close();

            XMLFileGetFileList();
        }

        private void copy_modified_XML_files_to_UT100()
        {
            StringBuilder ef = new StringBuilder();
            StringBuilder gh = new StringBuilder();

            // Transfer the file over
            ef = new StringBuilder("pscp ");
            gh = new StringBuilder(" -pw 2$7u]WsG " + strConfigXMLNewFile + @" weblogic@10.2.2.35:/WEBLOGIC_MDA2/UAT12C_domain/config");
                                                                
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            var proc = Process.Start(psi);
            try 
            {
                s = proc.StandardOutput.ReadToEnd();
            }
            catch(Exception ex) 
            { 
                MessageBox.Show(ex.ToString());
            }
            
            richTextBox1.AppendText("\n" + "Successfully Transferred file config.xml to the remote Server" + "\n");
            richTextBox1.Update();

            // copy the the Weblogic and the Tuxedo Data Sources files over
            ef = new StringBuilder("pscp ");
            gh = new StringBuilder(@"-pw 2$7u]WsG C:\UATDSS\intransit\jdbc\*.xml weblogic@10.2.2.35:/WEBLOGIC_MDA2/UAT12C_domain/config/jdbc");
            psi = new ProcessStartInfo();
            psi.FileName = ef.ToString();
            psi.Arguments = gh.ToString();

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            proc = Process.Start(psi);
            try
            {
                s = proc.StandardOutput.ReadToEnd();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            richTextBox1.AppendText("\n" + "Successfully Transferred Data Sources files Weblogic and Tuxedo XML files to the remote Server" + "\n");
            richTextBox1.Update();

            richTextBox1.AppendText(s + "\n" + "Completed Operations on the host Machine....." + "\n");
            richTextBox1.Update();
            richTextBox1.AppendText("Data Sources Swapping Completed.....\n");
            richTextBox1.Update();

            MessageBox.Show("Data Sources Swapping Completed.....");
        }

        private void XMLFileGetFileList()
        {
            richTextBox1.AppendText("\n----- Comparing Weblogic and Tuxedo for smallest couties -----\n");
            Application.DoEvents();

            // compare the Multi Data Sources Files
            DirectoryInfo currentDirectory = new DirectoryInfo(@"C:\\UATDSS");
            foreach (FileInfo uatdss_files in currentDirectory.GetFiles("MULTI_CM*.xml"))
            {
                string fileName = uatdss_files.FullName;

                StringBuilder ab;
                StringBuilder cd;
                try
                {
                    ab = new StringBuilder("fc ");
                    cd = new StringBuilder(fileName + @" c:\UATDSS\intransit\jdbc\" + Path.GetFileName(fileName));
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = ab.ToString();
                    psi.Arguments = cd.ToString();

                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    psi.CreateNoWindow = true;
                    var proc = Process.Start(psi);
                    #pragma warning disable 8602
                    string s = proc.StandardOutput.ReadToEnd();
                    #pragma warning restore 8602
                    richTextBox1.AppendText(s);
                    Application.DoEvents();

                    #region - Write to log
                    log_writer.WriteLine("\n" + s + "\n");
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            // compare the Multi Data Sources File
            richTextBox1.AppendText("----- Comparing the config.xml files -----\n");
            Application.DoEvents();

            currentDirectory = new DirectoryInfo(@"C:\UATDSS\config_xml_backup");
            foreach (FileInfo uatdss_files in currentDirectory.GetFiles("config.xml"))
            {
                string fileName = uatdss_files.FullName;

                StringBuilder ab;
                StringBuilder cd;
                try
                {
                    ab = new StringBuilder("fc ");
                    cd = new StringBuilder(fileName + @" c:\UATDSS\intransit\config\" + Path.GetFileName(fileName));
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = ab.ToString();
                    psi.Arguments = cd.ToString();

                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    psi.CreateNoWindow = true;
                    var proc = Process.Start(psi);
                    #pragma warning disable 8602
                    string s = proc.StandardOutput.ReadToEnd();
                    #pragma warning restore 8602
                    richTextBox1.AppendText(s);
                    Application.DoEvents();

                    #region - Write to log
                    log_writer.WriteLine("\n" + s + "\n");
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
