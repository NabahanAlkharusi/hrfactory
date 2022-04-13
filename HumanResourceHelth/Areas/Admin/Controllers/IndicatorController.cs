using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Areas.Admin.Data;
using HumanResourceHelth.Web.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class IndicatorController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Admin/Indicator
        public ActionResult Index()
        {
            List<Indicator> indicators = _uow.IndicatorRepo.GetAll();
            return View(indicators);
        }

        public ActionResult Free()
        {
            List<Indicator> indicators = _uow.IndicatorRepo.Search(x => x.SurveyTypeId == 1).ToList();
            ViewBag.SurveyType = "For Free Plan";
            ViewBag.SurveyTypeId = 1;
            return View("Index", indicators);
        }

        public ActionResult Paid()
        {
            List<Indicator> indicators = _uow.IndicatorRepo.Search(x => x.SurveyTypeId == 2).ToList();
            ViewBag.SurveyType = "For Paid Plan";
            ViewBag.SurveyTypeId = 2;
            return View("Index", indicators);
        }
        public ActionResult Business()
        {
            List<Indicator> indicators = _uow.IndicatorRepo.Search(x => x.SurveyTypeId == 3).ToList();
            ViewBag.SurveyType = "For Business Health Check";
            ViewBag.SurveyTypeId = 3;
            return View("Index", indicators);
        }
        public ActionResult Edit(int indicatorId)
        {
            Indicator indicator = _uow.IndicatorRepo.FindById(indicatorId);
            if (indicator.SurveyTypeId == 1)
            {
                ViewBag.SurveyTypeId = 1;
                ViewBag.SurveyType = "For Free Plan";
            }
            else if (indicator.SurveyTypeId == 2)
            {
                ViewBag.SurveyTypeId = 2;
                ViewBag.SurveyType = "For Paid Plan";
            }
            else
            {
                ViewBag.SurveyTypeId = 3;
                ViewBag.SurveyType = "Business Health Check";
            }
            IndicatorViewModel indicatorViewModel = new IndicatorViewModel()
            {
                Id = indicatorId,
                Name = indicator.Name,
                NameAr = indicator.NameAr,
                SurveyTypeId = indicator.SurveyTypeId,
            };
            return View("_Save", indicatorViewModel);
        }
        public ActionResult Create(int checkType)
        {
            if (checkType == 1)
            {
                ViewBag.SurveyTypeId = 1;
                ViewBag.SurveyType = "For Free Plan";
            }
            else if (checkType == 1)
            {
                ViewBag.SurveyTypeId = 2;
                ViewBag.SurveyType = "For Paid Plan";
            }
            else
            {
                ViewBag.SurveyTypeId = 3;
                ViewBag.SurveyType = "Business Health Check";
            }
            IndicatorViewModel indicatorViewModel = new IndicatorViewModel();
            return View("_Save", indicatorViewModel);
        }
        public ActionResult Delete(int id)
        {
            Indicator indicator = _uow.IndicatorRepo.FindById(id);
            _uow.IndicatorRepo.Delete(indicator);
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
        public ActionResult Save(Indicator indicator)
        {
            _uow.IndicatorRepo.Add(indicator);
            _uow.IndicatorRepo.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Update(string name, string nameAr, int surveyTypeId, int id)
        {
            Indicator myIndicator = _uow.IndicatorRepo.FindById(id);
            myIndicator.Name = name;
            myIndicator.NameAr = nameAr;
            myIndicator.SurveyTypeId = surveyTypeId;

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