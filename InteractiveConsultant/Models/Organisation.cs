using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public abstract class Organisation
    {
        public int IDOrganisation { get; set; }        
        public string NameOrganisation { get; set; }
        public string PropertysOrganisation { get; set; }

        public Organisation()
        {

        }
    }
}