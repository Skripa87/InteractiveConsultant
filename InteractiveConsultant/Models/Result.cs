using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class Result // таблица результатов
    {
        public byte IDResult { get; set; }

        public string TextResult { get; }
        
        public Result(int extendOfNeed)
        {
            if (extendOfNeed < 7)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Полустационарная форма обслуживания./n" + "Человек умеренно нуждается в посторонней помощи.";
            }
            else if (extendOfNeed >= 7)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Стационарная форма обслуживания./n" + "Человек полностью нуждается в посторонней помощи";
            }
            else
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Социальное обслуживание на дому";
            }
        }

    }
}
