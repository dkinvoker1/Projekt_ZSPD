using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSPD.Domain.Managers;
using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Models;

namespace ZSPD.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        IGraphManager _graphManager;
        IStudentManager _studentManager;
        IPsychologistManager _psychologistManager;
        public AdminController(IGraphManager graphManager, 
                                IStudentManager studentManager, 
                                IPsychologistManager psychologistManager)
        {
            _graphManager = graphManager;
            _studentManager = studentManager;
            _psychologistManager = psychologistManager;
        }
        // GET: Admin
        public ActionResult AddGraph()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadGraph(string name, IEnumerable<HttpPostedFileBase> files)
        { 
            List<string> paths = new List<string>();
            foreach (var file in files)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                   
                    file.SaveAs(path);

                    paths.Add(path);
                }
            }

            string poj = System.IO.File.ReadAllText(paths[0]); // Cofnięcie o 1 folder w drzewku
            // wczytanie z pliku tekstowego wartosci dla poprzednikow
            string poj1 = System.IO.File.ReadAllText(paths[1]);
            // wczytanie z pliku tekstowego zadan dla odp pojec
            string zad = System.IO.File.ReadAllText(paths[2]);

            _graphManager.AddGraphFromFiles(name, paths[0], paths[1], paths[2]);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult StudentsManage()
        {
            var allStudents = _psychologistManager.GetAllStudents();
            return View(allStudents);
        }

        public ActionResult StudentDetails(string id)
        {
            var actualStudent = _psychologistManager.GetAllStudents().FirstOrDefault(x => x.Id == id);
            var allSubjects = _graphManager.GetAllGraphs();
            var VM = new StudentDetailsViewModel(actualStudent, allSubjects);  

            return View(VM);
        }

        [HttpPost]
        public ActionResult AddNewSubject(StudentDetailsViewModel vm)
        {
            var allSubject = _graphManager.GetAllGraphs();

            _studentManager.SetActiveSubject(vm.ActualStudent.Id, allSubject.First(x => x.Name == vm.SelectedSubject).Id);
            return RedirectToAction("StudentsManage");
        }
    }
}