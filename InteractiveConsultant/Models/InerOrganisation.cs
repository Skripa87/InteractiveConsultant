using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class InerOrganisation:Organisation
    {
        public virtual ICollection<SocialForm> SocialForms { get; set; }
        public InerOrganisation()
        {
            SocialForms = new List<SocialForm>();
        }
    }
}