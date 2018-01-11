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
    }
}