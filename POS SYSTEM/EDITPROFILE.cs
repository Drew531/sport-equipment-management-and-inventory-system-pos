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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace POS_SYSTEM
{
    public partial class EDITPROFILE : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb;";
        private string currentUsername;  // Store the logged-in username
        private string username;


     


        public EDITPROFILE()
        {
            InitializeComponent();
            currentUsername = username;
        }

        private void label11_Click(object sender, EventArgs e)
        {
            USERPROFILE loginForm = new USERPROFILE();
            loginForm.Owner = this; 
            loginForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            USERPROFILE loginForm = new USERPROFILE();
            loginForm.Owner = this;
            loginForm.Show();
            this.Hide();
        }

        private void EDITPROFILE_Load(object sender, EventArgs e)
        {

            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM useracc WHERE USERNAME = ?";  // Using parameterized query
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Add the parameter with the correct value (the currentUsername)
                        command.Parameters.AddWithValue("?", currentUsername);

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the textboxes with the user's data from the database
                                tbFIRSTNAME.Text = reader["FIRSTNAME"].ToString();
                                tbMIDDLENAME.Text = reader["MIDDLENAME"].ToString();
                                tbLASTNAME.Text = reader["LASTNAME"].ToString();
                                tbEMAIL.Text = reader["EMAIL"].ToString();
                                dtpBIRTHDATE.Value = Convert.ToDateTime(reader["BIRTHDATE"]);
                                tbUSERNAME.Text = reader["USERNAME"].ToString();
                                tbPASSWORD.Text = reader["PASSWORD"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserData()
        {
           
              
            }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUPDATE_Click(object sender, EventArgs e)
        {
            string firstName = tbFIRSTNAME.Text;
            string middleName = tbMIDDLENAME.Text;
            string lastName = tbLASTNAME.Text;
            string email = tbEMAIL.Text;
            string password = tbPASSWORD.Text;
            DateTime birthDate = dtpBIRTHDATE.Value;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill out all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE useracc SET FIRSTNAME = ?, MIDDLENAME = ?, LASTNAME = ?, EMAIL = ?, BIRTHDATE = ?, PASSWORD = ? WHERE USERNAME = ?";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Add all parameters correctly
                        command.Parameters.AddWithValue("?", firstName);
                        command.Parameters.AddWithValue("?", middleName);
                        command.Parameters.AddWithValue("?", lastName);
                        command.Parameters.AddWithValue("?", email);
                        command.Parameters.AddWithValue("?", birthDate);
                        command.Parameters.AddWithValue("?", password);
                        command.Parameters.AddWithValue("?", currentUsername);  // Correctly add the current username

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to update profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSHOWPASS.Checked)
            {
                tbPASSWORD.UseSystemPasswordChar = false;
            }
            else
            {
                tbPASSWORD.UseSystemPasswordChar = true;
            }
        }
    }
}
