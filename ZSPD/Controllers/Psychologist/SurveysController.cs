using Microsoft.AspNet.Identity;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ZSPD.Domain.Managers;
using ZSPD.Domain.Models.EntityModels;
using ZSPD.Models;

namespace ZSPD.Controllers.Psychologist
{
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
            return View();
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
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        //...
    }
}