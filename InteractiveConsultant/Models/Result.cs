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
            var address = LocationUser.Area == null ? "" : LocationUser.Area + " район " + LocationUser.City == null ? "" : "г. " + LocationUser.City;
            var alterAddress = LocationUser.Area == null ? "" : LocationUser.Area + " район " + LocationUser.City == null ? "" : "г." + LocationUser.City;
            address = address.Trim().ToLower();
            alterAddress = alterAddress.Trim().ToLower();
            var area = _areas.Where(a => a.NameArea.Trim().ToLower().Contains(LocationUser.Area.Trim().ToLower())).ToList();
            centrs = _centrs.Where(c => c.Areas.Select(a => a.IDArea).ToList()
                                    .Intersect(area.Select(s => s.IDArea).ToList())
                                .ToList()
                                .Count > 0)
                                .ToList();
            Centr = centrs.Where(c => c.IsCentr == true).FirstOrDefault();
            var centrPrz = centrs.Where(c => c.IsCanPrz == true).ToList();
            GetCondition(extendOfNeed, _socialForms, recomendations, address, alterAddress, centrPrz);
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
                outOrganisation.NameOrganisation = outOrganisation.NameOrganisation == null ? ((dynamic)o).NameOrganisation : outOrganisation.NameOrganisation;
                outOrganisation.IDOrganisation = ((dynamic)o).IDOrganisation;
                outOrganisation.IsCanPrz = ((dynamic)o).IsCanPrz;
                outOrganisation.IsChilde = ((dynamic)o).IsChilde;
                outOrganisation.PropertysOrganisation = ((dynamic)o).PropertysOrganisation;
                outOrganisation.TargetingGender = ((dynamic)o).TargetingGender;
                outOrganisations.Add(outOrganisation);
            }
        }

        private void GetCondition(ExtendOfNeed extendOfNeed, ICollection<SocialForm> _socialForms, ICollection<Recomendation> Recomendations, string address, string alterAddress, List<CentralOrganisation> startOrgPrz)
        {
            var socialForm = extendOfNeed == null ? _socialForms.Where(s => s.IDSocialForm == 3).FirstOrDefault() : extendOfNeed.SocialForm;
            var conditionPay = extendOfNeed == null ? false : extendOfNeed.ConditionPay;
            var orgs = socialForm?.InerOrganisations?
                                  .Where(org => org.IsCanPrz == true).ToList();
            orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
            if (socialForm.IDSocialForm != 1)
            {
                orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
            }
            var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                               s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
            var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
            var viewOrgsPrz = new List<ViewOrganisation>();
            ConvertToViewOrg(buff, viewOrgsPrz,centrs);
            var _viewOrgsPrz = new List<ViewOrganisation>();
            ConvertToViewOrg(orgs, _viewOrgsPrz, centrs);
            viewOrgsPrz.AddRange(_viewOrgsPrz);
            orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
            orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
            orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
            List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
            ConvertToViewOrg(orgs, viewOrgs, centrs);
            Conditions.Add(new Condition()
            {
                ConditionPay = conditionPay,
                SocialForm = socialForm,
                OrganizationsPrz = viewOrgsPrz,
                SupportOrganizations = viewOrgs
            });
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
                    orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                    orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                    orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                    _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                            s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                    buff = _buff.Where(b => b.InerOrganisations
                        .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                        .Contains(socialForm.IDSocialForm))
                        .ToList().Count > 0).ToList();
                    viewOrgsPrz = new List<ViewOrganisation>();
                    ConvertToViewOrg(buff, viewOrgsPrz, centrs);
                    _viewOrgsPrz = new List<ViewOrganisation>();
                    ConvertToViewOrg(orgs, _viewOrgsPrz, centrs);
                    viewOrgsPrz.AddRange(_viewOrgsPrz);
                    if (socialForm.IDSocialForm != 1)
                    {
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                    }
                    orgs = orgs.Where(org => org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0").ToList();
                    orgs = orgs.Where(org => org.IsChilde == LocationUser.IsChilde || org.IsChilde == null).ToList();
                    viewOrgs = new List<ViewOrganisation>();
                    ConvertToViewOrg(orgs, viewOrgs, centrs);
                    if (!Conditions.Select(c => c.SocialForm.IDSocialForm).ToList().Contains(socialForm.IDSocialForm))
                    {
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });
                    }
                }
            }
        }
    }
}
