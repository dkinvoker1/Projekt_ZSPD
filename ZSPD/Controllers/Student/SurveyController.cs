using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

using System.Web.Mvc;
using ZSPD.Domain.Managers;
using ZSPD.Models;


namespace ZSPD.Controllers.Student
{
    public class SurveyController : Controller
    {
        private IStudentManager _studentManager;

        public SurveyController(IStudentManager studentManager)
        {
            _studentManager = studentManager;
        }
    
        public ActionResult FillSurvey()
        {
            var user = User.Identity.GetUserId();

            SurveyViewModel SVM = new SurveyViewModel();
            SVM.activeSurvey = _studentManager.GetActiveSurvey(user);

            return View(SVM);
        }

        [HttpPost]
        public ActionResult FillSurvey(SurveyViewModel SVM)
        {
            var answers = SVM.Answers;
            var user = User.Identity.GetUserId();
            var survey = _studentManager.GetActiveSurvey(user);
            var questions = survey.Questions.ToList();
            for (int i = 0; i < answers.Count; i++)
            {
                answers[i].AnswerDate = DateTime.Now;
                answers[i].Survey = survey;
                answers[i].Question = questions[i];
            }

            _studentManager.SaveAnswers(answers, user);
            return RedirectToAction("Users", "Home");
        }
    }
}