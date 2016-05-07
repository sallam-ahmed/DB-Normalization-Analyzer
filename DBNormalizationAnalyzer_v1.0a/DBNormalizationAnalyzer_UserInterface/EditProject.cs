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
    public partial class EditProject : Form
    {
        private Project _project { get; set; }
        public EditProject(Project _currentProject)
        {
            InitializeComponent();
            this.projectNameTextBox.Text = _currentProject.ProjectName;
            authorNameTextBox.Text = _currentProject.ProjectAuthor;
            descriptionTextBox.Text = _currentProject.ProjectDescription;
            _project = _currentProject;
        }

        private void createProject_Click(object sender, EventArgs e)
        {
            if(!CheckFields())
            {
                MessageBox.Show("Please fill in values", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                projectNameTextBox.Focus();
            }
            else
            {
                _project.ProjectName = projectNameTextBox.Text;
                _project.ProjectAuthor = authorNameTextBox.Text;
                _project.ProjectDescription = descriptionTextBox.Text;
                MessageBox.Show("Done","Project Edit",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
            }
        }
        private bool CheckFields()
        {
            return !(string.IsNullOrWhiteSpace(projectNameTextBox.Text) || string.IsNullOrWhiteSpace(authorNameTextBox.Text));
        }
    }
}
