using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InteractiveConsultant.Models;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace InteractiveConsultant.Controllers
{
    public class InterviewController : Controller
    {
        static InteractiveConsultantContext db = new InteractiveConsultantContext();
        static ICollection<Question> _questions;
        static Interview _interview;
        static int numberQuestion;
        static int _countFamilyPeople;
        static List<Answer> answers;
        static List<bool> checkQuestions;
        static List<bool> disabledQuestion;
        static bool voicerON = StateInterview._voicerOn;
        // GET: Interview
        public ActionResult Index()
        {
            bool _checked = StateInterview._checked;
            numberQuestion = 0;
            _countFamilyPeople = 0;
            answers = new List<Answer>();
            checkQuestions = new List<bool>();
            disabledQuestion = new List<bool>();
            _interview = new Interview();    // Создание экземпляра класса интервью для текущего пользователя
            _questions = new List<Question>(); // Создание экземпляра класса List Вопросов
            if (_checked == false) return RedirectToAction("ErrorAgree", "Home"); // Организовать переход к результату
            ManagerInterview.Agreement = _checked; //Установка переменной "согласие на прохождение опроса" в положение выбранное пользователем
            ManagerInterview.StartInterview(_questions, _interview); // инициализация всех переменных текущего опроса
            foreach(var q in _questions)
            {
                answers.Add(null);
                checkQuestions.Add(false);
            }
            ViewData["Cheker"] = checkQuestions;
            if (voicerON)
            {
                Task task = new Task(() =>
                {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
                });
                task.Start();
            }
            return View(_questions.FirstOrDefault());
        }

        public ActionResult StartInterview(bool _voicerON)
        {
            voicerON = _voicerON;
            foreach (var a in answers)
            {
                if (a != null) checkQuestions[answers.IndexOf(a)] = true;
                else checkQuestions[answers.IndexOf(a)] = false;
            }
            ViewData["Cheker"] = checkQuestions;
            if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
            if (voicerON)
            {
                Task task = new Task(() =>
                {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
                });
                task.Start();
            }
            return View(_questions.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult PreNextQuestion(string action, string responses, bool _voicerON)
        {
            voicerON = _voicerON;
            if (responses != null)
            {
                answers[numberQuestion] = _questions.ElementAt(numberQuestion).Answers.Where(a => a.IDAnswer.ToString().Equals(responses)).FirstOrDefault();
                if (numberQuestion == 0 && answers[0].TextAnswer.ToLower().Contains("иностран"))
                    return RedirectToAction("InostranecResult");
            }            
            foreach (var a in answers)
            {
                if (a != null) checkQuestions[answers.IndexOf(a)] = true;
                else checkQuestions[answers.IndexOf(a)] = false;
            }
            ViewData["Cheker"] = checkQuestions;
            /*Task task = new Task(() => {
                SpeechSynthesizer spech = new SpeechSynthesizer();
                spech.SetOutputToDefaultAudioDevice();
                spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
            });*/
            Task task = new Task(() =>
            {
            var namef = Server.MapPath("~/Content/") + "temp.mp3";
                using (SpeechSynthesizer spech = new SpeechSynthesizer())
                {
                    var tempaudiofile = CreateTempDataProvider();
                    if (!System.IO.File.Exists(namef))
                    {
                        System.IO.File.Delete(namef);
                    }
                    var file = System.IO.File.Create(namef);
                    file.Close();
                    spech.SetOutputToWaveFile(file.Name);
                    spech.Speak(_questions.ElementAt(numberQuestion).TextQuestion);
                    file.Close();
                    StateInterview.audio = "Content/" + "temp.mp3";
                }                
            });
            Task taskIncome = new Task(() => {
                SpeechSynthesizer spech = new SpeechSynthesizer();
                spech.SetOutputToDefaultAudioDevice();
                spech.Speak("Для определения условий предоставления социального обслуживания в зависимости от средне-душевого дохода семьи, необходимо ввести количество совместно проживающих родственников");
            });
            if (action.Equals("next"))
            {
                numberQuestion++;
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (numberQuestion < _questions.Count)
                {
                    if (voicerON) task.Start();
                    if  (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                }
                if (numberQuestion > _questions.Count - 1) { if (voicerON) taskIncome.Start(); return RedirectToAction("PreIncome", "Interview"); }
                else return View(_questions.ElementAt(numberQuestion));
            }
            else if (action.Equals("previos"))
            {
                numberQuestion--;
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                if (voicerON) task.Start();
                if(numberQuestion == 0) return RedirectToAction("StartInterview", new {_voicerON});
                else return View(_questions.ElementAt(numberQuestion));                
            }
            else if (action.Contains("page"))
            {
                numberQuestion = Convert.ToInt32(action.Substring(action.IndexOf('_') + 1));
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                if (voicerON) task.Start();
                if (numberQuestion == 0) return RedirectToAction("StartInterview", new {_voicerON});
                else return View(_questions.ElementAt(numberQuestion));
            }
            else if(action.Equals("end"))
            {
                return RedirectToAction("ResultPage",new List<string>{ });
            }
            else if (action.Equals("endIncome"))
            {
                return RedirectToAction("PreIncome");
            }
            return View();
        }

        public ActionResult ResultPage(List<string> revenues)
        {
            Income income = new Income(_countFamilyPeople);
            income.SetIncome(revenues);
            var answs_1 = _questions.Where(q => q.TextQuestion.Contains("возраст")).FirstOrDefault().Answers.Select(a => a.TextAnswer);
            var answs_2 = _questions.Where(q => q.TextQuestion.Contains("пол.")).FirstOrDefault().Answers.Select(a => a.TextAnswer); ;
            try
            {
                string peopleGroup = answers.Where(a => answs_1.Contains(a.TextAnswer)).FirstOrDefault().TextAnswer;
                string gender = answers.Where(a => answs_2.Contains(a.TextAnswer)).FirstOrDefault().TextAnswer;

                if (peopleGroup.Contains("Дети") || peopleGroup.Contains("Подростки")) income.PVSDD = 13489.5;
                else if (peopleGroup.Contains("Пенсионеры")) income.PVSDD = 10945.5;
                else if (peopleGroup.Contains("Трудоспособные в возрасте") && gender.Contains("Женский")) income.PVSDD = 10945.5;
                else income.PVSDD = 14247;

                if (income.GetResultSDDFamily())
                {
                    Answer answer = new Answer
                    {
                        CostAnswer = 1000
                    };
                    answers.Add(answer);
                }
            }
            catch
            {
                if (revenues != null)
                {
                    income.PVSDD = 14247;

                    if (income.GetResultSDDFamily())
                    {
                        Answer answer = new Answer
                        {
                            CostAnswer = 1000
                        };
                        answers.Add(answer);
                    }
                }
            }
            
            _interview.Answers = new List<Answer>();
            foreach (var a in answers)
            {
                _interview.Answers.Add(a);
            }
            Result result = ManagerInterview.GetResultInterview(_interview);
            if (voicerON)
            {
                Task task = new Task(() =>
                {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak(_interview.TextResult);
                });
                task.Start();
            }
            return View(_interview);
        }
        
        public ActionResult PreIncome()
        {
            return View();
        }

        public ActionResult Income(string countPeople)
        {
            if (voicerON)
            {
                Task task = new Task(() =>
                {
                    SpeechSynthesizer spech = new SpeechSynthesizer();
                    spech.SetOutputToDefaultAudioDevice();
                    spech.Speak("Для определения условий предоставления социального обслуживания в зависимости от средне-душевого дохода семьи, необходимо ввести среднюю зароботную плату за месяц для каждого члена семьи");
                });
                task.Start();
            }
            try
            {
                _countFamilyPeople = Convert.ToInt32(countPeople);
            }
            catch
            {
                _countFamilyPeople = 1;
            }
            ViewData["countPeople"] = _countFamilyPeople;
            return View();
        }

        public ActionResult InostranecResult()
        {
            return View();
        }

    }
}