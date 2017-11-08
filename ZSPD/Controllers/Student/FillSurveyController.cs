using System.Web.Mvc;

using ZSPD.Domain.Managers;

namespace ZSPD.Controllers.Student
{
    public class FillSurveyController : Controller
    {
        private IStudentManager _studentManager;

        public FillSurveyController(IStudentManager studentManager)
        {
            _studentManager = studentManager;
        }


        //...
    }
}