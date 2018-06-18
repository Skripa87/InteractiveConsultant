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

        public string CentrName { get; }
        public string CentrAddress { get; }
        public Dictionary<string,string> OrganisationsInfo { get; }         
        public Result(int extendOfNeed, ICollection<CentralOrganisation> _centrs)
        {
            OrganisationsInfo = new Dictionary<string, string>();
            List<CentralOrganisation> centrs = new List<CentralOrganisation>();
            bool mark = false;
            if (StateInterview._agreeToTheLocation != false)
            {
                foreach (var c in _centrs)
                {
                    foreach (var a in c.Areas)
                    {
                        if (a.NameArea.ToLower().Contains(LocationUser.Area.ToLower())) { mark = true; break; }
                    }
                    if (mark) { centrs.Add(c); }
                }
            }
            if (extendOfNeed == 1)
            {
                this.TextResult = "Я рекомендую Вам, обратится в специализированное, стационарное учреждение социального обслуживания населения. Стоимость обслуживания в Вашем случае составит: 70% от от Вашего средне-душевого дохода.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("стац"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else if (extendOfNeed == 2)
            {
                this.TextResult = "Я рекомендую Вам, обратится в стационарное учреждение социального обслуживания населения. Стоимость обслуживания в Вашем случае составит: 70% от от Вашего средне-душевого дохода.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("стац"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else if (extendOfNeed == 3)
            {
                this.TextResult = "Я рекомендую Вам, обратится в учреждение социального обслуживания населения для получения социального обслуживания в полустационарной форме. Стоимость обслуживания в Вашем случае составит: 70% от Вашего средне-душевого дохода.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("полу"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            if (extendOfNeed == 5)
            {
                this.TextResult = "Я рекомендую Вам, обратится в специализированное, стационарное учреждение социального обслуживания населения. Обслуживание для Вас бесплатно.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("стац"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else if (extendOfNeed == 6)
            {
                this.TextResult = "Я рекомендую Вам, обратится в стационарное учреждение социального обслуживания населения. Обслуживание для Вас бесплатно.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("стац"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(item.NameOrganisation + " " + o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else if (extendOfNeed == 7)
            {
                this.TextResult = "Я рекомендую Вам, обратится в учреждение социального обслуживания населения для получения социального обслуживания в полустационарной форме. Обслуживание для Вас бесплатно.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("полу"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(item.NameOrganisation + " " + o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else if (extendOfNeed == 4)
            {
                this.TextResult = "Я рекомендую Вам обратится за срочным социальным обслуживанием в государственные учреждения социального обслуживания населения. Помощь предоставляется бесплатно.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("сроч"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(item.NameOrganisation + " " + o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else if (extendOfNeed == 8)
            {
                this.TextResult = "Я рекомендую Вам обратится в учреждение социального обслуживания для оформления в приемную семью для пожилых и инвалидов.";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("ммм"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(item.NameOrganisation + " " + o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else if (extendOfNeed == 9)
            {
                this.TextResult = "Я рекомендую Вам обратится в одну из некоммерческих организаций для получения социального обслуживания на дому. Обслуживание для Вас должно предоставлятся бесплатно";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("дом"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(item.NameOrganisation + " " + o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            else
            {
                this.TextResult = "Я рекомендую Вам обратится в одну из некоммерческих организаций для получения социального обслуживания на дому. Стоимость обслуживания не должно превысить 70 % от вашего средне-душевого дохода";
                if (!StateInterview._agreeToTheLocation) { this.TextResult += "Вы не предоставили нам Ваше местоположение, поэтому мы не можем предоставить Вам информацию по учреждениям социального обслуживания."; }
                else
                {
                    foreach (var item in centrs)
                    {
                        CentrName = item.NameOrganisation;
                        CentrAddress = item.PropertysOrganisation;
                        foreach (var o in item.Organisations)
                        {
                            mark = false;
                            foreach (var s in o.SocialForms)
                            {
                                if (s.NameSocialForm.ToLower().Contains("дом"))
                                { mark = true; break; }
                            }
                            if (mark) { OrganisationsInfo.Add(item.NameOrganisation + " " + o.NameOrganisation, o.PropertysOrganisation); }
                        }
                    }
                }
            }
            
        }

    }
}
