using HumanResourceHelth.DataAccess;
using System.Web.Mvc;
using System.Linq;
using HumanResourceHelth.Web.Areas.Admin.Data;
using HumanResourceHelth.Model;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Plans(int userId)
        {
            UserPlansViewModel userPlansViewModel = new UserPlansViewModel()
            {
                UserPlans = _uow.UserPlanRepo.Search(x => x.UserId == userId).OrderBy(x => x.IsActive).ThenBy(y => y.StartDate).ToList(),
                User = _uow.UserRepo.FindById(userId),
                Plans = _uow.PlanRepo.Search(x => x.Id == 2 || x.Id == 3).ToList()
            };

            return View(userPlansViewModel);
        }

        public ActionResult Details(int userId)
        {
            User user = _uow.UserRepo.FindById(userId);
            return View(user);
        }

        public ActionResult LoginAs(int userId)
        {
            User user = _uow.UserRepo.FindById(userId);
            SemiNotifications notification = _uow.SemiNotificationRepo.Search(a => a.UserId == userId).FirstOrDefault();
            string unreadNotification = notification == null ? "" : notification.UnReadNotifi;
            if (unreadNotification != "" && unreadNotification != null)
            {
                List<int> unreadNotificationList = JsonConvert.DeserializeObject<List<int>>(unreadNotification);
                Session["Notification"] = unreadNotificationList.Count();
            }
            Session["UserId"] = user.Id;
            Session["User"] = user;
            Session["CMS"] = false;
            GetSubscription(user);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult AddPlan(int userId, int planId, int fromDay, int fromMonth, int fromYear, int toDay, int toMonth, int toYear, double price, bool isTrail = false)
        {
            User user = _uow.UserRepo.FindById(userId);
            List<UserPlan> userPlans = _uow.UserPlanRepo.Search(x => x.UserId == userId).ToList();
            if (userPlans.Where(x => x.PlanId == planId && x.IsActive).SingleOrDefault() != null)
                if (userPlans.Where(x => x.IsFreeActive == true).SingleOrDefault() == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User Already Have Active Subscription On This Plan");

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
                IsFreeUsed = false,
            };

            _uow.UserPlanRepo.Add(myPlan);
            try
            {
                if (planId == (int)SubscriptionPlan.ManualBuilder)
                {
                    CopySections(userId);
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

        public ActionResult Delete(int userId)
        {
            User user = _uow.UserRepo.FindById(userId);
            user.IsDeleted = true;
            try
            {
                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                var exxxx = ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
            List<User> users = _uow.UserRepo.Search(x => x.IsAdmin == false && x.IsDeleted == false).ToList();
            return PartialView("_Users", users);
        }

        public PartialViewResult Users()
        {
            List<User> users = _uow.UserRepo.Search(x => x.IsAdmin == false && x.IsDeleted == false).ToList();
            return PartialView("_Users", users);
        }
        public void CopySections(int userId)
        {
            List<Section> userSections = new List<Section>();
            if (_uow.SectionRepo.Search(x => x.UserId == userId).Count() > 0) return;
            int adminId = _uow.UserRepo.Search(x => x.IsAdmin).Single().Id;

            User user = _uow.UserRepo.FindById(userId);
            List<Section> adminSections = _uow.SectionRepo.Search(a => a.UserId == adminId && a.CountryID == user.CountryId).ToList();
            if (adminSections == null)
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
            Session["userPlansCount"] = subscriptionPlan.Count > 0 ? subscriptionPlan.Count.ToString() : null;
        }
    }
}