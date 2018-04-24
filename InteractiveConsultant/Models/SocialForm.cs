using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class SocialForm
    {
        public byte IDSocialForm { get; set; }
        public string NameSocialForm { get; set; }
        public virtual ICollection<InerOrganisation> InerOrganisations { get; set; }
        public SocialForm()
        {
            InerOrganisations = new List<InerOrganisation>();
        }

    }
}