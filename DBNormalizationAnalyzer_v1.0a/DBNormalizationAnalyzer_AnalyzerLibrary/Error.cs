using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_AnalyzerLibrary
{
    struct Error
    {
        public string Message { get; }
        public int Level { get; }

        public Error(string msg, int lvl)
        {
            Message = msg;
            Level = lvl;
        }
    }
}
