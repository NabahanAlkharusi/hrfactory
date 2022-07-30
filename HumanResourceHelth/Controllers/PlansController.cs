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
        bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        // GET: Plans
        public ActionResult Index(string plan)
        {
            DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}"));
            ViewBag.Plan = plan;
            return View(myDir.GetDirectories());
        }

        public ActionResult Materials(string plan)
        {
            int userId = 0;
            //int planId = 0;
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            else
                userId = (int)Session["UserId"];
            int Plan = (int)Enum.Parse(typeof(SubscriptionPlan), plan);
            getPlanFiles(Plan, userId);
            Session["FilesPlan"] = plan;
            Session["FilesPlanId"] = Plan;
            ViewBag.Isfree = Plan == 1 ? false : _uow.UserPlanRepo.Search(x => x.UserId == userId && x.IsActive && x.PlanId == Plan).SingleOrDefault().IsFreeActive;
            //if (plan == "StartUp")
            //{
            //    planId = (int)SubscriptionPlan.Startup;
            //    ViewBag.Isfree = _uow.UserPlanRepo.Search(x => x.UserId == userId && x.IsActive && x.PlanId == planId).SingleOrDefault().IsFreeActive;
            //    string lang = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft ? "Arabic" : "English";
            //    DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}/Categories{lang}"));
            //    ViewBag.Plan = plan;
            //    ViewBag.Lang = lang;
            //    return View(myDir.GetDirectories());
            //}
            //else
            //{
            //    ViewBag.Isfree = false;
            //    string lang = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft ? "Arabic" : "English";
            //    DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}/Categories{lang}"));
            //    ViewBag.Plan = plan;
            //    ViewBag.Lang = lang;
            //    return View(myDir.GetDirectories());

            //}
            return View();

        }

        private void getPlanFiles(int Plan, int userId = 0)
        {
            int CountryId = userId == 0 ? 0 : _uow.UserRepo.FindById(userId).CountryId;
            List<Category> planCategories = _uow.CategoryRepo.Search(z => z.CategoryPlan == Plan && z.isActive).OrderByDescending(a => a.Id).ToList();
            List<DocFile> PlanFiles = new List<DocFile>();
            foreach (Category category in planCategories)
            {
                List<DocFile> docs = _uow.DocFileRepo.GetAll().ToList();
                List<DocFile> UserCountrdocs = docs.Where(a => a.CategoryId == category.Id && a.isFileActive && a.CountryId == CountryId).ToList();
                UserCountrdocs = UserCountrdocs.Count > 0 ? UserCountrdocs : docs.Where(a => a.CategoryId == category.Id && a.isFileActive && a.CountryId == 158).ToList();
                foreach (DocFile docFile in UserCountrdocs)
                {
                    PlanFiles.Add(docFile);
                }
                if (userId != 0)
                {
                    UserCountrdocs = docs.Where(a => a.CategoryId == category.Id && a.isFileActive && a.CountryId == 0).ToList();
                    foreach (DocFile docFile in UserCountrdocs)
                    {
                        PlanFiles.Add(docFile);
                    }
                }
            }
            Session["PlanFiles"] = PlanFiles;
            Session["planCategories"] = planCategories;
        }

        public ActionResult WarmUp()
        {
            //if (Session["UserId"] == null)
            //    return RedirectToAction("Index", "Login");
            int userId = 0;
            string lang = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft ? "Arabic" : "English";
            DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/WarmUp/Categories{lang}/"));
            if (Session["UserId"] != null)
            {
                int numberOfChecks = 0;
                int numberOfBHChecks = 0;
                userId = (int)Session["UserId"];
                numberOfChecks = _uow.SurveyRepo.Search(x => x.UserId == userId).Count();
                numberOfBHChecks = _uow.SurveyRepo.Search(x => x.UserId == userId && x.SurveyTypeId == 3).Count();
                ViewBag.NumberOfChecks = numberOfChecks;
                ViewBag.numberOfBHChecks = numberOfBHChecks;
            }
            ViewBag.Lang = lang;
            getPlanFiles((int)SubscriptionPlan.WarmUp, userId);
            return View();
        }
        public ActionResult StartUp()
        {
            int userId = 0;
            ViewBag.isHaveStartupPlan = false;
            ViewBag.isFreePlan = false;
            if (Session["UserId"] != null)

            {
                userId = (int)Session["UserId"];
                int UserCountryId = _uow.UserRepo.FindById(userId).CountryId;
                TermsConditions terms = _uow.TermsConditionsRepo.Search(a => a.CountryId == UserCountryId && a.TermsConditionType == (int)TermsConditionType.StartUpPlanMonthly).FirstOrDefault();
                terms = terms == null ? _uow.TermsConditionsRepo.Search(a => a.CountryId == 158 && a.TermsConditionType == (int)TermsConditionType.StartUpPlanMonthly).FirstOrDefault() : terms;

                ViewBag.SUMTitle = isArabic ? terms.ArabicTitle : terms.EnglishTitle;
                ViewBag.SUMText = isArabic ? terms.ArabicText : terms.EnglishText;
                terms = _uow.TermsConditionsRepo.Search(a => a.CountryId == UserCountryId && a.TermsConditionType == (int)TermsConditionType.StartUpPlanAnnually).FirstOrDefault();
                terms = terms == null ? _uow.TermsConditionsRepo.Search(a => a.CountryId == 158 && a.TermsConditionType == (int)TermsConditionType.StartUpPlanAnnually).FirstOrDefault() : terms;

                ViewBag.SUATitle = isArabic ? terms.ArabicTitle : terms.EnglishTitle;
                ViewBag.SUAText = isArabic ? terms.ArabicText : terms.EnglishText;

                UserPlan startUpPlan = _uow.UserPlanRepo.Search(x => x.IsActive && x.UserId == userId && x.PlanId == (int)SubscriptionPlan.Startup).SingleOrDefault();
                if (startUpPlan == null)
                    ViewBag.isHaveStartupPlan = false;
                else
                {
                    ViewBag.isHaveStartupPlan = true;
                    ViewBag.isFreePlan = startUpPlan.IsFreeActive;
                }
            }
            string lang = isArabic ? "Arabic" : "English";
            getPlanFiles((int)SubscriptionPlan.Startup, userId);

            //DirectoryInfo myDir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/StartUp/Categories{lang}/"));




            Plan plan = _uow.PlanRepo.FindById((int)SubscriptionPlan.Startup);
            ViewBag.AnnualPrice = plan.AnnualPrice;
            ViewBag.MonthlyPrice = plan.ManthlyPrice;
            ViewBag.AnnualPriceDisp = (int)(plan.AnnualPrice * 2.6008);
            ViewBag.MonthlyPriceDisp = (int)(plan.ManthlyPrice * 2.6008);
            ViewBag.Lang = lang;
            return View();
        }

        public ActionResult ManualBuilder()
        {


            ViewBag.isFreePlan = false;
            if (Session["UserId"] == null)
            {
                ViewBag.PlanType = 0;

            }
            else
            {

                int userId = (int)Session["UserId"];
                int UserCountryId = _uow.UserRepo.FindById(userId).CountryId;
                TermsConditions terms = _uow.TermsConditionsRepo.Search(a => a.CountryId == UserCountryId && a.TermsConditionType == (int)TermsConditionType.ManualBuilderPlanMonthly).FirstOrDefault();
                terms = terms == null ? _uow.TermsConditionsRepo.Search(a => a.CountryId == 158 && a.TermsConditionType == (int)TermsConditionType.ManualBuilderPlanMonthly).FirstOrDefault() : terms;

                ViewBag.MBMTitle = isArabic ? terms.ArabicTitle : terms.EnglishTitle;
                ViewBag.MBMText = isArabic ? terms.ArabicText : terms.EnglishText;

                terms = _uow.TermsConditionsRepo.Search(a => a.CountryId == UserCountryId && a.TermsConditionType == (int)TermsConditionType.ManualBuilderPlanAnnually).FirstOrDefault();
                terms = terms == null ? _uow.TermsConditionsRepo.Search(a => a.CountryId == 158 && a.TermsConditionType == (int)TermsConditionType.ManualBuilderPlanAnnually).FirstOrDefault() : terms;

                ViewBag.MBATitle = isArabic ? terms.ArabicTitle : terms.EnglishTitle;
                ViewBag.MBAText = isArabic ? terms.ArabicText : terms.EnglishText;
                UserPlan builderPlan = _uow.UserPlanRepo.Search(x => x.IsActive && x.UserId == userId && x.PlanId == (int)SubscriptionPlan.ManualBuilder).SingleOrDefault();
                if (builderPlan != null)
                {
                    ViewBag.isFreePlan = builderPlan.IsFreeActive ? true : false;
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
            ViewBag.AnnualPriceDisp = (int)(plan.AnnualPrice * 2.6008);
            ViewBag.MonthlyPriceDisp = (int)(plan.ManthlyPrice * 2.6008);
            return View();
        }

        public void CopySections(int userId, bool isTrial = false)
        {
            List<Section> userSections = new List<Section>();
            if (_uow.SectionRepo.Search(x => x.UserId == userId).Count() > 0) return;
            int adminId = _uow.UserRepo.Search(x => x.IsAdmin).Single().Id;
            User user = _uow.UserRepo.FindById(userId);
            List<Section> adminSections = _uow.SectionRepo.Search(a => a.UserId == adminId && a.CountryID == user.CountryId).ToList();
            if (adminSections.Count == 0)
                adminSections = _uow.SectionRepo.Search(a => a.UserId == adminId && a.CountryID == 158).ToList();
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
                    CountryID = parent.CountryID,
                    SectionId = parent.SectionId,
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
                        CountryID = child.CountryID,
                        SectionId = child.SectionId,
                    };
                    parentUserSection.Childs.Add(childUserSection);
                }
                userSections.Add(parentUserSection);
            }
            foreach (Section section in userSections)
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

                Countries = _uow.CountryRepo.GetAll(),
                Industries = _uow.IndustryRepo.GetAll()

            };
            return View(requestServiceViewModel);
        }

        public ActionResult Subscribe(int planId, SubscriptionPeriod subscriptionPeriod, int UserId, bool isTrail = false)
        {
            bool isUpgrade = false;
            Session["UserId"] = UserId;

            User user = _uow.UserRepo.FindById((int)Session["UserId"]);

            int userId = (int)Session["UserId"];

            if (user.UserPlans.Where(x => x.PlanId == planId && x.IsActive).Count() == 1)
            {
                UserPlan userPlan =
                    user.UserPlans.Where(x => x.PlanId == planId && x.IsActive && x.IsFreeActive).Count() == 1 ?
                    user.UserPlans.Where(x => x.PlanId == planId && x.IsActive && x.IsFreeActive).FirstOrDefault() : null;
                if (userPlan != null)
                {
                    isUpgrade = true;
                    userPlan.IsFreeActive = false;
                    userPlan.IsActive = false;
                    _uow.UserPlanRepo.Update(userPlan);
                    _uow.UserPlanRepo.SaveChanges();
                }
                return RedirectToAction("Index", "Section");
            }

            if (planId == (int)SubscriptionPlan.ManualBuilder)
            {
                CopySections(user.Id);
                SetUpManual(user.Id, isTrail, isUpgrade);
            }
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
                SubscriptionPeriod = subscriptionPeriod
            };
            Session["User"] = _uow.UserRepo.FindById((int)Session["UserId"]);

            GetSubscription(user);
            Session["eligableForFree"] = false;
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

            if (Session["UserId"] == null)
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
        public void GetSubscription(User user)
        {
            bool havingMBSub = false;
            bool havingSUSub = false;
            Session["eligableForFreeSU"] = false;
            Session["eligableForFreeMB"] = false;
            List<UserPlan> subscriptionPlan = _uow.UserPlanRepo.Search(x => x.UserId == user.Id && x.IsActive).ToList();
            List<UserPlan> subscriptionPlan1 = _uow.UserPlanRepo.Search(x => x.UserId == user.Id).ToList();
            if (subscriptionPlan1.Count == 0)
            {
                Session["eligableForFree"] = true;
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
            }
            else
            {
                foreach (UserPlan Plansub in subscriptionPlan1)
                {
                    if (Plansub.IsFreeActive)
                    {
                        DateTime dateToday = DateTime.Now;
                        if ((Plansub.EndDate.Date - dateToday.Date).Days <= 0)
                        {
                            Plansub.IsActive = false;
                            Plansub.IsFreeActive = false;
                            _uow.SaveChanges();
                        }
                    }
                    if (Plansub.PlanId == (int)SubscriptionPlan.Startup)
                        havingSUSub = true;
                    if (Plansub.PlanId == (int)SubscriptionPlan.ManualBuilder)
                        havingMBSub = true;
                }
                if (!havingMBSub)
                    Session["eligableForFreeMB"] = true;
                else
                    Session["eligableForFreeMB"] = false;
                if (!havingSUSub)
                    Session["eligableForFreeSU"] = true;
                else
                    Session["eligableForFreeSU"] = false;
                if (!havingMBSub || !havingSUSub)
                    Session["eligableForFree"] = true;
                else
                    Session["eligableForFree"] = false;
            }
            Session["userPlans"] = subscriptionPlan;

        }

        public ActionResult SubscribeFree(int userId, int planId, int fromDay, int fromMonth, int fromYear, int toDay, int toMonth, int toYear, double price, bool isTrail = false)
        {
            bool isUpgrade = false;
            User user = _uow.UserRepo.FindById(userId);
            List<UserPlan> userPlans = _uow.UserPlanRepo.Search(x => x.UserId == userId).ToList();
            if (userPlans.Where(x => x.PlanId == planId && x.IsActive).FirstOrDefault() != null)
                if (userPlans.Where(x => x.IsFreeActive).Count() == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User Already Have Active Subscription On This Plan");
            UserPlan userPlan =
                    user.UserPlans.Where(x => x.PlanId == planId && x.IsActive && x.IsFreeActive).Count() > 0 ?
                    user.UserPlans.Where(x => x.PlanId == planId && x.IsActive && x.IsFreeActive).FirstOrDefault() : null;
            if (userPlan != null)
            {

                isUpgrade = true;
                userPlan.IsFreeActive = false;
                userPlan.IsActive = false;
                _uow.UserPlanRepo.Update(userPlan);
                _uow.UserPlanRepo.SaveChanges();
            }
            DateTime fromDate = new DateTime(fromYear, fromMonth, fromDay);
            DateTime toDate = new DateTime(toYear, toMonth, toDay);
            int userPlanid = _uow.UserPlanRepo.GetAll().Last().Id + 1;
            Plan plan = _uow.PlanRepo.FindById(planId);
            UserPlan myPlan = new UserPlan()
            {
                Id = userPlanid,
                Plan = plan,
                StartDate = fromDate,
                EndDate = toDate,
                IsActive = true,
                PlanId = planId,
                Price = price,
                UserId = userId,
                IsFreeActive = isTrail,
                IsFreeUsed = true,
            };

            _uow.UserPlanRepo.Add(myPlan);
            try
            {
                if (planId == (int)SubscriptionPlan.ManualBuilder)
                {
                    CopySections(userId);
                    SetUpManual(userId, isTrail, isUpgrade);
                }
                _uow.UserPlanRepo.SaveChanges();
                GetSubscription(user);
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
            //Web.Controllers.PlansController toAddSections = new Web.Controllers.PlansController();
            //toAddSections.CopySections(userId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void SetUpManual(int userId, bool isTrail, bool isUpgrade = false)
        {
            List<Section> sections = _uow.SectionRepo.Search(a => a.UserId == userId).ToList();
            int looper;
            if (isTrail && !isUpgrade)
            {
                foreach (Section section in sections)
                {
                    if (section.ParenId == null)
                    {
                        looper = 1;
                        foreach (Section ChildSection in section.Childs.OrderBy(x => x.Ordering))
                        {
                            if (looper > 3)
                            {
                                ChildSection.IsActive = false;
                                _uow.SectionRepo.Update(ChildSection);
                                _uow.SectionRepo.SaveChanges();
                            }
                            looper++;
                        }
                    }
                }
            }
            else
            {
                foreach (Section section in sections)
                {
                    if (section.ParenId == null)
                    {

                        foreach (Section ChildSection in section.Childs.OrderBy(x => x.Ordering))
                        {

                            ChildSection.IsActive = section.IsActive;
                            _uow.SectionRepo.Update(ChildSection);
                            _uow.SectionRepo.SaveChanges();

                        }
                    }
                }
            }
        }
        public ActionResult SearchForFile()
        {
            try
            {
                int userId = 0;
                if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
                else
                    userId = (int)Session["UserId"];
                string key = Request["SearchKey"];
                int plan = (int)Session["FilesPlanId"];
                int country = _uow.UserRepo.FindById(userId).CountryId;

                List<DocFile> files = new List<DocFile>();
                List<DocFile> EachCatfiles = new List<DocFile>();
                List<Category> categories = new List<Category>();
                if (key.Length > 0)
                {
                    categories = _uow.CategoryRepo.Search(a => a.CategoryPlan == plan).ToList();
                    foreach (Category category in categories)
                    {
                        EachCatfiles = isArabic ? _uow.DocFileRepo.Search(x => x.CategoryId == category.Id && (x.CountryId == country || x.CountryId == 0) && x.NameAr.Contains(key)).ToList() : _uow.DocFileRepo.Search(x => x.CategoryId == category.Id && (x.CountryId == country || x.CountryId == 0) && x.Name.Contains(key)).ToList();
                        foreach (DocFile docFile in EachCatfiles)
                        {
                            files.Add(docFile);
                        }
                    }
                }
                else
                {
                    categories = _uow.CategoryRepo.Search(a => a.CategoryPlan == plan).ToList();
                    foreach (Category category in categories)
                    {
                        EachCatfiles = _uow.DocFileRepo.Search(x => x.CategoryId == category.Id).ToList();
                        foreach (DocFile docFile in EachCatfiles)
                        {
                            files.Add(docFile);
                        }
                    }
                }
                bool isPlanFree = plan == 1 ? false : _uow.UserPlanRepo.Search(a => a.PlanId == plan && a.UserId == userId && a.IsActive).SingleOrDefault().IsFreeActive;
                var list = new List<KeyValuePair<string, dynamic>>();
                list.Add(new KeyValuePair<string, dynamic>("categories", categories));
                list.Add(new KeyValuePair<string, dynamic>("files", files));
                list.Add(new KeyValuePair<string, dynamic>("isFree", isPlanFree));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                return Json("Error: " + er.Message + "<br>" + er.StackTrace, JsonRequestBehavior.AllowGet);
            }

        }
    }
}