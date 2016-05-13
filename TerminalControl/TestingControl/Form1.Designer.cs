namespace TestingControl
{
    partial class Form1
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
            this.terminalControl1 = new TerminalControl.TerminalControl();
            this.SuspendLayout();
            // 
            // terminalControl1
            // 
            this.terminalControl1.BackColor = System.Drawing.SystemColors.Window;
            this.terminalControl1.Delimiters = new string[] {
        " "};
            this.terminalControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terminalControl1.Location = new System.Drawing.Point(0, 0);
            this.terminalControl1.Margin = new System.Windows.Forms.Padding(5);
            this.terminalControl1.MessageColor = System.Drawing.SystemColors.ControlText;
            this.terminalControl1.Name = "terminalControl1";
            this.terminalControl1.PromptColor = System.Drawing.SystemColors.ControlText;
            this.terminalControl1.Size = new System.Drawing.Size(573, 321);
            this.terminalControl1.TabIndex = 0;
            this.terminalControl1.Command += new TerminalControl.TerminalControl.CommandEventHandler(this.terminalControl1_Command);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 321);
            this.Controls.Add(this.terminalControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TerminalControl.TerminalControl terminalControl1;
    }
}

