using HumanResourceHelth.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Admin/Login
        public ActionResult Index()
        {
            if (Session["UserId"] != null && (bool)Session["IsAdmin"])
                return Redirect("/Admin/Home");
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password,string backUrl)
        {
            Session["Content"] = _uow.ContentRepo.GetAll();
            Model.User user = _uow.UserRepo.Search(x => x.Email == email && x.Password == password && x.IsAdmin && x.IsDeleted == false).SingleOrDefault();
            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["User"] = user;
                Session["CMS"] = true;
                Session["IsAdmin"] = true;
                //string previousePage = backUrl;
                //if (Session["Backto"] != null)
                //    return new HttpStatusCodeResult(HttpStatusCode.OK, Session["Backto"].ToString());
                Dictionary<string, dynamic> pairs = new Dictionary<string, dynamic>();
                pairs.Add("StatusCode", HttpStatusCode.OK);
                pairs.Add("previousePage", backUrl);
                return  Json(pairs,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Wrong Mail or Password");
            }
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }

    }
}