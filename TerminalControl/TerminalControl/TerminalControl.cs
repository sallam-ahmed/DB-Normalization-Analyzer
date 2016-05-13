using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace TerminalControl
{
    public partial class TerminalControl : UserControl
    {
        public TerminalControl()
        {
            InitializeComponent();
            this.BackColor = SystemColors.Window;
            rtbMessages.Height = 0;
        }


        private bool showMessages = true;
        [Description("Show the previous commands")]
        [DefaultValue(true)]
        public bool ShowMessages
        {
            get { return showMessages; }
            set
            {
                if (showMessages != value)
                {
                    if (value == true)
                    {
                       // this.Height = panelBottom.Height + txtInput.PreferredHeight * 5;
                        rtbMessages.Visible = true;
                    }
                    else
                    {
                        rtbMessages.Visible = false;
                        this.Height = txtInput.Height;
                    }
                }
                showMessages = value;
            }
        }

        private bool ignoreBlankCommands = true;
        [Description("Determines whether event will fire for blank commands")]
        [DefaultValue(true)]
        public bool IgnoreBlankCommands
        {
            get { return ignoreBlankCommands; }
            set { ignoreBlankCommands = value; }
        }

        private string promptString = ">";
        [Description("String showed as the prompt")]
        [DefaultValue(">")]
        public string PromptString
        {
            get { return promptString; }
            set
            {
                promptString = value;
                lblPrompt.Text = promptString;
            }
        }

        [Description("Forecolor of the prompt")]
        public Color PromptColor
        {
            get { return lblPrompt.ForeColor; }
            set { lblPrompt.ForeColor = value; }
        }

        private string[] delimiters = new string[] { " " };
        [Description("String of characters which act as delimiters between parameters")]
        [DefaultValue(new string[] { " " })]
        public string[] Delimiters
        {
            get { return delimiters; }
            set { delimiters = value; }
        }

        private string[] ParseInput()
        {
            string temp = txtInput.Text;
            return temp.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        [Description("Adds a message to the message RichTextBox")]
        public void AddMessage(string msg)
        {
            int prevLength = rtbMessages.Text.Length;

            if (rtbMessages.Lines.Length > 0)
                rtbMessages.AppendText("\n" + msg);
            else
                rtbMessages.AppendText(msg);

            rtbMessages.SelectionStart = prevLength;
            rtbMessages.SelectionLength = rtbMessages.Text.Length - prevLength;
            rtbMessages.SelectionColor = messageColor;

            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.SelectionLength = 0;
            rtbMessages.SelectionColor = rtbMessages.ForeColor;
            rtbMessages.ScrollToCaret();
            rtbMessages.Select(rtbMessages.TextLength, 0);

            txtInput.Focus();
        }


        /// <summary>
        /// Adds a command to the messages list
        /// </summary>
        /// <param name="prompt">The prompt string</param>
        /// <param name="command">The command entered</param>
        private void AddCommand(string prompt, Color color, string command)
        {
            int prevLength = rtbMessages.Text.Length;

            rtbMessages.AppendText(prompt + " " + command);

            rtbMessages.SelectionStart = prevLength;
            rtbMessages.SelectionLength = prompt.Length;
            rtbMessages.SelectionColor = color;

            rtbMessages.SelectionLength = 0;
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();

            txtInput.Focus();
        }

        [Description("Clear all messages from RichTextBox")]
        public void ClearMessages()
        {
            rtbMessages.Clear();
            rtbMessages.Height = 0;
            rtbMessages.ScrollBars = RichTextBoxScrollBars.None;
        }

        public delegate void CommandEventHandler(object sender, CommandEventArgs e);
        [Description("Event raised when the user enters a command and presses the Enter key")]
        public event CommandEventHandler Command;

        protected virtual void OnCommand(CommandEventArgs e)
        {
            if (Command != null)
                Command(this, e);
        }

        /*public delegate void CommandEnteringEventHandler(object sender, CommandEnteringEventArgs e);
        [Description("Event raised on KeyPress in input area")]
        public event CommandEnteringEventHandler CommandEntering;

        protected virtual void OnCommandEntering(CommandEnteringEventArgs e)
        {
            if (CommandEntering != null)
                CommandEntering(this, e);
        }*/

        private Color messageColor = SystemColors.ControlText;
        [Description("Color of the messages added using AddMessage method")]
        public Color MessageColor
        {
            get { return messageColor; }
            set { messageColor = value; }
        }


        [Description("Indicates text completion behaviour at prompt")]
        [DefaultValue(AutoCompleteMode.None)]
        public AutoCompleteMode AutoComplete
        {
            get { return txtInput.AutoCompleteMode; }
            set
            {
                txtInput.AutoCompleteMode = value;
                txtInput.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
        }

        private bool autoCompleteStore = false;
        [Description("Store messages for AutoComplete feature")]
        [DefaultValue(false)]
        public bool AutoCompleteStore
        {
            get { return autoCompleteStore; }
            set { autoCompleteStore = value; }
        }

        [Description("Add AutoComplete data")]
        public void AutoCompleteAdd(string str)
        {
            txtInput.AutoCompleteCustomSource.Add(str);
        }

        [Description("Clear all AutoComplete data")]
        public void AutoCompleteClear()
        {
            txtInput.AutoCompleteCustomSource.Clear();
        }

        private void TerminalControl_Load(object sender, EventArgs e)
        {

        }

        private void Prompt_FontChanged(object sender, EventArgs e)
        {
            txtInput.Font = this.Font;
        }

        private void Prompt_ForeColorChanged(object sender, EventArgs e)
        {
            txtInput.ForeColor = this.ForeColor;
            rtbMessages.ForeColor = this.ForeColor;
        }

        private void Prompt_BackColorChanged(object sender, EventArgs e)
        {
            rtbMessages.BackColor = this.BackColor;
            txtInput.BackColor = this.BackColor;
           // panelBottom.BackColor = this.BackColor;
            lblPrompt.BackColor = this.BackColor;
        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            if (showMessages == true)
            {
                //if (rtbMessages.Height < this.Height - panelBottom.Height)
                //    rtbMessages.Height = rtbMessages.Lines.Length * txtInput.PreferredHeight;
                //else
                //    rtbMessages.ScrollBars = RichTextBoxScrollBars.Both;

                //if (rtbMessages.Height > this.Height - panelBottom.Height)
                //{
                //    rtbMessages.Height = this.Height - panelBottom.Height;
                //    rtbMessages.ScrollBars = RichTextBoxScrollBars.Both;
                //}
                rtbMessages.ScrollToCaret();
            }
        }

        private void rtbMessages_Click(object sender, EventArgs e)
        {
            if (rtbMessages.SelectionLength == 0)
                txtInput.Focus();
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                return;
            }
        }

        // Stores the current line
        // Used while navigating the previous commands list
        private int currentLine = 0;

        // Stores the previous messages
        private ArrayList prevMessages = new ArrayList();

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (txtInput.Text != "" && IgnoreBlankCommands)
                {
                    SuspendLayout();

                    string prevPrompt = lblPrompt.Text;
                    Color prevPromptColor = PromptColor;

                    // Raise the command event
                    CommandEventArgs args = new CommandEventArgs(ParseInput()[0], ParseInput());
                    OnCommand(args);

                    if (args.Cancel == false)
                    {
                        if (rtbMessages.Lines.Length > 0)
                            rtbMessages.AppendText("\r\n");

                        AddCommand(prevPrompt, prevPromptColor, txtInput.Text);

                        if (args.Message != "")
                            AddMessage(args.Message);

                        rtbMessages.ScrollToCaret();
                        prevMessages.Add(txtInput.Text);
                        if (autoCompleteStore == true && args.Record == true)
                            txtInput.AutoCompleteCustomSource.Add(txtInput.Text);

                        currentLine = prevMessages.Count - 1;
                    }

                    txtInput.Text = "";
                    ResumeLayout();
                }
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                if (currentLine >= 0 && prevMessages.Count > 0)
                {
                    txtInput.Text = prevMessages[currentLine].ToString();
                    txtInput.SelectionLength = 0;
                    txtInput.SelectionStart = txtInput.Text.Length;

                    currentLine--;
                }

                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                if (currentLine < prevMessages.Count - 2)
                {
                    currentLine++;

                    txtInput.Text = prevMessages[currentLine + 1].ToString();
                    txtInput.SelectionLength = 0;
                    txtInput.SelectionStart = txtInput.Text.Length;
                }

                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (txtInput.SelectionLength > 0 && txtInput.AutoCompleteMode != AutoCompleteMode.None)
                {
                    txtInput.Text = txtInput.Text.Substring(0, txtInput.SelectionStart);
                    txtInput.SelectionStart = txtInput.Text.Length;
                }

                e.Handled = true;
                return;
            }
        }

        private void lblPrompt_Click(object sender, EventArgs e)
        {

        }

        private void txtInput_TextChanged_1(object sender, EventArgs e)
        {

        }
    }

    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(string cmd, string[] param)
        {
            command = cmd;
            message = "";
            record = true;
            parameters = param;
        }

        private bool cancel = false;
        [Description("Indicates whether command will be displayed in messages list")]
        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }

        private string command;
        [Description("The command entered by the user")]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        private string message;
        [Description("Message to be displayed to user in response to command")]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private bool record;
        [Description("Indicates whether the command should be stored for AutoComplete")]
        public bool Record
        {
            get { return record; }
            set { record = value; }
        }

        private string[] parameters;
        [Description("Parameters of the command")]
        public string[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }

    public class CommandEnteringEventArgs : EventArgs
    {
        public CommandEnteringEventArgs(string comm)
        {
            command = comm;
        }

        private string command;
        [Description("Command in the input area")]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }
    }

    public class ParameterEnteredEventArgs : EventArgs
    {
        public ParameterEnteredEventArgs(string cmd, string[] list)
        {
            command = cmd;
            parameters = list;
            enterKey = false;
        }

        public ParameterEnteredEventArgs(string cmd, string[] list, bool enter)
        {
            command = cmd;
            parameters = list;
            enterKey = enter;
        }

        private string command;
        [Description("Command entered currently")]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }


        private string[] parameters;
        [Description("List of paramters entered")]
        public string[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private bool enterKey;
        [Description("Determines whether the enter key has been pressed")]
        public bool EnterKey
        {
            get { return enterKey; }
            set { enterKey = value; }
        }
    }
}