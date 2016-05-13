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
                case "Open":
                    try
                    {
                        Program.LoadedProject = DataManager.ReadProject(listBox1.SelectedValue as string);
                        var editForm = new EditorForm(Program.LoadedProject);
                        editForm.Show();
                        this.Hide();
                    }
                    catch
                    {
                        MessageBox.Show("An error occured, couldn't load project");
                    }
                    break;
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
                    var credits = new AboutUs();
                    credits.Closed += (s, args) => Show();
                    credits.Show();
                    Hide();
                    break;
                case "Exit":
                    var res = MessageBox.Show("Are your sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this project from the refrences menu ?","Confirm delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                
            }
        }
    }
}
