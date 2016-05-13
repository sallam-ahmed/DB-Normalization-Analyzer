using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DBNormalizationAnalyzer.Formations;
using DBNormalizationAnalyzer.AnalyzerLibrary;
using DBNormalizationAnalyzer.PresistentDataManager;
using System.ComponentModel;

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
        public EditorForm(Project _project)
        {
            InitializeComponent();
            LoadProject(_project);
            _currentTable = _project.Tables.Count != 0 ? _project.Tables[0] : null;
            ClearCLI();
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

            LogText("Loaded project tables with count of " + project.Tables.Count + " projects.");
            Text = "DB Normalization Analyzer - " + CurrentProject.ProjectName+ " -";
            ClearCLI();
        }
        private void PerformSaveActions(bool prompt)
        {
            if (prompt)
            {
                //Implement Save Auction
                var res = MessageBox.Show("Save current project changes?", "Save project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    //Save
                }
            }
            else
            {
                //Save
            }
            BHasChanges = false;
        }
        #endregion
        #region # CLI - LOG #
        internal void ClearCLI()
        {
            commandPanel.Text = "~" + CurrentProject.ProjectName + @"\\";
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
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    var lastLine = commandPanel.Text.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    
                    if (lastLine.StartsWith("add"))
                    {
                        lastLine = lastLine.Remove(0, 3);
                        lastLine = lastLine.Replace(" ", "").Replace("{", "").Replace("}", "");
                        var tokens = lastLine.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                        var dep0 = tokens[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var dep1 = tokens[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var cols = dep0.Select(s => new Column(s)).ToList();

                        var from = _currentTable.ColumnSet(cols);
                        cols.Clear();

                        cols.AddRange(dep1.Select(s => new Column(s)));

                        var to = _currentTable.ColumnSet(cols);
                        _currentTable.TableDependency.AddDependency(from, to);
                    }
                    else if (lastLine.StartsWith("rem"))
                    {
                        lastLine = lastLine.Remove(0, 3);
                        lastLine = lastLine.Replace(" ", "").Replace("{", "").Replace("}", "");
                        var tokens = lastLine.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                        var dep0 = tokens[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var dep1 = tokens[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var cols = dep0.Select(s => new Column(s)).ToList();

                        var from = _currentTable.ColumnSet(cols);
                        cols.Clear();

                        cols.AddRange(dep1.Select(s => new Column(s)));

                        var to = _currentTable.ColumnSet(cols);
                        _currentTable.TableDependency.RemoveDependency(from, to);
                    }
                    else if (lastLine.StartsWith("tbchng"))
                    {
                        //CHANGE WORKING TABLE
                        lastLine = lastLine.Remove(0, 5);
                        MessageBox.Show(lastLine);
                    }
                    commandPanel.AppendText("~"+CurrentProject.ProjectName + "\\Command Done\n");
                    commandPanel.AppendText(_commandBuilder);
                }
                catch (Exception ex)
                {
                    commandPanel.AppendText(ex.Message);
                }
            }
           
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void ChangeTable(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedValue == null)
                return;
            _currentTable = tablesListBox.SelectedValue as Table;
            colListBox.DataSource = null;
            colListBox.ValueMember = "Self";
            colListBox.DisplayMember = "Name";
            colListBox.DataSource = Program.LoadedProject.Tables[tablesListBox.SelectedIndex].Columns;

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
            Program.LoadedProject.Tables.Add(new Table("NEW TABLE", 1));
            BHasChanges = true;
            Refresh(null,null);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

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
            MessageBox.Show(renameDialog.GetName());
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
            Program.LoadedProject.Tables[tablesListBox.SelectedIndex].AddColumn(new Column("New Column"));
            BHasChanges = true;
            Refresh(null, null);
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
            MessageBox.Show(renameDialog.GetName());
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
