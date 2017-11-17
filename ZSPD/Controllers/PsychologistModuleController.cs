using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSPD.Controllers
{
    public class PsychologistModuleController : Controller
    {
        // GET: PsychologistModule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateSurvey()
        {
            return View();
        }
        
        public ActionResult AssignSurvey()
        {
            return View();
        }

        public ActionResult ModifyQuestions()
        {
            return View();
        }

        public ActionResult RateQuestions()
        {
            return View();
        }

        public ActionResult ManageSurveys()
        {
            return View();
        }

        public ActionResult RateSurveys()
        {
            return View();
        }
    }
}