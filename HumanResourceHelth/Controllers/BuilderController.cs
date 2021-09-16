using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class BuilderController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
       
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);
            return View(user);
        }

        public ActionResult SaveDocName(string docName)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);
            if (Session["lang"].ToString() == "en-US")
                user.DocumentName = docName;
            else
                user.DocumentNameAr = docName;
            _uow.UserRepo.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}