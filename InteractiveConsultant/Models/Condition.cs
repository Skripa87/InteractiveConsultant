using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class Condition
    {
        public int IDCondition { get; set; }
        public SocialForm SocialForm { get; set; }
        public bool? ConditionPay { get; set; }
        public virtual List<Result> Results { get; set; }
        public List<ViewOrganisation> OrganizationsPrz { get; set; }
        public List<ViewOrganisation> SupportOrganizations { get; set; }
        public Condition()
        {
            OrganizationsPrz = new List<ViewOrganisation>();
            SupportOrganizations = new List<ViewOrganisation>();
            Results = new List<Result>();
        }
    }
}