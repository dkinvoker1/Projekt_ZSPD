using System.Web.Mvc;

using ZSPD.Domain.Managers;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Controllers.Student
{
    [Authorize(Roles = Roles.User)]
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