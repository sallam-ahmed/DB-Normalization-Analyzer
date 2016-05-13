using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            terminalControl1.Delimiters = new string[] { " ", "->" };
            terminalControl1.AutoCompleteAdd("analyze");
            terminalControl1.AutoCompleteAdd("shdepen");
            terminalControl1.AutoCompleteAdd("log");
            terminalControl1.AutoCompleteAdd("export");
            terminalControl1.AutoCompleteAdd("help");
            terminalControl1.AutoCompleteAdd("add");
            terminalControl1.AutoCompleteAdd("rem");
            terminalControl1.AutoCompleteAdd("exit");
            terminalControl1.AutoCompleteAdd("tbchng");
            terminalControl1.PromptString = "ProjectTest >";
            terminalControl1.AutoComplete = AutoCompleteMode.Append;
            terminalControl1.PromptColor = Color.Blue;
            terminalControl1.ForeColor = Color.Green;
            terminalControl1.MessageColor = Color.Red;
            terminalControl1.BackColor = Color.White;
        }

        private void terminalControl1_Command(object sender, TerminalControl.CommandEventArgs e)
        {
            if (e.Command == "add")
            {
                foreach (var item in e.Parameters)
                {
                    e.Message += " @ " + item;
                }
                //e.Message = "Added";
            }
            else if (e.Command == "rem")
            {
                e.Message = "Removed";
            }
        }
    }
}
