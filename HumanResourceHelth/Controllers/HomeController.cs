using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Models;
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
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            Session["Lang"] = lang;
            var url = Request.UrlReferrer.ToString();
            var splitedURl = url.Split('/');
            if (splitedURl.Length==4 && splitedURl[3] == "Section")
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest , ex.Message);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult UserProfile()
        {
            if (Session["UserId"] == null) return RedirectToAction("Index","Login");
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
            ViewBag.Country = _uow.CountryRepo.GetAll();
            ViewBag.Industry = _uow.IndustryRepo.GetAll();

            User user = _uow.UserRepo.Search(x => x.Email == myUser.Email).Single();

            user.ContactInformation = myUser.ContactInformation;
            user.ContactPerson = myUser.ContactPerson;
            user.CountryId = myUser.CountryId;
            user.IndustryId = myUser.IndustryId;
            user.Name = myUser.Name;
            user.NameAr = myUser.NameAr;
            user.NumberOFEmployees = myUser.NumberOFEmployees;
            user.Password = myUser.Password;


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
                content.ArabicText = arabicText.Replace("<p>","").Replace("</p>", "");
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
    }
}