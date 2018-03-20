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
            if (action == "Yes") { return RedirectToAction("ProcessingOfGeodata"); }
            else { return RedirectToAction("UsersInputLocation"); }
        }

        public ActionResult ProcessingOfGeodata()
        {
            XmlApiMaster _manager = new XmlApiMaster(HttpContext);
            LocationUser userLocation = new LocationUser();
            var location = _manager.GetLocationsName();
            if (location.Select(l => l.Key).Where(k => k.Equals("city")).FirstOrDefault() != null) { userLocation.City = location["city"]; } else { userLocation.City = "";}
            if (location.Select(l => l.Key).Where(k => k.Equals("country")).FirstOrDefault() != null) { userLocation.Country = location["country"]; } else { userLocation.Country = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("region")).FirstOrDefault() != null) { userLocation.Region = location["region"]; } else { userLocation.Region = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("district")).FirstOrDefault() != null) { userLocation.District = location["district"]; } else { userLocation.District = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("lng")).FirstOrDefault() != null) { Coords.Lng = location["lng"].Replace(',','.'); } else { Coords.Lng = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("lat")).FirstOrDefault() != null) { Coords.Lat = location["lat"].Replace(',','.'); } else { Coords.Lat = ""; }
            return View(userLocation);
        }
    }
}