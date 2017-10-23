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
            ManagerInterview _manager = new ManagerInterview(db);

            _manager.Agreement = false;

            ViewBag.Manager = _manager;

            return View();
        }
    }
}