using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class Question // объект вопрос
    {
        public byte IDQuestion { get; set; }
        public string TextQuestion { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public Question()
        {
            ICollection<Answer> Answers = new List<Answer>();
        }
    }
}
