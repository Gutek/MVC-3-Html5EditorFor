using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Html5.Web.Models;

namespace Html5.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult Numeric()
        {
            return View(new NumericViewModel());
        }

        public ActionResult Date()
        {
            return View(new DateViewModel());
        }

        public ActionResult String()
        {
            return View(new StringViewModel());
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
