﻿using System;
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

        ManagerInterview _manager = new ManagerInterview(db);

        ICollection<Question> _questions = new List<Question>();

        // GET: Interview
        public ActionResult Index(int numberQuestion)
        {


            _manager.StartInterview();

            _questions = _manager.questions;

            return View(_questions.ElementAt(numberQuestion));
        }

        /*public ActionResult Initial()
        {
            Question question = new Question();

            ICollection<Question> questions = new List<Question>();

            ICollection<Answer> answers = new List<Answer>();

            for(int i=0; i<2; i++)
            {
                Answer answer = new Answer();
                answer.CheckedAnswer = false;
                answer.CostAnswer = 0;
                answer.IDAnswer = (byte)(i+1);
                answer.TextAnswer = $"ответ" + i + $"на вопрос 1";
                answers.Add(answer);
            }
            question.IDQuestion = 1;
            question.TextQuestion = $"Вопрос 1";
            question.Answers = answers;
            db.Questions.Add(question);
            db.SaveChanges();
            return View();
        }*/
       
    }
}