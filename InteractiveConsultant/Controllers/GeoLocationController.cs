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
        [HttpPost]
        public ActionResult ConsentToTheProcessingOfGeodata(bool _cheker, bool _voicerOn)
        {
            StateInterview._checked = _cheker;
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
            var location = _manager.GetLocationsName();
            try
            {
                if (location["city"] != null) { LocationUser.City = location["city"]; } else { LocationUser.City = ""; }
                if (location["country"] != null) { LocationUser.Country = location["country"]; } else { LocationUser.Country = ""; }
                if (location["region"] != null) { LocationUser.Region = location["region"]; } else { LocationUser.Region = ""; }
                if (location["district"] != null) { LocationUser.District = location["district"]; } else { LocationUser.District = ""; }
                if (location["lng"] != null) { Coords.Lng = location["lng"].Replace(',', '.'); } else { Coords.Lng = ""; }
                if (location["lat"] != null) { Coords.Lat = location["lat"].Replace(',', '.'); } else { Coords.Lat = ""; }                
            }
            catch
            {
                LocationUser.City = "";
                LocationUser.Country = "";
                LocationUser.District = "";
                LocationUser.Region = "";
                Coords.Lat = "";
                Coords.Lng = "";
            }
            return View();
        }

        public ActionResult UsersInputLocation()
        {
            return View();
        }
    }
}