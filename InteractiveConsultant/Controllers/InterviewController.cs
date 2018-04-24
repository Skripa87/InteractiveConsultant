using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using InteractiveConsultant.Models;
using suggestionscsharp;

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
        // GET: Interview
        public ActionResult Index(string action, string address, List<string> bufer)
        {
            if (action == "No")
            {
                Coords.Lng = "";
                Coords.Lat = "";
                return RedirectToAction("UsersInputLocation", "GeoLocation");
            }
            else
            {
                numberQuestion = 0;
                _countFamilyPeople = 0;
                answers = new List<Answer>();
                checkQuestions = new List<bool>();
                disabledQuestion = new List<bool>();
                _interview = new Interview();    // Создание экземпляра класса интервью для текущего пользователя
                _questions = new List<Question>(); // Создание экземпляра класса List Вопросов
                if (StateInterview._checked == false) return RedirectToAction("ErrorAgree", "Home"); // Организовать переход к результату
                ManagerInterview.Agreement = StateInterview._checked; //Установка переменной "согласие на прохождение опроса" в положение выбранное пользователем
                ManagerInterview.StartInterview(_questions, _interview); // инициализация всех переменных текущего опроса
                foreach (var q in _questions)
                {
                    answers.Add(null);
                    checkQuestions.Add(false);
                }
                ViewData["Cheker"] = checkQuestions;
                if(bufer!=null)
                {
                    LocationUser.Country = bufer[0];
                    LocationUser.Region = bufer[1];
                    if (bufer[2] == "") { LocationUser.Area = bufer[3]; }
                    else LocationUser.Area = bufer[2];
                    LocationUser.City = bufer[3];
                }
                else
                {
                    var token = "4004c1e7cb9c2523dae8fc6a885bc74c140d3d90";
                    var url = "https://suggestions.dadata.ru/suggestions/api/4_1/rs";
                    var api = new SuggestClient(token, url);
                    var query = LocationUser.Country +" г. "+LocationUser.City;
                    var response = api.QueryAddress(query);
                    if (response.suggestions[0].data.area != null) { LocationUser.Area = response.suggestions[0].data.area; }
                    else LocationUser.Area = LocationUser.City;
                }
                return View(_questions.FirstOrDefault());
            }            
        }

        public ActionResult StartInterview(bool _voicerON)
        {
            StateInterview._voicerOn = _voicerON;
            foreach (var a in answers)
            {
                if (a != null) checkQuestions[answers.IndexOf(a)] = true;
                else checkQuestions[answers.IndexOf(a)] = false;
            }
            ViewData["Cheker"] = checkQuestions;
            if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
            return View(_questions.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult PreNextQuestion(string action, string responses, bool _voicerON)
        {
            StateInterview._voicerOn = _voicerON;
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
            if (action.Equals("next"))
            {
                numberQuestion++;
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (numberQuestion < _questions.Count) if  (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                if (numberQuestion > _questions.Count - 1) return RedirectToAction("PreIncome", "Interview");
                else return View(_questions.ElementAt(numberQuestion));
            }
            else if (action.Equals("previos"))
            {
                numberQuestion--;
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                if(numberQuestion == 0) return RedirectToAction("StartInterview", new {_voicerON});
                else return View(_questions.ElementAt(numberQuestion));                
            }
            else if (action.Contains("page"))
            {
                numberQuestion = Convert.ToInt32(action.Substring(action.IndexOf('_') + 1));
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
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
            return View(_interview);
        }
        
        public ActionResult PreIncome()
        {
            return View();
        }

        public ActionResult Income(string countPeople)
        {
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