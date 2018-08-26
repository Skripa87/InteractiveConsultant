using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class CentralOrganisation:Organisation
    {
        public virtual ICollection<InerOrganisation> InerOrganisations { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        //public virtual ICollection<Result> Results { get; set; }
        public bool? IsCentr { get; set; }
        public CentralOrganisation()
        {
            InerOrganisations = new List<InerOrganisation>();
            Areas = new List<Area>();
            //Results = new List<Result>();
        }       
    }
}