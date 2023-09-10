using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class LoginController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Login
        public ActionResult Index()
        {
            Session["Content"] = _uow.ContentRepo.GetAll();
            //ViewBag.Referrer = Request.UrlReferrer.ToString();
            bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
            TermsConditions terms = _uow.TermsConditionsRepo.Search(
                z => z.TermsConditionType == (int)TermsConditionType.Registeration &&
                z.CountryId == 0).FirstOrDefault();
            ViewBag.RegistrationTitle = isArabic ? terms.ArabicTitle : terms.EnglishTitle;
            ViewBag.Registration = isArabic ? terms.ArabicText : terms.EnglishText;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string backUrl)
        {
            string call = Request.UrlReferrer.ToString();
            Session["Content"] = _uow.ContentRepo.GetAll();
            User user = _uow.UserRepo.Search(x => x.Email == email && x.Password == password).SingleOrDefault();
            if (user != null)
            {
                FillSessions(user);
                DateTime today = DateTime.Today;
                DisableExpiredPlans(today);
                GetSubscription(user);
                /* if (referrer.Contains("Survey/Intro"))
                     return new HttpStatusCodeResult(HttpStatusCode.OK, "Survey");
 */
                //return Redirect(Request.UrlReferrer.ToString());
                SemiNotifications notification = _uow.SemiNotificationRepo.Search(a => a.UserId == user.Id).FirstOrDefault();
                string unreadNotification = notification == null ? "" : notification.UnReadNotifi;
                if (unreadNotification != "" && unreadNotification != null)
                {
                    List<int> unreadNotificationList = JsonConvert.DeserializeObject<List<int>>(unreadNotification);
                    Session["Notification"] = unreadNotificationList.Count();
                }
                Dictionary<string, dynamic> pairs = new Dictionary<string, dynamic>();
                pairs.Add("StatusCode", HttpStatusCode.OK);
                pairs.Add("previousePage", backUrl);
                return Json(pairs, JsonRequestBehavior.AllowGet);
                //return new HttpStatusCodeResult(HttpStatusCode.OK, "Home");
            }
            //ffffffff
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Wrong Mail or Password");
            }
        }

        private void FillSessions(User user)
        {
            Session["UserId"] = user.Id;
            Session["User"] = user;
            if (user.IsAdmin)
            {
                Session["CMS"] = true;
                Session["IsAdmin"] = true;
            }
            else
            {
                Session["CMS"] = false;
                Session["IsAdmin"] = false;
            }
        }

        private void DisableExpiredPlans(DateTime today)
        {
            List<UserPlan> userPlans = _uow.UserPlanRepo.Search(x => x.IsActive && x.EndDate < today).ToList();
            foreach (UserPlan userPlan in userPlans)
                userPlan.IsActive = false;
            _uow.SaveChanges();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Register()
        {
            RegisterViewModels registerViewModels = new RegisterViewModels
            {
                Countries = _uow.CountryRepo.GetAll().OrderBy(x => x.NameAr).OrderByDescending(s => s.IsArabCountry).ToList(),
                Industries = _uow.IndustryRepo.GetAll()
            };
            bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
            TermsConditions terms = _uow.TermsConditionsRepo.Search(
                z => z.TermsConditionType == (int)TermsConditionType.Registeration &&
                z.CountryId == 0).FirstOrDefault();
            ViewBag.RegistrationTitle = isArabic ? terms.ArabicTitle : terms.EnglishTitle;
            ViewBag.Registration = isArabic ? terms.ArabicText : terms.EnglishText;
            return View(registerViewModels);
        }

        [HttpPost]
        public ActionResult Save(User myUser)
        {
            List<Country> countries = _uow.CountryRepo.GetAll();
            ViewBag.Country = countries;
            ViewBag.Industry = _uow.IndustryRepo.GetAll();
            var countryCode = Request["CountryId"];
            User user = _uow.UserRepo.Search(x => x.Email == myUser.Email).SingleOrDefault();
            myUser.LastLoginDate = DateTime.Now;
            myUser.CountryId = countries.Find(x => x.CountryCode == countryCode).Id;
            if (user != null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Email Already Exist");
            ModelState["CountryId"].Errors.Clear();
            ModelState.SetModelValue("CountryId", new ValueProviderResult(myUser.CountryId, "", System.Globalization.CultureInfo.CurrentCulture));

            if (!ModelState.IsValid)
            {
                foreach (ModelState item in ModelState.Values)
                {
                    if (item.Errors.Count == 0) continue;
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, item.Errors[0].ErrorMessage);
                }
            }

            try
            {
                SendMail(myUser.Email, myUser.Name, myUser.NameAr);
                _uow.UserRepo.Add(myUser);
                _uow.UserRepo.SaveChanges();

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
        public void SendMail(string userEmail, string en_name, string ar_name)
        {
            try
            {

                MailMessage msgs = new MailMessage();
                msgs.To.Add(userEmail);
                MailAddress address = new MailAddress("care@hrfactoryapp.com");
                msgs.From = address;
                msgs.Subject = "Welcome to HR FACTORY";
                string htmlBody = "<h4>Dear " + en_name + "</h4><br>Greetings from HR Factory.<br><br>"
                                    + "Please find below videos that explain the concept of the HR platform(they are in Arabic).<br>"
                                    + "<a href='https://www.youtube.com/watch?v=Q31WLrcsARg'> https://www.youtube.com/watch?v=Q31WLrcsARg </a><br>"
                                    + "<a href='https://www.youtube.com/watch?v=Q8sn6AyrOKQ'> https://www.youtube.com/watch?v=Q8sn6AyrOKQ </a><br><br><br>"
                                    + "As noted, you may kickstart your journey by:<ul><li> Taking the free online HR health check.</li>"
                                    + "<li> Learning HR online.</li><li> Enjoying  the HR Startup plan : to enjoy the use of HR resources.  <a href='https://hrfactoryapp.com' > HR FACTORY(hrfactoryapp.com) </a></li>"
                                    + "<li> Enjoying to the HR manual builder plan: to enjoy a user-friendly and dynamic builder of HR policy manual and receive updates. <a href='https://hrfactoryapp.com'> HR FACTORY(hrfactoryapp.com) <a></li>"
                                    + "<li> Subscribing to the HR Doctor plan to provide you with HR mentoring and advisory support.</li></ul><br><br>"
                                    + "Looking forward and we are happy to be part of your success journey.<br><br><br>"
                                    + "<div dir='rtl'><h4>أهلا بك " + ar_name + " في منصة HR Factory </h4><br><br><br>"
                                    + "نرحب بك و نتمنى لك تحقيق الاستفادة القصوى من المنصة.<br><br><br>"
                                    + "يمكنك الآن الاستفادة من التالي:<br><br><ul>"
                                    + "<li> عمل تشخيص مجاني إلكتروني. (إن أردت حفظ التقارير يمكنك التسجيل في باقة الانطلاق والقيام بالتشخيص بشكل مستمر أثناء فترة الاشتراك)"
                                    + "<li>دورات عن بعد في الموارد البشرية.</li>"
                                    + "<li>الاستفادة من باقة الانطلاق والاستمتاع بالتالي:"
                                    + "<ul><li> باقة مدفوعة تهدف إلى تزويد رواد الأعمال بأهم الأدوات والممارسات الأساسية بشكل أعمق وأشمل في HR.</li>"
                                    + "<li> احصل على همسات ونصائح دورية في HR. </li>"
                                    + "<li> عمل تشخيص مستمر وغير محدود لمستوى ممارسات الموارد البشرية.</li></ul></ul><br><br><br>"
                                    + "<a href='https://www.youtube.com/watch?v=Ft7WVAuQ3T0' > https://www.youtube.com/watch?v=Ft7WVAuQ3T0 </a><br><br><br>"
                                    + "الاستفادة في باقة صانع لائحة سياسات العمل والحصول على نموذج ديناميكي يتيح لك إنشاء لائحة نظام العمل الخاصة بك.<br><br>"
                                    + "<a href='hrfactoryapp.com' > HR FACTORY(hrfactoryapp.com) </a></div>"
                                    + "<img src='https://www.hrfactoryapp.com/Images/emailfooter.jpg'/>";
                msgs.Body = htmlBody;
                msgs.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                //client.Host = "smtpout.asia.secureserver.net";
                client.Host = "smtp.gmail.com";
                //client.Port = 80;
                client.Port = 587;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                //client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "HRFA@pp(@2023)");
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "vymtkpdcvkcwwzmm");
                //Send the msgs  
                client.Send(msgs);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception er)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest, er.Message);
            }



        }

        public ActionResult ForgetPassword(string email)
        {

            try
            {

                string randomPassword = System.Web.Security.Membership.GeneratePassword(8, 2);
                User user = _uow.UserRepo.Search(x => x.Email == email).SingleOrDefault();
                if (user == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Email Not Exist");

                user.Password = randomPassword;
                var usrname = user.Name;
                _uow.SaveChanges();

                MailMessage msgs = new MailMessage();
                msgs.To.Add(email);
                MailAddress address = new MailAddress("care@hrfactoryapp.com");
                msgs.From = address;
                msgs.Subject = "HR FACTORY - Reset Password";
                string htmlBody = "<html><head></head><body><div style='width:100%;'><h4>Dear " + usrname + "</h4><br>Greetings from HR Factory.<br><br>"
                                    + "Please find below your new password<br>"
                                    + "The new password: <h4>" + randomPassword + "</h4><br>"
                                    + "Please try to change it as soon as possible<br>"
                                    + "<a href='hrfactoryapp.com' > HR FACTORY(hrfactoryapp.com) </a></div>"
                                    + "<img src='https://www.hrfactoryapp.com/Images/emailfooter.jpg'/></div></body></html>";
                msgs.Body = htmlBody;
                msgs.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                //client.Host = "smtpout.asia.secureserver.net";
                client.Host = "smtp.gmail.com";
                //client.Port = 80;
                client.Port = 587;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                //client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "HRFA@pp(@2023)");
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "vymtkpdcvkcwwzmm");
                //Send the msgs  
                client.Send(msgs);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception er)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest, er.Message);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult SendPassword(string userEmail)
        {
            try
            {

                string randomPassword = System.Web.Security.Membership.GeneratePassword(8, 2);
                User user = _uow.UserRepo.Search(x => x.Email == userEmail).SingleOrDefault();
                if (user == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Email Not Exist");

                user.Password = randomPassword;
                var usrname = user.Name;
                _uow.SaveChanges();

                MailMessage msgs = new MailMessage();
                msgs.To.Add(userEmail);
                MailAddress address = new MailAddress("care@hrfactoryapp.com");
                msgs.From = address;
                msgs.Subject = "Welcome to HR FACTORY";
                string htmlBody = "<h4>Dear " + usrname + "</h4><br>Greetings from HR Factory.<br><br>"
                                    + "Please find below your new password<br>"
                                    + "The new password: <h4>" + randomPassword + "</h4><br>"
                                    + "Please try to change it as soon as possible<br>"
                                    + "<a href='hrfactoryapp.com' > HR FACTORY(hrfactoryapp.com) </a></div>"
                                    + "<img src='https://www.hrfactoryapp.com/Images/emailfooter.jpg'/>";
                msgs.Body = htmlBody;
                msgs.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = "relay-hosting.secureserver.net";
                client.Port = 25;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.EnableSsl = false;
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "Wahb@1985");
                //Send the msgs  
                client.Send(msgs);
                ViewBag.Status = "Email Sent Successfully.";
                //string senderMail = System.Configuration.ConfigurationManager.AppSettings["senderMail"].ToString();
                //string senderPassword = System.Configuration.ConfigurationManager.AppSettings["senderPassword"].ToString();
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //client.EnableSsl = true;
                //client.Timeout = 100000;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new NetworkCredential(senderMail, senderPassword);
                //MailMessage message = new MailMessage(senderMail, userEmail, "Password Reset", "your new password is : " + randomPassword);
                //message.BodyEncoding = UTF8Encoding.UTF8;
                //message.IsBodyHtml = true;
                //client.Send(message);
            }
            catch (Exception er)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest, er.Message);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public void NotLoggedin()
        {
            String test = Session["Backto"].ToString();
            test = test.Trim('/');
            var test2 = test.Split('/');
            Response.Write(test2[0]);
            Response.Write(test2[1]);
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