﻿using System;
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
            if (extendOfNeed == 1)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Стационарная в специализированной форме.\n" + "Человек полностью утратил способность к самообслуживанию.\n" + "Оплата из доходов получателя";
            }
            else if (extendOfNeed == 2)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Стационарная форма обслуживания.\n" + "Человек умеренно нуждается в посторонней помощи\n" + "Оплата из доходов получателя";
            }
            else if (extendOfNeed == 3)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Полустационарное обслуживание.\n" + "Человек нуждается в помощи" + "Оплата из доходов получателя";
            }
            if (extendOfNeed == 5)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Стационарная в специализированной форме.\n" + "Человек полностью утратил способность к самообслуживанию.\n" + "Бесплатно!";
            }
            else if (extendOfNeed == 6)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Стационарная форма обслуживания.\n" + "Человек умеренно нуждается в посторонней помощи\n" + "Бесплатно!";
            }
            else if (extendOfNeed == 7)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Полустационарное обслуживание.\n" + "Человек нуждается в помощи" + "Бесплатно!";
            }
            else if (extendOfNeed == 8 || extendOfNeed == 4)
            {
                this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Срочное социальное обслуживание.\n" + "Человек нуждается в помощи" + "Бесплатно!";
            }
            else if (extendOfNeed == 9)
            {
                this.TextResult = "Рекомендуем обратится в ГКУ РЦСОН для устройства в приемную семью, для пожилых и инвалидов!";
            }
            else this.TextResult = "Рекомендуемая форма предоставления социальных услуг - Cоциальное обслуживание на дому.\n";
        }

    }
}
