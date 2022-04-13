using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class QuestionController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Admin/Question
        public ActionResult Index(int indicatorID)
            
        {
            ViewBag.indicatorId = indicatorID;
            List<Question> Questions = _uow.QuestionRepo.GetAll().Where(a=>a.IndicatorId== indicatorID).ToList();
            

            return View(Questions);
        }
        public ActionResult Edit(int questionId)
        {
            Question question = _uow.QuestionRepo.FindById(questionId);
            QuestionViewModel questionViewModel = new QuestionViewModel()
            {
                Id = questionId,
                Name = question.Name,
                NameAr = question.NameAr,
                Practice = question.Practice,
                PracticeAr = question.PracticeAr,
                IndicatorId=question.IndicatorId,
                indicators = _uow.IndicatorRepo.GetAll()
            };
            return View("_Save" , questionViewModel);
        }
        public ActionResult Create(int indicatorId)
        {
            int surveyID = _uow.IndicatorRepo.FindById(indicatorId).SurveyTypeId;
            QuestionViewModel questionViewModel = new QuestionViewModel()
            {
                IndicatorId= indicatorId,
                indicators = _uow.IndicatorRepo.GetAll().Where(z=>z.SurveyTypeId==surveyID).ToList()
            };
            return View("_Save", questionViewModel);
        }
        public ActionResult Save(Question question)
        {
            _uow.QuestionRepo.Add(question);
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public ActionResult Delete(int id)
        {
            Question question = _uow.QuestionRepo.FindById(id);
            _uow.QuestionRepo.Delete(question);


            try
            {
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend");
            }
        }

        [HttpPost]
        public ActionResult Update(string name, string nameAr, string practice, string practiceAr, int indicatorId, int id)
        {
            Question myQuestion = _uow.QuestionRepo.FindById(id);
            myQuestion.Name = name;
            myQuestion.NameAr = nameAr;
            myQuestion.Practice = practice;
            myQuestion.PracticeAr = practiceAr;
            myQuestion.IndicatorId = indicatorId;
            

            try
            {
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend");
            }
        }
    }
   
}