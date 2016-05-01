using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_Formations
{
    public class Column
    {
        public Column(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public bool Equals(Column obj)
        {
            return Name.Equals(obj.Name);
        }
    }
}
