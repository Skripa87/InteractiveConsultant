using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class ViewOrganisation
    {
        public int IDOrganisation { get; set; }
        public string CentrPrefix { get; set; }
        public string NameOrganisation { get; set; }
        public string PropertysOrganisation { get; set; }
        public bool? IsCanPrz { get; set; }
        public bool? IsChilde { get; set; }
        public string TargetingGender { get; set; }
    }
}