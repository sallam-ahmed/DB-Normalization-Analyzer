using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBNormalizationAnalyzer_Formations;
using DBNormalizationAnalyzer_AnalyzerLibrary;
using System.Collections;

namespace DBNormalizationAnalyzer_UserInterface
{

    public partial class MainForm : Form
    {
        #region Variables
        public static bool bHasChanges = false;
        public string commandBuilder = "";
        private Database m_projectDB;
        private Table currentTable;
        List<Table> Tables;
        #endregion
        public MainForm()
        {
            InitializeComponent();
            Tables = new List<Table>();
            currentTable = new Table(10);
            for (int i = 0; i < 10; i++)
                currentTable.Columns.Add(new Column(Convert.ToChar(((Convert.ToInt32('a') + i))).ToString()));
        }
        /// <summary>
        /// Controls all the buttons actions in the main form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PerformButtonActions(object sender,EventArgs e)
        {
            switch ((sender as Button).Tag as string)
            {
                case "Save/Update":
                    break;
                case "Commit":
                    break;
                case "Analyze":
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
                default:
                    break;
            }
        }
        private void PerformMenuItemsActions(object sender,EventArgs e)
        {
            switch ((sender as ToolStripMenuItem).Tag as string)
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
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    string lastLine = richTextBox1.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Last();

                    if (lastLine.StartsWith("add"))
                    {
                        lastLine = lastLine.Remove(0, 3);
                        lastLine = lastLine.Replace(" ", "").Replace("{", "").Replace("}", "");
                        string[] tokens = lastLine.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                        string[] dep0 = tokens[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] dep1 = tokens[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<Column> cols = new List<Column>();
                        foreach (string s in dep0)
                            cols.Add(new Column(s));

                        BitArray from = currentTable.ColumnSet(cols);
                        cols.Clear();

                        foreach (string s in dep1)
                            cols.Add(new Column(s));

                        BitArray to = currentTable.ColumnSet(cols);
                        currentTable.TableDependency.AddDependency(from, to);
                    }
                    else if (lastLine.StartsWith("rem"))
                    {
                        lastLine = lastLine.Remove(0, 3);
                        lastLine = lastLine.Replace(" ", "").Replace("{", "").Replace("}", "");
                        string[] tokens = lastLine.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                        string[] dep0 = tokens[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] dep1 = tokens[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<Column> cols = new List<Column>();
                        foreach (string s in dep0)
                            cols.Add(new Column(s));

                        BitArray from = currentTable.ColumnSet(cols);
                        cols.Clear();

                        foreach (string s in dep1)
                            cols.Add(new Column(s));

                        BitArray to = currentTable.ColumnSet(cols);
                        currentTable.TableDependency.RemoveDependency(from, to);
                    }

                    richTextBox1.AppendText("Command Done\n");
                    richTextBox1.AppendText(commandBuilder);
                }
                catch (Exception ex)
                {
                    richTextBox1.AppendText(ex.Message);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
