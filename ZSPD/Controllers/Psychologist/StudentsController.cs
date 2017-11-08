using System.Web.Mvc;

using ZSPD.Domain.Managers;

namespace ZSPD.Controllers.Psychologist
{

    public class StudentsController : Controller
    {
        private IStudentManager _studentManager;
        private IPsychologistManager _psychologistManager;

        public StudentsController(IStudentManager studentManager, IPsychologistManager psychologistManager)
        {
            _studentManager = studentManager;
            _psychologistManager = psychologistManager;
        }

        //...
    }
}