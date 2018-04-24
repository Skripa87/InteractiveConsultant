using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class CentralOrganisation:Organisation
    {
        public virtual ICollection<InerOrganisation> Organisations { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        public CentralOrganisation()
        {
            Organisations = new List<InerOrganisation>();
            Areas = new List<Area>();
        }       
    }
}