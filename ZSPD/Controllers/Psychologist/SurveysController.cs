using Microsoft.AspNet.Identity;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ZSPD.Domain.Managers;
using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Models;

namespace ZSPD.Controllers.Psychologist
{
    [Authorize(Roles = Roles.Psychologist)]
    public class SurveysController : Controller
    {
        private IStudentManager _studentManager;
        private IPsychologistManager _psychologistManager;

		public SurveysController(IStudentManager studentManager, IPsychologistManager psychologistManager)
        {
            _studentManager = studentManager;
            _psychologistManager = psychologistManager;
		}

        public ActionResult Create()
        {
            List<CreateSurveyViewModel> questions = new List<CreateSurveyViewModel>();
            var allQuestions = _psychologistManager.GetAllQuestions();

            foreach (var question in allQuestions)
            {
                questions.Add(new CreateSurveyViewModel
                {
                    Question = question,
                    AddToSurvey = false
                });
            }

            return View(questions);
        }

        [HttpPost]
        public ActionResult Create(List<CreateSurveyViewModel> questions)
        {
            var questionsToAdd = questions.Where(x => x.AddToSurvey == true).Select(x => x.Question).ToList();

            _psychologistManager.AddSurvey(questionsToAdd, User.Identity.GetUserId());

            return RedirectToAction("Psychologist","Home");
        }

        public ActionResult Assign()
        {
            return View();
        }


		public ActionResult RateQuestions()
		{
			string userId = User.Identity.GetUserId();

			var reatedQuestions = _psychologistManager.GetPsychologist(userId).QuestionsGrades.ToList();
			var listofAllQuestions = _psychologistManager.GetAllQuestions();
			var model = new RateQuestionViewModel();
			model.QuestionRates = new List<QuestionRate>();
			model.UserId = userId;
			foreach (Question question in listofAllQuestions)
			{
				QuestionRate singleRate = new QuestionRate();
				singleRate.Id = question.Id;
				singleRate.Question = question.Content;
				var reatedQuestion = reatedQuestions.SingleOrDefault(x => x.Question.Id == question.Id);
				singleRate.Grade = reatedQuestion?.Value;

				model.QuestionRates.Add(singleRate);
			}

			return View(model);
		}

		public JsonResult SaveRateForQuestion(int? grade, int questionId, string userId)
		{
			userId = User.Identity.GetUserId();
            _psychologistManager.AddOrUpdateRateQuestion(grade, userId, questionId);
			string message = "success";
			return Json(message, JsonRequestBehavior.AllowGet);
		}

        public ActionResult ModifyQuestions()
        {
            return View();
        }


        public ActionResult Manage()
        {
            var surveys = _psychologistManager.GetOwnSurveys(User.Identity.GetUserId());

            return View(surveys);
        }

        public ActionResult RemoveSurvey(int surveyId)
        {
            var survey = _psychologistManager.GetSurvey(surveyId);

            if (survey != null && survey.Author.Id == User.Identity.GetUserId())
            {
                _psychologistManager.RemoveSurvey(surveyId);
            }

            return RedirectToAction("Manage");
        }

        public ActionResult Edit(int surveyId)
        {
            var survey = _psychologistManager.GetSurvey(surveyId);
            var allQuestions = _psychologistManager.GetAllQuestions();

            if (survey != null && survey.Author.Id == User.Identity.GetUserId())
            {
                EditSurveyViewModel editVM = new EditSurveyViewModel()
                {
                    Survey = survey,
                    Questions = new List<CreateSurveyViewModel>()
                };

                foreach (var question in allQuestions)
                {
                    editVM.Questions.Add(new CreateSurveyViewModel
                    {
                        Question = question,
                        AddToSurvey = survey.Questions.Any(x => x.Id == question.Id),
                    });
                }
                return View(editVM);
            }
            else
            {
                return RedirectToAction("Psychologist", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(EditSurveyViewModel editVM)
        {
            var newQuestions = editVM.Questions.Where(x => x.AddToSurvey == true)
                                                .Select(x => x.Question)
                                                .ToList();

            _psychologistManager.EditSurvey(newQuestions, editVM.Survey.Id);

            return RedirectToAction("Manage");
        }
        //...
    }
}