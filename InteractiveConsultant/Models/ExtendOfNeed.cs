using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class ExtendOfNeed //степени нуждаемости, таблица Бартела и Лаутона
    {
        public byte IDExtendOfNeed { get; set; }
        public int MaxScore { get; set; }
        public int MinScore { get; set; }
        public int? SocialFormID { get; set; }
        [ForeignKey("SocialFormID")]
        public SocialForm SocialForm { get; set; }
        public bool? ConditionPay { get; set; }
        public virtual ICollection<string> ChsIndividualNeed { get; set; }
        public ExtendOfNeed()
        {
            ICollection<string> ChsIndividualNeed = new List<string>();
        }
    }
}
