using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_AnalyzerLibrary
{
    public struct Error
    {
        public string Message { get; set; }
        public int Level { get; set; }
        
        public List<Tuple<BitArray, BitArray>> SuggestedSplit;

        public Error(string msg, int lvl)
        {
            Message = msg;
            Level = lvl;
            SuggestedSplit = new List<Tuple<BitArray, BitArray>>();
        }
    }
}
