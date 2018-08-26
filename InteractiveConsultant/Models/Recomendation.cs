using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class Recomendation
    {
        public int ID { get; set; }
        public int? SocialForms_ID { get; set; }
        [ForeignKey("SocialForms_ID")]
        public SocialForm SocialForm { get; set; }
        public bool? ConditionPay { get; set; }
        public string Description { get; set; }
    }
}