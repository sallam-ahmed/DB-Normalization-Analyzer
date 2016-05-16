namespace DBNormalizationAnalyzer_UserInterface
{
    partial class SplashScreen
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
            this.components = new System.ComponentModel.Container();
            this.FadeInTimer = new System.Windows.Forms.Timer(this.components);
            this.FadeOutTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // FadeInTimer
            // 
            this.FadeInTimer.Enabled = true;
            this.FadeInTimer.Interval = 25;
            this.FadeInTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FadeOutTimer
            // 
            this.FadeOutTimer.Interval = 25;
            this.FadeOutTimer.Tick += new System.EventHandler(this.FadeOutTimer_Tick);
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DBNormalizationAnalyzer_UserInterface.Properties.Resources.ApplicationLogo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1161, 768);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer FadeInTimer;
        private System.Windows.Forms.Timer FadeOutTimer;
    }
}