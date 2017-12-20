using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSPD.Controllers.Student
{
    public class ExerciseController : Controller
    {
        // GET: Exercise
        public ActionResult ChoseYourLevel()
        {
            return View();
        }

        public ActionResult ChoseExerciseLevel()
        {
            return View();
        }
    }
}