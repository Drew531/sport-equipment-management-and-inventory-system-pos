using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_SYSTEM
{
    public partial class ITEMS : Form
    {

        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb;";
        private int selectedItemId = -1;


        public ITEMS()
        {
            InitializeComponent();
           
        }


        private void LoadData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, ItemName, ItemCategory, ItemPrice, ItemStock, ItemImage FROM Items";
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (!dgvITEMLIST.Columns.Contains("ItemImageColumn"))
                        {
                            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
                            {
                                Name = "ItemImageColumn",
                                HeaderText = "Item Image",
                                ImageLayout = DataGridViewImageCellLayout.Zoom
                            };
                            dgvITEMLIST.Columns.Add(imageColumn);
                        }

                        dgvITEMLIST.DataSource = table;

                        foreach (DataGridViewRow row in dgvITEMLIST.Rows)
                        {
                            if (row.Cells["ItemImage"].Value is byte[] imageBytes)
                            {
                                using (var ms = new MemoryStream(imageBytes))
                                {
                                    row.Cells["ItemImageColumn"].Value = Image.FromStream(ms);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            tbITEMNAME.Clear();
            cbITEMCATEGORY.SelectedIndex = -1;
            tbPRICE.Clear();
            tbSTOCK.Clear();
            pbIMAGE.Image = null;
            selectedItemId = -1;
        }

        private void ITEMS_Load(object sender, EventArgs e)
        {
            LoadData(); // Load items into DataGridView
            LoadCategories();
        }

        private void LoadCategories()
        {

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT CategoryName FROM Categories";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            cbITEMCATEGORY.Items.Clear();
                            while (reader.Read())
                            {
                                cbITEMCATEGORY.Items.Add(reader["CategoryName"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearFields(); // Clear text boxes for new item entry
            tbITEMNAME.Focus();

        }

        private void btnEDITITEM_Click(object sender, EventArgs e)
        {
            if (selectedItemId == -1)
            {
                MessageBox.Show("Please select an item to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tbITEMNAME.Focus();
        }

        private void btnSAVE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbITEMNAME.Text) || string.IsNullOrWhiteSpace(cbITEMCATEGORY.Text) ||
                string.IsNullOrWhiteSpace(tbPRICE.Text) || string.IsNullOrWhiteSpace(tbSTOCK.Text))
            {
                MessageBox.Show("Please fill out all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] imageBytes = null;
            if (pbIMAGE.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pbIMAGE.Image.Save(ms, pbIMAGE.Image.RawFormat);
                    imageBytes = ms.ToArray();
                }
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = selectedItemId == -1
                        ? "INSERT INTO Items (ItemName, ItemCategory, ItemPrice, ItemStock, ItemImage) VALUES (@name, @category, @price, @stock, @image)"
                        : "UPDATE Items SET ItemName = @name, ItemCategory = @category, ItemPrice = @price, ItemStock = @stock, ItemImage = @image WHERE ID = @id";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", tbITEMNAME.Text);
                        command.Parameters.AddWithValue("@category", cbITEMCATEGORY.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@price", Convert.ToDouble(tbPRICE.Text));
                        command.Parameters.AddWithValue("@stock", Convert.ToInt32(tbSTOCK.Text));
                        command.Parameters.AddWithValue("@image", (object)imageBytes ?? DBNull.Value);

                        if (selectedItemId != -1)
                        {
                            command.Parameters.AddWithValue("@id", selectedItemId);
                        }

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(selectedItemId == -1 ? "Item added successfully!" : "Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDELETE_Click(object sender, EventArgs e)
        {
            if (selectedItemId == -1)
            {
                MessageBox.Show("Please select an item to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Items WHERE ID = @id";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", selectedItemId);
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void dtvITEMLIST_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvITEMLIST.Rows[e.RowIndex].Cells["ID"].Value != null)
            {
                selectedItemId = Convert.ToInt32(dgvITEMLIST.Rows[e.RowIndex].Cells["ID"].Value);
                tbITEMNAME.Text = dgvITEMLIST.Rows[e.RowIndex].Cells["ItemName"].Value.ToString();
                cbITEMCATEGORY.SelectedItem = dgvITEMLIST.Rows[e.RowIndex].Cells["ItemCategory"].Value.ToString();
                tbPRICE.Text = dgvITEMLIST.Rows[e.RowIndex].Cells["ItemPrice"].Value.ToString();
                tbSTOCK.Text = dgvITEMLIST.Rows[e.RowIndex].Cells["ItemStock"].Value.ToString();

                // Load image into PictureBox
                if (dgvITEMLIST.Rows[e.RowIndex].Cells["ItemImage"].Value is byte[] imageBytes)
                {
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        pbIMAGE.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pbIMAGE.Image = null;
                }
            }
        }

        private void tbSEARCHITEMNAME_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = tbSEARCHITEMNAME.Text;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, ItemName, ItemCategory, ItemPrice, ItemStock FROM Items WHERE ItemName LIKE @search OR ItemCategory LIKE @search";
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dgvITEMLIST.DataSource = table; // Bind data to DataGridView
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnLOGOUT_Click(object sender, EventArgs e)
        {
            ADMINLOGOUT loginForm = new ADMINLOGOUT();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void btnITEMIMAGE_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pbIMAGE.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            TRANSACTION loginForm = new TRANSACTION();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CASHIER loginForm = new CASHIER();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }
    }
    }
    

