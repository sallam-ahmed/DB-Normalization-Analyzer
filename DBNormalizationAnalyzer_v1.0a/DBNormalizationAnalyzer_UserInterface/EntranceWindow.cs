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
    public partial class EntranceWindow : Form
    {
        public EntranceWindow()
        {
            InitializeComponent();
        }
        public void PerformButtonAction(object sender, EventArgs e)
        {
            switch (((sender as Glass.GlassButton).Tag as string))
            {
                case "Create New":
                    NewProject _new = new NewProject();
                    _new.Show();
                    this.Hide();
                    break;
                case "Tutorial":
                    TutorialDialog _tut = new TutorialDialog();
                    _tut.Show();
                    this.Hide();
                    break;
                case "About":
                    AboutUs _credits = new AboutUs();
                    _credits.Show();
                    this.Hide();
                    break;
                case "Exit":
                    DialogResult _res = MessageBox.Show("Are your sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo);
                    if (_res == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    break;
            }
        }
        internal void LoadRecentProjects()
        {

        }
    }
}
