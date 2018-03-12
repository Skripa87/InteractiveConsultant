using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InteractiveConsultant.Models;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

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
            Task task = new Task(() => {
                SpeechSynthesizer spech = new SpeechSynthesizer();
                spech.SetOutputToDefaultAudioDevice();
                spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
            });
            task.Start();
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
            Task task = new Task(() => {
                SpeechSynthesizer spech = new SpeechSynthesizer();
                spech.SetOutputToDefaultAudioDevice();
                spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
            });
            task.Start();
            return View(_questions.FirstOrDefault());
        }

        [HttpPost]
        public async Task<ActionResult> PreNextQuestion(string action, string responses)
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
                Task task = new Task(() => {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
                });
                task.Start();
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
                Task task = new Task(() => {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
                });
                task.Start();
                return View(_questions.ElementAt(numberQuestion));                
            }
            else if (action.Equals("previos") && (numberQuestion == 1))
            {
                numberQuestion--;
                ViewData["Cheker"] = checkQuestions;
                if (answers[0] != null)
                {
                    ViewData["AnswerID"] = answers[0].IDAnswer;
                }
                Task task = new Task(() => {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
                });
                task.Start();
                return RedirectToAction("StartInterview");
            }
            else if (action.Equals("next") && (numberQuestion == _questions.Count - 1))
            {
                return RedirectToAction("PreIncome", "Interview"); //Необходимо реализовать переход к результату
            }
            else if (action.Contains("page"))
            {
                numberQuestion = Convert.ToInt32(action.Substring(action.IndexOf('_') + 2));
                ViewData["Cheker"] = checkQuestions;
                if (answers[numberQuestion] != null)
                {
                    ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                }
                Task task = new Task(() => {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
                });
                task.Start();
                return View(_questions.ElementAt(numberQuestion));
            }
            return View();
        }

        public ActionResult ResultPage(List<string> zp)
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
        
        public ActionResult PreIncome()
        {
            return View();
        }

        public ActionResult Income(string countPeople)
        {
            ViewData["countPeople"] = countPeople;
            return View();
        }

    }
}