using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Data;
using HumanResourceHelth.Web.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class PlansController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Plans
        public ActionResult Index(string plan)
        {
            DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}"));
            ViewBag.Plan = plan;
            return View(myDir.GetDirectories());
        }

        public ActionResult Materials(string plan)
        {
            string lang = (string)Session["lang"] == "ar-EG" ? "Arabic" : "English";
            DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}/Categories{lang}"));
            ViewBag.Plan = plan;
            ViewBag.Lang = lang;
            return View(myDir.GetDirectories());
        }

        public ActionResult WarmUp()
        {
            //if (Session["UserId"] == null)
            //    return RedirectToAction("Index", "Login");

            string lang = (string)Session["lang"] == "ar-EG" ? "Arabic" : "English";
            DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/WarmUp/Categories{lang}/"));
            if (Session["UserId"] != null)
            {
                int numberOfChecks = 0;
                int userId = (int)Session["UserId"];
                numberOfChecks = _uow.SurveyRepo.Search(x => x.UserId == userId).Count();

                ViewBag.NumberOfChecks = numberOfChecks;
            }
            ViewBag.Lang = lang;
            return View(myDir.GetDirectories());
        }
        public ActionResult StartUp()
        {
            int userId = 0;
            ViewBag.isHaveStartupPlan = false;
            if (Session["UserId"] != null)
                
            {
                userId = (int)Session["UserId"];
                UserPlan startUpPlan = _uow.UserPlanRepo.Search(x => x.IsActive && x.UserId == userId && x.PlanId == (int)SubscriptionPlan.Startup).SingleOrDefault();
                if (startUpPlan == null)
                    ViewBag.isHaveStartupPlan = false;
                else
                    ViewBag.isHaveStartupPlan = true;
            }
            string lang = (string)Session["lang"] == "ar-EG" ? "Arabic" : "English";
            

            DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/StartUp/Categories{lang}/"));

 
            

            Plan plan = _uow.PlanRepo.FindById((int)SubscriptionPlan.Startup);
            ViewBag.AnnualPrice = plan.AnnualPrice;
            ViewBag.MonthlyPrice = plan.ManthlyPrice;
            ViewBag.Lang = lang;
            return View(myDir.GetDirectories());
        }

        public ActionResult ManualBuilder()
        {
            if (Session["UserId"] == null)
                ViewBag.PlanType = 0;
            else
            {
                int userId = (int)Session["UserId"];
                UserPlan builderPlan = _uow.UserPlanRepo.Search(x => x.IsActive && x.UserId == userId && x.PlanId == (int)SubscriptionPlan.ManualBuilder).SingleOrDefault();
                if (builderPlan != null)
                {
                    TimeSpan planPeriod = builderPlan.EndDate - builderPlan.StartDate;
                    if (planPeriod.TotalDays > 60)
                        ViewBag.PlanType = 2;
                    else
                        ViewBag.PlanType = 1;
                }
                else
                {
                    ViewBag.PlanType = 0;
                }
            }
            //if (Session["UserId"] == null)
            //    return RedirectToAction("Index", "Login");

            //int userId = (int)Session["UserId"];
            //UserPlan builderPlan = _uow.UserPlanRepo.Search(x => x.IsActive && x.UserId == userId && x.PlanId == (int)SubscriptionPlan.ManualBuilder).SingleOrDefault();
            //if (builderPlan != null)
            //{
            //    TimeSpan planPeriod = builderPlan.EndDate - builderPlan.StartDate;
            //    if (planPeriod.TotalDays > 60)
            //        ViewBag.PlanType = 2;
            //    else
            //        ViewBag.PlanType = 1;
            //}
            //else
            //{
            //    ViewBag.PlanType = 0;
            //}

            Plan plan = _uow.PlanRepo.FindById((int)SubscriptionPlan.ManualBuilder);
            ViewBag.AnnualPrice = plan.AnnualPrice;
            ViewBag.MonthlyPrice = plan.ManthlyPrice;

            return View();
        }

        public void CopySections(int userId)
        {
            List<Section> userSections = new List<Section>();
            if (_uow.SectionRepo.Search(x => x.UserId == userId).Count() > 0) return;
            int adminId = _uow.UserRepo.Search(x => x.IsAdmin).Single().Id;
            List<Section> adminSections = _uow.SectionRepo.Search(a => a.UserId == adminId).ToList();
            List<Section> parentAdminSections = adminSections.Where(x => x.ParenId == null).ToList();
            foreach (Section parent in parentAdminSections)
            {
                Section parentUserSection = new Section()
                {
                    Description = parent.Description,
                    Title = parent.Title,
                    Ordering = parent.Ordering,
                    UserId = userId,
                    LanguageId = parent.LanguageId,
                    IsActive = parent.IsActive,
                    Content = parent.Content,
                    Childs = new List<Section>(),
                };
                
                foreach (Section child in parent.Childs)
                {
                    Section childUserSection = new Section()
                    {
                        Description = child.Description,
                        Title = child.Title,
                        Ordering = child.Ordering,
                        UserId = int.Parse(Session["UserId"].ToString()),
                        LanguageId = parent.LanguageId,
                        IsActive = parent.IsActive,
                        Content = child.Content,
                    };
                    parentUserSection.Childs.Add(childUserSection);
                }
                userSections.Add(parentUserSection);
            }
            foreach(Section section in userSections)
            {
                _uow.SectionRepo.Add(section);
            }
            _uow.SaveChanges();
        }

        public ActionResult Doctor()
        {
            RequestServiceViewModel requestServiceViewModel = new RequestServiceViewModel()
            {

                Countries = _uow.CountryRepo.GetAll(),
                Industries = _uow.IndustryRepo.GetAll()

            };
            return View(requestServiceViewModel);
        }

        public ActionResult Tech()
        {
            RequestServiceViewModel requestServiceViewModel = new RequestServiceViewModel()
            {

                Countries = _uow.CountryRepo.GetAll(),
                Industries = _uow.IndustryRepo.GetAll()

            };
            return View(requestServiceViewModel);
        }

        public ActionResult Plugin()
        {
            RequestServiceViewModel requestServiceViewModel = new RequestServiceViewModel()
            {
               
                Countries=_uow.CountryRepo.GetAll(),
                Industries=_uow.IndustryRepo.GetAll()

            };
            return View(requestServiceViewModel);
        }

        public ActionResult Subscribe(int planId, SubscriptionPeriod subscriptionPeriod,int UserId)
        {
            Session["UserId"] = UserId;
                
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);

            int userId = (int)Session["UserId"];

            if(user.UserPlans.Where(x => x.PlanId == planId && x.IsActive).Count() == 1)
                return RedirectToAction("Index", "Section");

            if (planId == (int)SubscriptionPlan.ManualBuilder)
                CopySections(user.Id);

            double subscriptionPrice = 0;
            Plan plan = _uow.PlanRepo.FindById(planId); 
            DateTime subscriptionEndDate = DateTime.Today;
            switch (subscriptionPeriod)
            {
                case SubscriptionPeriod.Month:
                    subscriptionEndDate = DateTime.Today.AddMonths(1).AddDays(-1);
                    subscriptionPrice = plan.ManthlyPrice;
                    break;
                case SubscriptionPeriod.Year:
                    subscriptionEndDate = DateTime.Today.AddYears(1).AddDays(-1);
                    subscriptionPrice = plan.AnnualPrice;
                    break;
                case SubscriptionPeriod.UpgradeToYear:
                    subscriptionEndDate = DateTime.Today.AddYears(1).AddDays(-1);
                    subscriptionPrice = plan.AnnualPrice;
                    break;
            }


            UserPlan myPlan = new UserPlan()
            {
                StartDate = DateTime.Now,
                EndDate = subscriptionEndDate,
                PlanId = planId,
                IsActive = true,
                UserId = userId,
                Price = subscriptionPrice
            };



            _uow.UserPlanRepo.Add(myPlan);
            _uow.SaveChanges();

            SubscriptionViewModel mySubscription = new SubscriptionViewModel()
            {
                Plan = _uow.PlanRepo.FindById(planId),
                UserPlan = myPlan,
                SubscriptionPeriod =  subscriptionPeriod
            };
            Session["User"] = _uow.UserRepo.FindById((int)Session["UserId"]);


            return View(mySubscription);
        }

        [HttpPost]
        public ActionResult PluginRequest(PluginRequest pluginRequest)
        {
            if (Session["UserId"] == null)
            {
                User user = _uow.UserRepo.Search(x => x.IsAdmin).Single();
                pluginRequest.UserId = user.Id;
            }
            else
                pluginRequest.UserId = (int)Session["UserId"];
            pluginRequest.RequestDate = DateTime.Now;
            pluginRequest.PlanName = SubscriptionPlan.PlugIn;
            if (ModelState.IsValid)
            {
                _uow.PluginRequestRepo.Add(pluginRequest);
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult TechRequest(TechRequest techRequest)
        {
            
            if(Session["UserId"] == null)
            {
                User user = _uow.UserRepo.Search(x => x.IsAdmin).Single();
                techRequest.UserId = user.Id;
            }
            else
                techRequest.UserId = (int)Session["UserId"];
            techRequest.RequestDate = DateTime.Now;
            techRequest.PlanName = SubscriptionPlan.Tech;
            if (ModelState.IsValid)
            {
                _uow.TechRequestRepo.Add(techRequest);
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult DoctorRequest(DoctorRequest doctorRequest)
        {

            if (Session["UserId"] == null)
            {
                User user = _uow.UserRepo.Search(x => x.IsAdmin).Single();
                doctorRequest.UserId = user.Id;
            }
            else
                doctorRequest.UserId = (int)Session["UserId"];
            doctorRequest.RequestDate = DateTime.Now;
            doctorRequest.PlanName = SubscriptionPlan.Tech;
            if (ModelState.IsValid)
            {
                _uow.DoctorRequestRepo.Add(doctorRequest);
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

    }
}