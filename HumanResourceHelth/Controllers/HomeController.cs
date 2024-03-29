﻿using HumanResourceHelth.DataAccess;
using HumanResourceHelth.DataAccess.Repositories;
using HumanResourceHelth.Model;
using HumanResourceHelth.Model.Resources.Coupons;
using HumanResourceHelth.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        UnitOfWork _uow = new UnitOfWork();

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
                Session["eligableForFree"] = true;
            }
            else
            {
                int UserId = (int)Session["UserId"];
                User user = _uow.UserRepo.FindById(UserId);
                GetSubscription(user);
                SemiNotifications notification = _uow.SemiNotificationRepo.Search(a => a.UserId == UserId).FirstOrDefault();
                string unreadNotification = notification == null ? "" : notification.UnReadNotifi;
                if (unreadNotification != "" && unreadNotification != null)
                {
                    List<int> unreadNotificationList = JsonConvert.DeserializeObject<List<int>>(unreadNotification);
                    Session["Notification"] = unreadNotificationList.Count() == 0 ? null : unreadNotificationList.Count().ToString();
                }
            }

            return View();
        }

        public ActionResult ContactUs()
        {
            if (Session["UserId"] == null)
            {
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
                Session["eligableForFree"] = true;
            }
            else
            {
                int UserId = (int)Session["UserId"];
                User user = _uow.UserRepo.FindById(UserId);
                GetSubscription(user);
            }
            return View();
        }

        public ActionResult AboutUs()
        {
            if (Session["UserId"] == null)
            {
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
                Session["eligableForFree"] = true;
            }
            else
            {
                int UserId = (int)Session["UserId"];
                User user = _uow.UserRepo.FindById(UserId);
                GetSubscription(user);
            }
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            if (Session["UserId"] == null)
            {
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
                Session["eligableForFree"] = true;
            }
            else
            {
                int UserId = (int)Session["UserId"];
                User user = _uow.UserRepo.FindById(UserId);
                GetSubscription(user);
            }
            Session["Lang"] = lang;
            var url = Request.UrlReferrer.ToString();
            var splitedURl = url.Split('/');
            if (splitedURl.Length == 4 && splitedURl[3] == "Section")
                return Redirect("../Builder");
            //return Redirect("https://www.hrfactoryapp.com/Builder");
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult EnableCms()
        {
            Session["CMS"] = true;
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult DisableCms()
        {
            Session["CMS"] = false;
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult EditContent(int contentId)
        {
            try
            {
                int userId = 0;
                if (Session["UserId"] == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Please Login");

                userId = (int)Session["UserId"];
                User myUser = _uow.UserRepo.FindById(userId);
                if (!myUser.IsAdmin)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "you don't have rights to do this action");

                Content content = _uow.ContentRepo.FindById(contentId);
                return PartialView("_EditContent", content);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult ChangeImage(string currentSource, string imageId)
        {
            string fileName = currentSource.Replace("/HumanResourceHelth.Web/ContentImages/", "");
            ViewBag.FileName = fileName;
            ViewBag.ImageId = imageId;
            return PartialView("_ChangeImage");

        }


        [HttpPost]
        public ActionResult UploadImage(string filePath)
        {
            if (Session["UserId"] == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No Active User");
            int userId = (int)Session["UserId"];
            HttpFileCollectionBase files = Request.Files;
            try
            {
                files[0].SaveAs(Server.MapPath($"~/{filePath}"));
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult UserProfile()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            int userId = (int)Session["UserId"];
            User user = _uow.UserRepo.FindById(userId);

            RegisterViewModels registerViewModels = new RegisterViewModels
            {
                ContactInformation = user.ContactInformation,
                ContactPerson = user.ContactPerson,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                LastLoginDate = user.LastLoginDate,
                Name = user.Name,
                Countries = _uow.CountryRepo.GetAll(),
                Industries = _uow.IndustryRepo.GetAll(),
                NameAr = user.NameAr,
                NumberOFEmployees = user.NumberOFEmployees,
                Password = user.Password,
                CompanyId = user.CountryId,
                IndustryId = user.IndustryId
            };
            ViewBag.isPartner = !(_uow.PartnersRepo.Search(x => x.UserID == userId).FirstOrDefault() == null);
            return View(registerViewModels);
        }

        [HttpPost]
        public ActionResult SaveProfile(User myUser)
        {
            List<Country> countries = _uow.CountryRepo.GetAll();
            ViewBag.Country = countries;
            ViewBag.Industry = _uow.IndustryRepo.GetAll();

            User user = _uow.UserRepo.Search(x => x.Email == myUser.Email).Single();

            user.ContactInformation = myUser.ContactInformation;
            user.ContactPerson = myUser.ContactPerson;
            int CountryID = countries.Find(z => z.CountryCode == Request["CountryIdxx"].ToString()).Id;
            myUser.CountryId = CountryID;
            user.CountryId = CountryID;
            user.IndustryId = myUser.IndustryId;
            user.Name = myUser.Name;
            user.NameAr = myUser.NameAr;
            user.NumberOFEmployees = myUser.NumberOFEmployees;
            int counter = 0;
            ModelState.SetModelValue("CountryId", new ValueProviderResult(myUser.CountryId, CountryID.ToString(), System.Globalization.CultureInfo.CurrentCulture));
            ModelState.SetModelValue("Password", new ValueProviderResult(myUser.Password, "", System.Globalization.CultureInfo.CurrentCulture));
            if (!ModelState.IsValid)
            {
                foreach (ModelState item in ModelState.Values)
                {
                    counter++;
                    if (counter > 7) continue;
                    if (item.Errors.Count == 0) continue;
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, item.Errors[0].ErrorMessage);
                }
            }

            try
            {

                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                if (ex.InnerException != null)
                    error += ex.InnerException.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, error);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveContent(string arabicText, string englishText, int contentId)
        {

            try
            {
                int userId = 0;
                if (Session["UserId"] == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Please Login");

                userId = (int)Session["UserId"];
                User myUser = _uow.UserRepo.FindById(userId);
                if (!myUser.IsAdmin)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "you don't have rights to do this action");

                Content content = _uow.ContentRepo.FindById(contentId);
                content.ArabicText = arabicText.Replace("<p>", "").Replace("</p>", "");
                content.EnglishText = englishText.Replace("<p>", "").Replace("</p>", "");
                _uow.SaveChanges();
                Session["Content"] = _uow.ContentRepo.GetAll();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        //public ActionResult StartDiagnosis()
        //{
        //    int planId = 0;
        //    if (Session["UserId"] != null)
        //    {
        //        if (((User)Session["User"]).PlanId != null)
        //            planId = ((User)Session["User"]).PlanId.Value;
        //    }
        //    else
        //    {

        //    }


        //}

        public ActionResult EditProfile()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            int userId = (int)Session["UserId"];
            User user = _uow.UserRepo.FindById(userId);

            RegisterViewModels registerViewModels = new RegisterViewModels
            {
                ContactInformation = user.ContactInformation,
                ContactPerson = user.ContactPerson,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                LastLoginDate = user.LastLoginDate,
                Name = user.Name,
                Countries = _uow.CountryRepo.GetAll(),
                Industries = _uow.IndustryRepo.GetAll(),
                NameAr = user.NameAr,
                NumberOFEmployees = user.NumberOFEmployees,
                Password = user.Password,
                CompanyId = user.CountryId,
                IndustryId = user.IndustryId
            };
            return View(registerViewModels);
        }
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            int userId = (int)Session["UserId"];
            User user = _uow.UserRepo.FindById(userId);

            RegisterViewModels registerViewModels = new RegisterViewModels
            {

                Email = user.Email,

            };
            return View(registerViewModels);
        }

        public ActionResult SavePassword(string Email, string OldPassword, string Password)
        {

            User user = _uow.UserRepo.Search(x => x.Email == Email && x.Password == OldPassword).SingleOrDefault();

            if (user != null)
            {
                user.Password = Password;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Wrong Mail or Password");
            }
            try
            {
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                if (ex.InnerException != null)
                    error += ex.InnerException.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, error);
            }
        }

        public ActionResult UserSubscriptions()
        {
            ViewBag.ManualBuilderSu = true;
            ViewBag.StartUpSu = true;
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            int userId = (int)Session["UserId"];
            try
            {
                var subscriptionPlan = _uow.UserPlanRepo.Search(x => x.UserId == userId && x.IsActive);
                foreach (UserPlan userPlan in subscriptionPlan)
                {
                    if (userPlan.PlanId == (int)SubscriptionPlan.Startup)
                        ViewBag.StartUpSu = false;
                    else
                        ViewBag.ManualBuilderSu = false;
                }
                Session["thisUserPlans"] = subscriptionPlan.ToList();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);


            }
            ViewBag.isPartner = !(_uow.PartnersRepo.Search(x => x.UserID == userId).FirstOrDefault() == null);
            User user = _uow.UserRepo.FindById(userId);
            GetSubscription(user);
            return View();


        }
        public void GetSubscription(User user)
        {
            bool havingMBSub = false;
            bool havingSUSub = false;
            Session["eligableForFreeSU"] = false;
            Session["eligableForFreeMB"] = false;
            List<UserPlan> subscriptionPlan = _uow.UserPlanRepo.Search(x => x.UserId == user.Id && !x.IsActive).ToList();
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
            Session["NotActiveuserPlans"] = subscriptionPlan;

        }

        public ActionResult Updates()
        {

            List<Updates> updates = new List<Updates>();
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            int UserId = (int)Session["UserId"];
            ViewBag.isPartner = !(_uow.PartnersRepo.Search(x => x.UserID == UserId).FirstOrDefault() == null);
            SemiNotifications notifications = _uow.SemiNotificationRepo.Search(a => a.UserId == UserId).FirstOrDefault();
            if (notifications == null)
                return View(updates);
            else
            {
                string unreadNotification = notifications == null ? "" : notifications.UnReadNotifi;
                if (unreadNotification != "" && unreadNotification != null)
                {
                    List<int> unreadNotificationList = JsonConvert.DeserializeObject<List<int>>(unreadNotification);
                    Session["Notification"] = unreadNotificationList.Count() == 0 ? null : unreadNotificationList.Count().ToString();
                }
                string notificationstr = notifications.UnReadNotifi;
                List<int> notificationslist = new List<int>();
                if (notificationstr != "" & notificationstr != null)
                    notificationslist = JsonConvert.DeserializeObject<List<int>>(notificationstr);

                foreach (int updateID in notificationslist)
                {
                    updates.Add(_uow.UpdatesRepo.FindById(updateID));
                }
                ViewBag.UnreadNoti = updates;
                ViewBag.UnreadNotiCount = updates.Count();
                notificationstr = notifications.ReadNotifi;
                notificationslist = new List<int>();
                if (notificationstr != "" & notificationstr != null)
                    notificationslist = JsonConvert.DeserializeObject<List<int>>(notificationstr);
                updates = new List<Updates>();
                foreach (int updateID in notificationslist)
                {
                    updates.Add(_uow.UpdatesRepo.FindById(updateID));
                }
                ViewBag.ReadNotifi = updates;
                ViewBag.ReadNotifiCount = updates.Count();

                return View(updates);
            }
        }
        public ActionResult test()
        {
            int aieje;
            List<Tuple<int, DateTime>> arr = new List<Tuple<int, DateTime>>();
            arr.Add(new Tuple<int, DateTime>(45, DateTime.Now));
            arr.Add(new Tuple<int, DateTime>(99, DateTime.Now));

            //string result = string.Join("", arr);
            //var j = Json(arr);
            string t = JsonConvert.SerializeObject(arr);
            List<Tuple<int, DateTime>> ddd = JsonConvert.DeserializeObject<List<Tuple<int, DateTime>>>(t);
            foreach (Tuple<int, DateTime> tuple in ddd)
            {
                aieje = tuple.Item1;
            }
            DirectoryInfo di = new DirectoryInfo("/");
            foreach (FileInfo fi in di.GetFiles())
            {

            }
            //int[] ints = Array.ConvertAll(result, s => int.Parse(s));
            return Json(arr, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadNotification()
        {
            try
            {
                if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
                var type = Request["type"];
                int id = int.Parse(Request["id"]);
                Updates update = _uow.UpdatesRepo.FindById(id);

                if (type == "unread")
                {
                    int UserId = (int)Session["UserId"];
                    SemiNotifications notifications = _uow.SemiNotificationRepo.Search(a => a.UserId == UserId).FirstOrDefault();
                    List<int> unreadlist = new List<int>();
                    List<int> readlist = new List<int>();
                    string unreadliststr = notifications.UnReadNotifi;
                    string readliststr = notifications.ReadNotifi;
                    unreadlist = JsonConvert.DeserializeObject<List<int>>(unreadliststr);
                    if (readliststr != "" && readliststr != null)
                        readlist = JsonConvert.DeserializeObject<List<int>>(readliststr);
                    unreadlist.Remove(id);
                    readlist.Add(id);
                    unreadliststr = JsonConvert.SerializeObject(unreadlist);
                    readliststr = JsonConvert.SerializeObject(readlist);
                    notifications.ReadNotifi = readliststr;
                    notifications.UnReadNotifi = unreadliststr;
                    _uow.SemiNotificationRepo.Update(notifications);
                    _uow.SaveChanges();
                }
                return Json(update, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Partner(int id)
        {
            OurPartners partner = _uow.OurPartnersRepo.FindById(id);
            return View(partner);

        }
        public ActionResult shipData()
        {

            //create dictionary
            Dictionary<string, dynamic> shipData = new Dictionary<string, dynamic>();
            List<Content> contents = _uow.ContentRepo.GetAll().ToList();
            shipData.Add("contents", contents);
            //get list of category
            List<Category> categories = _uow.CategoryRepo.GetAll().ToList();
            shipData.Add("categories", categories);
            //get list of DocFile
            List<DocFile> docFiles = _uow.DocFileRepo.GetAll().ToList();
            shipData.Add("docFiles", docFiles);
            //get list of Users
            List<User> users = _uow.UserRepo.GetAll().ToList();
            shipData.Add("users", users);
            // get list of plans
            List<Plan> plans = _uow.PlanRepo.GetAll().ToList();
            shipData.Add("plans", plans);
            //get list of termsConditions
            List<TermsConditions> termsConditions = _uow.TermsConditionsRepo.GetAll().ToList();
            shipData.Add("termsConditions", termsConditions);
            //get list of coupons
            List<Model.Coupons> coupons = _uow.couponsRepo.GetAll().ToList();
            shipData.Add("coupons", coupons);

            //return json of contents
            return Json(shipData, JsonRequestBehavior.AllowGet);


        }
        public ActionResult shipCountries()
        {

            //create dictionary
            Dictionary<string, dynamic> shipData = new Dictionary<string, dynamic>();

            //get list of countries
            List<Country> countries = _uow.CountryRepo.GetAll().Select(a => new Country { Id = a.Id, Name = a.Name, CountryCode = a.CountryCode, NameAr = a.NameAr, IsArabCountry = a.IsArabCountry }).ToList();
            shipData.Add("countries", countries);
            //get list of industries
            List<Industry> industries = _uow.IndustryRepo.GetAll().ToList();
            shipData.Add("industries", industries);
            //return json of contents
            //string json = JsonConvert.SerializeObject(shipData, Formatting.Indented,
            //    new JsonSerializerSettings()
            //    {
            //        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //    });

            return Json(shipData, JsonRequestBehavior.AllowGet);


        }
        public ActionResult shipCoupons()
        {

            //create dictionary
            Dictionary<string, dynamic> shipData = new Dictionary<string, dynamic>();
            //get list of coupons
            List<Model.Coupons> coupons = _uow.couponsRepo.GetAll().ToList();
            shipData.Add("coupons", coupons);
            //get list of introvedio
            List<IntroVedio> introVedios = _uow.introVedioRepo.GetAll().ToList();
            shipData.Add("introVedios", introVedios);
            //get list of userplans
            List<UserPlan> userPlans = _uow.UserPlanRepo.GetAll().ToList();
            shipData.Add("userPlans", userPlans);
            return Json(shipData, JsonRequestBehavior.AllowGet);


        }
        public ActionResult shipDef()
        {

            //create dictionary
            Dictionary<string, dynamic> shipData = new Dictionary<string, dynamic>();
            //get list of coupons
            List<DefaultMB> defaultMBs = _uow.DefaultMBRepo.GetAll().Where(a => a.CountryID == 182 && a.CompanySize == 6 && a.LanguageId == 2).ToList();
            shipData.Add("defaultMBs", defaultMBs);
            return Json(shipData, JsonRequestBehavior.AllowGet);


        }
        //get users ids by array of emails
        public ActionResult GetUsersIdsByEmails()
        {
            string[] emails = Request["emails"].Split(',');
            List<int> ids = new List<int>();
            foreach (var email in emails)
            {
                User user = _uow.UserRepo.Search(a => a.Email == email).FirstOrDefault();
                if (user != null)
                    ids.Add(user.Id);
            }
            return Json(ids, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserByEmail(string email)
        {
            User user = _uow.UserRepo.Search(a => a.Email == email).FirstOrDefault();
            return Json(user, JsonRequestBehavior.AllowGet);
        }
        //return sections of active user
        public ActionResult GetUserSections()
        {
            Dictionary<string, dynamic> shipData = new Dictionary<string, dynamic>();
            //get user by email
            //User user = _uow.UserRepo.Search(a => a.Email == email).FirstOrDefault();
            //get IDs of active users
            List<int> activeUsers = _uow.UserRepo.Search(a => !a.IsDeleted && !a.IsAdmin).Select(a => a.Id).ToList();
            //get allsection of all active users
            var userSections = _uow.SectionRepo.Search(a => activeUsers.Contains(a.UserId) && a.ParenId == null).Select(z => new
            {
                z.SectionId,
                z.Id,
                z.IsHaveLineBefore,
                z.UserId,
                z.Ordering,
                z.Title,
                z.LanguageId,
                z.ParenId,
                z.CountryID,
                //z.Childs,
                z.CompanyIndustry,
                z.CompanySize,
                z.Content,
                z.Description,
                z.IsActive
            }).ToList();
            //List<Section> userSections = _uow.SectionRepo.Search(a => a.UserId == user.Id && a.LanguageId==lang).ToList();
            shipData.Add("userSections", userSections);

            var list = JsonConvert.SerializeObject(shipData, Formatting.None, new JsonSerializerSettings()
            { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }
            );
            //download list to json physical file
            //byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(list);

            //return File(byteArray, "application/force-download", "file1.json");
            return Content(list, "application/json");
        }
        //get subsection by parent id
        public ActionResult GetSubSections(int parent)
        {
            Dictionary<string, dynamic> shipData = new Dictionary<string, dynamic>();
            //get user by email

            //get allsection of all active users
            List<Section> userSections = _uow.SectionRepo.Search(a => a.ParenId == parent).ToList();
            shipData.Add("userSections", userSections);
            return Json(shipData, JsonRequestBehavior.AllowGet);
        }
    }
}