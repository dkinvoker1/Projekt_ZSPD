using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSPD.Domain.Managers;
using ZSPD.Models;

namespace ZSPD.Controllers.Student
{
    public class ExerciseController : Controller
    {
        private IStudentManager _studentManager;

        public ExerciseController(IStudentManager studentManager)
        {
            _studentManager = studentManager;
        }

        // GET: Exercise
        public ActionResult ChoseYourLevel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChoseYourLevel(StudentAdvancementViewModel vm)
        {
            string userId = User.Identity.GetUserId(); 
            int exNumber = _studentManager.GetStudentAdvacement(vm.Advacement); //poprawić, żeby się zwracało faktycznie
            return RedirectToAction("ChoseExerciseLevel", "Exercise", new { exerc = exNumber });
        }

        public ActionResult ChoseExerciseLevel(int? exerc)
        {
            int exercise = 0;
            if (exerc != null)
            {
                exercise = (int)exerc;
            }

            var ex = _studentManager.GetNextExcerciseNumber(exercise, true);

            var vm = new ExcerciseViewModel()
            {
                ExcerciseNumber = exercise,
                Answer = true,
                NextExcerciseNumber = ex,
            };
            return View(vm);
        }



        [HttpPost]
        public ActionResult ChoseExerciseLevel(ExcerciseViewModel vm)
        {
            vm.NextExcerciseNumber = _studentManager.GetNextExcerciseNumber(vm.ExcerciseNumber, vm.Answer);
            return View(vm);
        }

    }
}