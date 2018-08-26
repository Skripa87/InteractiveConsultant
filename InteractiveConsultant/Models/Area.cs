using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class Area
    {
        public int IDArea { get; set; }
        public string NameArea { get; set; }
        public virtual List<CentralOrganisation> CentralOrganizations {get;set;}
        public Area()
        {
            CentralOrganizations = new List<CentralOrganisation>();
        }
    }
}