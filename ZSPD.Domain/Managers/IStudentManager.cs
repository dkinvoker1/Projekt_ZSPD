using System.Collections.Generic;

using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Domain.Managers
{
    public interface IStudentManager
    {
        

        Models.EntityModels.Survey GetActiveSurvey(string userID);

        void SaveAnswers(List<Answer> answers, string userID);

        void GetExcerciseOrder();

        int GetStudentAdvacement(int advacement);

        int GetNextExcerciseNumber(int excerciseNumber, bool answer, string userID);

    }
}
