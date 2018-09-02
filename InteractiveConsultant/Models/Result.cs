using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class Result // таблица результатов
    {
        public int IDResult { get; set; }
        public CentralOrganisation Centr { get; set; }
        public List<Condition> Conditions { get; set; }
        private List<CentralOrganisation> centrs;
        public Result(ExtendOfNeed extendOfNeed, ICollection<CentralOrganisation> _centrs, ICollection<Area> _areas, ICollection<SocialForm> _socialForms, ICollection<Answer> answers, List<Recomendation> _recomendations)
        {
            centrs = new List<CentralOrganisation>();
            var recomendations = _recomendations.Where(r => answers.Select(s => s?.Recomendation_ID).Contains(r.ID)).ToList();
            Conditions = new List<Condition>();
            var address = (LocationUser.Area.Trim().ToLower() != LocationUser.City.Trim().ToLower()) ?
                          ((LocationUser.Area == null || LocationUser.Area  == "") ? "" : LocationUser.Area + " район ") + 
                          ((LocationUser.City == null || LocationUser.City == "") ? "" : ("г. " + LocationUser.City)):
                          ((LocationUser.Area == null || LocationUser.Area == "") ? ("г. " + LocationUser.City) : LocationUser.Area);
            var alterAddress = (LocationUser.Area.Trim().ToLower() != LocationUser.City.Trim().ToLower()) ?
                               ((LocationUser.Area == null || LocationUser.Area == "") ? "" : LocationUser.Area + " район ") + 
                               ((LocationUser.City == null || LocationUser.City == "") ? "" : ("г." + LocationUser.City)):
                               ((LocationUser.Area == null || LocationUser.Area == "") ? ("г. " + LocationUser.City) : LocationUser.Area); ;
            address = address.Trim().ToLower();
            alterAddress = alterAddress.Trim().ToLower();
            Area area = null;
            if (_areas.Where(a => a.NameArea.Trim().ToLower().Contains(LocationUser.Area.Trim().ToLower())).ToList().Count == 0)
            {
                StateInterview._agreeToTheLocation = false;
            }
            else if(_areas.Where(a => a.NameArea.Trim().ToLower().Contains(LocationUser.Area.Trim().ToLower())).ToList().Count > 0)
            {
                area = _areas.Where(a => a.NameArea.Trim().ToLower().Contains(LocationUser.Area.Trim().ToLower())).ToList().First();
            }
            centrs = area.CentralOrganizations;
            Centr = (LocationUser.City.Trim().ToLower() == "уфа") ? (centrs.Where(c => c.IDOrganisation == 256).Count() > 0 ?
                                                                     centrs.Where(c => c.IDOrganisation == 256).FirstOrDefault() :
                                                                     (centrs.Where(c => c.IsCanPrz == true).Count() > 0 ?
                                                                      centrs.Where(c => c.IsCanPrz == true).FirstOrDefault() :
                                                                      _centrs.Where(c => c.IDOrganisation == 256).FirstOrDefault())) :
                                                                      (centrs.Where(c => c.IsCanPrz == true).Count() > 0 ?
                                                                      centrs.Where(c => c.IsCanPrz == true).FirstOrDefault() :
                                                                      _centrs.Where(c => c.IDOrganisation == 256).FirstOrDefault());
            var organisationsPrz = Centr.InerOrganisations.Where(i => i.IsCanPrz == true).Count() > 0 ?
                                   Centr.InerOrganisations.Where(i => i.IsCanPrz == true).ToList() :
                                   new List<InerOrganisation>() { };
            GetCondition(Conditions, extendOfNeed.SocialForm, extendOfNeed.ConditionPay, area, _socialForms, recomendations, address, alterAddress, organisationsPrz);
        }

        private void ConvertToViewOrg<T>(List<T> inOrganisations, List<ViewOrganisation> outOrganisations, List<CentralOrganisation> centrals)
        {
            if (inOrganisations == null || outOrganisations == null) { return; }
            foreach (var o in inOrganisations)
            {
                if (o == null) { continue; }
                var outOrganisation = new ViewOrganisation();
                if (!o.GetType().ToString().Trim().ToLower().Contains("centralorganisation"))
                {
                    outOrganisation.NameOrganisation = centrals.Where(c => c.InerOrganisations.Select(s => s.IDOrganisation).Contains(((InerOrganisation)(dynamic)o).IDOrganisation))?.FirstOrDefault()?.NameOrganisation + ": " + ((dynamic)o).NameOrganisation;
                }
                else
                {
                    outOrganisation.NameOrganisation = ((dynamic)o).NameOrganisation;
                }
                outOrganisation.IDOrganisation = ((dynamic)o).IDOrganisation;
                outOrganisation.IsCanPrz = ((dynamic)o).IsCanPrz;
                outOrganisation.IsChilde = ((dynamic)o).IsChilde;
                outOrganisation.PropertysOrganisation = ((dynamic)o).PropertysOrganisation;
                outOrganisation.TargetingGender = ((dynamic)o).TargetingGender;
                outOrganisations.Add(outOrganisation);
            }
        }

        private void GetCondition(List<Condition> conditions, SocialForm eSocialForm, bool? eConditionPay, Area area, ICollection<SocialForm> _socialForms, 
                                  ICollection<Recomendation> Recomendations, string address, string alterAddress, List<InerOrganisation> startOrgPrz)
        {
            var socialForm = eSocialForm;
            var conditionPay = eConditionPay;
            var orgs = startOrgPrz;
            orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
            if (orgs.Count == 0) { orgs = startOrgPrz; }
            if (socialForm.IDSocialForm != 1)
            {
                orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
            }
            var viewOrgsPrz = new List<ViewOrganisation>();
            ConvertToViewOrg(orgs, viewOrgsPrz,centrs);            
            if (socialForm.IDSocialForm != 1)
            {
                orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
            }
            else
            {
                orgs = socialForm?.InerOrganisations?.ToList();
            }
            orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
            List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
            ConvertToViewOrg(orgs, viewOrgs, centrs);
            conditions.Add(new Condition()
            {
                ConditionPay = conditionPay,
                SocialForm = socialForm,
                OrganizationsPrz = viewOrgsPrz,
                SupportOrganizations = viewOrgs
            });
            if (Recomendations != null && Recomendations.Count != 0)
            {
                var xConditions = Recomendations.Where(r => r.SocialForm != null &&
                                                                  !Conditions.Where(c => c.SocialForm != null)
                                                                             .Select(cs => cs.SocialForm)
                                                                             .Contains(r.SocialForm))
                                                                             .ToList();
                if (xConditions.Count > 0)
                {
                    foreach (var item in xConditions)
                    {
                        socialForm = null;
                        socialForm = item.SocialForm;
                        bool? xConditionPay = item.ConditionPay == null ? false : item.ConditionPay;
                        List<Condition> _conditions = new List<Condition>();
                        GetCondition(_conditions, socialForm, xConditionPay, area, _socialForms, null, address, alterAddress, startOrgPrz);
                        _conditions.RemoveAll(c => conditions.Select(s => s.SocialForm.IDSocialForm).Contains(c.SocialForm.IDSocialForm));
                        conditions.AddRange(_conditions);
                    }
                }
            }
        }
    }
}
