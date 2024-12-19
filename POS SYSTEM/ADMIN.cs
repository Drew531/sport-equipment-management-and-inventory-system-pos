using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_SYSTEM
{
    public partial class ADMIN : Form
    {

        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb";
        public ADMIN()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ADMINREGISTER registerForm = new ADMINREGISTER();


            registerForm.Show();


            this.Hide();
        }

        private void btnLOGIN_Click(object sender, EventArgs e)
        {
            string username = tbUSERNAME.Text.Trim();
            string password = tbPASSWORD.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Query to check user credentials
                    string query = "SELECT COUNT(*) FROM adminacc WHERE [USERNAME] = ? AND [PASSWORD] = ?";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", username);
                        command.Parameters.AddWithValue("?", password);

                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                        {
                            // Login successful
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Open main form and close login form
                            ITEMS mainForm = new ITEMS();
                            mainForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Invalid credentials
                            MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCASHIER_Click(object sender, EventArgs e)
        {
            LOGIN loginForm = new LOGIN();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }
    }
}
