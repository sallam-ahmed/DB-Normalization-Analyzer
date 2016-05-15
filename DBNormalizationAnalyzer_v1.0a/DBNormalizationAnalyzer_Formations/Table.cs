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
        public List<Column> Columns { get; set; }
        public FunctionalDependency TableDependency { get; set; }
        public Table Self => this;
        public List<Column> PrimaryKey { get; set; } 
        
        public Table(string name ,int columnsCount)
        {
            Name = name;
            Columns = new List<Column>(columnsCount);
            Columns.AddRange(Enumerable.Repeat(new Column("NEW_COLUMN"),columnsCount));
            PrimaryKey = new List<Column>();
            TableDependency = new FunctionalDependency(columnsCount, new BitArray(columnsCount));
        }
        public Table(string name,List<Column> columns)
        {
            Name = name;
            Columns = columns;
            PrimaryKey = new List<Column>();
            TableDependency = new FunctionalDependency(columns.Count, new BitArray(columns.Count));
        }

        public Table(string name,List<Column> columns,List<Column> primaryKey)
        {
            Name = name;
            PrimaryKey = primaryKey;
            Columns = columns;
            TableDependency = new FunctionalDependency(columns.Count, ColumnSet(primaryKey));
        }

        public BitArray ColumnSet(List<Column> columns)
        {
            var res = new BitArray(Columns.Count);
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

        public List<Column> ColumnSet(BitArray columns)
        {
            if (columns.Count != Columns.Count)
                throw new ArgumentException();
            return Columns.Where((t, i) => columns[i]).ToList();
        }

        public List<Column> ColumnSet(List<string> columns)
        {
            return Columns.Where(t => columns.Contains(t.Name)).ToList();
        }

        public void RemoveColumn(int index)
        {
            if (index >= Columns.Count)
                return;
            var column = Columns[index];
            for(var i = 0;i < PrimaryKey.Count;i++)
                if (PrimaryKey[i].Equals(column))
                {
                    PrimaryKey.RemoveAt(i);
                    i--;
                }
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
