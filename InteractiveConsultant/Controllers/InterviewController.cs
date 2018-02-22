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

        static Interview _interview;

        static int numberQuestion = 0;

        static List<Answer> answers = new List<Answer>();

        static List<bool> checkQuestions = new List<bool>();

        // GET: Interview
        public ActionResult Index(bool _checked)
        {
            _interview = new Interview();    // Создание экземпляра класса интервью для текущего пользователя

            _questions = new List<Question>(); // Создание экземпляра класса List Вопросов

            if (_checked == false)
            {
                return RedirectToAction("ErrorAgree", "Home"); // Организовать переход к результату
            }

            ManagerInterview.Agreement = _checked; //Установка переменной "согласие на прохождение опроса" в положение выбранное пользователем

            ManagerInterview.StartInterview(_questions, _interview); // инициализация всех переменных текущего опроса

            for (int i = 0; i < _questions.Count; i++)
            {
                answers.Add(null);
                checkQuestions.Add(false);
            }
            for (int i = 0; i < _questions.Count; i++)
            {
                if (answers[i] != null)
                {
                    checkQuestions[i] = true;
                }
                else checkQuestions[i] = false;
            }
            ViewData["Cheker"] = checkQuestions;
            if (answers[numberQuestion] != null)
            {
                ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
            }

            return View(_questions.FirstOrDefault());
        }

        public ActionResult StartInterview()
        {
            for (int i = 0; i < _questions.Count; i++)
            {
                if (answers[i] != null)
                {
                    checkQuestions[i] = true;
                }
                else checkQuestions[i] = false;
            }
            ViewData["Cheker"] = checkQuestions;
            if (answers[numberQuestion] != null)
            {
                ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
            }

            return View(_questions.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult PreNextQuestion(string action, string responses)
        {
            if (responses != null)
            {
                answers[numberQuestion] = _questions.ElementAt(numberQuestion).Answers.Where(a => a.IDAnswer.ToString().Equals(responses)).FirstOrDefault();
            }
            for (int i = 0; i < _questions.Count; i++)
            {
                if (answers[i] != null)
                {
                    checkQuestions[i] = true;
                }
                else checkQuestions[i] = false;
            }
            if (action.Equals("next") && (numberQuestion < _questions.Count - 1))
            {
                numberQuestion++;
                ViewData["Cheker"] = checkQuestions;
                if (answers[numberQuestion] != null)
                {
                    ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                }

                return View(_questions.ElementAt(numberQuestion));
            }
            else if (action.Equals("previos") && (numberQuestion > 1))
            {
                numberQuestion--;
                ViewData["Cheker"] = checkQuestions;
                if (answers[numberQuestion] != null)
                {
                    ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                }

                return View(_questions.ElementAt(numberQuestion)); //если переходим к предыдущему вопросу
            }
            else if (action.Equals("previos") && (numberQuestion == 1))
            {
                numberQuestion--;
                ViewData["Cheker"] = checkQuestions;
                if (answers[0] != null)
                {
                    ViewData["AnswerID"] = answers[0].IDAnswer;
                }

                return RedirectToAction("StartInterview");
            }
            else if (action.Equals("next") && (numberQuestion == _questions.Count - 1))
            {
                return RedirectToAction("ResultPage", "Interview"); //Необходимо реализовать переход к результату
            }
            else if (action.Contains("page"))
            {
                numberQuestion = Convert.ToInt32(action.Substring(action.IndexOf('_') + 1));
                ViewData["Cheker"] = checkQuestions;
                if (answers[numberQuestion] != null)
                {
                    ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                }
                return View(_questions.ElementAt(numberQuestion));
            }

            return View();
        }

        public ActionResult ResultPage()
        {
            _interview.Answers = new List<Answer>();
            foreach (var a in answers)
            {
                _interview.Answers.Add(a);
            }
            Result result = ManagerInterview.GetResultInterview(_interview);
            _interview.TextResult = result.TextResult;
            return View(_interview);
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