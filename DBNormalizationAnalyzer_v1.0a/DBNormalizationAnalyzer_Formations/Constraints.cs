using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_Formations
{
    public class Constraints
    {
        #region Constructors
        public Constraints()
        {
            NotNull = false;
            Unique = false;
            DefaultValue = null;
            Check = false;
            CheckStatment = "";
        }
        public Constraints(bool notNull,bool isUnique,bool check,string checkStatment,object defaultValue)
        {
            NotNull = notNull;
            Unique = isUnique;
            DefaultValue = defaultValue;
            Check = check;
            CheckStatment = checkStatment;
        }
        public Constraints(bool notNull,bool isUnique,object defaultValue)
        {
            NotNull = notNull;
            Unique = isUnique;
            DefaultValue = defaultValue;
        }
        public Constraints(bool notNull,bool isUnique)
        {
            NotNull = notNull;
            Unique = isUnique;
        }
        #endregion
        public bool NotNull { get; set; }
        public bool Unique { get; set; }
        public object DefaultValue { get; set; }
        public bool Check { get; set; }
        public string CheckStatment { get; set; }
    }
}
