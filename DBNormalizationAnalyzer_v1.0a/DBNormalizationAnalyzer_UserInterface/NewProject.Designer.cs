namespace DBNormalizationAnalyzer_UserInterface
{
    partial class NewProject
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.projectNameTextBox = new System.Windows.Forms.TextBox();
            this.authorNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.glassButton1 = new Glass.GlassButton();
            this.glassButton3 = new Glass.GlassButton();
            this.glassButton2 = new Glass.GlassButton();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.glassButton2);
            this.groupBox1.Controls.Add(this.glassButton3);
            this.groupBox1.Controls.Add(this.glassButton1);
            this.groupBox1.Controls.Add(this.filePathTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.authorNameTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.projectNameTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(564, 348);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project Information";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project Name :";
            // 
            // projectNameTextBox
            // 
            this.projectNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectNameTextBox.Location = new System.Drawing.Point(20, 54);
            this.projectNameTextBox.Name = "projectNameTextBox";
            this.projectNameTextBox.Size = new System.Drawing.Size(532, 31);
            this.projectNameTextBox.TabIndex = 2;
            this.projectNameTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // authorNameTextBox
            // 
            this.authorNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.authorNameTextBox.Location = new System.Drawing.Point(20, 131);
            this.authorNameTextBox.Name = "authorNameTextBox";
            this.authorNameTextBox.Size = new System.Drawing.Size(532, 31);
            this.authorNameTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Author:";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePathTextBox.Location = new System.Drawing.Point(20, 223);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.ReadOnly = true;
            this.filePathTextBox.Size = new System.Drawing.Size(367, 31);
            this.filePathTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 197);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Save Location:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 323);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(564, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(99, 20);
            this.toolStripStatusLabel1.Text = "DATA /  TIME";
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "dpn";
            this.saveDialog.FileName = "NewProject";
            // 
            // glassButton1
            // 
            this.glassButton1.Location = new System.Drawing.Point(393, 221);
            this.glassButton1.Name = "glassButton1";
            this.glassButton1.Size = new System.Drawing.Size(159, 37);
            this.glassButton1.TabIndex = 10;
            this.glassButton1.Text = "Choose";
            // 
            // glassButton3
            // 
            this.glassButton3.Location = new System.Drawing.Point(12, 273);
            this.glassButton3.Name = "glassButton3";
            this.glassButton3.Size = new System.Drawing.Size(209, 40);
            this.glassButton3.TabIndex = 12;
            this.glassButton3.Text = "Create Project";
            // 
            // glassButton2
            // 
            this.glassButton2.Location = new System.Drawing.Point(343, 273);
            this.glassButton2.Name = "glassButton2";
            this.glassButton2.Size = new System.Drawing.Size(209, 40);
            this.glassButton2.TabIndex = 13;
            this.glassButton2.Text = "Create Project";
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 348);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "NewProject";
            this.Opacity = 0.95D;
            this.Text = "Create New Project*";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox projectNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox authorNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Glass.GlassButton glassButton2;
        private Glass.GlassButton glassButton3;
        private Glass.GlassButton glassButton1;
        private System.Windows.Forms.SaveFileDialog saveDialog;
    }
}