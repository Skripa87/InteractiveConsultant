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
        
        public Result(int extendOfNeed, ICollection<CentralOrganisation> centrs)
        {
            CentralOrganisation centr = new CentralOrganisation(); 
            bool mark = false;
            if (StateInterview._agreeToTheLocation != false)
            {
                foreach (var c in centrs)
                {
                    foreach (var a in c.Areas)
                    {
                        if (a.NameArea.Contains(LocationUser.Area)) { mark = true; break; }
                    }
                    if (mark) { centr = c; break; }
                }
            }
            if (extendOfNeed == 1)
            {
                this.TextResult = "Я рекомендую Вам, обратится в специализированное, стационарное учреждение социального обслуживания населения. Стоимость обслуживания в Вашем случае составит: 70% от от Вашего средне-душевого дохода.";
            }
            else if (extendOfNeed == 2)
            {
                this.TextResult = "Я рекомендую Вам, обратится в стационарное учреждение социального обслуживания населения. Стоимость обслуживания в Вашем случае составит: 70% от от Вашего средне-душевого дохода.";
            }
            else if (extendOfNeed == 3)
            {
                this.TextResult = "Я рекомендую Вам, обратится в учреждение социального обслуживания населения для получения социального обслуживания в полустационарной форме. Стоимость обслуживания в Вашем случае составит: 70% от Вашего средне-душевого дохода.";
            }
            if (extendOfNeed == 5)
            {
                this.TextResult = "Я рекомендую Вам, обратится в специализированное, стационарное учреждение социального обслуживания населения. Обслуживание для Вас бесплатно.";
            }
            else if (extendOfNeed == 6)
            {
                this.TextResult = "Я рекомендую Вам, обратится в стационарное учреждение социального обслуживания населения. Обслуживание для Вас бесплатно.";
            }
            else if (extendOfNeed == 7)
            {
                this.TextResult = "Я рекомендую Вам, обратится в учреждение социального обслуживания населения для получения социального обслуживания в полустационарной форме. Обслуживание для Вас бесплатно.";
            }
            else if (extendOfNeed == 4)
            {
                this.TextResult = "Я рекомендую Вам обратится за срочным социальным обслуживанием в государственные учреждения социального обслуживания населения. Помощь предоставляется бесплатно.";
            }
            else if (extendOfNeed == 8)
            {
                this.TextResult = "Я рекомендую Вам обратится в учреждение социального обслуживания для оформления в приемную семью для пожилых и инвалидов.";
            }
            else if (extendOfNeed == 9)
            {
                this.TextResult = "Я рекомендую Вам обратится в одну из некоммерческих организаций для получения социального обслуживания на дому. Обслуживание для Вас должно предоставлятся бесплатно";
            }
            else
            {
                this.TextResult = "Я рекомендую Вам обратится в одну из некоммерческих организаций для получения социального обслуживания на дому. Стоимость обслуживания не должно превысить 70 % от вашего средне-душевого дохода";
            }
            
        }

    }
}
