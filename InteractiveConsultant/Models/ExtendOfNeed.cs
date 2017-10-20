using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class ExtendOfNeed //степени нуждаемости, таблица Бартела и Лаутона
    {
        public byte IDExtendOfNeed { get; set; }
        public int MaxScore { get; set; }
        public int MinScore { get; set; }
        public int PowerExtendOfNeed { get; set; }
        public ICollection<string> ChsIndividualNeed { get; set; }
        public ExtendOfNeed()
        {
            ICollection<string> ChsIndividualNeed = new List<string>();
        }
    }
}
