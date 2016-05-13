using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBNormalizationAnalyzer.PresistentDataManager;

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
            switch ((sender as Glass.GlassButton)?.Tag as string)
            {
                case "Create New":
                    var _new = new NewProject();
                    _new.Show();
                    Hide();
                    break;
                case "Tutorial":
                    var tut = new TutorialDialog();
                    tut.Closed += (s, args) => Show();
                    tut.Show();
                    Hide();
                    break;
                case "About":
                    AboutUs credits = new AboutUs();
                    credits.Closed += (s, args) => Show();
                    credits.Show();
                    Hide();
                    break;
                case "Exit":
                    var _res = MessageBox.Show("Are your sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo);
                    if (_res == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    break;
            }
        }
        internal void LoadRecentProjects()
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Path";

            listBox1.DataSource = DataManager.LoadRecentProjects();
        }

        private void EntranceWindow_Load(object sender, EventArgs e)
        {
            LoadRecentProjects();
        }
    }
}
