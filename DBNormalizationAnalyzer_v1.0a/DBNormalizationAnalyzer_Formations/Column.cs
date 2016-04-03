using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_Formations
{
    public enum DataType
    {
        CHARACTER,
        VARCHAR,
        BINARY,
        BOOLEAN,
        VARBINARY,
        INTEGER,
        SMALLINT,
        BIGINT,
        DECIMAL,
        NUMERIC,
        FLOAT,
        REAL,
        DOUBLE_PRECISION,
        DATE,
        TIME,
        TIMESTAMP,
        INTERVAL,
        ARRAY,
        MULTISET,
        XML,
        NAN
    }
    public class Column
    {
        public Column()
        {
            Name = null;
            DType = DataType.NAN;
            this.Value = null;
        }
        public Column(string name,DataType type,object value,Constraints constraints)
        {
            Name = name;
            DType = type;
            this.Value = value;
            this.Constraints = constraints;
        }
        public string Name { get; set; }
        public DataType DType { get; set; }
        public object Value { get; set; }
        public Constraints Constraints { get; set; }
    }
}
