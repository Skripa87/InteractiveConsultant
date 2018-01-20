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
                if (a != null)
                {
                    sumScore += a.CostAnswer;
                }
            }
            return sumScore;
        }

        static private int GetExtendOfNeed(Interview interview)
        {
            int score = 0;
            int extendOfNeed = 0;
            score = CalculateScores(interview);
            foreach(var t in tableBartelLouten)
            {
                if ((score < t.MaxScore) && (score > t.MinScore))
                {
                    extendOfNeed = t.PowerExtendOfNeed;
                }
            }
            return extendOfNeed;
        }

        static public void StartInterview(ICollection<Question> questions, Interview interview)
        {
            tableBartelLouten = new List<ExtendOfNeed>();

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
            }             
        }

        static public Result GetResultInterview(Interview interview)
        {
            int extendOfNeed = 0;
            extendOfNeed = GetExtendOfNeed(interview);
            Result result = new Result(extendOfNeed);
            return result;
        }
    }
}
