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
    public partial class FORGOTPASSWORD : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb";
        public FORGOTPASSWORD()
        {
            InitializeComponent();
        }

        private void FORGOTPASSWORD_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string username = tbUSERNAME.Text;
            string currentPassword = tbCURRENTPASS.Text;
            string newPassword = tbNEWPASS.Text;
            string confirmPassword = tbCONFIRMPASS.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(currentPassword) ||
                string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirmation password do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Verify current password for the given username
                    string query = "SELECT [PASSWORD] FROM useracc WHERE [USERNAME] = ? AND [PASSWORD] = ?";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", username);
                        command.Parameters.AddWithValue("?", currentPassword);
                        var result = command.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Invalid username or current password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Update the password
                    query = "UPDATE useracc SET [PASSWORD] = ? WHERE [USERNAME] = ?";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", newPassword);
                        command.Parameters.AddWithValue("?", username);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Redirect to LOGIN form after successful password reset
                        LOGIN loginForm = new LOGIN();
                        loginForm.Owner = this;
                        loginForm.Show();
                        this.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LOGIN loginForm = new LOGIN();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            LOGIN loginForm = new LOGIN();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            LOGIN loginForm = new LOGIN();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
                
        }
    }
}
