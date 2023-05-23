using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class HPlansToPartnerShipsController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();

        // GET: Admin/Plans
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View(db.HpartnerShipPlans.ToList());
        }

        // GET: Admin/Plans/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HPartnerShipPlans plans = db.HpartnerShipPlans.Find(id);
            if (plans == null)
            {
                return HttpNotFound();
            }
            return View(plans);
        }

        // GET: Admin/Plans/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View();
        }

        // POST: Admin/Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,PlanTitle,Objective,Process,Participants,Report,DeliveryMode,Limitations,PlanTitleAr,ObjectiveAr,ProcessAr,ParticipantsAr,ReportAr,DeliveryModeAr,LimitationsAr,TamplatePath,Price,Status")] HPartnerShipPlans plans)
        {



            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                   // HttpPostedFileBase postedFile = Request.Files["Tamplatefile"];
                if (Request.Files["Tamplatefile"].ContentLength > 0)
                {
                    string path = Server.MapPath("~/Areas/PartnersPlansFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    Request.Files["Tamplatefile"].SaveAs(path + Path.GetFileName(Request.Files["Tamplatefile"].FileName));
                    plans.TamplatePath = "/Areas/PartnersPlansFiles/" + Path.GetFileName(Request.Files["Tamplatefile"].FileName);
                }
                if (Request.Form["PaymentMethodOptions"] == "" || Request.Form["audienceOptions"] == "") return View(plans);
                plans.audience = int.Parse(Request.Form["audienceOptions"]);
                plans.PaymentMethod = int.Parse(Request.Form["PaymentMethodOptions"]);
                db.HpartnerShipPlans.Add(plans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(plans);
        }

        // GET: Admin/Plans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HPartnerShipPlans plans = db.HpartnerShipPlans.Find(id);
            if (plans == null)
            {
                return HttpNotFound();
            }
            return View(plans);
        }

        // POST: Admin/Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,PlanTitle,Objective,Process,Participants,Report,DeliveryMode,Limitations,PlanTitleAr,ObjectiveAr,ProcessAr,ParticipantsAr,ReportAr,DeliveryModeAr,LimitationsAr,TamplatePath,Price,Status")] HPartnerShipPlans plans)
        {

            //if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                int postedFile = Request.Files["Tamplatefile"].ContentLength;
                if (Request.Files["Tamplatefile"].ContentLength > 0)
                {
                    string path = Server.MapPath("~/Areas/PartnersPlansFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    Request.Files["Tamplatefile"].SaveAs(path + Path.GetFileName(Request.Files["Tamplatefile"].FileName));
                    plans.TamplatePath = "/Areas/PartnersPlansFiles/" + (Request.Files["Tamplatefile"].FileName);
                }
                if (Request.Form["PaymentMethodOptions"] == "" || Request.Form["audienceOptions"] == "") return View(plans);
                plans.audience = int.Parse(Request.Form["audienceOptions"]);
                plans.PaymentMethod = int.Parse(Request.Form["PaymentMethodOptions"]);
                db.Entry(plans).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(plans);
        }

        // GET: Admin/Plans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HPartnerShipPlans plans = db.HpartnerShipPlans.Find(id);
            if (plans == null)
            {
                return HttpNotFound();
            }
            return View(plans);
        }

        // POST: Admin/Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            HPartnerShipPlans plans = db.HpartnerShipPlans.Find(id);
            db.HpartnerShipPlans.Remove(plans);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult getPlans()
        {
            int partnerId = int.Parse(Request["PartnerID"].ToString());
            var plansId = db.HPartnerships.Where(x => x.PartnerID == partnerId).Select(x => x.PlanID).ToArray();
            var plans = db.HpartnerShipPlans.Where(a=>plansId.Contains(a.Id)).ToList();
            return Json(plans, JsonRequestBehavior.AllowGet);
        }
    }
}
