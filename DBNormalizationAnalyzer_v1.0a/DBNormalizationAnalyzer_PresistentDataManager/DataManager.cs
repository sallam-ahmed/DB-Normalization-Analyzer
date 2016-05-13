using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
namespace DBNormalizationAnalyzer.PresistentDataManager
{
    
    public class DataManager
    {

        #region # Fields #
        private static BinaryFormatter m_Formatter;
        private static FileStream m_FileStream;
        private static JavaScriptSerializer m_JSerializer;
        #endregion
        #region # Properties #
        public static string ConfigurationFile { get; set; }
        public const string RecentProjectsFilePath = "D:\\MOCK.json";
        #endregion
        public DataManager()
        {
            m_Formatter = new BinaryFormatter();
        }
        public static void CreateProject(Project project)
        {
            if (File.Exists(project.ProjectPath))
                File.Delete(project.ProjectPath);
            using (m_FileStream = new FileStream(project.ProjectPath, FileMode.Create))
            {
                m_Formatter = new BinaryFormatter();
                m_Formatter.Serialize(m_FileStream, project);
            }
            //Create JSON Script
            string jSonString;
            using (StreamReader sr = new StreamReader(new FileStream(RecentProjectsFilePath, FileMode.OpenOrCreate)))
            {
                jSonString = sr.ReadToEnd();
            }
            m_JSerializer = new JavaScriptSerializer();
            var data = m_JSerializer.Deserialize<List<ProjectJson>>(jSonString);
            if (data == null)
                data = new List<ProjectJson>();

            data.Add(project.ProjectJson);
            m_FileStream = new FileStream(RecentProjectsFilePath, FileMode.OpenOrCreate);
            jSonString = m_JSerializer.Serialize(data);
            using (StreamWriter sw = new StreamWriter(m_FileStream))
            {
                sw.WriteLine(jSonString);
                sw.Close();
            }
            
        }
        public static List<ProjectJson> LoadRecentProjects()
        {
            if(File.Exists(RecentProjectsFilePath))
                return (new JavaScriptSerializer().Deserialize<List<ProjectJson>>(File.ReadAllText(RecentProjectsFilePath)));
            else
            {
                return new List<ProjectJson>(0);
            }
        }
        public static void UpdateRecentProjects(List<ProjectJson> _data)
        {

        }
        public static Project ReadProject(string path)
        {
            using (m_FileStream = new FileStream(path, FileMode.Open))
            {
                m_Formatter = new BinaryFormatter();
                return m_Formatter.Deserialize(m_FileStream) as Project;
            }
        }
    }
}
