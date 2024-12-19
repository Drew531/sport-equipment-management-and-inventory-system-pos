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
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;


namespace POS_SYSTEM
{
    public partial class USER : Form
    {
        private decimal grandTotal = 0;

        private void SetupDataGridView()


        {
            // Clear any existing columns
            dgvORDERS.Columns.Clear();

            // Add columns to DataGridView
            dgvORDERS.Columns.Add("ItemName", "Item Name");
            dgvORDERS.Columns.Add("ItemPrice", "Price");
            dgvORDERS.Columns.Add("Quantity", "Quantity");
            dgvORDERS.Columns.Add("Total", "Total");
        }

        // This method is called when you need to add an order to the DataGridView
        private void AddOrderToDataGridView(string itemName, decimal itemPrice, int quantity)
        {
            // Calculate the total price (itemPrice * quantity)
            decimal total = itemPrice * quantity;

            // Add the order to the DataGridView with Peso format
            dgvORDERS.Rows.Add(itemName, itemPrice.ToString("₱#,##0.00"), quantity, total.ToString("₱#,##0.00"));
        }

        public USER()
        {
            InitializeComponent();

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void USER_Load(object sender, EventArgs e)
        {
            LoadItems();
            SetupDataGridView();
        }

        private void LoadItems()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb;"))
                {
                    connection.Open();
                    string query = "SELECT ID, ItemName, ItemCategory, ItemPrice, ItemStock, ItemImage FROM Items";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    System.Data.DataTable itemsTable = new System.Data.DataTable();
                    adapter.Fill(itemsTable);

                    // Clear existing items before adding new ones
                    flpLIST.Controls.Clear();

                    flpLIST.FlowDirection = FlowDirection.LeftToRight;
                    flpLIST.WrapContents = true;
                    flpLIST.AutoScroll = true;

                    foreach (DataRow row in itemsTable.Rows)
                    {
                        Panel itemPanel = new Panel
                        {
                            Width = 240,
                            Height = 230,
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        byte[] imageBytes = row["ItemImage"] as byte[];
                        Image itemImage = null;
                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                itemImage = Image.FromStream(ms);
                            }
                        }

                        PictureBox pbItemImage = new PictureBox
                        {
                            Width = 90,
                            Height = 90,
                            Image = itemImage,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Location = new System.Drawing.Point(75, 10)
                        };

                        Label lblItemName = new Label
                        {
                            Text = row["ItemName"].ToString(),
                            Location = new System.Drawing.Point(10, 120),
                            Width = 220,
                            Height = 20,
                            TextAlign = ContentAlignment.MiddleCenter
                        };

                        Label lblItemCategory = new Label
                        {
                            Text = "Category: " + row["ItemCategory"].ToString(),
                            Location = new System.Drawing.Point(10, 150),
                            Width = 220,
                            Height = 20,
                            TextAlign = ContentAlignment.MiddleCenter
                        };

                        Label lblItemPrice = new Label
                        {
                            Text = "Price: ₱" + row["ItemPrice"].ToString(),
                            Location = new System.Drawing.Point(10, 180),
                            Width = 220,
                            Height = 20,
                            TextAlign = ContentAlignment.MiddleCenter
                        };

                        Label lblItemStock = new Label
                        {
                            Text = "Stock: " + row["ItemStock"].ToString(),
                            Location = new System.Drawing.Point(10, 210),
                            Width = 220,
                            Height = 20,
                            TextAlign = ContentAlignment.MiddleCenter
                        };

                        itemPanel.Controls.Add(pbItemImage);
                        itemPanel.Controls.Add(lblItemName);
                        itemPanel.Controls.Add(lblItemCategory);
                        itemPanel.Controls.Add(lblItemPrice);
                        itemPanel.Controls.Add(lblItemStock);

                        // Attach click event to add the item to the order
                        itemPanel.Click += (sender, e) =>
                        {
                            string itemName = row["ItemName"].ToString();
                            decimal itemPrice = Convert.ToDecimal(row["ItemPrice"]);

                            QUANTITY quantityForm = new QUANTITY
                            {
                                ItemName = itemName,
                                ItemPrice = itemPrice
                            };

                            quantityForm.FormClosed += (s, args) =>
                            {
                                AddOrderToDataGridView(itemName, itemPrice, quantityForm.Quantity);
                            };

                            quantityForm.Show();
                        };

                        flpLIST.Controls.Add(itemPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            {
                grandTotal = 0;

                // Loop through each row in the order DataGridView to calculate the grand total
                foreach (DataGridViewRow row in dgvORDERS.Rows)
                {
                    if (row.Cells["Total"].Value != null)
                    {
                        grandTotal += Convert.ToDecimal(row.Cells["Total"].Value.ToString().Replace("₱", "").Replace(",", ""));
                    }
                }

                // Show the grand total in a message box
                MessageBox.Show("Grand Total: ₱" + grandTotal.ToString("₱#,##0.00"));

                // Call the method to print the receipt to Word
                PrintReceiptToWord();


                // Update stock in the database for each item ordered
                UpdateStockInDatabase();

                // Refresh the cashier dashboard to reflect updated stock
                RefreshCashierDashboard();


            }

            // Method to update stock in the database
 
        }

        private void UpdateStockInDatabase()
        {
            foreach (DataGridViewRow row in dgvORDERS.Rows)
            {
                if (row.Cells["ItemName"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    string itemName = row.Cells["ItemName"].Value.ToString();
                    int quantityOrdered = Convert.ToInt32(row.Cells["Quantity"].Value);

                    // Update the stock in the database
                    try
                    {
                        // Open a connection
                        using (var conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb;"))
                        {
                            conn.Open();

                            // Update query based on correct column names
                            string updateQuery = "UPDATE Items SET ItemStock = ItemStock - ? WHERE ItemName = ?";
                            using (var cmd = new OleDbCommand(updateQuery, conn))
                            {
                                // Adding parameters for quantity and item name
                                cmd.Parameters.AddWithValue("?", quantityOrdered);
                                cmd.Parameters.AddWithValue("?", itemName);

                                // Debug: Log the SQL query and parameters for troubleshooting
                                Console.WriteLine($"Executing query: {cmd.CommandText}");
                                foreach (OleDbParameter param in cmd.Parameters)
                                {
                                    Console.WriteLine($"Parameter: {param.ParameterName}, Value: {param.Value}");
                                }

                                // Execute the query and check if any rows were affected
                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected == 0)
                                {
                                    MessageBox.Show($"No item found with the name {itemName}");
                                }
                                else
                                {
                                    Console.WriteLine($"{rowsAffected} rows updated for item {itemName}");
                                }
                            }
                        }
                    }
                    catch (OleDbException ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
        }


        private void RefreshCashierDashboard()
        {
            flpLIST.Controls.Clear(); // Clear existing items in the FlowLayoutPanel

            // Fetch updated stock from the database
            try
            {
                using (var conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=register.accdb;"))
                {
                    conn.Open();
                    string selectQuery = "SELECT ItemName, ItemPrice, ItemStock FROM Items"; // Updated query to reflect correct column names
                    using (var cmd = new OleDbCommand(selectQuery, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string itemName = reader["ItemName"].ToString();
                                decimal price = Convert.ToDecimal(reader["ItemPrice"]);
                                int stock = Convert.ToInt32(reader["ItemStock"]);

                                // Create a new label to display the item information
                                Label itemLabel = new Label();
                                itemLabel.Text = $"{itemName} - ₱{price.ToString("₱#,##0.00")} - Stock: {stock}";
                                itemLabel.Width = flpLIST.Width - 20; // Set label width to fit the panel
                                itemLabel.AutoSize = true;

                                // Add the label to the FlowLayoutPanel
                                flpLIST.Controls.Add(itemLabel);
                            }
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void PrintReceiptToWord()
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document doc = wordApp.Documents.Add();

            // Set the default document background color to white and text to black
            doc.Content.Font.Color = WdColor.wdColorBlack;
            doc.Background.Fill.ForeColor.RGB = (int)WdColor.wdColorWhite;

            // Add a Header with a Title
            foreach (Section section in doc.Sections)
            {
                HeaderFooter header = section.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary];
                header.Range.Text = "SPORTS EQUIPMENT MANAGEMENT AND INVENTORY POS SYSTEM";
                header.Range.Font.Size = 16;
                header.Range.Font.Bold = 1;
                header.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            }

            // Add Title
            Paragraph titlePara = doc.Content.Paragraphs.Add();
            titlePara.Range.Text = "Order Receipt";
            titlePara.Range.Font.Size = 18;
            titlePara.Range.Font.Bold = 1;
            titlePara.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            titlePara.Range.InsertParagraphAfter();

            // Add Table for Receipt Items
            Paragraph tablePara = doc.Content.Paragraphs.Add();
            Table table = doc.Tables.Add(tablePara.Range, dgvORDERS.Rows.Count + 1, 4);
            table.Borders.Enable = 1; // Enable table borders
            table.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Font.Color = WdColor.wdColorBlack;

            // Header Row Styling
            table.Rows[1].Range.Font.Bold = 1;

            table.Cell(1, 1).Range.Text = "Item Name";
            table.Cell(1, 2).Range.Text = "Price";
            table.Cell(1, 3).Range.Text = "Quantity";
            table.Cell(1, 4).Range.Text = "Total";

            // Populate Table Rows
            for (int i = 0; i < dgvORDERS.Rows.Count - 1; i++)
            {
                table.Cell(i + 2, 1).Range.Text = dgvORDERS.Rows[i].Cells["ItemName"].Value.ToString();
                table.Cell(i + 2, 2).Range.Text = dgvORDERS.Rows[i].Cells["ItemPrice"].Value.ToString();
                table.Cell(i + 2, 3).Range.Text = dgvORDERS.Rows[i].Cells["Quantity"].Value.ToString();
                table.Cell(i + 2, 4).Range.Text = dgvORDERS.Rows[i].Cells["Total"].Value.ToString();
            }

            // Add Grand Total at the bottom
            Paragraph totalPara = doc.Content.Paragraphs.Add();
            totalPara.Range.Text = $"\nGrand Total: ₱{grandTotal.ToString("₱#,##0.00")}";
            totalPara.Range.Font.Size = 14;
            totalPara.Range.Font.Bold = 1;
            totalPara.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            totalPara.Range.InsertParagraphAfter();

            // Make the document visible
            wordApp.Visible = true;
        }








        private void button6_Click(object sender, EventArgs e)
        {
            dgvORDERS.Rows.Clear();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            USERPROFILE loginForm = new USERPROFILE();
            loginForm.Owner = this; // Set the current form as the owner
            loginForm.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
           
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flpLIST_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flpORDER_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
