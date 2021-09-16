using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class PymentsController : Controller
    {
         UnitOfWork _unitOfWork = new UnitOfWork();
        public PymentsController()
        {
         
        }

        public ActionResult Index()
        {
            var Pyments = _unitOfWork.PaymentRepo.GetAll().OrderByDescending(x => x.PymentID);
            return View(Pyments);
        }

        public ActionResult CheckOut()
        {
            var Pyments = new Pyments();
            return View(Pyments);
        }


        [HttpPost]
        public ActionResult CheckOut(Pyments pyments)
        {
            string UserId = Session["UserId"].ToString();

            if (ModelState.IsValid)
            {
                pyments.PymentDate = DateTime.Now;

                pyments.UserID = UserId;
                pyments.PaymentMethod = "Card";
                //_unitOfWork.PaymentRepo.Add(pyments);
                //_unitOfWork.PaymentRepo.SaveChanges();
            }
            else
            {
                return RedirectToAction("CheckOut", "TrainingHome", new { CID = pyments.CourseID });
            }
            //  _unitOfWork.Pyments.Add(pyments);
            return RedirectToAction("CourseMaterial", "TrainingHome", new { CID = pyments.CourseID });

            //  return Redirect("home/CourseMaterial")  ;
        }


    }
}