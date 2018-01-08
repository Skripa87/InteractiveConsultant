using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InteractiveConsultant.Models;

namespace InteractiveConsultant.Controllers
{
    public class InterviewController : Controller
    {
        static InteractiveConsultantContext db = new InteractiveConsultantContext();

        static ICollection<Question> _questions;

        static Interview interview;

        int numberQuestion = 0;

        // GET: Interview
        public ActionResult Index(bool _checked)
        {
            interview = new Interview();

            _questions = new List<Question>();

            ManagerInterview.Agreement = _checked;

            ManagerInterview.StartInterview(_questions, interview);

            if (_questions.Count == 0)
            {
                return RedirectToAction("Default", "Home");
            }

            return View(_questions.FirstOrDefault());
        }

        public ActionResult StartInterview()
        {
            return View(_questions.FirstOrDefault());
        }

        public ActionResult PreNextQuestion(string action, List<string> proposed_responses)
        {
            if (action == "next")
            {
                if ((_questions.Count > 1) && (numberQuestion < _questions.Count - 1))
                {
                    numberQuestion++;

                    ViewBag.Number = numberQuestion;

                    return View(_questions.ElementAt(numberQuestion)); //если переходим к следующему вопросу
                }
                else if (numberQuestion == _questions.Count - 1)
                {
                    return RedirectToAction("Default", "Home"); //организовать переход к результату
                }
                else
                {
                    return RedirectToAction("Default", "Home"); //организовать если вопрос всего один
                }
            }
            if(action == "previos")
            {
                if (numberQuestion > 1)
                {
                    numberQuestion--;

                    ViewBag.Number = numberQuestion;

                    return View(_questions.ElementAt(numberQuestion)); //если переходим к предыдущему вопросу
                }
                else if (numberQuestion == 1)
                {
                    return RedirectToAction("StartInterview");
                }                
            }
            return View();
        }

        public ActionResult ResultPage()
        {
            return View();
        }





















        public ActionResult Initial()
        {
            Question question = new Question();

            ICollection<Question> questions = new List<Question>();

            ICollection<Answer> answers = new List<Answer>();

            for (int i = 0; i < 2; i++)
            {
                Answer answer = new Answer();
                answer.CostAnswer = 0;
                answer.IDAnswer = (byte)(i + 1);
                answer.TextAnswer = $"ответ" + i + $"на вопрос 1";
                answers.Add(answer);
            }
            question.IDQuestion = 1;
            question.TextQuestion = $"Вопрос 1";
            question.Answers = answers;
            db.Questions.Add(question);
            db.SaveChanges();
            return View();
        }

    }
}