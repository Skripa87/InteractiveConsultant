using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InteractiveConsultant.Models;

namespace InteractiveConsultant.Controllers
{
    public class HomeController : Controller
    {
        static InteractiveConsultantContext db = new InteractiveConsultantContext();
        public ActionResult Default()
        {
            ManagerInterview.Agreement = false;

            //ViewBag.Agreement = ManagerInterview.Agreement;

            return View();
        }
    }
}