using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfDataGridView
{
    public partial class Form1 : Form
    {
        ContextMenu contextMenu;
        List<Customer> customers;
        public Form1()
        {
            InitializeComponent();
            init();
        }

        public void init() {
            contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Delete");
            contextMenu.MenuItems[0].Click += new EventHandler(contextMenu_Delete_ItemClicked);
            contextMenu.MenuItems.Add("Add");
            contextMenu.MenuItems[1].Click += new EventHandler(contextMenu_Add_ItemClicked);
            dataGridView1.ContextMenu = contextMenu;
            customers = SeedCustomers(); ;
            Reload();
        }

        public void Reload() {
            
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = customers;
            dataGridView1.Refresh();
        }

        public void ClearIsntSaved() {
            foreach (Customer customer in customers.ToList()) {
                if (customer.ID == -1) {
                    customers.Remove(customer);
                }
            }
            Reload();
        }
        public void SaveElements() {
            int max = 1;
            if (customers.Where(x => x.ID > 0).Count() > 0)
            {
                max = customers.Max(x => x.ID) + 1;
            }
            foreach (Customer customer in customers.ToList())
            {
                if (customer.ID == -1)
                {
                    customer.ID = max;
                    max++;
                }
            }
            //Save in DB
            Reload();
        }


        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        contextMenu.Show(this, new Point(e.X, e.Y));

                    }
                    break;
            }
        }

        private void contextMenu_Delete_ItemClicked(object sender, EventArgs e) {
            DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows) {
                deleteCustomer((int) row.Cells[0].Value);
            }
            Reload();
        }

        private void deleteCustomer(int ID) {
             Customer customer= customers.Where(x=>x.ID == ID).FirstOrDefault();
            if (customer != null) {
                customers.Remove(customer);
            }
        }

        private void contextMenu_Add_ItemClicked(object sender, EventArgs e)
        {

            AddCustomer();
            Reload();
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = false;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].ReadOnly = true;
        }

        public List<Customer> SeedCustomers() {
            List<Customer> customers = new List<Customer>
            {
                new Customer() { ID = 1, Name = "Joe", City = "Camarillo" },
                new Customer() { ID = 2, Name = "Pitter", City = "Oxnard" },
                new Customer() { ID = 3, Name = "Marie", City = "Ventura" },
                new Customer() { ID = 4, Name = "Alfred", City = "Santa Barbara" },
                new Customer() { ID = 5, Name = "Smith", City = "Camarillo" },
                new Customer() { ID = 6, Name = "Marlon", City = "Ojai" },
                new Customer() { ID = 7, Name = "Joseph", City = "Filrmore" }
            };
            return customers;

        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddCustomer();
            Reload();
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = false;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].ReadOnly = true;
        }

        private void AddCustomer() {
            customers.Add(new Customer() { ID = -1, Name = "Click Name", City = "Change City" });
            //Save in DB
        }


        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            ClearIsntSaved();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveElements();
        }
       

        private void UpdateCustomer(int row, int col)
        {
            //Save in DB
         
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            string ColumnName = dataGridView1.Columns[col].Name;
            if (col > 0)
            {
                UpdateCustomer(row,col);
            }
        }
    }
}
