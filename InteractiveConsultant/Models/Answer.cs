using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class Answer // объект ответ
    {
        public int IDAnswer { get; set; }
        public string TextAnswer { get; set; }
        public int CostAnswer { get; set; }
        public string ImagePath { get; set; }
        public string NextQuestions { get; set; }
        public int? Recomendation_ID { get; set; }
        [ForeignKey("Recomendation_ID")]
        public Recomendation Recomendation { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
        public Answer()
        {
            ICollection<Interview> Interviews = new List<Interview>();
        }
    }
}
