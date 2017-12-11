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
		public void AddQuestion(string question, string authorId)
        {
            var newQuestion = new Question
            {
                Content = question,
                Author = _context.Psychologists.First(x => x.Id == authorId)
            };

            _context.Questions.Add(newQuestion);
            _context.SaveChanges();
        }

        public void AddSurvey(List<Question> questions, string authorId)
        {
            if (questions.Count > 0)
            {
                var questionsToAdd = GetAllQuestions().Where(x => questions.Any(y => y.Id == x.Id)).ToList();

                Survey newSurvey = new Survey
                {
                    Questions = questionsToAdd,
                    Author = _context.Psychologists.First(x => x.Id == authorId),
                    CreateDate = DateTime.Now
                };

                _context.Surveys.Add(newSurvey);
                _context.SaveChanges();
            }
        }

        public void AddSurveyToStudent(Survey survey, string studentId)
        {
            throw new NotImplementedException();
        }

        public List<Question> GetAllQuestions()
        {
			return _context.Questions.ToList();
		}

        List<Question> IPsychologistManager.GetOwnQuestions(string userId)
        {
            return _context.Questions.Where(x => x.Author.Id == userId).ToList();
        }

        public List<Student> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetAllSurveys()
        {
            return _context.Surveys.ToList();
        }

        public List<Survey> GetOwnSurveys(string userId)
        {
            return _context.Surveys.Where(x => x.Author.Id == userId).ToList();
        }

        public Psychologist GetPsychologist(string userId)
        {
			return _context.Psychologists.SingleOrDefault(x => x.Id == userId);

		}

        public Survey GetSurvey(int surveyId)
        {
            return _context.Surveys.FirstOrDefault(x => x.Id == surveyId);
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

        public void RemoveSurvey(int surveyId)
        {
            var survey = _context.Surveys.FirstOrDefault(x => x.Id == surveyId);
            if (survey != null)
            {
                _context.Surveys.Remove(survey);
                _context.SaveChanges();
            }
        }

        public void EditSurvey(List<Question> questions, int surveyId)
        {
            var survey = _context.Surveys.FirstOrDefault(x => x.Id == surveyId);

            if (survey != null)
            {
                if (questions.Count <= 0)
                {
                    RemoveSurvey(surveyId);
                }
                else
                {
                    var questionsToAdd = GetAllQuestions().Where(x => questions.Any(y => y.Id == x.Id)).ToList();

                    if (survey != null)
                    {
                        survey.Questions.Clear();
                        _context.SaveChanges();

                        survey.Questions = questionsToAdd;
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void EditQuestion(Question question)
        {
            var questionToChange = _context.Questions.FirstOrDefault(x => x.Id == question.Id);
            questionToChange.Content = question.Content;
            _context.SaveChanges();
        }
    }
}
