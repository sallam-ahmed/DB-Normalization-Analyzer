using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DBNormalizationAnalyzer.Formations;
using DBNormalizationAnalyzer.AnalyzerLibrary;
using DBNormalizationAnalyzer.PresistentDataManager;
using System.ComponentModel;
using System.Drawing;

namespace DBNormalizationAnalyzer_UserInterface
{

    public partial class EditorForm : Form
    {
        #region Variables
        public static bool bHasChanges; // for saving
        private Table m_currentTable;
        private Project CurrentProject { get; set; }
        private Dictionary<Table, FunctionalDependency> tableMap;
        BindingSource Bs;
        Project MockProject;
        #endregion

        #region # Constructors #
        //TODO: Load project data into controls
        public EditorForm(Project _project)
        {
            InitializeComponent();
            LoadProject(_project);
            m_currentTable = (_project.Tables.Count != 0) ? _project.Tables[0] : null;
            ClearCLI();
            ResetLog();
        }
        #endregion


        #region # Project Manager #
        //TODO FINALIZE LOADING AS MULTITHREADED TECHNIQUE
        private void LoadProject(Project _project)
        {
            CurrentProject = _project;
            tablesListBox.DataSource = null;
            tablesListBox.ValueMember = "Self";
            tablesListBox.DisplayMember = "Name";
            tablesListBox.DataSource = _project.Tables;

            try
            { columnsListBox.DataSource = m_currentTable.Columns; }
            catch { }
            LogText("Loaded project tables with count of " + _project.Tables.Count.ToString() + " projects.");
            this.Text = "DB Normalization Analyzer - " + CurrentProject.ProjectName+ " -";
            ClearCLI();
        }
        private void PerformSaveActions(bool prompt)
        {
            if (prompt)
            {
                //Implement Save Auction
                DialogResult Res = MessageBox.Show("Save current project changes?", "Save project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Res == DialogResult.Yes)
                {
                    DataManager.CreateProject(CurrentProject);
                }
            }
            else
            {
                DataManager.CreateProject(CurrentProject);
            }
            bHasChanges = false;
        }
        #endregion
        #region # CLI - LOG #
        internal void ClearCLI()
        {
            //commandPanel.Text = "~" + CurrentProject.ProjectName + @"\\";
        }
        internal void ResetLog()
        {
            logTextBox.Clear();
            LogText("Log Started " + DateTime.Now.ToShortTimeString() + ".");
            LogText("Project: " + CurrentProject.ProjectName);
            LogText("Path: " + CurrentProject.ProjectPath);
            LogText("Welcome to DBNormalization Aanalyzer.");
        }
        void LogText(string value)
        {
            logTextBox.AppendText(CurrentProject.ProjectName + "~ "+DateTime.Now.ToShortTimeString() + " " + value+"\n");
        }
        #endregion

        private void Analyze()
        {
            if (m_currentTable == null)
            {
                LogText("Error selected table is null.");
                return;
            }

            var checker = new NfChecker(m_currentTable.TableDependency);
            var error = checker.Check();
            //COMPLETE
        }
        #region # UI ACTIONS #
        /// <summary>
        /// Controls all menu item actions
        /// </summary>
        private void PerformMenuItemsActions(object sender, EventArgs e)
        {
            switch ((sender as ToolStripMenuItem)?.Tag as string)
            {
                /*FILE*/
                case "New":
                    break;
                case "Save":
                    break;
                case "ExportPR":
                    break;
                case "Settings":
                    break;
                case "Exit":
                    //ShowExitMessage();
                    break;
                case "PropEdit":
                    ShowForm(new EditProject(CurrentProject), this, (object s, FormClosedEventArgs ce) => { this.Enabled = true; });
                    break;
                /*EDIT*/
                case "Cut":
                    break;
                case "Copy":
                    break;
                case "Paste":
                    break;
                case "TB_Insert":
                    break;
                case "COL_Insert":
                    break;
                case "FUN.DB_Insert":
                    break;
                case "RepGenerate":
                    break;
                /*VIEW*/
                case "Toolbar":
                    break;
                case "StatBar":
                    break;
                case "LogWindow":
                    break;
                /*HELP*/
                case "HowTO":
                    ShowForm(new TutorialDialog(), this, (object s, FormClosedEventArgs ce) => { this.Enabled = true; });
                    break;
                case "About":
                    ShowForm(new AboutUs(), this, (object s,FormClosedEventArgs ce) => { this.Enabled = true; });
                    break;
                case "License":
                    break;
            }
        }
        private void ShowForm(Form frm, Form owner, FormClosedEventHandler closeHandle)
        {
            frm.Owner = owner;
            frm.Show();
            frm.Activate();
            owner.Enabled = false;
            frm.FormClosed += new FormClosedEventHandler(closeHandle);
        }

        private void ShowExitMessage()
        {
            if (MessageBox.Show("Would you like to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (bHasChanges)
                   PerformSaveActions(false);

                Application.Exit();
            }
        }
        private void ShowExitMessage(FormClosingEventArgs e)
        {
            if (MessageBox.Show("Would you like to exit ?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
            
                Application.Exit();
           
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        private void tablesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (tablesListBox.SelectedValue != null)
                {
                    columnsListBox.DataSource = new BindingList<Column>((tablesListBox.SelectedValue as Table).Columns);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
            
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show((tablesListBox.SelectedValue as Table).ColumnsCount.ToString());

        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PerformSaveActions(true);
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            datetimeLBL.Text = DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToShortTimeString();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch ((sender as Control).Tag as string)
            {
                case "Analyze":
                    Analyze();
                    break;
                case "Export":

                    break;
                case "RefTable":
                    LoadProject(CurrentProject);
                    break;
                case "RefFunD":
                    LoadFunctionalDependencies(CurrentProject);
                    
                    break;
            }
        }
    
        private void LoadFunctionalDependencies(Project currentProject)
        {
            tableMap = new Dictionary<Table, FunctionalDependency>(currentProject.Tables.Count);
            foreach (var item in currentProject.Tables) //MAP EACH TABLE TO ITS FUNCTIONAL DEPENDENCY.
            {
                tableMap.Add(item, item.TableDependency);
            }
        }

        
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {          
          
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            DialogResult Res = loadfileDialog.ShowDialog(this);
            if (Res != DialogResult.Cancel)
            {
                PerformSaveActions(true);
                Program.LoadedProject = DataManager.ReadProject(loadfileDialog.FileName);
                CurrentProject = Program.LoadedProject;
                LoadProject(CurrentProject);
            }
            else
            {
                MessageBox.Show("Test");
            }
        }

        private void createTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.LoadedProject.Tables.Add(new Table("NEW TABLE", 1));
            Program.LoadedProject.Tables[0].Columns[0].Name = "NEW COLUMN";
            bHasChanges = true;
            LoadProject(Program.LoadedProject);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            InitializeTerminalControl();
        }
        private void InitializeTerminalControl()
        {
            terminalControl1.Delimiters = new string[] { " ", "->" };
            terminalControl1.AutoCompleteAdd("analyze"); // Aalysis Process
            terminalControl1.AutoCompleteAdd("show_depen");//Show current dependencies
            terminalControl1.AutoCompleteAdd("export");//Export to PDF
            terminalControl1.AutoCompleteAdd("help"); // Show help
            terminalControl1.AutoCompleteAdd("add");// Add dependency
            terminalControl1.AutoCompleteAdd("rem"); // Remove dependency
            terminalControl1.AutoCompleteAdd("exit"); // Exit Application
            terminalControl1.AutoCompleteAdd("tbchng"); // Change active table
            terminalControl1.PromptString = CurrentProject.ProjectName + ((m_currentTable != null)? m_currentTable.Name : "") + "~\\";
            terminalControl1.AutoComplete = AutoCompleteMode.Append;
            terminalControl1.PromptColor = Color.Blue;
            terminalControl1.ForeColor = Color.Green;
            terminalControl1.MessageColor = Color.Red;
            terminalControl1.BackColor = Color.Black;
        }

        private void terminalControl1_Load(object sender, EventArgs e)
        {

        }

        private void terminalControl1_Command(object sender, TerminalControl.CommandEventArgs e)
        {
            switch (e.Command)
            {
                case "help":
                    e.Message = "Showing help";
                    break;
                case "analyze":
                    Analyze();
                    break;
                case "show_depen":
                    e.Message = "FUNCTION DPENDECIES\n1-\n2-";
                    break;
                case "add":
                    foreach (var item in e.Parameters)
                    {
                        LogText("PARAM " + item);
                    }
                    break;
                case "rem":
                    break;
                case "exit":
                    break;
                case "tbchng":
                    break;
                default:
                    break;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            DataManager.CreateProject(CurrentProject);
        }

        private void columnsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            DialogResult r = saveFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                Project _new = CurrentProject;
                _new.ProjectPath = saveFileDialog1.FileName;
                DataManager.CreateProject(_new);
            }
        }
    }
    
}
