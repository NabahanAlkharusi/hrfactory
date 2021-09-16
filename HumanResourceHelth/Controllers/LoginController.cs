using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string referrer)
        {
            string call = Request.UrlReferrer.ToString();
            Session["Content"] = _uow.ContentRepo.GetAll();
            User user = _uow.UserRepo.Search(x => x.Email == email && x.Password == password).SingleOrDefault();
            if (user != null)
            {
                FillSessions(user);
                DateTime today = DateTime.Today;
                DisableExpiredPlans(today);
                if (referrer.Contains("Survey/Intro"))
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Survey");

                //return Redirect(Request.UrlReferrer.ToString());
                if (Session["Backto"]!=null)
                    return new HttpStatusCodeResult(HttpStatusCode.OK, Session["Backto"].ToString());
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Home");
            }
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
                Countries = _uow.CountryRepo.GetAll(),
                Industries = _uow.IndustryRepo.GetAll()
            };
            return View(registerViewModels);
        }

        [HttpPost]
        public ActionResult Save(User myUser)
        {
            ViewBag.Country = _uow.CountryRepo.GetAll();
            ViewBag.Industry = _uow.IndustryRepo.GetAll();

            User user = _uow.UserRepo.Search(x => x.Email == myUser.Email).SingleOrDefault();
            myUser.LastLoginDate = DateTime.Now;
            if (user != null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Email Already Exist");

            if (!ModelState.IsValid)
            {
                foreach(ModelState item in ModelState.Values)
                {
                    if (item.Errors.Count == 0) continue;
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, item.Errors[0].ErrorMessage);
                }
            }

            try
            {
                _uow.UserRepo.Add(myUser);
                _uow.UserRepo.SaveChanges();
                SendMail(myUser.Email);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                if (ex.InnerException != null)
                    error += ex.InnerException.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, error);
            }
        }
        public void SendMail(string userEmail)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                MailMessage message = new MailMessage("care@hrfactory", "tamernazmy@gmail.com")
                {
                    Body = "Welcome",
                    BodyEncoding = Encoding.UTF8,
                    Subject = "Test Subject",
                    IsBodyHtml = true,
                };
                client.Host ="relay-hosting.secureserver.net";
                client.Send(message);
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
                _uow.SaveChanges();

                SmtpClient client = new SmtpClient();
                client.EnableSsl = false;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                MailMessage message = new MailMessage("care@hrfactoryapp.com",email)
                {
                    Body = "your new password is : " + randomPassword,
                    BodyEncoding = Encoding.UTF8,
                    Subject = "Password Reset",
                    IsBodyHtml = true,
                };
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "Care@hrfactory");
                client.Host = "relay-hosting.secureserver.net";
                client.Port = 25;
                client.Send(message);
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
                _uow.SaveChanges();

                string senderMail = System.Configuration.ConfigurationManager.AppSettings["senderMail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["senderPassword"].ToString();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderMail, senderPassword);
                MailMessage message = new MailMessage(senderMail, userEmail, "Password Reset", "your new password is : " + randomPassword);
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.IsBodyHtml = true;
                client.Send(message);
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
    }
}