using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBNormalizationAnalyzer_UserInterface
{
    public partial class MainForm : Form
    {
        #region Variables
        public static bool bHasChanges = false;
        #endregion
        public MainForm()
        {
            InitializeComponent();
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

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
    }
}
