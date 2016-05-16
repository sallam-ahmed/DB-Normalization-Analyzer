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
    public partial class RenameForm : Form
    {
        public RenameForm()
        {
            InitializeComponent();
        }

        public void SetTitle(string toBeRenamed)
        {
            Text = "Rename " + toBeRenamed;
            label1.Text += toBeRenamed;
        }

        public string GetName()
        {
            return nameTXT.Text;
        }
    }
}
