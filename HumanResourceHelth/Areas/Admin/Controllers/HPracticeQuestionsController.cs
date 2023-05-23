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
    public class HPracticeQuestionsController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();

        // GET: Admin/HPracticeQuestions
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View(db.HPracticeQuestions.ToList());
        }

        // GET: Admin/PracticeQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HPracticeQuestions practiceQuestions = db.HPracticeQuestions.Find(id);
            if (practiceQuestions == null)
            {
                return HttpNotFound();
            }
            return View(practiceQuestions);
        }

        // GET: Admin/PracticeQuestions/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View();
        }

        // POST: Admin/PracticeQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Question,QuestionAr,Practice,Status")] HPracticeQuestions practiceQuestions)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                practiceQuestions.Respondent = int.Parse(Request["Respondent"].ToString());
                db.HPracticeQuestions.Add(practiceQuestions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(practiceQuestions);
        }

        // GET: Admin/PracticeQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HPracticeQuestions practiceQuestions = db.HPracticeQuestions.Find(id);
            if (practiceQuestions == null)
            {
                return HttpNotFound();
            }
            return View(practiceQuestions);
        }

        // POST: Admin/PracticeQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,QuestionAr,Practice,Status")] HPracticeQuestions practiceQuestions)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                practiceQuestions.Respondent = int.Parse(Request["Respondent"].ToString());
                db.Entry(practiceQuestions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(practiceQuestions);
        }

        // GET: Admin/PracticeQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HPracticeQuestions practiceQuestions = db.HPracticeQuestions.Find(id);
            if (practiceQuestions == null)
            {
                return HttpNotFound();
            }
            return View(practiceQuestions);
        }

        // POST: Admin/PracticeQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            HPracticeQuestions practiceQuestions = db.HPracticeQuestions.Find(id);
            db.HPracticeQuestions.Remove(practiceQuestions);
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
            int practiceId = Int16.Parse(Request["practiceId"].ToString());
            int functionId = db.HFunctionPractices.Find(practiceId).FunctionId;
            int PlanId = db.HFunctions.Find(functionId).planId;
            bool isPlanPublic = db.HpartnerShipPlans.Find(PlanId).audience == 10 ? true : false;
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            response.Add("isPlanPublic", isPlanPublic ? "public" : "not-public");
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPracticeQuestions()
        {
            int practiceId = Int16.Parse(Request["practiceId"].ToString());
            var practiceQuestions = db.HPracticeQuestions.Where(x => x.Practice == practiceId).ToList();
            //Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            //response.Add("practiceQuestions", practiceQuestions);
            return Json(practiceQuestions, JsonRequestBehavior.AllowGet);
        }
    }
}
