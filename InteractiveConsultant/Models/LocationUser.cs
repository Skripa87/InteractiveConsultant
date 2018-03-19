using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class LocationUser
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public List<string> LatLng { get;}
        public LocationUser()
        {
            LatLng = new List<string>();
        }
        public void SetLatLng(List<string> list)
        {
            if (list != null) { foreach (var l in list) { LatLng.Add(l); } }
            else { LatLng.Add(null); }
        }
    }
}