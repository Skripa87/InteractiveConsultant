using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace InteractiveConsultant.Models
{
    public class XmlApiMaster
    {
        private string _ipUser;
        private string urlService = "http://ipgeobase.ru:7020/geo?ip=109.187.205.68";

        public XmlApiMaster(HttpContextBase httpContext)
        {
            _ipUser = "";//httpContext.Request.UserHostAddress;
            urlService += _ipUser;
        }

        public Dictionary<string, string> GetLocationsName()
         {
            try
            {
                Dictionary<string, string> locations = new Dictionary<string, string>();
                System.Net.WebRequest request = System.Net.WebRequest.Create(urlService);
                System.Net.WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                XmlDocument xdoc = new XmlDocument();
                List<XmlElement> elements = new List<XmlElement>();
                xdoc.Load(stream);
                xdoc.Normalize();
                XmlElement xroot = xdoc.DocumentElement;
                XmlElement xip = xroot.GetElementsByTagName("ip").Cast<XmlElement>().FirstOrDefault();
                try { XmlElement xcountry = xip.GetElementsByTagName("country").Cast<XmlElement>().FirstOrDefault(); elements.Add(xcountry); } catch { XmlElement xcountry = null; elements.Add(xcountry); }
                try { XmlElement xcity = xip.GetElementsByTagName("city").Cast<XmlElement>().FirstOrDefault(); elements.Add(xcity); } catch { XmlElement xcity = null; elements.Add(xcity); }
                try { XmlElement xregion = xip.GetElementsByTagName("region").Cast<XmlElement>().FirstOrDefault(); elements.Add(xregion); } catch { XmlElement xregion = null; elements.Add(xregion); }
                try { XmlElement xdistrict = xip.GetElementsByTagName("district").Cast<XmlElement>().FirstOrDefault(); elements.Add(xdistrict); } catch {XmlElement xdistrict = null; elements.Add(xdistrict); }
                try { XmlElement xlat = xip.GetElementsByTagName("lat").Cast<XmlElement>().FirstOrDefault(); elements.Add(xlat); } catch { XmlElement xlat = null; elements.Add(xlat); }
                try { XmlElement xlng = xip.GetElementsByTagName("lng").Cast<XmlElement>().FirstOrDefault(); elements.Add(xlng); } catch { XmlElement xlng = null; elements.Add(xlng); }
                foreach(XmlElement element in elements)
                {
                    if (element != null){ locations.Add(element.Name, element.InnerText); }
                }
                return locations;
            }
            catch
            {
                return null;
            }
        }

    }
}