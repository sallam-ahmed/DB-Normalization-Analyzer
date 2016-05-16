using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBNormalizationAnalyzer.Formations;
using DBNormalizationAnalyzer.AnalyzerLibrary;
using DBNormalizationAnalyzer.PresistentDataManager;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DBNormalizationAnalyzer_UserInterface
{

    public partial class EditorForm : Form
    {
        #region Variables
        public static bool BHasChanges; // for saving
        private int _selectedIndex;
        private readonly List<Error> _analsysisErrors; 
        public Table CurrentTable
        {
            get
            {
                return _selectedIndex >= Program.LoadedProject.Tables.Count ? null : Program.LoadedProject.Tables[_selectedIndex];
            }
            set
            {
                if (_selectedIndex >= Program.LoadedProject.Tables.Count)
                    throw new ArgumentOutOfRangeException();
                Program.LoadedProject.Tables[_selectedIndex] = value;
            }
        }

        private List<Table> suggestion;
        private bool _visibleSuggestions;
        public bool ViewSuggestions
        {
            get { return _visibleSuggestions; }
            set
            {
                _visibleSuggestions = value;
                pinPic.Image = value ? Properties.Resources.pinned : Properties.Resources.unpinned;
            }
        }
        #endregion

        #region # Constructors #
        //TODO: Load project data into controls
        public EditorForm(Project project)
        {
            InitializeComponent();
            _selectedIndex = 0;
            BHasChanges = false;
            _analsysisErrors = new List<Error>();
            LoadProject(project);
            ClearCli();
            ResetLog();
            ApplySettings();
            _visibleSuggestions = false;
        }
        #endregion


        #region # Project Manager #
        private void LoadProject(Project project)
        {
            Program.LoadedProject = project;
            tablesListBox.DataSource = null;
            tablesListBox.ValueMember = "Self";
            tablesListBox.DisplayMember = "Name";
            tablesListBox.DataSource = project.Tables;

            try
            {
                if (project.Tables.Count > 0)
                {
                    tablesListBox.SetSelected(0,true);
                    ApplyTable();
                }
            }
            catch
            {
                // ignored
            }
            LogText("Loaded project tables with count of " + project.Tables.Count.ToString() + " projects.");
            Text = $"DB Normalization Analyzer -{Program.LoadedProject.ProjectName}-";
            ClearCli();
        }
        private void PerformSaveActions(bool prompt)
        {
            if (!BHasChanges)
                return;
            if (prompt)
            {
                //Implement Save Auction
                var res = MessageBox.Show("Save current project changes?", "Save project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    DataManager.SaveProject(Program.LoadedProject);
                }
            }
            else
            {
                DataManager.SaveProject(Program.LoadedProject);
            }
            BHasChanges = false;
        }
        private void PerformSaveActions(bool prompt,bool exit)
        {
            if (!BHasChanges)
            {
                if (exit)
                {
                    Application.ExitThread();
                }
                else
                {
                    return;
                }
            }
            if (prompt)
            {
                
                var res = MessageBox.Show("Save current project changes?", "Save project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    DataManager.SaveProject(Program.LoadedProject);
                }
            }
            else
            {
                DataManager.SaveProject(Program.LoadedProject);
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
            LogText("Project: " + Program.LoadedProject.ProjectName);
            LogText("Path: " + Program.LoadedProject.ProjectPath);
            LogText("Welcome to DBNormalization Aanalyzer.");
        }
        void LogText(string value)
        {
            logTextBox.AppendText(Program.LoadedProject.ProjectName + "~ "+DateTime.Now.ToShortTimeString() + " " + value+"\n");
        }
        private void InitializeTerminalControl()
        {
            terminalControl1.Delimiters = new[] { " ", "->" };
            terminalControl1.AutoCompleteAdd("analyze"); // Aalysis Process
            terminalControl1.AutoCompleteAdd("show_depen");//Show current dependencies
            terminalControl1.AutoCompleteAdd("export");//Export to PDF
            terminalControl1.AutoCompleteAdd("help"); // Show help
            terminalControl1.AutoCompleteAdd("add");// Add dependency
            terminalControl1.AutoCompleteAdd("rem"); // Remove dependency
            terminalControl1.AutoCompleteAdd("exit"); // Exit Application
            terminalControl1.AutoCompleteAdd("tbchng"); // Change active table
            terminalControl1.AutoCompleteAdd("cls");// Clear messages window.
            terminalControl1.AutoCompleteAdd("set_primary_key");//Set primary key
            terminalControl1.AutoCompleteAdd("show_primary_key");//Set primary key
            UpdatePromptString();
            terminalControl1.AutoComplete = AutoCompleteMode.Append;
            terminalControl1.PromptColor = Color.Blue;
            terminalControl1.ForeColor = Color.Green;
            terminalControl1.MessageColor = Color.Red;
            terminalControl1.BackColor = Color.Black;
        }
        private void terminalControl1_Command(object sender, TerminalControl.CommandEventArgs e)
        {
            List<Column> independSet, dependSet;
            List<string> independStr, dependStr;
            var msgBuilder = new StringBuilder();
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
                    AnalyzeDatabase(null, null);
                    BHasChanges = true;
                    break;
                case "show_depen":
                    foreach (var table in Program.LoadedProject.Tables)
                    {
                        msgBuilder.AppendLine("Table " + table.Name + " functional dependency:");
                        foreach (var dependency in table.TableDependency.DependencyList)
                        {
                            independSet = table.ColumnSet(dependency.Item1);
                            dependSet = table.ColumnSet(dependency.Item2);
                            msgBuilder.Append("{");
                            for (var i = 0; i < independSet.Count; i++)
                            {
                                msgBuilder.Append(independSet[i].Name);
                                msgBuilder.Append(i + 1 == independSet.Count ? "}" : ",");
                            }
                            msgBuilder.Append("->{");
                            for (var i = 0; i < dependSet.Count; i++)
                            {
                                msgBuilder.Append(dependSet[i].Name);
                                msgBuilder.Append(i + 1 == dependSet.Count ? "}" : ",");
                            }
                            msgBuilder.AppendLine();
                        }
                    }
                    e.Message = msgBuilder.ToString();
                    break;
                case "add":
                    if (CurrentTable == null)
                    {
                        e.Message = "Select table first!";
                        break;
                    }
                    if (e.Parameters.Length != 3)
                    {
                        e.Message = "Error parsing command!";
                        break;
                    }
                    independStr = e.Parameters[1].Split(',', '{', '}').Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                    dependStr = e.Parameters[2].Split(',', '{', '}').Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                    independSet = CurrentTable.ColumnSet(independStr);
                    dependSet = CurrentTable.ColumnSet(dependStr);
                    if (independSet.Count != independStr.Count || dependSet.Count != dependStr.Count)
                    {
                        e.Message = "Error parsing command! No such columns!";
                        break;
                    }
                    CurrentTable.TableDependency.AddDependency(CurrentTable.ColumnSet(independSet), CurrentTable.ColumnSet(dependSet));
                    e.Message = "Command executed successfully! Mbrouk!";
                    BHasChanges = true;
                    break;
                case "rem":
                    if (CurrentTable == null)
                    {
                        e.Message = "Select table first!";
                        break;
                    }
                    if (e.Parameters.Length != 3)
                    {
                        e.Message = "Error parsing command!";
                        break;
                    }
                    independStr = e.Parameters[1].Split(',', '{', '}').Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                    dependStr = e.Parameters[2].Split(',', '{', '}').Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                    independSet = CurrentTable.ColumnSet(independStr);
                    dependSet = CurrentTable.ColumnSet(dependStr);
                    if (independSet.Count != independStr.Count || dependSet.Count != dependStr.Count)
                    {
                        e.Message = "Error parsing command! No such columns!";
                        break;
                    }
                    try
                    {
                        CurrentTable.TableDependency.RemoveDependency(CurrentTable.ColumnSet(independSet), CurrentTable.ColumnSet(dependSet));
                        e.Message = "Command executed successfully! Mbrouk!";
                        BHasChanges = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        e.Message = "No such dependency exist!";
                    }
                    break;
                case "exit":
                    ShowExitMessage();
                    break;
                case "tbchng":
                    if (e.Parameters[1] == null)
                    {
                        terminalControl1.AddMessage("Syntax error.");
                        return;
                    }

                    for (var i = 0; i < Program.LoadedProject.Tables.Count; i++)
                    {
                        if (Program.LoadedProject.Tables[i].Name == e.Parameters[1])
                        {
                            tablesListBox.SetSelected(i, true);
                        }
                    }
                    UpdatePromptString();
                    BHasChanges = true;
                    break;
                case "cls":
                    terminalControl1.ClearMessages();
                    break;
                case "set_primary_key":
                    if (CurrentTable == null)
                    {
                        e.Message = "Select table first!";
                        break;
                    }
                    independStr = e.Parameters[1].Split(',', '{', '}').Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                    try
                    {
                        CurrentTable.setPrimaryKey(independStr, true);
                        e.Message = "Mbrouk!";
                    }
                    catch (ArgumentException)
                    {
                        e.Message = "Can't found some keys!";
                    }
                    BHasChanges = true;
                    break;
                case "show_primary_key":
                    if (CurrentTable == null)
                    {
                        e.Message = "Select table first!";
                        break;
                    }
                    if (CurrentTable.PrimaryKey.Count == 0)
                    {
                        e.Message = "{}";
                        break;
                    }
                    msgBuilder.Append("{");
                    for (var i = 0; i < CurrentTable.PrimaryKey.Count; i++)
                    {
                        msgBuilder.Append(CurrentTable.PrimaryKey[i].Name);
                        msgBuilder.Append(i + 1 == CurrentTable.PrimaryKey.Count ? "}" : ",");
                    }
                    e.Message = msgBuilder.ToString();
                    break;
            }
        }

        #endregion
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
                    var _new = new NewProject();
                    _new.Show( );
                    Close();
                    break;
                case "Save":
                    PerformSaveActions(false);
                    break;
                case "ExportPR":
                    break;
                case "Settings":
                    ShowForm(new Settings(), this, (s, ce) => { Enabled = true; ApplySettings();});
                    break;
                case "Exit":
                    ShowExitMessage();
                    break;
                case "PropEdit":
                    ShowForm(new EditProject(Program.LoadedProject), this, (s, ce) => { Enabled = true; });
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
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch ((sender as Control)?.Tag as string)
            {
                case "Analyze":
                    AnalyzeDatabase(null, null);
                    break;
                case "Export":
                    //TODO Export Report
                    break;
                case "RefTable":
                    LoadProject(Program.LoadedProject);
                    break;
                case "Load":
                    var res = loadfileDialog.ShowDialog(this);
                    if (res != DialogResult.Cancel)
                    {
                        PerformSaveActions(true);
                        LoadProject(DataManager.ReadProject(loadfileDialog.FileName));
                    }
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
            if (!Properties.Settings.Default.AskOnExit)
            {
                PerformSaveActions(false,true);
            }
            else
            {
                if (
                    MessageBox.Show(@"Would you like to exit?", @"Exit?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) !=
                    DialogResult.Yes
                    ) return;

                PerformSaveActions(true,true);
            }
        }
        #endregion

        private void ChangeTable(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedValue == null)
                return;
            _selectedIndex = tablesListBox.SelectedIndex;
            ApplyTable();
        }

        private void ApplyTable()
        {
            colListBox.DataSource = null;
            colListBox.ValueMember = "Self";
            colListBox.DisplayMember = "Name";
            colListBox.DataSource = CurrentTable.Columns;
        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowExitMessage();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            datetimeLBL.Text = DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToShortTimeString();
        }

        #region # Project Manibulation #

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

        #endregion
        private void EditorForm_Load(object sender, EventArgs e)
        {
            InitializeTerminalControl();
        }

        void UpdatePromptString()
        {
            terminalControl1.PromptString = Program.LoadedProject.ProjectName + "\\" + ((CurrentTable != null) ? CurrentTable.Name : "") + "~\\";
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            DataManager.SaveProject(Program.LoadedProject);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            DialogResult r = saveFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                Project _new = Program.LoadedProject;
                _new.ProjectPath = saveFileDialog1.FileName;
                DataManager.CreateProject(_new);
			}
		}
		

        private void ApplySettings()
        {
            var def = Properties.Settings.Default;

            colListBox.Font = def.AppFont;
            tablesListBox.Font = def.AppFont;
            analysisDatagridView.Font = def.GridFont;
            terminalControl1.Font = def.TerminalFont;
        }
        private void AnalyzeDatabase(object sender, EventArgs e)
        {
            LogText($"Started analysis process of total {Program.LoadedProject.Tables.Count.ToString()} tables");
            analysisDatagridView.Rows.Clear();
            _analsysisErrors.Clear();
            var checker = new NfChecker();
            foreach (var table in Program.LoadedProject.Tables)
            {
                checker.Fd = table.TableDependency;
                var err = checker.Check();
                analysisDatagridView.Rows.Add(table.Name, err.Level > 1, err.Level > 2, err.Level > 3, err.Level > 4,
                    err.Message);
                if (err.SuggestedSplit.Count != 0)
                    _analsysisErrors.Add(err);
            }
            //Apply style
            for (var i = 0; i < analysisDatagridView.Rows.Count; i++)
            {
                for (var j = 0; j < analysisDatagridView.Columns.Count; j++)
                {
                    switch (analysisDatagridView.Rows[i].Cells[j].Value.ToString())
                    {
                        case "False":
                            analysisDatagridView.Rows[i].Cells[j].Style.BackColor = Color.Red;
                            break;
                        case "True":
                            analysisDatagridView.Rows[i].Cells[j].Style.BackColor = Color.Green;
                            break;
                        //analysisDatagridView.Rows[i].Cells[j].Style.Font
                    }
                }
            }
        }

        private void ShowSuggestion(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _analsysisErrors.Count)
                return;
            if (_analsysisErrors[e.RowIndex].SuggestedSplit.Count == 0)
                return;
            suggestion = _analsysisErrors[e.RowIndex].SuggestedSplit.Select((t, i) => new Table("Table" + i.ToString(), Program.LoadedProject.Tables[e.RowIndex].ColumnSet(t.Item1), Program.LoadedProject.Tables[e.RowIndex].ColumnSet(t.Item2))).ToList();
            newTablesList.DataSource = null;
            newTablesList.ValueMember = "Self";
            newTablesList.DisplayMember = "Name";
            newTablesList.DataSource = suggestion;
            suggestionPanel.Visible = true;
            suggestionPanel.Focus();
        }

        private void ToggleSuggestion(object sender, EventArgs e)
        {
            ViewSuggestions = !ViewSuggestions;
            suggestionPanel.Focus();
        }

        private void HideSuggesion(object sender, EventArgs e)
        {
            if (!ViewSuggestions)
            {
                suggestionPanel.Visible = false;
            }
        }

        private void ChangeSuggestedColumns(object sender, EventArgs e)
        {
            if (newTablesList.SelectedItems.Count == 0 || newTablesList.SelectedIndex < 0 ||
                newTablesList.SelectedIndex >= suggestion.Count)
                return;
            newColList.DataSource = newPrimeList.DataSource = null;
            newColList.ValueMember = newPrimeList.ValueMember = "Self";
            newColList.DisplayMember = newPrimeList.DisplayMember = "Name";
            newColList.DataSource = suggestion[newTablesList.SelectedIndex].Columns;
            newPrimeList.DataSource = suggestion[newTablesList.SelectedIndex].PrimaryKey;
            suggestionPanel.Focus();
        }
    }
    
}
