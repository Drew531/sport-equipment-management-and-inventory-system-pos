namespace POS_SYSTEM
{
    partial class ADMINLOGOUT
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCONFIRM = new System.Windows.Forms.Button();
            this.btnCANCEL = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCONFIRM
            // 
            this.btnCONFIRM.BackColor = System.Drawing.Color.Maroon;
            this.btnCONFIRM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCONFIRM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCONFIRM.ForeColor = System.Drawing.Color.White;
            this.btnCONFIRM.Location = new System.Drawing.Point(53, 91);
            this.btnCONFIRM.Name = "btnCONFIRM";
            this.btnCONFIRM.Size = new System.Drawing.Size(98, 33);
            this.btnCONFIRM.TabIndex = 0;
            this.btnCONFIRM.Text = "Log out";
            this.btnCONFIRM.UseVisualStyleBackColor = false;
            this.btnCONFIRM.Click += new System.EventHandler(this.btnCONFIRM_Click);
            // 
            // btnCANCEL
            // 
            this.btnCANCEL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCANCEL.ForeColor = System.Drawing.Color.Maroon;
            this.btnCANCEL.Location = new System.Drawing.Point(157, 91);
            this.btnCANCEL.Name = "btnCANCEL";
            this.btnCANCEL.Size = new System.Drawing.Size(98, 33);
            this.btnCANCEL.TabIndex = 1;
            this.btnCANCEL.Text = "Cancel";
            this.btnCANCEL.UseVisualStyleBackColor = true;
            this.btnCANCEL.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log out of your account?";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ADMINLOGOUT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCANCEL);
            this.Controls.Add(this.btnCONFIRM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ADMINLOGOUT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ADMINLOGOUT";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCONFIRM;
        private System.Windows.Forms.Button btnCANCEL;
        private System.Windows.Forms.Label label1;
    }
}