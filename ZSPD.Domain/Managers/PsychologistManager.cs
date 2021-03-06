﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        public void AddSurveyToStudent(int surveyId, string studentId)
        {
            var surveyToAdd = this.GetSurvey(surveyId);

            _context.Students.First(x => x.Id == studentId).ActiveSurvey = surveyToAdd;
            _context.SaveChanges();
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
            return _context.Students.ToList();
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
                _context.Surveys.Attach(survey);
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

        public void RemoveQuestion(Question question)
        {
            _context.Questions.Attach(question);
            _context.Questions.Remove(question);
            _context.SaveChanges();
        }

        public void AddQuestionFromExcel(string path, string authorId)
        {
            var question = new Question();
            var answer = new Answer();
            string questionSTR;
           DataTable table = ReadExcel(path);
            foreach (DataRow row in table.Rows)
            {
                if ((row.ItemArray[0].ToString()) == "")
                {
                    break;
                }
                if (row.ItemArray[0].ToString() != "")
                {
                    questionSTR = row.ItemArray[0].ToString();
                    AddQuestion(questionSTR, authorId);
                }

            }
        }

        private DataTable ReadExcel(string path)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage(new FileInfo(path)))
            {

                ExcelWorksheet worksheet = pck.Workbook.Worksheets.First();
                DataTable dt = new DataTable();
                foreach (var header in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    dt.Columns.Add(header.Text);
                }
                for (var numRow = 1; numRow <= worksheet.Dimension.End.Row; numRow++)
                {
                    var row = worksheet.Cells[numRow, 1, numRow, worksheet.Dimension.End.Column];
                    var newRow = dt.NewRow();
                    foreach (var cell in row)
                    {
                        newRow[cell.Start.Column - 1] = cell.Text;
                    }
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
        }
    }
}
