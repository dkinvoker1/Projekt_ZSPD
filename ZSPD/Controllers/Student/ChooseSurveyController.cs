using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Controllers.Student
{
    [Authorize(Roles = Roles.User)]
    public class ChooseSurveyController : Controller
    {
        // GET: ChooseSurvey
        public ActionResult Index()
        {
            return View();
        }
    }
}