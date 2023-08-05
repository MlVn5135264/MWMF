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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

         private void Form9_Load(object sender, EventArgs e)
        {
            string csvFile = System.IO.Path.Combine(Application.StartupPath, @"c:\temp\ContactList.txt");
            List<string[]> rows = File.ReadAllLines(csvFile).Select(x => x.Split('\t')).ToList();
            DataTable dataTable = new DataTable();

            //add cols to datatable:
            dataTable.Columns.Add("Cnty No");
            dataTable.Columns.Add("County");
            dataTable.Columns.Add("Contact Type/Team");
            dataTable.Columns.Add("Hours");
            dataTable.Columns.Add("Primary");
            dataTable.Columns.Add("Primary Desk");
            dataTable.Columns.Add("Primary Cell");
            dataTable.Columns.Add("Primary Email");
            dataTable.Columns.Add("Backup");
            dataTable.Columns.Add("Backup Desk");
            dataTable.Columns.Add("Backup Cell");
            dataTable.Columns.Add("Backup Email");
            dataTable.Columns.Add("Notes");
            dataTable.Columns.Add("Reserved1");
            // dataTable.Columns.Add("Reserved2");
            // dataTable.Columns.Add("Reserved3");
            // dataTable.Columns.Add("Reserved4");
            // dataTable.Columns.Add("Reserved5");

            rows.ForEach(x => { dataTable.Rows.Add(x); });

            dataGridView1.DataSource = dataTable;
         }

        private void dataGridView1_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            string syolo = "";
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            bs.Filter = dataGridView1.Columns[1].HeaderText.ToString() + " LIKE '%" + syolo + "%'";
            dataGridView1.DataSource = bs;
        }
    }
}
