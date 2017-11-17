using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSPD.Domain.Models;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //tymczasowe, dopytać DL jak to widzi - menu modułu psychologa chyba powinno być w jego kontrolerze,
        //aczkolwiek trzebaby takowy stworzyć i byłaby w nim tylko jedna metoda, bo reszta w students i surveys.
        public ActionResult Psychologist()
        {
            return View();
        }

    }
}