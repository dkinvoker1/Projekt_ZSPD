using System;
using System.Collections.Generic;
using System.Linq;
using ZSPD.Domain.Models;
using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Managers
{
    public class PsychologistManager : IPsychologistManager
    {
		private ApplicationDbContext _context = new ApplicationDbContext();
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
			return _context.Questions.ToList();
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
			return _context.Psychologists.SingleOrDefault(x => x.Id == userId);

		}

        public List<Survey> GetSurvey(string surveyId)
        {
            throw new NotImplementedException();
        }

        public void RateQuestions(List<Grade> grades)
        {
            throw new NotImplementedException();
        }

		public void AddOrUpdateRateQuestion(int? grade, string userId, int questionId)
		{
			if (!grade.HasValue) grade = 0;
			var psychologist = GetPsychologist(userId);

			if (psychologist.QuestionsGrades.Any(x => x.Question.Id == questionId))
			{
				var question = psychologist.QuestionsGrades.Single(x => x.Question.Id == questionId);
				question.Value = grade.Value;
				_context.SaveChanges();
				return;
			}

			Grade g = new Grade();
			g.Value = grade.Value;
			g.Question = _context.Questions.Single(x => x.Id == questionId);

			psychologist.QuestionsGrades.Add(g);
			_context.SaveChanges();

		}
	}
}
