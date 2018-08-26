using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    static public class ManagerInterview
    {
        static ICollection<ExtendOfNeed> tableBartelLouten;
        static InteractiveConsultantContext context;
        static List<CentralOrganisation> centralOrganisations;
        static List<Area> areas;
        static List<SocialForm> socialForms;
        static List<InerOrganisation> inerOrganisations;
        static List<Recomendation> recomendations;

        static public bool Agreement { get; set; }
        static ManagerInterview()
        {
            context = new InteractiveConsultantContext();
        }
        static private int CalculateScores(Interview interview)
        {
            int sumScore = 0;
            foreach (var a in interview.Answers)
            {
                if (a != null) sumScore += a.CostAnswer;
            }
            return sumScore;
        }
        static private int GetExtendOfNeed(Interview interview, out int scr)
        {
            int score = 0;
            int extendOfNeed = 0;
            score = CalculateScores(interview);
            scr = score;
            foreach(var t in tableBartelLouten)
            {
                if ((score <= t.MaxScore) && (score >= t.MinScore)) extendOfNeed = t.PowerExtendOfNeed;
            }
            return extendOfNeed;
        }

        static public void StartInterview(ICollection<Question> questions, Interview interview)
        {
            tableBartelLouten = new List<ExtendOfNeed>();
            centralOrganisations = new List<CentralOrganisation>();
            areas = new List<Area>();
            socialForms = new List<SocialForm>();
            inerOrganisations = new List<InerOrganisation>();
            recomendations = new List<Recomendation>();
            if (Agreement)
            {
                foreach (var q in context.Questions)
                {
                    questions.Add(q);
                }
                foreach (var e in context.TableBartelLouton)
                {
                    tableBartelLouten.Add(e);
                }
                foreach (var o in context.CentralOrganisations)
                {
                    centralOrganisations.Add(o);
                }
                foreach (var item in context.Areas)
                {
                    areas.Add(item);
                }
                foreach (var item in context.SocialForms)
                {
                    socialForms.Add(item);
                }
                foreach(var item in context.InerOrganisations)
                {
                    inerOrganisations.Add(item);
                }
                foreach (var item in context.Recomendations)
                {
                    recomendations.Add(item);
                }
            }             
        }

        static public Result GetResultInterview(Interview interview, out int scorePointTest)
        {
            int extendOfNeed = 0, score = 0;
            extendOfNeed = GetExtendOfNeed(interview, out score);
            scorePointTest = score;
            Result result = new Result(extendOfNeed, centralOrganisations, areas, socialForms,interview.Answers,recomendations);
            interview.Result = result;
            return result;
        }
    }
}
