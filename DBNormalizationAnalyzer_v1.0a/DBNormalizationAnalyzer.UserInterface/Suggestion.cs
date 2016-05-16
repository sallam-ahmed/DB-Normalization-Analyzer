using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBNormalizationAnalyzer.AnalyzerLibrary;
using DBNormalizationAnalyzer.Formations;

namespace DBNormalizationAnalyzer_UserInterface
{
    public partial class Suggestion : Form
    {
        private readonly Error _error;

        public Suggestion(Error error)
        {
            _error = error;
            InitializeComponent();
        }

        private void Suggestion_Load(object sender, EventArgs e)
        {
            errLevel.Text = _error.Level.ToString();
            errMessage.Text = _error.Message;
            //TODO Display suggestions.
        }

    }
}
