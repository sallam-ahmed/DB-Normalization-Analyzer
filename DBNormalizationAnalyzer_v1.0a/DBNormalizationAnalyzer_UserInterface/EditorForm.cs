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
        public static bool bHasChanges; // for saving
        private string _commandBuilder = "";
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
                    //Save
                }
            }
            else
            {
                //Save
            }
            bHasChanges = false;
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

        private void Analyze()
        {
            if (m_currentTable == null)
            {
                LogText("Error selected table is null.");
                return;
            }

            var checker = new NfChecker(m_currentTable.TableDependency);
            var error = checker.Check();
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

                        var from = m_currentTable.ColumnSet(cols);
                        cols.Clear();

                        cols.AddRange(dep1.Select(s => new Column(s)));

                        var to = m_currentTable.ColumnSet(cols);
                        m_currentTable.TableDependency.AddDependency(from, to);
                    }
                    else if (lastLine.StartsWith("rem"))
                    {
                        lastLine = lastLine.Remove(0, 3);
                        lastLine = lastLine.Replace(" ", "").Replace("{", "").Replace("}", "");
                        var tokens = lastLine.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                        var dep0 = tokens[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var dep1 = tokens[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var cols = dep0.Select(s => new Column(s)).ToList();

                        var from = m_currentTable.ColumnSet(cols);
                        cols.Clear();

                        cols.AddRange(dep1.Select(s => new Column(s)));

                        var to = m_currentTable.ColumnSet(cols);
                        m_currentTable.TableDependency.RemoveDependency(from, to);
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

        private void tablesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (tablesListBox.SelectedValue != null)
                {
                    //BindingList<Column> BList;

                    //Bs = new BindingSource();
                    //Bs.DataSource = 
                    
                    columnsDatagridview.DataSource = new BindingList<Column>((tablesListBox.SelectedValue as Table).Columns);
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

        private void columnsDatagridview_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            MessageBox.Show(columnsDatagridview.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
        }

        private void analysisDatagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void columnsDatagridview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void tablesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void createTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.LoadedProject.Tables.Add(new Table("NEW TABLE", 1));
            Program.LoadedProject.Tables[0].Columns[0].Name = "NEW COLUMN";
            bHasChanges = true;
            LoadProject(Program.LoadedProject);
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
    
}
