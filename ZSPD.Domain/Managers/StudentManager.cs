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
                var survey = db.Students.FirstOrDefault(x => x.Id == userID).ActiveSurvey;
                if (survey != null)
                {
                    survey.Questions = survey.Questions.ToList();
                    return survey;
                }
                return null;
            }
        }

        public void SaveAnswers(List<Answer> answers, string userID)
        {
            var survey = GetActiveSurvey(userID);
            var questions = survey.Questions.ToList();

            for(int i = 0; i < questions.Count; i++)
            {
                answers[i].QuestionId = questions[i].Id;
            }

            var completedSurvey = new CompletedSurvey()
            {
                Answers = answers,
                DateOfComplete = DateTime.Now,
                SurveyId = survey.Id
            };

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userID);

                if (user != null)
                {
                    user.CompletedSurveys.Add(completedSurvey);

                    user.SurveyIsStarted = true;
                    db.SaveChanges();
                }
            }
        }
    }
}
