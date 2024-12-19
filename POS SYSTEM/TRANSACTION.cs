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
    public partial class TRANSACTION : Form
    {
        public TRANSACTION()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ITEMS loginForm = new ITEMS();
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

        private void TRANSACTION_Load(object sender, EventArgs e)
        {

        }
    }
}
