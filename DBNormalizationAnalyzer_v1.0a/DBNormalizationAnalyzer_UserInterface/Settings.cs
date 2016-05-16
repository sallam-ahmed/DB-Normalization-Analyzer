using System;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace DBNormalizationAnalyzer_UserInterface
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Font = Properties.Settings.Default.AppFont;
            listBox1.Font = Properties.Settings.Default.AppFont;
            checkBox1.Checked = Properties.Settings.Default.AskOnExit;
        }

        private void glassButton1_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.AppFont = fontDialog1.Font;
            }
           
        }

        private void ChangeFont(object sender, EventArgs e)
        {
            var def = Properties.Settings.Default;
            Font fnt = new Font("",1.0f);
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                fnt = fontDialog1.Font;

            }
            else
            {
                return;
            }
           switch ((((Control) sender).Tag as string))
            {
                case "Grid":
                    def.GridFont = fnt;
                    break;
                case "Tables":
                    def.AppFont = fnt;
                    break;
                case "Terminal":
                    def.TerminalFont = fnt;
                    break;
            }
            textBox1.Font = def.TerminalFont;
            listBox1.Font = def.AppFont;
            def.Save();
        } 
           

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AskOnExit = checkBox1.Checked;
        }

        private void glassButton4_Click(object sender, EventArgs e)
        {
            this.Close();
            Properties.Settings.Default.Save();
        }
    }
}
