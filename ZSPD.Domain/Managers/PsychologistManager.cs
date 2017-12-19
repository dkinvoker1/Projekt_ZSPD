using System;
using System.Collections.Generic;

using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Managers
{
    public class PsychologistManager : IPsychologistManager
    {
        public void AddQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public void AddSurvey(Survey survey)
        {
            throw new NotImplementedException();
        }

        public void AddSurveyToStudent(Survey survey, string studentId)
        {
            
            throw new NotImplementedException();
        }

        public List<Question> GetAllQuestions()
        {
            throw new NotImplementedException();
        }

        public List<Student> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetAllSurveys()
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetOwnSurveys(string userId)
        {
            throw new NotImplementedException();
        }

        public Psychologist GetPsychologist(string userId)
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetSurvey(string surveyId)
        {
            throw new NotImplementedException();
        }

        public void RateQuestions(List<Grade> grades)
        {
            throw new NotImplementedException();
        }
    }
}
