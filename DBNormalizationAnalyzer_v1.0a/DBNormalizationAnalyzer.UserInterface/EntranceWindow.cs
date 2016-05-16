using System;
using System.Collections.Generic;
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

        private void OpenProject()
        {
            try
            {
                Program.LoadedProject = DataManager.ReadProject(listBox1.SelectedValue as string);
                var editForm = new EditorForm(Program.LoadedProject);
                editForm.Show();
                this.Hide();
            }
            catch
            {

                MessageBox.Show(@"An error occured, couldn't load project
Please consider removing this project from the recent projects list.");
            }
        }
        public void PerformButtonAction(object sender, EventArgs e)
        {
            switch ((sender as Glass.GlassButton)?.Tag as string)
            {
                case "Open":
                    OpenProject();
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
            if (listBox1.Items.Count == 0)
                openButton.Enabled = false;
            
        }


        private void EntranceWindow_Load(object sender, EventArgs e)
        {
            LoadRecentProjects();
        }
        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(@"Do you want to delete this project from the refrences menu ?", @"Confirm delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            var indexHolder = ((ProjectJson)(listBox1.Items[listBox1.SelectedIndex])).Path;

            if (MessageBox.Show(@"Would you like to delete the original source file?",@"Confirm source file removal.",MessageBoxButtons.YesNo) == DialogResult.Yes)
                if(System.IO.File.Exists(indexHolder))
                    System.IO.File.Delete(indexHolder);

            var recentProjectsList = new List<ProjectJson>();
            foreach (var item in listBox1.Items)
            {
                if(((ProjectJson)item).Path == indexHolder) // Pass the current val
                    continue;
                recentProjectsList.Add((ProjectJson)item);
            }
            DataManager.UpdateRecentProjects(recentProjectsList);
            LoadRecentProjects();
        }

        private void EntranceWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                OpenProject();
            }
        }
    }
}
