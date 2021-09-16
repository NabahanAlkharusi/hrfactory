using HumanResourceHelth.DataAccess;
using System.Web.Mvc;
using System.Linq;
using HumanResourceHelth.Web.Areas.Admin.Data;
using HumanResourceHelth.Model;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System;

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
            Session["UserId"] = user.Id;
            Session["User"] = user;
            Session["CMS"] = false;
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult AddPlan(int userId, int planId, int fromDay, int fromMonth, int fromYear,  int toDay, int toMonth, int toYear, double price)
        {
            User user = _uow.UserRepo.FindById(userId);
            List<UserPlan> userPlans = _uow.UserPlanRepo.Search(x => x.UserId == userId).ToList();
            if(userPlans.Where(x => x.PlanId == planId && x.IsActive).SingleOrDefault() != null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User Already Have Active Subscription On This Plan");

            DateTime fromDate = new DateTime(fromYear, fromMonth, fromDay);
            DateTime toDate = new DateTime(toYear, toMonth, toDay);
            int userPlanid = _uow.UserPlanRepo.GetAll().Last().Id+1;
            Plan plan = _uow.PlanRepo.FindById(planId);
            UserPlan myPlan = new UserPlan()
            {
                Id=userPlanid,
                Plan=plan,
                StartDate = fromDate,
                EndDate = toDate,
                IsActive = true,
                PlanId = planId,
                Price = price,
                UserId = userId,
            };

            _uow.UserPlanRepo.Add(myPlan);
            try
            {
                _uow.UserPlanRepo.SaveChanges();
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest , ex.Message);
            }
            Web.Controllers.PlansController toAddSections = new Web.Controllers.PlansController();
            toAddSections.CopySections(userId);
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
    }
}