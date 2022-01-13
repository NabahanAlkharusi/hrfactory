using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Data;
using HumanResourceHelth.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HumanResourceHelth.Web.Controllers
{
    public class SurveyController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Survey
        public ActionResult Index()
        {

            int surveyTypeId = 0;
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            int userId = (int)Session["UserId"];
            User user = _uow.UserRepo.FindById(userId);
            if (user.UserPlans.Where(x => x.PlanId == (int)SubscriptionPlan.Startup && x.IsActive).Count() > 0)
                surveyTypeId = 2;
            else
            {
                if (_uow.SurveyRepo.Search(x => x.UserId == userId).Count() == 0)
                    surveyTypeId = 1;
            }

            if (surveyTypeId == 0)
                return RedirectToAction("Index", "Home");

            List<Indicator> indicators = _uow.IndicatorRepo.Search(x => x.SurveyTypeId == surveyTypeId).ToList();
            SurveyViewModel surveyViewModel = new SurveyViewModel()
            {
                indicators=indicators
            };
            ViewBag.SurveyTypeId = surveyTypeId;
            return View(surveyViewModel);
        }

        [HttpPost]
        public ActionResult Save(List<AnswersViewModel> answers, int surveyTypeId)
        {
            Survey survey = new Survey()
            {
                Name = "Survey On " + DateTime.Now.ToString("dd MMM yyyy HH:mm"),
                SurveyDate = DateTime.Now,
                SurveyTypeId = surveyTypeId,
                UserId = (int)Session["UserId"]
            };

            survey.Answers = CreateAnswersList(answers);
            survey.SurveyResults = CreateSurveyResultList(answers, surveyTypeId);

            _uow.SurveyRepo.Add(survey);
            _uow.SurveyRepo.SaveChanges();
            //_uow.SaveChanges();


            return Json(new { Success = true, Result = survey.Id }, JsonRequestBehavior.AllowGet);
            //return PartialView("_Result", survey.SurveyResults);
        }

        private List<SurveyResult> CreateSurveyResultList(List<AnswersViewModel> answers, int surveyTypeId)
        {
            List<SurveyResult> surveyResults = new List<SurveyResult>();
            foreach(Indicator indicator in _uow.IndicatorRepo.Search(x => x.SurveyTypeId == surveyTypeId && x.Questions.Count > 0))
            {
                //if (indicator.Questions.Count == 0) continue;
                SurveyResult surveyResult = new SurveyResult()
                {
                    IndicatorId = indicator.Id,
                    Result = answers.Where(x => x.IndicatorId == indicator.Id && x.Answer != -1).Sum(x => x.Answer) / answers.Where(x => x.IndicatorId == indicator.Id && x.Answer != -1).Count()
                };
                surveyResults.Add(surveyResult);
            }
            
            return surveyResults;

        }

        private List<Answer> CreateAnswersList(List<AnswersViewModel> answers)
        {
            List<Answer> surveyAnswers = new List<Answer>();
            foreach (AnswersViewModel answer in answers)
            {
                Answer myAnswer = new Answer()
                {
                    QuestionId = answer.QuestionId,
                    Mark = answer.Answer
                };
                surveyAnswers.Add(myAnswer);
            }
            return surveyAnswers;
        }

        public ViewResult SurveyResult(int surveyId)
        {
            ViewBag.SurveyId = surveyId;
            return View();
        }

        public PartialViewResult Result(int surveyId)
        {
            Survey mySurvey = _uow.SurveyRepo.FindById(surveyId);
            
            List<Result> results = new List<Result>();

            foreach(Answer answer in mySurvey.Answers)
            {
                Result result = new Result()
                {
                    Answer = answer,
                    Question = answer.Question,
                    Indicator = answer.Question.Indicator,
                    SurveyResult = mySurvey.SurveyResults.Where(x => x.IndicatorId == answer.Question.Indicator.Id).Single()
                };
                results.Add(result);
            }



            SurveyResultViewModels surveyResultViewModels = new SurveyResultViewModels()
            {
                Survey = mySurvey,
                Results = results
            };
            
            return PartialView("_Result", surveyResultViewModels);
        }

        public ViewResult List(int userId)
        {
            List<Survey> surveys = _uow.SurveyRepo.Search(x => x.UserId == userId).ToList();
            return View(surveys);

        }

        public ActionResult History()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            int userId = (int)Session["UserId"];
            List<Survey> surveies = _uow.SurveyRepo.Search(x => x.UserId == userId).ToList();
            return View(surveies);
        }

        public ActionResult Intro()
        {
            return View();
        }
    }
}