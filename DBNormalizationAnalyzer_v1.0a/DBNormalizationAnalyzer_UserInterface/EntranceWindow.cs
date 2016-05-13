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
            switch (((sender as Glass.GlassButton).Tag as string))
            {
                case "Open":
                    try {
                        EditorForm _editForm = new EditorForm(DataManager.ReadProject(listBox1.SelectedValue as string));
                        _editForm.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured, couldn't load project");
                    }
                    break;
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
