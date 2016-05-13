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
using System.IO;

namespace DBNormalizationAnalyzer_UserInterface
{
    public partial class NewProject : Form
    {
        public NewProject()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void glassButton1_Click(object sender, EventArgs e)
        {
            if(!CheckFields())
            {
                MessageBox.Show("Please fill in values","Warning",MessageBoxButtons.OK,MessageBoxIcon.Error);
                projectNameTextBox.Focus();
            }
            else
            {
                DialogResult res = saveDialog.ShowDialog(this);
                if(res == DialogResult.OK)
                {
                    filePathTextBox.Text = saveDialog.FileName;
                    createProject.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Operation Canceled");
                }
                
            }
        }
        private bool CheckFields()
        {
            return !(string.IsNullOrWhiteSpace(projectNameTextBox.Text) || string.IsNullOrWhiteSpace(authorNameTextBox.Text));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLabel.Text = DateTime.Now.ToShortTimeString();
        }
        private void createProject_Click(object sender, EventArgs e)
        {
            Program.LoadedProject = new Project(projectNameTextBox.Text, authorNameTextBox.Text, saveDialog.FileName,descriptionTextBox.Text,DateTime.Now);
            DataManager.CreateProject(Program.LoadedProject);
            Program.LoadedProject.Tables = new List<DBNormalizationAnalyzer.Formations.Table>();
            Program.LoadedProject.Tables.Add(new DBNormalizationAnalyzer.Formations.Table("NEW_TABLE", 1));
            Program.LoadedProject.Tables[0].AddColumn(new DBNormalizationAnalyzer.Formations.Column("NEW_COL"));
            EditorForm _formInstance = new EditorForm(Program.LoadedProject);
            _formInstance.Show();
            this.Hide();
        }

        private void NewProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowExitMessage(e);
        }
        private void ShowExitMessage(FormClosingEventArgs e)
        {
            if (!(e.CloseReason == CloseReason.ApplicationExitCall))
            {

                if (MessageBox.Show("Would you like to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
