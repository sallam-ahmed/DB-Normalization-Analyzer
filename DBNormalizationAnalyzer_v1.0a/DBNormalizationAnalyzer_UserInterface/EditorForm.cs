using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBNormalizationAnalyzer.Formations;
using DBNormalizationAnalyzer.AnalyzerLibrary;
using DBNormalizationAnalyzer.PresistentDataManager;
using System.Drawing;

namespace DBNormalizationAnalyzer_UserInterface
{

    public partial class EditorForm : Form
    {
        #region Variables
        public static bool BHasChanges; // for saving
        private Table _currentTable;
        private Project CurrentProject { get; set; }
        private Dictionary<Table, FunctionalDependency> _tableMap;
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
        //todo FINALIZE LOADING AS MULTITHREADED TECHNIQUE
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
                    DataManager.SaveProject(CurrentProject);
                }
            }
            else
            {
                DataManager.SaveProject(CurrentProject);
            }
        }
        private void PerformSaveActions(bool prompt,bool exit)
        {
            if (prompt)
            {
                //Implement Save Auction
                var res = MessageBox.Show("Save current project changes?", "Save project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    DataManager.SaveProject(CurrentProject);
                }
            }
            else
            {
                DataManager.SaveProject(CurrentProject);
            }
            if(exit)
                Application.ExitThread();
        }

        #endregion

        #region # CLI - LOG #
        internal void ClearCli()
        {
            terminalControl1.ClearMessages();
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
            var result = MessageBox.Show("Would you like to perform test on all the project tables?\nIf no, only the current table will be analyzed.");
            if (result == DialogResult.No)
            {
                 // Checking current table.
                if (_currentTable == null)
                {
                    //TODO should we check table dependency null ?? 
                    LogText("Error selected table is null.");
                    return;
                }
                var checker = new NfChecker(_currentTable.TableDependency);
                var error = checker.Check();

                //TODO Insert tables into the result grid view

                MessageBox.Show("The table is in the " + (error.Level - 1) + " Normal Form\n" + error.Message);
            }
            else
            {
                // Check all of em;
                foreach (Table table in CurrentProject.Tables)
                {
                    var tbCheceker = new NfChecker(table.TableDependency);
                    LogText("==========================================");
                    LogText("Checking table : " + table.Name + " with the column number of : " + table.Columns.Count);
                    var error = tbCheceker.Check();
                    //TODO Consider multithreaded table checking
                    LogText("Analysis result of table " + table.Name + ":\nThe table is in the "+(error.Level -1) + "Normal form\nResult Message:"+error.Message);
                    //TODO Insert tables in the result grid view
                }
            }
            
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
                    PerformSaveActions(true);
                    NewProject _new = new NewProject();
                    _new.Show( );
                    this.Close();
                    break;
                case "Save":
                    PerformSaveActions(false);
                    break;
                case "ExportPR":
                    break;
                case "Settings":
                    ShowForm(new Settings(), this, (s, ce) => { Enabled = true; });
                    break;
                case "Exit":
                    ShowExitMessage();
                    break;
                case "PropEdit":
                    ShowForm(new EditProject(CurrentProject), this, (s, ce) => { Enabled = true; });
                    break;
                /*VIEW*/
                case "Toolbar":
                    Toolbar.Visible = toolbarToolStripMenuItem.Checked;
                    break;
                case "StatBar":
                    Statusbar.Visible = statusBarToolStripMenuItem.Checked;
                    break;
                /*HELP*/
                case "HowTO":
                    ShowForm(new TutorialDialog(), this, (s, ce) => { Enabled = true; });
                    break;
                case "About":
                    ShowForm(new AboutUs(), this, (s, ce) => { Enabled = true; });
                    break;
                case "License":
                    ShowForm(new License(), this,(s,ce)=> { Enabled = true; });
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
            PerformSaveActions(true,true);
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
            terminalControl1.AutoCompleteAdd("cls");// Clear messages window.
            terminalControl1.PromptString = CurrentProject.ProjectName + ((_currentTable != null)? _currentTable.Name : "") + "~\\";
            terminalControl1.AutoComplete = AutoCompleteMode.Append;
            terminalControl1.PromptColor = Color.Blue;
            terminalControl1.ForeColor = Color.Green;
            terminalControl1.MessageColor = Color.Red;
            terminalControl1.BackColor = Color.Black;
        }
        private void terminalControl1_Command(object sender, TerminalControl.CommandEventArgs e)
        {
            switch (e.Command)
            {
                case "help":
                    e.Message = @"
You can use the following commands:
1- analyze : to start the functional analysis process.
2- show_depen : to show the current stored functional dependencies.
3- add : adds a new functional dependency.
    Command Syntax:
            add {a,b}->{c,d} | or | add a->{c,d} | or | add a->b.
4- rem : remove a current stored function depen.
    Command Syntax:
            rem [N] where N is the index of the stored dependecy.
5- tbchng : changes the current active table of function dependency editing.
        Command Syntaax:
                tbchng [NAME OF TABLE].
6- exit : terminates the application with saving option eneabled.
7- help : show this help menu.
8- cls : Clear the messages window.
";
                    break;
                case "analyze":
                    AnalyzeDatabase(null,null);
                    break;
                case "show_depen":
                    List<string> dependecyStrings = new List<string>();
                    foreach (var table in CurrentProject.Tables)
                    {
                        //TODO show dependecy
                    }
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
                    ShowExitMessage();
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
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            DataManager.SaveProject(CurrentProject);
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
