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
        public static bool BHasChanges; // for saving
        private string _commandBuilder = "";
        private Table _currentTable;
        private Project CurrentProject { get; set; }
        private Dictionary<Table, FunctionalDependency> _tableMap;
        BindingSource Bs;
        Project MockProject;
        #endregion

        #region # Constructors #
        //TODO: Load project data into controls
        public EditorForm(Project project)
        {
            InitializeComponent();
            LoadProject(project);
            _currentTable = project.Tables.Count != 0 ? project.Tables[0] : null;
            ClearCli();
            ResetLog();
        }
        #endregion


        #region # Project Manager #
        //TODO FINALIZE LOADING AS MULTITHREADED TECHNIQUE
        private void LoadProject(Project project)
        {
            CurrentProject = project;
            tablesListBox.DataSource = null;
            tablesListBox.ValueMember = "Self";
            tablesListBox.DisplayMember = "Name";
            tablesListBox.DataSource = project.Tables;

            try
            {
                if (project.Tables.Count > 0)
                {
                    tablesListBox.SetSelected(0,true);
                    ApplyTable(0);
                }
            }
            catch
            {
                // ignored
            }
            LogText("Loaded project tables with count of " + project.Tables.Count.ToString() + " projects.");
            Text = "DB Normalization Analyzer - " + CurrentProject.ProjectName+ " -";
            ClearCli();
        }
        private void PerformSaveActions(bool prompt)
        {
            if (prompt)
            {
                //Implement Save Auction
                var res = MessageBox.Show("Save current project changes?", "Save project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    DataManager.CreateProject(CurrentProject);
                }
            }
            else
            {
                DataManager.CreateProject(CurrentProject);
            }
            BHasChanges = false;
        }
        #endregion
        #region # CLI - LOG #
        internal void ClearCli()
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

        private void AnalyzeTable()
        {
            if (_currentTable == null)
            {
                LogText("Error selected table is null.");
                return;
            }

            var checker = new NfChecker(_currentTable.TableDependency);
            var error = checker.Check();
            MessageBox.Show("The table is in the " + (error.Level - 1) + " Normal Form\n" + error.Message);
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
                    ShowForm(new EditProject(CurrentProject), this, (s, ce) => { Enabled = true; });
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
                    ShowForm(new TutorialDialog(), this, (s, ce) => { Enabled = true; });
                    break;
                case "About":
                    ShowForm(new AboutUs(), this, (s, ce) => { Enabled = true; });
                    break;
                case "License":
                    break;
            }
        }
        private static void ShowForm(Form frm, Form owner, FormClosedEventHandler closeHandle)
        {
            frm.Owner = owner;
            frm.Show();
            frm.Activate();
            owner.Enabled = false;
            frm.FormClosed += closeHandle;
        }

        private void ShowExitMessage()
        {
            if (MessageBox.Show("Would you like to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (BHasChanges)
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

        private void ChangeTable(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedValue == null)
                return;
            _currentTable = tablesListBox.SelectedValue as Table;
            ApplyTable(tablesListBox.SelectedIndex);
        }

        private void ApplyTable(int index)
        {
            colListBox.DataSource = null;
            colListBox.ValueMember = "Self";
            colListBox.DisplayMember = "Name";
            colListBox.DataSource = Program.LoadedProject.Tables[index].Columns;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show((tablesListBox.SelectedValue as Table)?.Columns.Count.ToString());

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
            switch ((sender as Control)?.Tag as string)
            {
                case "AnalyzeTable":
                    AnalyzeTable();
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
            _tableMap = new Dictionary<Table, FunctionalDependency>(currentProject.Tables.Count);
            foreach (var item in currentProject.Tables) //MAP EACH TABLE TO ITS FUNCTIONAL DEPENDENCY.
            {
                _tableMap.Add(item, item.TableDependency);
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {          
          
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            var res = loadfileDialog.ShowDialog(this);
            if (res != DialogResult.Cancel)
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

        private void CreateTable(object sender, EventArgs e)
        {
            Program.LoadedProject.Tables.Add(new Table("NEW_TABLE", 1));
            BHasChanges = true;
            Refresh(null,null);
        }

        private void DeleteTable(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a table first!");
                return;
            }
            Program.LoadedProject.Tables.RemoveAt(tablesListBox.SelectedIndex);
            BHasChanges = true;
            Refresh(null, null);
        }
        private void Refresh(object sender, EventArgs e)
        {
            LoadProject(Program.LoadedProject);
        }

        private void RenameTable(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a table first!");
                return;
            }
            var renameDialog = new RenameForm();
            renameDialog.SetTitle("Table " + Program.LoadedProject.Tables[tablesListBox.SelectedIndex].Name);
            if (renameDialog.ShowDialog(this) != DialogResult.OK)
                return;
            Program.LoadedProject.Tables[tablesListBox.SelectedIndex].Name = renameDialog.GetName();
            BHasChanges = true;
            Refresh(null,null);
        }

        private void CreateColumn(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a table first!");
                return;
            }
            Program.LoadedProject.Tables[tablesListBox.SelectedIndex].AddColumn(new Column("NEW_COLUMN"));
            BHasChanges = true;
            Refresh(null, null);
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            InitializeTerminalControl();
        }
        private void InitializeTerminalControl()
        {
            terminalControl1.Delimiters = new [] { " ", "->" };
            terminalControl1.AutoCompleteAdd("analyze"); // Aalysis Process
            terminalControl1.AutoCompleteAdd("show_depen");//Show current dependencies
            terminalControl1.AutoCompleteAdd("export");//Export to PDF
            terminalControl1.AutoCompleteAdd("help"); // Show help
            terminalControl1.AutoCompleteAdd("add");// Add dependency
            terminalControl1.AutoCompleteAdd("rem"); // Remove dependency
            terminalControl1.AutoCompleteAdd("exit"); // Exit Application
            terminalControl1.AutoCompleteAdd("tbchng"); // Change active table
            terminalControl1.PromptString = CurrentProject.ProjectName + ((_currentTable != null)? _currentTable.Name : "") + "~\\";
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
                    AnalyzeDatabase(null,null);
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
                    for (var i = 0; i < Program.LoadedProject.Tables.Count; i++)
                    {
                        if (Program.LoadedProject.Tables[i].Name == e.Parameters[0])
                        {
                            tablesListBox.SetSelected(i,true);
                        }
                    }
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
		
        private void DelColumn(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a table first!");
                return;
            }
            if (colListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a column first!");
                return;
            }
            Program.LoadedProject.Tables[tablesListBox.SelectedIndex].RemoveColumn(colListBox.SelectedIndex);
            BHasChanges = true;
            Refresh(null, null);

        }

        private void RenameColumn(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a table first!");
                return;
            }
            if (colListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a column first!");
                return;
            }
            var renameDialog = new RenameForm();
            renameDialog.SetTitle("Column " + Program.LoadedProject.Tables[tablesListBox.SelectedIndex].Columns[colListBox.SelectedIndex].Name);
            if (renameDialog.ShowDialog(this) != DialogResult.OK)
                return;
            Program.LoadedProject.Tables[tablesListBox.SelectedIndex].Columns[colListBox.SelectedIndex].Name = renameDialog.GetName();
            BHasChanges = true;
            Refresh(null, null);
        }

        private void AnalyzeDatabase(object sender, EventArgs e)
        {
            analysisDatagridView.Rows.Clear();
            var checker = new NfChecker();
            foreach (var table in Program.LoadedProject.Tables)
            {
                checker.Fd = table.TableDependency;
                var err = checker.Check();
                analysisDatagridView.Rows.Add(table.Name, err.Level > 1, err.Level > 2, err.Level > 3, err.Level > 4,
                    err.Message);
            }
        }
    }
    
}
