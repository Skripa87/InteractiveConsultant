using InteractiveConsultant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using suggestionscsharp;

namespace InteractiveConsultant.Controllers
{
    public class GeoLocationController : Controller
    {
        // GET: GeoLocation
        public ActionResult ConsentToTheProcessingOfGeodata(bool _checked, bool _voicerOn)
        {
            StateInterview._checked = _checked;
            StateInterview._voicerOn = _voicerOn;
            if (StateInterview._checked == false) return RedirectToAction("ErrorAgree", "Home");
            else return View();
        }

        public ActionResult ControlProcessingOfGeodata(string action)
        {
            if (action == "Yes") { StateInterview._agreeToTheLocation = true; return RedirectToAction("ProcessingOfGeodata"); }
            else { StateInterview._agreeToTheLocation = false;  return RedirectToAction("Index", "Interview", new { a = "Yes" }); }
        }

        public ActionResult ProcessingOfGeodata()
        {
            XmlApiMaster _manager = new XmlApiMaster(HttpContext);
            //LocationUser userLocation = new LocationUser();
            var location = _manager.GetLocationsName();
            if (location.Select(l => l.Key).Where(k => k.Equals("city")).FirstOrDefault() != null) { LocationUser.City = location["city"]; } else { LocationUser.City = "";}
            if (location.Select(l => l.Key).Where(k => k.Equals("country")).FirstOrDefault() != null) { LocationUser.Country = location["country"]; } else { LocationUser.Country = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("region")).FirstOrDefault() != null) { LocationUser.Region = location["region"]; } else { LocationUser.Region = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("district")).FirstOrDefault() != null) { LocationUser.District = location["district"]; } else { LocationUser.District = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("lng")).FirstOrDefault() != null) { Coords.Lng = location["lng"].Replace(',','.'); } else { Coords.Lng = ""; }
            if (location.Select(l => l.Key).Where(k => k.Equals("lat")).FirstOrDefault() != null) { Coords.Lat = location["lat"].Replace(',','.'); } else { Coords.Lat = ""; }
            return View();
        }

        public ActionResult UsersInputLocation()
        {
            return View();
        }
    }
}