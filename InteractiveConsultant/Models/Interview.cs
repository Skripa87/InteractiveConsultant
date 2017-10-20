using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class Interview // объект опрос 
    {
        public byte IDInterview { get; set; }
        public DateTime DateInterview { get; set; }
        public Result Result { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public Interview()
        {
            ICollection<Answer> Answers = new List<Answer>();
        }
    }
}
