using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_Formations
{
    public class Table
    {
        public int ColumnsCount { get; private set; }
        public List<Column> Columns { get; set; }

        public Table()
        {
            ColumnsCount = -1;
            Columns = null;
        }
        public Table(int _columnsCount)
        {
            ColumnsCount = _columnsCount;
            Columns = new List<Column>(ColumnsCount);
        }
        public Table(List<Column> _columns)
        {
            Columns = _columns;
            ColumnsCount = Columns.Count;
            Tuple<List<Column>, List<Column>> stat = new Tuple<List<Column>, List<Column>>(Columns,Columns);
            
        }

    }
}
