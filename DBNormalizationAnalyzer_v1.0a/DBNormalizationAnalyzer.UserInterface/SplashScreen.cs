using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBNormalizationAnalyzer_UserInterface
{
    public partial class SplashScreen : Form
    {
        private double i = 0;
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            i += 0.1;
            if (i >= 1)
            {//if form is fully visible, we execute the Fade Out Effect
                this.Opacity = 1;
                FadeInTimer.Enabled = false;//stop the Fade In Effect
                System.Threading.Thread.Sleep(2000);
                FadeOutTimer.Enabled = true;
                return;
            }
            this.Opacity = i;
            
        }
        private void FadeOutTimer_Tick(object sender, EventArgs e)
        {
            //Fade out effect
            i -= 0.05;
            if (i <= 0.01)
            {//if form is invisible, we execute the Fade In Effect again
                this.Opacity = 0.0;
                FadeOutTimer.Enabled = false;//stop the Fade Out Effect
                EntranceWindow frmi = new EntranceWindow();
                frmi.Show();
                this.Hide();
                return;
            }
            this.Opacity = i;
        }
    }
}
