using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_Formations
{
    public class Database
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string ProjectName { get; set; }
        public string Describtion { get; set; }
        public Database()
        {
            Name = Author = ProjectName = Describtion = null;
        }
        public Database(string name,string author,string projectName,string describtion)
        {
            Name = name;
            Author = author;
            ProjectName = projectName;
            Describtion = describtion;
        }
    }
}
