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
            ICollection<Question> questions = new List<Question>();
            foreach(var q in db.Questions)
            {
                questions.Add(q);
            }
            return View(questions.FirstOrDefault());
        }
    }
}