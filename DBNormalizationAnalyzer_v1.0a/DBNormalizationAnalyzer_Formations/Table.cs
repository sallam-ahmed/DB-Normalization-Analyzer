using System;
using System.Collections;
using System.Collections.Generic;
using DBNormalizationAnalyzer_AnalyzerLibrary;

namespace DBNormalizationAnalyzer_Formations
{
    public class Table
    {
        public int ColumnsCount { get; private set; }
        public List<Column> Columns { get; set; }
        public FunctionalDependency TableDependency { get; set; }
        
        public List<Column> PrimaryKey { get; set; } 
        
        public Table(int columnsCount)
        {
            ColumnsCount = columnsCount;
            Columns = new List<Column>(ColumnsCount);
            TableDependency = new FunctionalDependency(columnsCount, new BitArray(columnsCount));
        }
        public Table(List<Column> columns)
        {
            Columns = columns;
            ColumnsCount = Columns.Count;
            TableDependency = new FunctionalDependency(columns.Count, new BitArray(columns.Count));
        }

        public Table(List<Column> columns,List<Column> primaryKey)
        {
            PrimaryKey = primaryKey;
            Columns = columns;
            ColumnsCount = Columns.Count;
            TableDependency = new FunctionalDependency(columns.Count, ColumnSet(primaryKey));
        }

        public BitArray ColumnSet(List<Column> columns)
        {
            var res = new BitArray(ColumnsCount);
            foreach (var column in columns)
            {
                var index = -1;
                for(var i = 0;i < Columns.Count;i++)
                {
                    if (Columns[i].Equals(column))
                        index = i;
                }
                if(index == -1)
                    throw new ArgumentOutOfRangeException();
                res[index] = true;
            }
            return res;
        }

        public void RemoveColumn(Column column)
        {
            var index = -1;
            for(var i = 0;i < Columns.Count;i++)
                if (Columns[i].Equals(column))
                    index = i;
            for(var i = 0;i < PrimaryKey.Count;i++)
                if (PrimaryKey[i].Equals(column))
                {
                    PrimaryKey.RemoveAt(i);
                    i--;
                }
            if (index == -1)
                throw new ArgumentOutOfRangeException();
            TableDependency.RemoveKey(index);
            Columns.RemoveAt(index);
        }

        public void AddColumn(Column column)
        {
            if(Columns.Contains(column))
                throw new ArgumentException();
            Columns.Add(column);
            TableDependency.AddKey();
        }
    }
}
