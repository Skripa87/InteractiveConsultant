using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InteractiveConsultant.Models;

namespace InteractiveConsultant.Controllers
{
    public class InterviewController : Controller
    {
        InteractiveConsultantContext db = new InteractiveConsultantContext();
        
        // GET: Interview
        public ActionResult Index()
        {
            
        }

        public ActionResult NextQuestion()
        {

        }

    }
}