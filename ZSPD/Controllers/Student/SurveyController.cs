using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

using System.Web.Mvc;
using ZSPD.Domain.Managers;


namespace ZSPD.Controllers.Student
{
    public class SurveyController : Controller
    {
        private IStudentManager _studentManager;

        public SurveyController(IStudentManager studentManager)
        {
            _studentManager = studentManager;
        }

        
        public ActionResult ChooseSurvey()
        {
            var surveys = _studentManager.GetAllSurveys();
            return View(surveys);
        }

        [HttpGet]
        public ActionResult ChooseSurvey(int surveyID)
        {
            //Convert.ToInt32(Request[])
            //var surveyID = Convert.ToInt32(Request["surveyID"]);
            var survey = _studentManager.GetSurvey(surveyID);
            var user = User.Identity.GetUserId();

            _studentManager.SetActiveSurvey(survey, user);

            return RedirectToAction("FillSurvey", "Survey"); 
        }
   
        public ActionResult FillSurvey()
        {
            var user = User.Identity.GetUserId();
            var survey = _studentManager.GetActiveSurvey(user);
            return View(survey);
        }

        //[HttpPost]
        //public ActionResult FillSurvey()
        //{
        //    return RedirectToAction("Index", "Home");
        //}

        //...
    }
}