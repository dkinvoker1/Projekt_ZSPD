using System.Collections.Generic;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Domain.Managers
{
    public interface IGraphManager
    {
        void AddGraphFromFiles(string subjectName, string path1, string path2, string path3);
        List<SubjectGraph> GetAllGraphs();
    }
}