using System;
using System.Collections.Generic;
using ZSPD.Domain.Diagrams.Database;
using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Domain.Models.EntityModels;
using System.Linq;

using ZSPD.Domain.Models;


namespace ZSPD.Domain.Managers
{
    public class StudentManager : IStudentManager
    {

        public List<Models.EntityModels.Survey> GetAllSurveys()
        {
            List<Models.EntityModels.Survey> listaAnkiet = new List<Models.EntityModels.Survey>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                var ankiety = db.Surveys;
                foreach (var ankieta in ankiety)
                {
                    listaAnkiet.Add(new Models.EntityModels.Survey {
                        Id = ankieta.Id, Questions = ankieta.Questions ,
                        Author = ankieta.Author, CreateDate = ankieta.CreateDate });
                }
            }
            return listaAnkiet;
        }

        public Student GetStudent(string userId)
        {
            //string userId = User.Identity.GetUserId();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var student = db.Students.FirstOrDefault(x => x.Id == userId);

                return student;
            }

            throw new NotImplementedException();

        }

        public Models.EntityModels.Survey GetSurvey(int surveyId)
        {
            var surveys = GetAllSurveys();

            var survey = surveys.FirstOrDefault(x => x.Id == surveyId);
            return survey;

        }

        public Models.EntityModels.Survey GetActiveSurvey(string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                var survey = db.Students.FirstOrDefault(x => x.Id == userID).ActiveSurvey;
                return survey;
            }
        }

        public void SetActiveSurvey(Models.EntityModels.Survey survey, string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Students.FirstOrDefault(x => x.Id == userID).ActiveSurvey = survey;
                db.SaveChanges();              
            }
        }



        public void SaveAnswers(List<Models.EntityModels.Answer> answers)
        {
            throw new NotImplementedException();
        }


    }
}
