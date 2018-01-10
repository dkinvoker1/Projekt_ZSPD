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
            string userId = User.Identity.GetUserId();
            if(_studentManager.SolvedAnyExercise(userId))
            {
                int exNumber = _studentManager.GetResolvedExerciseNumber(userId);
                return RedirectToAction("ChoseExerciseLevel", "Exercise", new { exerc = exNumber });
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChoseYourLevel(StudentAdvancementViewModel vm)
        {
            int exNumber = _studentManager.GetStudentAdvacement(vm.Advacement); //poprawić, żeby się zwracało faktycznie
            return RedirectToAction("ChoseExerciseLevel", "Exercise", new { exerc = exNumber });
        }

        public ActionResult ChoseExerciseLevel(int? exerc)
        {
            string userId = User.Identity.GetUserId();
            int exercise = 0;
            if (exerc != null)
            {
                exercise = (int)exerc;
            }

            var ex = _studentManager.GetNextExcerciseNumber(exercise, true, userId);

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
            string userId = User.Identity.GetUserId();
            vm.NextExcerciseNumber = _studentManager.GetNextExcerciseNumber(vm.ExcerciseNumber, vm.Answer, userId);
            return View(vm);
        }

    }
}