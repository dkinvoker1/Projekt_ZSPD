using System.Collections.Generic;

using ZSPD.Domain.Diagrams.Database;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Managers
{
    public interface IStudentManager
    {
        Survey GetSurvey(string userId);
        void SaveAnswers(List<Answer> answers);

        Student GetStudent(string userId);
        //...
    }
}
