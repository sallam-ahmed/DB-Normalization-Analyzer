using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_PresistentDataManager
{
    public class DataManager
    {

        #region # Fields #
        private BinaryFormatter m_Formatter;
        private FileStream m_FileStream;

        #endregion
        #region # Properties #
        public string ConfigurationFile { get; set; }
        #endregion
        public DataManager()
        {
            m_Formatter = new BinaryFormatter();
            m_FileStream = new FileStream(ConfigurationFile, FileMode.Open);
        }

        public void WriteData()
        {

        }
        public void ReadData()
        {

        }
    }
}
