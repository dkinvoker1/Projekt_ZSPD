using System.Collections.Generic;

using ZSPD.Domain.Diagrams.Database;
using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Domain.Managers
{
    public interface IStudentManager
    {
        List<Models.EntityModels.Survey> GetAllSurveys();

        Models.EntityModels.Survey GetSurvey(int surveyId);

        void SetActiveSurvey(Models.EntityModels.Survey survey, string userID);
        Models.EntityModels.Survey GetActiveSurvey(string userID);

        Student GetStudent(string userId);

        void SaveAnswers(List<Models.EntityModels.Answer> answers);


        //...
    }
}
