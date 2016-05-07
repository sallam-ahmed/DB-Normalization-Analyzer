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
        public ProjectJSON ProjectJson {
            get
            {
                ProjectJSON PJ = new ProjectJSON();
                PJ.Name = ProjectName;
                PJ.LastOpenDate = ProjectLastOpenDate;
                PJ.Author = ProjectAuthor;
                PJ.Description = ProjectDescription;
                PJ.Path = ProjectPath;
                return PJ;
            }
            }

        public Project()
        {
            ProjectName = "01";
        }

        public Project(string _name,string _author,string _path,string _description,DateTime date)
        {
            Tables = new List<Table>();

            ProjectPath = _path;
            ProjectName = _name;
            ProjectAuthor = _author;
            ProjectDescription = _description;
            ProjectLastOpenDate = date;

           // DataManager.CreateProject(this);
        }
        public Project(string _path)
        {
            var loadedProj = DataManager.ReadProject(_path);

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
    public struct ProjectJSON
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime LastOpenDate { get; set; }
    }
}
