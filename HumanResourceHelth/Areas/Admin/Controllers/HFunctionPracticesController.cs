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
    public class HFunctionPracticesController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();

        // GET: Admin/FunctionPractices
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View(db.HFunctionPractices.ToList());
        }

        // GET: Admin/FunctionPractices/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HFunctionPractice functionPractice = db.HFunctionPractices.Find(id);
            if (functionPractice == null)
            {
                return HttpNotFound();
            }
            return View(functionPractice);
        }

        // GET: Admin/FunctionPractices/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View();
        }

        // POST: Admin/FunctionPractices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PracticeTitle,PracticeTitleAr,FunctionId,Status")] HFunctionPractice functionPractice)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                db.HFunctionPractices.Add(functionPractice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(functionPractice);
        }

        // GET: Admin/FunctionPractices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HFunctionPractice functionPractice = db.HFunctionPractices.Find(id);
            if (functionPractice == null)
            {
                return HttpNotFound();
            }
            return View(functionPractice);
        }

        // POST: Admin/FunctionPractices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PracticeTitle,PracticeTitleAr,FunctionId,Status")] HFunctionPractice functionPractice)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                db.Entry(functionPractice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(functionPractice);
        }

        // GET: Admin/FunctionPractices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HFunctionPractice functionPractice = db.HFunctionPractices.Find(id);
            if (functionPractice == null)
            {
                return HttpNotFound();
            }
            return View(functionPractice);
        }

        // POST: Admin/FunctionPractices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            HFunctionPractice functionPractice = db.HFunctionPractices.Find(id);
            db.HFunctionPractices.Remove(functionPractice);
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
        public ActionResult GetFunctionPractices()
        {
            int FunctionID = Int16.Parse(Request["FunctionID"].ToString());
            var functionPractices = db.HFunctionPractices.Where(x => x.FunctionId == FunctionID).ToList();
            return Json(functionPractices, JsonRequestBehavior.AllowGet);
        }
    }
}
