using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Web.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Admin/Home
        public ActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                FreeIndicators = _uow.IndicatorRepo.Search(x => x.SurveyTypeId == 1).Count(),
                PaidIndicators = _uow.IndicatorRepo.Search(x => x.SurveyTypeId == 2).Count(),
                Users = _uow.UserRepo.Search(x => x.IsAdmin == false && x.IsDeleted == false).Count(),
                Countries = _uow.CountryRepo.Count(),
                Industreis = _uow.IndustryRepo.Count(),
                TechRequests = _uow.TechRequestRepo.Count(),
                PluginRequests = _uow.PluginRequestRepo.Count(),
                Builders = _uow.SectionRepo.NumberOfBuilders() - 1,
                Courses = 0,
                Experts = _uow.ExpertsProfileRepo.Count()
            };

            return View(homeViewModel);
        }
    }
}