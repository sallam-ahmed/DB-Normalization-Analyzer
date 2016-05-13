using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBNormalizationAnalyzer.AnalyzerLibrary;
using DBNormalizationAnalyzer.Formations;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

namespace DBNormalizationAnalyzer.PresistentDataManager
{
    [Serializable]
    public class Project
    {
        [DataMember]
        public string ProjectPath { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectAuthor { get; set; }
        public List<Table> Tables { get; set; }
        public string Log { get; set; }
        [DataMember]
        public string ProjectDescription { get; set; }
        [DataMember]
        public DateTime ProjectLastOpenDate { get; set; }
        public ProjectJson ProjectJson {
            get
            {
                var pj = new ProjectJson
                {
                    Name = ProjectName,
                    LastOpenDate = ProjectLastOpenDate,
                    Author = ProjectAuthor,
                    Description = ProjectDescription,
                    Path = ProjectPath
                };
                return pj;
            }
            }

        public Project()
        {
            ProjectName = "New Project";
        }

        public Project(string name,string author,string path,string description,DateTime date)
        {
            Tables = new List<Table>();

            ProjectPath = path;
            ProjectName = name;
            ProjectAuthor = author;
            ProjectDescription = description;
            ProjectLastOpenDate = date;

           // DataManager.CreateProject(this);
        }
        public Project(string path)
        {
            var loadedProj = DataManager.ReadProject(path);

            ProjectName = loadedProj.ProjectName;
            ProjectPath = loadedProj.ProjectPath;
            ProjectDescription = loadedProj.ProjectDescription;
            ProjectLastOpenDate = loadedProj.ProjectLastOpenDate;
            ProjectAuthor = loadedProj.ProjectAuthor;
            Log = loadedProj.Log;

            foreach (var item in loadedProj.Tables)
            {
                Tables.Add(item);
            }

        }
    }
    [DataContract]
    public struct ProjectJson
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime LastOpenDate { get; set; }
    }
}
