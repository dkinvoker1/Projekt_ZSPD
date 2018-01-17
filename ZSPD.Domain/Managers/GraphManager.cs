using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using ZSPD.Domain.Models;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Domain.Managers
{
    public class GraphManager : IGraphManager
    {
        public void AddGraphFromFiles(string subjectName, string path1, string path2, string path3)
        {
            // wczytanie z pliku tekstowego wartosci dla nastepnikow
            string poj = System.IO.File.ReadAllText(path1); // Cofnięcie o 1 folder w drzewku
            // wczytanie z pliku tekstowego wartosci dla poprzednikow
            string poj1 = System.IO.File.ReadAllText(path2);
            // wczytanie z pliku tekstowego zadan dla odp pojec
            string zad = System.IO.File.ReadAllText(path3);

            SubjectGraph graph = new SubjectGraph()
            {
                Name = subjectName,
                NextConcepts = poj,
                PreviousConcepts = poj1,
                Exercises = zad
            };

            using (var context = new ApplicationDbContext())
            {
                context.SubjectGraphs.Add(graph);
                context.SaveChanges();
            }
        }

        public List<SubjectGraph> GetAllGraphs()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.SubjectGraphs.ToList();
            }
        }


        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath; //was AbsolutePath but didn't work with spaces according to comments
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, "..\\.." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }
    }
}
