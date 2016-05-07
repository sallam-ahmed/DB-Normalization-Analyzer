using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBNormalizationAnalyzer.PresistentDataManager;

namespace DBNormalizationAnalyzer_UserInterface
{
    static class Program
    {
        #region # Configurations #
        public const string cDATA_PATH = "/app_data/";
        public const string cCONFIG_FILE = "systemdata.dat";
        #endregion
        #region # ProjectData #
        
        public static Project LoadedProject { get; set; }
        #endregion
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EntranceWindow());
        }
    }
}
