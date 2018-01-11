using System.Web.Mvc;

namespace InteractiveConsultant.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Default()
        {
            return View();
        }

        public ActionResult ErrorAgree()
        {
            return View();
        }

        public ActionResult ErrorCompliteUser(string action)
        {
            if(action == "start")
            {
                return RedirectToAction("Default", "Home");
            }
            else 
            {
                return Redirect("http://mintrudrb.ru");
            }
            //return View();
        }
    }
}