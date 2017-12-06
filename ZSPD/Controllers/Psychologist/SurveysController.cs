﻿using System.Web.Mvc;

using ZSPD.Domain.Managers;

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

        public ActionResult ModifyQuestions()
        {
            return View();
        }

        public ActionResult RateQuestions()
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