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
        static List<Question> _questions;
        static List<Question> questions;
        static Interview _interview;
        static int numberQuestion;
        static int _countFamilyPeople;
        static List<Answer> answers;
        static List<bool> checkQuestions;
        static List<bool> disabledQuestion;
        static int? lastChangedAnswer;
        static Answer zeroAnswer;
        // GET: Interview
        private void ReconfigurateQuestions(Answer answer, List<Question> questions, List<Question> startQuestions, int number, List<bool> checkQuestions, Answer zeroAnswer, List<Answer> answers, bool? cheker, int? lastChangedAnswer)
        {
            List<string> qNext = new List<string>();
            if (cheker == true)
            {
                for (int i = (int)lastChangedAnswer + 1; i < answers.Count; i++)
                {
                    answers[i] = null;
                }
            }
            var minimal = answers.Where(a => a != null).Select(sa => sa?.NextQuestions?.Length)?.Min();
            if (minimal == null || minimal > answer?.NextQuestions?.Length)
            { 
                qNext = answer?.NextQuestions?.Split(';').ToList();
            }
            else
            {
                qNext = answers.Where(a => a != null && a?.NextQuestions?.Length == minimal).Select(sa => sa?.NextQuestions)?.FirstOrDefault()?.Split(';')?.ToList();
            }
            if(qNext == null) { return; }
            questions.RemoveAll(q => questions.IndexOf(q) > number && !qNext.Contains(q.IDQuestion.ToString()));
            var epsondQ = qNext.Where(qn => !questions.Select(q => q.IDQuestion.ToString()).Contains(qn)).ToList();
            if (epsondQ.Count > 0)
            {
                foreach (var item in epsondQ)
                {
                    var x = startQuestions.Where(sq => sq.IDQuestion.ToString() == item)?.FirstOrDefault();
                    if(x == null) { continue; }
                    if (startQuestions.IndexOf(x) > questions.Count - 1)
                    {
                        questions.Add(x);
                    }
                    else
                    {
                        questions.Insert(startQuestions.IndexOf(x), x);
                    }
                }
            }
            checkQuestions.RemoveAll(c=>true);
            List<Answer> _answers = new List<Answer>();
            foreach (var q in questions)
            {
                checkQuestions.Add(false);
                _answers.Add(null);
            }
            questions.Sort();            
            foreach (var item in answers)
            {
                if(item == null) { continue; }
                var x = questions.Where(q => q.Answers.Select(a => a.IDAnswer).Contains(item.IDAnswer))?.FirstOrDefault();
                if (x == null) { continue; }
                _answers[questions.IndexOf(x)]= item;                
            }
            answers.RemoveAll(a => true);
            foreach (var item in _answers)
            {
                answers.Add(item);
            }
            foreach(var a in _answers)
            {
                if (a != null) checkQuestions[answers.IndexOf(a)] = true;
                else checkQuestions[answers.IndexOf(a)] = false;
            }
        }

        public ActionResult Index(string action, string address, List<string> bufer)
        {
            zeroAnswer = new Answer()
            {
                NextQuestions = "2;3;4;5;6;7;8;10;11;12;13;14;15;16;17;18",
                Recomendation = null
            };            
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
                questions = new List<Question>();
                LocationUser.TargetingGender = "";
                if (StateInterview._checked == false) return RedirectToAction("ErrorAgree", "Home"); // Организовать переход к результату
                ManagerInterview.Agreement = StateInterview._checked; //Установка переменной "согласие на прохождение опроса" в положение выбранное пользователем
                ManagerInterview.StartInterview(_questions, _interview); // инициализация всех переменных текущего опроса
                _questions.Sort();
                foreach (var q in _questions)
                {
                    questions.Add(q);
                    answers.Add(null);
                    checkQuestions.Add(false);
                }
                ReconfigurateQuestions(zeroAnswer,questions,_questions,numberQuestion,checkQuestions,zeroAnswer, answers, false, null);
                ViewData["Cheker"] = checkQuestions;
                if (bufer != null)
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
                    var query = LocationUser.Country + " г. " + LocationUser.City;
                    var response = api.QueryAddress(query);
                    if (response.suggestions[0].data.area != null) { LocationUser.Area = response.suggestions[0].data.area; }
                    else LocationUser.Area = LocationUser.City;
                }
                return View(questions.FirstOrDefault());
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
            if (answers[numberQuestion] != null)
            {
                ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                ReconfigurateQuestions(answers[numberQuestion], questions, _questions, numberQuestion, checkQuestions, zeroAnswer, answers, false, lastChangedAnswer);
            }
            else
            {                
                ReconfigurateQuestions(zeroAnswer, questions, _questions, numberQuestion, checkQuestions, zeroAnswer, answers, false,lastChangedAnswer);
            }
            return View(questions.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult PreNextQuestion(string action, string responses, bool _voicerON)
        {
            lastChangedAnswer = null;
            bool? cheker = null;
            StateInterview._voicerOn = _voicerON;
            if (responses != null)
            {
                var answ = questions.ElementAt(numberQuestion).Answers.Where(a => a.IDAnswer.ToString().Equals(responses)).FirstOrDefault();
                if (answers[numberQuestion] != null && answers[numberQuestion].IDAnswer != answ.IDAnswer )
                {
                    cheker = true;
                    lastChangedAnswer = numberQuestion;
                }
                else
                {
                    cheker = false;
                }
                answers[numberQuestion] = answ;
                ReconfigurateQuestions(answers[numberQuestion], questions, _questions, numberQuestion, checkQuestions, zeroAnswer, answers,cheker,lastChangedAnswer);
                if (numberQuestion == 0 && answers[0].TextAnswer.ToLower().Contains("иностран"))
                    return RedirectToAction("InostranecResult");
            }
            foreach (var a in answers)
            {
                if (a != null)
                {
                    checkQuestions[answers.IndexOf(a)] = true;
                }
                else
                {
                    checkQuestions[answers.IndexOf(a)] = false;
                }
            }
            ViewData["Cheker"] = checkQuestions;
            if (action.Equals("next"))
            {
                numberQuestion++;
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (numberQuestion < questions.Count) if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                if (numberQuestion > questions.Count - 1) return RedirectToAction("PreIncome", "Interview");
                else return View(questions.ElementAt(numberQuestion));
            }
            else if (action.Equals("previos"))
            {
                numberQuestion--;
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (answers[numberQuestion] != null)
                {
                    ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                    ReconfigurateQuestions(answers[numberQuestion], questions, _questions, numberQuestion, checkQuestions, zeroAnswer, answers,cheker,lastChangedAnswer);
                }
                if (numberQuestion == 0) return RedirectToAction("StartInterview", new { _voicerON });
                else return View(questions.ElementAt(numberQuestion));
            }
            else if (action.Contains("page"))
            {
                numberQuestion = Convert.ToInt32(action.Substring(action.IndexOf('_') + 1));
                ViewData["numberQuestion"] = numberQuestion + 1;
                if (answers[numberQuestion] != null) ViewData["AnswerID"] = answers[numberQuestion].IDAnswer;
                if (numberQuestion == 0) return RedirectToAction("StartInterview", new { _voicerON });
                else return View(questions.ElementAt(numberQuestion));
            }
            else if (action.Equals("end"))
            {
                return RedirectToAction("ResultPage", new List<string> { });
            }
            else if (action.Equals("endIncome"))
            {
                return RedirectToAction("PreIncome");
            }
            return View();
        }

        public ActionResult ResultPage(List<string> revenues)
        {
            LocationUser.IsChilde = false;
            LocationUser.TargetingGender = "";
            Income income = new Income(_countFamilyPeople);
            income.SetIncome(revenues);
            var answs_1 = _questions.Where(q => q.TextQuestion.Contains("возраст")).FirstOrDefault().Answers.Select(a => a.TextAnswer);
            var answs_2 = _questions.Where(q => q.TextQuestion.Contains("пол.")).FirstOrDefault().Answers.Select(a => a.TextAnswer); ;
            try
            {
                string peopleGroup = answers.Where(a => answs_1.Contains(a.TextAnswer)).FirstOrDefault().TextAnswer;
                string gender = answers.Where(a => answs_2.Contains(a.TextAnswer)).FirstOrDefault().TextAnswer;

                if (peopleGroup.ToLower().Trim().Contains("дети") || peopleGroup.ToLower().Trim().Contains("подростки")) { income.PVSDD = 13489.5; LocationUser.IsChilde = true; }
                else if (peopleGroup.ToLower().Trim().Contains("пенсионеры")) income.PVSDD = 10945.5;
                else if (peopleGroup.ToLower().Trim().Contains("трудоспособные в возрасте") && gender.ToLower().Trim().Contains("женский")) { income.PVSDD = 10945.5; LocationUser.TargetingGender = "w"; }
                else if (gender.ToLower().Trim().Contains("женский")) { LocationUser.TargetingGender = "w"; }
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
            int scorePointforTest = 0;
            Result result = ManagerInterview.GetResultInterview(_interview, out scorePointforTest);
            return View(_interview.Result);
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