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
                var res = saveDialog.ShowDialog(this);
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
            var formInstance = new EditorForm(Program.LoadedProject);
            formInstance.Show();
            Hide();
        }

        private void NewProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowExitMessage(e);
        }
        private static void ShowExitMessage(FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
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
