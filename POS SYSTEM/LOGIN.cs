using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace POS_SYSTEM
{
    public partial class LOGIN : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb";

        public LOGIN()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            REGISTER registerForm = new REGISTER();

          
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
                    string query = "SELECT COUNT(*) FROM useracc WHERE [USERNAME] = ? AND [PASSWORD] = ?";
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
                            USER mainForm = new USER();
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

        private void btnCANCEL_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Exit the application
        }

        private void LOGIN_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ADMIN loginForm = new ADMIN();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void lbFORGOTPASS_Click(object sender, EventArgs e)
        {
            FORGOTPASSWORD loginForm = new FORGOTPASSWORD();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }
    }
}
    