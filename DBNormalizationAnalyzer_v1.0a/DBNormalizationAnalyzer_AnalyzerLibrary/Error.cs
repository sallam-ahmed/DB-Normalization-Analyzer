using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DBNormalizationAnalyzer.AnalyzerLibrary
{
    [Serializable]
    public struct Error : ISerializable
    {
        public string Message { get; set; }
        public int Level { get; set; }

        
        public List<Tuple<BitArray, BitArray>> SuggestedSplit;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("lvl", Level);
            info.AddValue("msg", Message);
            var suggestionsList = new List<string>();
            foreach (var suggestion in SuggestedSplit)
            {
                suggestionsList.Add(suggestion.Item1.ToBitString());
                suggestionsList.Add(suggestion.Item2.ToBitString());
            }
            info.AddValue("split", suggestionsList, typeof(List<string>));
        }

        public Error(SerializationInfo info, StreamingContext context)
        {
            Message = info.GetString("msg");
            Level = info.GetInt32("lvl");
            var suggestionsList = (List<string>)info.GetValue("split", typeof(List<string>));
            SuggestedSplit = new List<Tuple<BitArray, BitArray>>();
            for (var i = 0; i < suggestionsList.Count; i += 2)
            {
                SuggestedSplit.Add(new Tuple<BitArray, BitArray>(suggestionsList[i].ToBitArray(),
                    suggestionsList[i + 1].ToBitArray()));
            }
        }

        public Error(string msg, int lvl)
        {
            Message = msg;
            Level = lvl;
            SuggestedSplit = new List<Tuple<BitArray, BitArray>>();
        }
    }
}
