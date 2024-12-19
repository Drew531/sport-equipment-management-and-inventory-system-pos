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
    public partial class REGISTER : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb";

        public REGISTER()
        {
            InitializeComponent();
        }

        private void btnCANCEL_Click(object sender, EventArgs e)
        {
            tbFIRSTNAME.Clear();
            tbMIDDLENAME.Clear();
            tbLASTNAME.Clear();
            tbEMAIL.Clear();
            tbUSERNAME.Clear();
            tbPASSWORD.Clear();
            tbCONFIRMPASSWORD.Clear();
            dtpBIRTHDATE.Value = DateTime.Now;
            pbIMAGE.Image = null;
        }

        private void btnREGISTER_Click(object sender, EventArgs e)
        {
            // Check if any fields are empty or if image/birthdate is not selected
            // Check if any fields are empty or if image/birthdate is not selected
            if (string.IsNullOrEmpty(tbFIRSTNAME.Text) || string.IsNullOrEmpty(tbMIDDLENAME.Text) ||
                string.IsNullOrEmpty(tbLASTNAME.Text) || string.IsNullOrEmpty(tbEMAIL.Text) ||
                string.IsNullOrEmpty(tbUSERNAME.Text) || string.IsNullOrEmpty(tbPASSWORD.Text) ||
                string.IsNullOrEmpty(tbCONFIRMPASSWORD.Text) ||
                pbIMAGE.Image == null ||
                dtpBIRTHDATE.Value == DateTimePicker.MinimumDateTime)
            {
                MessageBox.Show("Please fill in all fields and select an image and birthdate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if passwords match
            if (tbPASSWORD.Text != tbCONFIRMPASSWORD.Text)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Convert the selected image to a byte array
            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                pbIMAGE.Image.Save(ms, pbIMAGE.Image.RawFormat);
                imageBytes = ms.ToArray();
            }

            // Insert user data into the database
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO useracc ([FIRSTNAME], [MIDDLENAME], [LASTNAME], [EMAIL], [BIRTHDATE], [IMAGE], [USERNAME], [PASSWORD]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", tbFIRSTNAME.Text);
                        command.Parameters.AddWithValue("?", tbMIDDLENAME.Text);
                        command.Parameters.AddWithValue("?", tbLASTNAME.Text);
                        command.Parameters.AddWithValue("?", tbEMAIL.Text);
                        command.Parameters.AddWithValue("?", dtpBIRTHDATE.Value);
                        command.Parameters.AddWithValue("?", imageBytes);
                        command.Parameters.AddWithValue("?", tbUSERNAME.Text);
                        command.Parameters.AddWithValue("?", tbPASSWORD.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSELECTIMAGE_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbIMAGE.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void cbSHOWPASSWORD_CheckedChanged(object sender, EventArgs e)
        {
            tbPASSWORD.UseSystemPasswordChar = !cbSHOWPASSWORD.Checked;
            tbCONFIRMPASSWORD.UseSystemPasswordChar = !cbSHOWPASSWORD.Checked;
        }

        private void REGISTER_Load(object sender, EventArgs e)
        {
           
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            LOGIN loginForm = new LOGIN();
            loginForm.Owner = this;
            loginForm.Show();
            this.Hide();
        }

        private void tbLASTNAME_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
