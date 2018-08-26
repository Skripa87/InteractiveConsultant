using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class SocialForm:IEquatable<SocialForm>
    {
        public int IDSocialForm { get; set; }
        public string NameSocialForm { get; set; }
        public string TextForView { get; set; }
        public virtual ICollection<InerOrganisation> InerOrganisations { get; set; }
        public SocialForm()
        {
            InerOrganisations = new List<InerOrganisation>();
        }

        public bool Equals(SocialForm other)
        {
            return this.IDSocialForm == other.IDSocialForm;
        }
    }
}