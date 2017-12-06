using System;
using System.Collections.Generic;
using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Domain.Models.EntityModels;
using System.Linq;

using ZSPD.Domain.Models;


namespace ZSPD.Domain.Managers
{
    public class StudentManager : IStudentManager
    {

        public Models.EntityModels.Survey GetActiveSurvey(string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if( userID != null){
                    var survey = db.Students.FirstOrDefault(x => x.Id == userID).ActiveSurvey;
                    return survey;
                }
                return null;
            }
        }

        public void SaveAnswers(List<Answer> answers, string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (answers != null && userID != null){
                    db.Students.FirstOrDefault(x => x.Id == userID).Answers = answers;
                    db.Students.FirstOrDefault(x => x.Id == userID).SurveyIsStarted = true;
                    db.SaveChanges();
                }
            }
        }
    }
}
