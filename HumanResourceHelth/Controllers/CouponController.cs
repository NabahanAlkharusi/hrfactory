using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using System.Web.Routing;


namespace HumanResourceHelth.Web.Controllers
{
    //  [SessionAdminAuthorize]
    public class CouponController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork();
        //private UserManager<AppUser> userManager;

        //public AttachController(UserManager<AppUser> userMgr)
        //{


        //    userManager = userMgr;
        //}
        public ActionResult Index()
        {
            var coupons = _unitOfWork.couponsRepo.GetAll();

            return View(coupons);
        }
        public ActionResult Create()
        {
            //SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["HumanResourceContext"].ConnectionString);
            //SqlCommand sqlCommand = new SqlCommand("Select ident_current(ID)", con);
            //if(con.State==System.Data.ConnectionState.Open)
            //{
            //    con.Close();
            //}
            //con.Open();
            //int request_ID = sqlCommand.ExecuteNonQuery();
            //con.Close();
            var coupons = _unitOfWork.couponsRepo.GetAll();
            int TotalRows = coupons.Count;
            int lastCouponID = 0;
            if (coupons.Count > 0)
            {
                Coupons coupon = coupons.LastOrDefault();
                lastCouponID = coupon.ID;
            }
            //return TotalRows.ToString() + " == " + lastCouponID.ToString()+ " ========   "+ request_ID;
            ViewBag.CouponCode = "HRFA" + ((coupons.Count) + 1).ToString().PadLeft(5, '0');

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Coupons coupon)
        {
            if (ModelState.IsValid)
            {
                coupon.coupon_created_at = DateTime.Today; // fill out the field Coupon Create at automaticlly
                _unitOfWork.couponsRepo.Add(coupon); // add Coupon Object to the model and make it ready for save it into database.
                _unitOfWork.couponsRepo.SaveChanges(); // save Coupon details into database
                return RedirectToAction("Index", "Coupon"); // redirect to the index page (view) if adding into database done successfully 
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            //var coupon = _unitOfWork.couponsRepo.FindById(id);
            //return coupon.coupon_satrt_date.ToString("MM/dd/yyyy");
            if (id == null)
            {
                return HttpNotFound();
            }

            var coupon = _unitOfWork.couponsRepo.FindById(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            //coupon.coupon_satrt_date= Convert.ToDateTime(coupon.coupon_satrt_date.ToString("MM/dd/yyyy"));
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Coupons coupon)
        {
            if (id != coupon.ID)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.couponsRepo.Update(coupon);
                    _unitOfWork.couponsRepo.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    if (!CouponExists(coupon.ID))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return View(coupon);
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var coupons = _unitOfWork.couponsRepo.GetAll().
                FirstOrDefault(x => x.ID == id);
            if (coupons == null)
            {
                return HttpNotFound();
            }

            return View(coupons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Coupons coupons)
        {
            _unitOfWork.couponsRepo.Remove(coupons.ID);
            _unitOfWork.couponsRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public JsonResult getCouponRate(string CouponFor, string code)
        {
            var dictionary = new Dictionary<string, object>();
            if (code != null && code!="")
            {
                var coupons = _unitOfWork.couponsRepo.GetAll().Where(c=>c.coupon_code.Equals(code,StringComparison.OrdinalIgnoreCase));
                dictionary.Add("result", "success");
                dictionary.Add("msg", "");
                if (coupons.Count() == 0)
                {
                    dictionary["result"] = "error";
                    dictionary["msg"] = "This Code is Not Available in Our Coupons";
                }
                else
                {
                    if (coupons.First().coupon_for == "1" || coupons.First().coupon_for == CouponFor)
                    {
                        dictionary["msg"] = "This Coupon Has Following Rate";
                        dictionary.Add("rate", coupons.First().coupon_discount_rate);
                    }
                    else
                    {
                        dictionary["msg"] = "This Code is Not For This Plan";
                        dictionary["result"] = "error";
                    }

                }



                dictionary.Add("code", code);
            }
            else
            {
                dictionary["result"] = "error";
                dictionary.Add("code", "you need to submit code");
            }
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }
        private bool CouponExists(int id)
        {
            return _unitOfWork.couponsRepo.GetAll().Any(x => x.ID == id);
        }
    }
}
