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
        public Result(int extendOfNeed, ICollection<CentralOrganisation> _centrs, ICollection<Area> _areas, ICollection<SocialForm> _socialForms, ICollection<Answer> answers, List<Recomendation> _recomendations)
        {
            var recomendations = _recomendations.Where(r => answers.Select(s => s.Recomendation_ID).Contains(r.ID)).ToList();
            Conditions = new List<Condition>();
            var address = LocationUser.Area == null ? "" : LocationUser.Area + " район " + LocationUser.City == null ? "" : "г. " + LocationUser.City;
            var alterAddress = LocationUser.Area == null ? "" : LocationUser.Area + " район " + LocationUser.City == null ? "" : "г." + LocationUser.City;
            address = address.Trim().ToLower();
            alterAddress = alterAddress.Trim().ToLower();
            var area = _areas.Where(a => a.NameArea.Trim().ToLower().Contains(LocationUser.Area.Trim().ToLower())).ToList();
            var centrs = _centrs.Where(c => c.Areas.Select(a => a.IDArea).ToList()
                                    .Intersect(area.Select(s => s.IDArea).ToList())
                                .ToList()
                                .Count > 0)
                                .ToList();
            Centr = centrs.Where(c => c.IsCentr == true).FirstOrDefault();
            var centrPrz = centrs.Where(c => c.IsCanPrz == true).ToList();
            GetCondition(extendOfNeed, _socialForms, recomendations, address, alterAddress, centrPrz);
        }

        private void ConvertToViewOrg<T>(List<T> inOrganisations, List<ViewOrganisation> outOrganisations)
        {
            if (inOrganisations == null || outOrganisations == null) { return; }
            foreach (var o in inOrganisations)
            {
                if (o == null) { continue; }
                var outOrganisation = new ViewOrganisation();
                outOrganisation.IDOrganisation = ((dynamic)o).IDOrganisation;
                outOrganisation.IsCanPrz = ((dynamic)o).IsCanPrz;
                outOrganisation.IsChilde = ((dynamic)o).IsChilde;
                outOrganisation.NameOrganisation = ((dynamic)o).NameOrganisation;
                outOrganisation.PropertysOrganisation = ((dynamic)o).PropertysOrganisation;
                outOrganisation.TargetingGender = ((dynamic)o).TargetingGender;
                outOrganisations.Add(outOrganisation);
            }
        }

        private void GetCondition(int extendOfNeed, ICollection<SocialForm> _socialForms, ICollection<Recomendation> Recomendations, string address, string alterAddress, List<CentralOrganisation> startOrgPrz)
        {
            switch (extendOfNeed)
            {
                case 1:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 1).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });

                    }
                    break;
                case 2:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 1).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address)
                        || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });
                    }
                    break;
                case 3:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 2).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });
                    }
                    break;
                case 4:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 4).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });

                    }
                    break;
                case 5:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 1).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });

                    }
                    break;
                case 6:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 1).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                               .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });

                    }
                    break;
                case 7:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 2).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });
                    }
                    break;
                case 8:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 5).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });

                    }
                    break;
                case 9:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 3).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs.RemoveAll(org => !(org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0"));
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });
                    }
                    break;
                default:
                    {
                        var socialForm = _socialForms.Where(s => s.IDSocialForm == 3).FirstOrDefault();
                        var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                        orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                        orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                        var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                                s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        var buff = _buff.Where(b => b.InerOrganisations
                            .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                            .Contains(socialForm.IDSocialForm))
                            .ToList().Count > 0).ToList();
                        var viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(buff, viewOrgsPrz);
                        var _viewOrgsPrz = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, _viewOrgsPrz);
                        viewOrgsPrz.AddRange(_viewOrgsPrz);
                        orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                        orgs = orgs.Where(org => org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0").ToList();
                        List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                        ConvertToViewOrg(orgs, viewOrgs);
                        Conditions.Add(new Condition()
                        {
                            ConditionPay = true,
                            SocialForm = socialForm,
                            OrganizationsPrz = viewOrgsPrz,
                            SupportOrganizations = viewOrgs
                        });
                    }
                    break;
            }
            var xConditions = Recomendations.Where(r => r.SocialForm != null &&
                                                  !Conditions.Where(c => c.SocialForm != null)
                                                             .Select(cs => cs.SocialForm)
                                                             .Contains(r.SocialForm))
                                                             .ToList();
            if (xConditions.Count > 0)
            {
                foreach (var item in xConditions)
                {
                    SocialForm socialForm = null;
                    socialForm = item.SocialForm;
                    var orgs = socialForm?.InerOrganisations?
                                              .Where(org => org.IsCanPrz == true).ToList();
                    orgs.RemoveAll(r => !(r.IsChilde == LocationUser.IsChilde || r.IsChilde == null));
                    orgs.RemoveAll(r => !(r.PropertysOrganisation.ToLower().Trim().Contains(address) || r.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)));
                    var _buff = startOrgPrz.Where(s => s.PropertysOrganisation.ToLower().Trim().Contains(address) ||
                                                            s.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                    var buff = _buff.Where(b => b.InerOrganisations
                        .Where(i => i.SocialForms.Select(sf => sf.IDSocialForm)
                        .Contains(socialForm.IDSocialForm))
                        .ToList().Count > 0).ToList();
                    var viewOrgsPrz = new List<ViewOrganisation>();
                    ConvertToViewOrg(buff, viewOrgsPrz);
                    var _viewOrgsPrz = new List<ViewOrganisation>();
                    ConvertToViewOrg(orgs, _viewOrgsPrz);
                    viewOrgsPrz.AddRange(_viewOrgsPrz);
                    orgs = socialForm?.InerOrganisations?.Where(org => org.PropertysOrganisation.ToLower().Trim().Contains(address) || org.PropertysOrganisation.ToLower().Trim().Contains(alterAddress)).ToList();
                    orgs = orgs.Where(org => org.TargetingGender == LocationUser.TargetingGender || org.TargetingGender.Trim() == "0").ToList();
                    orgs = orgs.Where(org => org.IsChilde == LocationUser.IsChilde || org.IsChilde == null).ToList();
                    List<ViewOrganisation> viewOrgs = new List<ViewOrganisation>();
                    ConvertToViewOrg(orgs, viewOrgs);
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
