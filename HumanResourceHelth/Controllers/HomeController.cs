using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
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
    }
}