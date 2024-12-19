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
    public partial class CASHIER : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb"; // Database connection string


        public CASHIER()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TRANSACTION loginForm = new TRANSACTION();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ITEMS loginForm = new ITEMS();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void CASHIER_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT [FIRSTNAME], [MIDDLENAME], [LASTNAME], [EMAIL], [BIRTHDATE], [USERNAME], [IMAGE] FROM useracc";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Add an additional column to display images
                    dataTable.Columns.Add("DISPLAY_IMAGE", typeof(Image));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["IMAGE"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])row["IMAGE"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                row["DISPLAY_IMAGE"] = Image.FromStream(ms); // Convert byte array to Image for display
                            }
                        }
                        else
                        {
                            row["DISPLAY_IMAGE"] = null; // If no image is available
                        }
                    }

                    // Bind data to dgvCASHIER
                    dgvCASHIER.AutoGenerateColumns = false;
                    dgvCASHIER.DataSource = dataTable;

                    // Clear existing columns and add new ones
                    dgvCASHIER.Columns.Clear();

                    dgvCASHIER.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "FIRSTNAME",
                        HeaderText = "First Name"
                    });

                    dgvCASHIER.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "MIDDLENAME",
                        HeaderText = "Middle Name"
                    });

                    dgvCASHIER.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "LASTNAME",
                        HeaderText = "Last Name"
                    });

                    dgvCASHIER.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "EMAIL",
                        HeaderText = "Email"
                    });

                    dgvCASHIER.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "BIRTHDATE",
                        HeaderText = "Birthdate"
                    });

                    dgvCASHIER.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "USERNAME",
                        HeaderText = "Username"
                    });

                    DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
                    {
                        DataPropertyName = "DISPLAY_IMAGE",
                        HeaderText = "Image",
                        ImageLayout = DataGridViewImageCellLayout.Zoom // Adjust image display
                    };
                    dgvCASHIER.Columns.Add(imageColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // You may also need to set up the DataGridView in the designer
        private void SetupDataGridView()
        {
            // Add columns for user details
            dgvCASHIER.Columns.Add("FIRSTNAME", "First Name");
            dgvCASHIER.Columns.Add("MIDDLENAME", "Middle Name");
            dgvCASHIER.Columns.Add("LASTNAME", "Last Name");
            dgvCASHIER.Columns.Add("EMAIL", "Email");
            dgvCASHIER.Columns.Add("BIRTHDATE", "Birthdate");
            dgvCASHIER.Columns.Add("USERNAME", "Username");

            // Add an image column for user photo
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = "IMAGE";
            imageColumn.HeaderText = "Image";
            dgvCASHIER.Columns.Add(imageColumn);
        }
    }
}


    

