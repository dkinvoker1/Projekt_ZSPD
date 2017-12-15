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
    public class StudentsController : Controller
    {
        private IStudentManager _studentManager;
        private IPsychologistManager _psychologistManager;

        public StudentsController(IStudentManager studentManager, IPsychologistManager psychologistManager)
        {
            _studentManager = studentManager;
            _psychologistManager = psychologistManager;
        }


        public ActionResult Assign()
        {
            var students = _psychologistManager.GetAllStudents();
            var surveys = _psychologistManager.GetAllSurveys();

            AssignSurveyToStudentVM vm = new AssignSurveyToStudentVM
            {
                StudentsToAssign = new List<StudentToAssign>(),
                SurveysToAssign = new List<Survey>(),
            };

            foreach (var student in students)
            {
                vm.StudentsToAssign.Add(new StudentToAssign
                {
                    Student = student,
                    ToAssign = false,
                });
            }

            foreach (var survey in surveys)
            {
                vm.SurveysToAssign.Add(survey);
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult Assign(AssignSurveyToStudentVM vm)
        {
            var students = vm.StudentsToAssign.Where(x => x.ToAssign == true).Select(x => x.Student);

            foreach (var student in students)
            {
                _psychologistManager.AddSurveyToStudent(vm.SelectedSurveyId, student.Id);
            }

            return RedirectToAction("Assign");
        }
    }
}