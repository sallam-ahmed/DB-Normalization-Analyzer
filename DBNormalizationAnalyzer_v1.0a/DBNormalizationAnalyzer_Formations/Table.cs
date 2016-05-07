using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DBNormalizationAnalyzer.AnalyzerLibrary;

namespace DBNormalizationAnalyzer.Formations
{
    [Serializable] //Hence built in members are serializables it will be serialized auto
    public class Table
    {
        public string Name { get; set; }
        public int ColumnsCount { get; }
        public List<Column> Columns { get; set; }
        public FunctionalDependency TableDependency { get; set; }
        public Table Self { get { return this; } }
        public List<Column> PrimaryKey { get; set; } 
        
        public Table(string name ,int columnsCount)
        {
            Name = name;
            ColumnsCount = columnsCount;
            Columns = new List<Column>(ColumnsCount);
            Columns.AddRange(Enumerable.Repeat(new Column(""),columnsCount));
            TableDependency = new FunctionalDependency(columnsCount, new BitArray(columnsCount));
        }
        public Table(string name,List<Column> columns)
        {
            Name = name;
            Columns = columns;
            ColumnsCount = Columns.Count;
            TableDependency = new FunctionalDependency(columns.Count, new BitArray(columns.Count));
        }

        public Table(string name,List<Column> columns,List<Column> primaryKey)
        {
            Name = name;
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

        public List<Column> BitList(BitArray columns)
        {
            if(columns.Count != Columns.Count)
                throw new ArgumentException();
            return Columns.Where((t, i) => columns[i]).ToList();
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
