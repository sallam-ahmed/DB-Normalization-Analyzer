using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DBNormalizationAnalyzer_Formations;
using DBNormalizationAnalyzer_AnalyzerLibrary;

namespace DBNormalizationAnalyzer_UserInterface
{

    public partial class MainForm : Form
    {
        #region Variables
        public static bool bHasChanges;
        private string _commandBuilder = "";
        private Database m_projectDB;
        private Table _currentTable;
        private List<Table> Tables;
        #endregion
        public MainForm()
        {
            m_projectDB = new Database();
            InitializeComponent();
            Tables = new List<Table>();
            _currentTable = new Table(10);
        }
        /// <summary>
        /// Controls all the buttons actions in the main form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PerformButtonActions(object sender,EventArgs e)
        {
            switch ((sender as Button)?.Tag as string)
            {
                case "Save/Update":
                    break;
                case "Commit":
                    break;
                case "Analyze":
                    Analyze();
                    break;
                case "ExportPDF":
                    break;
                case "RepGenerate":
                    break;
                case "LogWindow":
                    break;
                case "TB_Insert":
                    break;
                case "Del_Table":
                    break;
            }
        }
        private void PerformMenuItemsActions(object sender,EventArgs e)
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
                    exitToolStripMenuItem_Click();
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
                    break;
                case "About":
                    break;
                case "License":
                    break;
            }
        }
        private void exitToolStripMenuItem_Click()
        {
            if (MessageBox.Show("Would you like to exit ?","Exit?",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (bHasChanges)
                {
                    if (MessageBox.Show("Save Changes ? ", "Save ? ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        PerformSaveActions();
                        Application.Exit();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
        }

        private void PerformSaveActions()
        {
            //Implement Save Auction
            bHasChanges = false;
        }

        private void Analyze()
        {
            if (_currentTable == null)
                return;
            var checker = new NfChecker(_currentTable.TableDependency);
            var error = checker.Check();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    var lastLine = richTextBox1.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Last();

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

                    richTextBox1.AppendText("Command Done\n");
                    richTextBox1.AppendText(_commandBuilder);
                }
                catch (Exception ex)
                {
                    richTextBox1.AppendText(ex.Message);
                }
            }
        }
    }
}
