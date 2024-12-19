using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_SYSTEM
{
    public partial class RECIEPT : Form
    {
        private PrintDocument printDocument = new PrintDocument();
        public ListView.ListViewItemCollection ReceiptItems { get; set; }
        public decimal GrandTotal { get; set; }
        public PrintPageEventHandler PrintDocument_PrintPage { get; private set; }

        public RECIEPT()
        {
            InitializeComponent();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RECIEPT_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            USER loginForm = new USER();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }
    }
}
