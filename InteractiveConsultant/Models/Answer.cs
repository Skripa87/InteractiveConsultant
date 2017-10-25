using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class Answer // объект ответ
    {
        public byte IDAnswer { get; set; }
        public string TextAnswer { get; set; }
        public int CostAnswer { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
        public Answer()
        {
            ICollection<Interview> Interviews = new List<Interview>();
        }
    }
}
