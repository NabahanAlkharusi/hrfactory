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
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);
            return View(user);
        }

        public ActionResult SaveDocName(string docName)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { r = Request.Url.ToString() }));
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);
            
            if (Session["lang"].ToString() == "en-US")
                user.DocumentName = docName;
            else
                user.DocumentNameAr = docName;
            try
            {
                _uow.UserRepo.SaveChanges();
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}