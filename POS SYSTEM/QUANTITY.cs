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
    public partial class QUANTITY : Form
    {
        public string ItemName { get; internal set; }
        public decimal ItemPrice { get; internal set; }
        public int Quantity { get; set; } = 0;

        // Event to pass the order details back to the USER form
        public event Action<string, decimal, int> OnOrderAdded;

        public QUANTITY()
        {
            InitializeComponent();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            OnOrderAdded?.Invoke(ItemName, ItemPrice, Quantity);

            // Close the form after the order is added
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Quantity = 0;
            this.Close();
    
    // Open the User Dashboard form (or any form you'd like to navigate to)
    USER userDashboardForm = new USER();
    userDashboardForm.Show();
        }

        private void QUANTITY_Load(object sender, EventArgs e)
        {
            lblItemName.Text = ItemName;
            lblItemPrice.Text = "Price: ₱" + ItemPrice.ToString("F2");
            textBox1.Text = Quantity.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Quantity++;
            textBox1.Text = Quantity.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Quantity > 1)
            {
                Quantity--;
            }
            textBox1.Text = Quantity.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Quantity+= 2;
            textBox1.Text = Quantity.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Quantity+= 3;
            textBox1.Text = Quantity.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Quantity += 5;
            textBox1.Text = Quantity.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Quantity += 10;
        textBox1.Text = Quantity.ToString();
        }
    }
}
