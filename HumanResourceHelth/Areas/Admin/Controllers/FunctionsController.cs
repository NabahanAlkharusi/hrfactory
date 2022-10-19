using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class FunctionsController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();

        // GET: Admin/Functions
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View(db.Functions.ToList());
        }

        // GET: Admin/Functions/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Functions functions = db.Functions.Find(id);
            if (functions == null)
            {
                return HttpNotFound();
            }
            return View(functions);
        }

        // GET: Admin/Functions/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View();
        }

        // POST: Admin/Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FunctionTitle,FunctionTitleAr,planId,status")] Functions functions)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                functions.Respondent = int.Parse(Request["Respondent"].ToString());
                db.Functions.Add(functions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(functions);
        }

        // GET: Admin/Functions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Functions functions = db.Functions.Find(id);
            if (functions == null)
            {
                return HttpNotFound();
            }
            return View(functions);
        }

        // POST: Admin/Functions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FunctionTitle,FunctionTitleAr,planId,status")] Functions functions)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                functions.Respondent = int.Parse(Request["Respondent"].ToString());
                db.Entry(functions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(functions);
        }

        // GET: Admin/Functions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Functions functions = db.Functions.Find(id);
            if (functions == null)
            {
                return HttpNotFound();
            }
            return View(functions);
        }

        // POST: Admin/Functions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            Functions functions = db.Functions.Find(id);
            db.Functions.Remove(functions);
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
        public ActionResult CheckPlan()
        {
            int PlanId = Int16.Parse(Request["planID"].ToString());
            
            bool isPlanPublic = db.partnerShipPlans.Find(PlanId).audience == 10 ? true : false;
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            response.Add("isPlanPublic", isPlanPublic ? "public" : "not-public");
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFunctions()
        {
            {
                int PlanId = Int16.Parse(Request["planID"].ToString());
                var functions = db.Functions.Where(x => x.planId == PlanId).ToList();
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
                response.Add("functions", functions);
                return Json(functions, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
