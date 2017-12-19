using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
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


        public ActionResult ShowStudents()
        {
            var students = _psychologistManager.GetAllStudents();

            return View(students);
        }


        public ActionResult ShowStudentDetails(string userId)
        {
            var students = _psychologistManager.GetAllStudents();
            var user = students.FirstOrDefault(x => x.Id == userId);

            return View(user);
        }

        public ActionResult ExcelResults(string userId)
        {
            var students = _psychologistManager.GetAllStudents();
            var user = students.FirstOrDefault(x => x.Id == userId);
            var completedSurveys=user.CompletedSurveys;


            if (completedSurveys.Count > 0 && completedSurveys!=null)
            {
                XLWorkbook workbook = new XLWorkbook();
                foreach (var survey in completedSurveys)
                {
                    var sheetName = 
                        survey.DateOfComplete.Day.ToString()+"_"
                        + survey.DateOfComplete.Month.ToString()+"_"
                        + survey.DateOfComplete.Year.ToString() + "_"
                        + survey.DateOfComplete.Hour.ToString() + "_"
                        + survey.DateOfComplete.Minute.ToString() + "_"
                        + survey.DateOfComplete.Second.ToString();
                    IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
                    int n= 1;
                    worksheet.Cell(n, 1).Value = "Ankieta:";
                    worksheet.Cell(n, 2).Value = survey.Survey.Id;
                    n++;
                    worksheet.Cell(n, 1).Value = "Autor:";
                    worksheet.Cell(n, 2).Value = survey.Survey.Author.UserName;
                    n++;
                    n++;
                    worksheet.Cell(n,1).Value = "Pytanie:";
                    worksheet.Cell(n,2).Value = "Ocena:";

                    foreach (var answer in survey.Answers)
                    {
                        worksheet.Cell(n,1).Value = answer.Question.Content;
                        worksheet.Cell(n,2).Value = answer.AnswerRate;
                        n++;
                    }

                }

                Response.ClearContent();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls",user.UserName));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    workbook.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }

            }
            else
            {
                ViewBag.Message = string.Format("Nie można pobrać wynikow ankiet tego pacjenta, ponieważ nie uzupełnił on jeszcze żadnej ankiety");
            }

            return View("ShowStudents", students);
        }
    }
}