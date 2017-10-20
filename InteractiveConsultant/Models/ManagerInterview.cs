using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveConsultant.Models
{
    public class ManagerInterview
    {
        ICollection<Question> questions;

        ICollection<ExtendOfNeed> tableBartelLouten;

        Interview interview;

        InteractiveConsultantContext context;

        public ManagerInterview(InteractiveConsultantContext context)
        {
            this.context = context;
        }

        public bool Agreement { get; set; }

        private int CalculateScores(Interview interview)
        {
            int sumScore = 0;
            foreach (var a in interview.Answers)
            {
                sumScore += a.CostAnswer;
            }
            return sumScore;
        }

        private int GetExtendOfNeed()
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

        public void StartInterview()
        {
            questions = new List<Question>();
            interview = new Interview();
            tableBartelLouten = new List<ExtendOfNeed>();

            if (this.Agreement)
            {
                foreach (var q in context.Questions)
                {
                    Question que = new Question();
                    que = q;
                    foreach (var a in q.Answers)
                    {
                        que.Answers.Add(a);
                    }
                    questions.Add(que);
                }
                foreach(var e in context.TableBartelLouton)
                {
                    tableBartelLouten.Add(e);
                }
            }
        }

        public Result GetResultInterview()
        {
            int extendOfNeed = 0;
            extendOfNeed = GetExtendOfNeed();
            Result result = new Result(extendOfNeed);
            return result;
        }
    }
}
