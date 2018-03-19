using InteractiveConsultant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InteractiveConsultant.Controllers
{
    public class GeoLocationController : Controller
    {
        // GET: GeoLocation
        public ActionResult ConsentToTheProcessingOfGeodata(bool _checked)
        {
            if (_checked == false) return RedirectToAction("ErrorAgree", "Home");
            return View();
        }

        public ActionResult ControlProcessingOfGeodata(string action)
        {
            if (action == "Yes") { return RedirectToAction("ProcessingOfGeodata",new { this.HttpContext}); }
            else { return RedirectToAction("UsersInputLocation"); }
        }

        public ActionResult ProcessingOfGeodata(HttpContext httpContext)
        {
            XmlApiMaster _manager = new XmlApiMaster(httpContext);
            var location = _manager.GetLocationsName();
            LocationUser locationUser = new LocationUser();
            List<string> coords = new List<string>();
            if (location.Select(l => l.Key).Where(k => k.Equals("city")).FirstOrDefault() != null) { locationUser.City = location["city"]; } else { locationUser.City = "";}
            if (location.Select(l => l.Key).Where(k => k.Equals("country")).FirstOrDefault() != null) { locationUser.Country = location["country"]; } else { locationUser.Country = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("region")).FirstOrDefault() != null) { locationUser.Region = location["region"]; } else { locationUser.Region = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("district")).FirstOrDefault() != null) { locationUser.District = location["district"]; } else { locationUser.District = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("lng")).FirstOrDefault() != null) { coords.Add(location["lng"]); } else { coords.Add(""); }
            if (location.Select(l => l.Key).Where(k => k.Equals("lat")).FirstOrDefault() != null) { coords.Add(location["lat"]); } else { coords.Add(""); }
            locationUser.SetLatLng(coords);
            return View();
        }
    }
}