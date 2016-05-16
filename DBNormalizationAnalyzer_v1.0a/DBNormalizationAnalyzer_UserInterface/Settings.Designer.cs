namespace DBNormalizationAnalyzer_UserInterface
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.glassButton2 = new Glass.GlassButton();
            this.glassButton3 = new Glass.GlassButton();
            this.glassButton1 = new Glass.GlassButton();
            this.glassButton4 = new Glass.GlassButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sample Font:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Items.AddRange(new object[] {
            "Table 1",
            "Table 2",
            "Table 3"});
            this.listBox1.Location = new System.Drawing.Point(14, 37);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(308, 116);
            this.listBox1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(328, 37);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(193, 245);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 297);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(102, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Ask on exit.";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // glassButton2
            // 
            this.glassButton2.Location = new System.Drawing.Point(12, 159);
            this.glassButton2.Name = "glassButton2";
            this.glassButton2.Size = new System.Drawing.Size(310, 37);
            this.glassButton2.TabIndex = 6;
            this.glassButton2.Tag = "Grid";
            this.glassButton2.Text = "Change Grid Font";
            this.glassButton2.Click += new System.EventHandler(this.ChangeFont);
            // 
            // glassButton3
            // 
            this.glassButton3.Location = new System.Drawing.Point(12, 202);
            this.glassButton3.Name = "glassButton3";
            this.glassButton3.Size = new System.Drawing.Size(310, 37);
            this.glassButton3.TabIndex = 7;
            this.glassButton3.Tag = "Terminal";
            this.glassButton3.Text = "Change Terminal Font";
            this.glassButton3.Click += new System.EventHandler(this.ChangeFont);
            // 
            // glassButton1
            // 
            this.glassButton1.Location = new System.Drawing.Point(12, 245);
            this.glassButton1.Name = "glassButton1";
            this.glassButton1.Size = new System.Drawing.Size(310, 37);
            this.glassButton1.TabIndex = 8;
            this.glassButton1.Tag = "Tables";
            this.glassButton1.Text = "Change Tables and Columns Font";
            this.glassButton1.Click += new System.EventHandler(this.ChangeFont);
            // 
            // glassButton4
            // 
            this.glassButton4.Location = new System.Drawing.Point(135, 288);
            this.glassButton4.Name = "glassButton4";
            this.glassButton4.Size = new System.Drawing.Size(380, 37);
            this.glassButton4.TabIndex = 9;
            this.glassButton4.Text = "Exit";
            this.glassButton4.Click += new System.EventHandler(this.glassButton4_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 334);
            this.Controls.Add(this.glassButton4);
            this.Controls.Add(this.glassButton1);
            this.Controls.Add(this.glassButton3);
            this.Controls.Add(this.glassButton2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private Glass.GlassButton glassButton2;
        private Glass.GlassButton glassButton3;
        private Glass.GlassButton glassButton1;
        private Glass.GlassButton glassButton4;
    }
}