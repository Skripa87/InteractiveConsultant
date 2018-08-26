using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    [Table("InerOrganisations")]
    public class InerOrganisation:Organisation
    {
        public int CentralOrganisations_IDOrganisation { get; set; }
        
        [ForeignKey("CentralOrganisations_IDOrganisation")]
        public CentralOrganisation CentralOrganisation { get; set; }
        public virtual ICollection<SocialForm> SocialForms { get; set; }
        //public virtual ICollection<Result> Results { get; set; }
        public InerOrganisation()
        {
            SocialForms = new List<SocialForm>();
            //Results = new List<Result>();           
        }
    }
}