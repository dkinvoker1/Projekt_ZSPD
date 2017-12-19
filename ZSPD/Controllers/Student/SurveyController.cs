using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

using System.Web.Mvc;
using ZSPD.Domain.Managers;
using ZSPD.Models;
using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Controllers.Student
{
    [Authorize(Roles = Roles.User)]
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
            _studentManager.SaveAnswers(SVM.Answers, User.Identity.GetUserId());
            return RedirectToAction("Users", "Home");
        }
    }
}