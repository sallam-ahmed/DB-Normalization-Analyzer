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
    public partial class License : Form
    {
        public License()
        {
            InitializeComponent();
        }

        private void License_Load(object sender, EventArgs e)
        {
            System.IO.FileStream stream = new System.IO.FileStream(Application.StartupPath + Program.cDATA_PATH + "License.md",System.IO.FileMode.Open);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            richTextBox1.Text = reader.ReadToEnd();
            reader.Close();
        }
    }
}
