using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_SYSTEM
{
    public partial class USERPROFILE : Form
    {
        public USERPROFILE()
        {
            InitializeComponent();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            USER loginForm = new USER();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EDITPROFILE loginForm = new EDITPROFILE();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LOGOUT loginForm = new LOGOUT();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }
    }
}
