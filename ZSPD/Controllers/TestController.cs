using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ZSPD.Domain.Models;
using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Controllers
{
    public class TestController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private Random _rand = new Random();


        // Surveys tests
        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult ShowAllQuestions()
        {
            var questions = _context.Questions.ToList();

            return View(questions);
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult AddQuestion()
        {
            var question = new Question
            {
                Content = $"Pytanie z losowym numerem: {_rand.Next(1, 1000)}?"
            };

            _context.Questions.Add(question);
            _context.SaveChanges();

            return RedirectToAction("ShowAllQuestions");
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult CreateSurvey(int id)
        {
            // Get questions
            var questions = _context.Questions.Take(id).ToList();

            // Get author
            string userId = User.Identity.GetUserId();
            var user = _context.Psychologists.FirstOrDefault(x => x.Id == userId);

            if(user == null)
            {
                return null;
            }

            // create the survey
            var survey = new Survey()
            {
                Questions = questions,
                CreateDate = DateTime.Now,
                Author = user
            };

            _context.Surveys.Add(survey);
            _context.SaveChanges();

            return RedirectToAction("ShowSurvey", new { id = survey.Id });
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult ShowAllSurveys()
        {
            var surveys = _context.Surveys;

            return View(surveys);
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult ShowSurvey(int id)
        {
            var survey = _context.Surveys.FirstOrDefault(x => x.Id == id);

            return View(survey);
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult ShowUserSurveys()
        {
            string userId = User.Identity.GetUserId();
            var user = _context.Psychologists.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return null;
            }

            var surveys = user.Surveys;

            return View(surveys);
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult ShowAllUsers()
        {
            var users = _context.AppUsers;

            return View(users);
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult AddSurveyToUser(string id)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.Id == id);
            if(user == null) { return null; }

            int surveyNumber = _rand.Next(0, _context.Surveys.Count()-1);

            var survey = _context.Surveys.ToList().ElementAt(surveyNumber);

            if(survey == null) { return null; }

            user.ActiveSurvey = survey;

            _context.SaveChanges();

            return RedirectToAction("ShowUser", new { id = id });
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult ShowUser(string id)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.Id == id);
            if (user == null) { return null; }

            return View("ShowUserData", user);
        }

        [Authorize(Roles = Roles.Psychologist)]
        public ActionResult RateQuestions()
        {
            string userId = User.Identity.GetUserId();
            var user = _context.Psychologists.FirstOrDefault(x => x.Id == userId);
            var userGrades = user.QuestionsGrades.ToList();
            var questions = _context.Questions.ToList();

            foreach(var question in questions)
            {
                var questionId = question.Id;

                var value = _rand.Next(1, 6);
                var userGrade = userGrades.FirstOrDefault(x => x.Question.Id == question.Id);

                if(userGrade == null)
                {
                    user.QuestionsGrades.Add(new Grade
                    {
                        Question = question,
                        Value = value
                    });
                }
                else
                {
                    userGrade.Value = value;
                }
            }

            _context.SaveChanges();

            return RedirectToAction("ShowActualUserData");
        }

        // Users
        [Authorize]
        public ActionResult ShowActualUserData()
        {
            var userId = User.Identity.GetUserId();

            if (User.IsInRole(Roles.User))
            {
                var user = _context.AppUsers.FirstOrDefault(x => x.Id == userId);
                return View("ShowUserData", user);
            }
            
            if(User.IsInRole(Roles.Psychologist))
            {
                var user = _context.Psychologists.FirstOrDefault(x => x.Id == userId);
                return View("ShowPsychologistData", user);
            }

            return null;
        }

        [Authorize(Roles=Roles.User)]
        public ActionResult FillSurvey()
        {
            string userId = User.Identity.GetUserId();
            var user = _context.AppUsers.FirstOrDefault(x => x.Id == userId);

            var survey = user.ActiveSurvey;

            foreach(var question in survey.Questions)
            {
                user.Answers.Add(
                    new Answer
                    {
                        Question = question,
                        AnswerRate = _rand.Next(1,5),
                        AnswerDate = DateTime.Now
                    }
                );
            }

            _context.SaveChanges();

            return RedirectToAction("ShowActualUserData");
        }


    }
}