using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer.Formations
{
    [Serializable]
    public class Column : ISerializable
    {
        
        public Column(string name)
        {
            Name = name;
        }
        public Column()
        {
            Name = "DEF";
        }
        public Column(SerializationInfo info,StreamingContext context)
        {
            Name = info.GetValue("name", typeof(string)) as string;
        }
        [DisplayName("Col Name")]
        public string Name { get ; set; }

        public bool Equals(Column obj)
        {
            return Name.Equals(obj.Name);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name, typeof(string));
        }
        
    }
}
