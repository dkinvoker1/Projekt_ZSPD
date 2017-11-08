using System;
using System.Collections.Generic;

using ZSPD.Domain.Diagrams.Database;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Managers
{
    public class StudentManager : IStudentManager
    {
        public Student GetStudent(string userId)
        {
            throw new NotImplementedException();
        }

        public Survey GetSurvey(string userId)
        {
            throw new NotImplementedException();
        }

        public void SaveAnswers(List<Answer> answers)
        {
            throw new NotImplementedException();
        }
    }
}
