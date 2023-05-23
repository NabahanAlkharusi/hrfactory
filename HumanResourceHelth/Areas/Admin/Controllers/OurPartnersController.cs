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
    public class OurPartnersController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();

        // GET: Admin/OurPartners
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View(db.OurPartners.ToList());
        }

        // GET: Admin/OurPartners/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurPartners ourPartners = db.OurPartners.Find(id);
            if (ourPartners == null)
            {
                return HttpNotFound();
            }
            return View(ourPartners);
        }

        // GET: Admin/OurPartners/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View();
        }

        // POST: Admin/OurPartners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,PartnerName,PartnerNameAr,PartnerofficalEmail,PartnerofficalPhone,PartnerCountry,PartnerAddress,PartnerFocalPointName,PartnerFocalPointNameAr,PartnerFocalPointEmail,PartnerFocalPointPhone,PartnerWebsite,PartnerLogo,PartnerDes,PartnerDesAr,PartnerSort,status")] OurPartners ourPartners)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                if (Request.Files["PartnerLogo"].ContentLength <= 0)
                {
                    ModelState.AddModelError("PartnerLogo", "Please Upload Partner Logo");
                    return View(ourPartners);
                }
                else
                {
                    string fileName = Path.GetFileName(Request.Files["PartnerLogo"].FileName);
                 
                    string path = Server.MapPath("~/ContentImages/");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    Request.Files["PartnerLogo"].SaveAs(path + fileName);
                    ourPartners.PartnerLogo = "/ContentImages/" + fileName;
                    int count = db.OurPartners.Count();
                    ourPartners.PartnerSort = count + 1;
                    ourPartners.PartnerWebsite = ourPartners.PartnerWebsite == null ? "#" : ourPartners.PartnerWebsite;
                    db.OurPartners.Add(ourPartners);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(ourPartners);
        }

        // GET: Admin/OurPartners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurPartners ourPartners = db.OurPartners.Find(id);
            if (ourPartners == null)
            {
                return HttpNotFound();
            }
            return View(ourPartners);
        }

        // POST: Admin/OurPartners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,PartnerName,PartnerNameAr,PartnerofficalEmail,PartnerofficalPhone,PartnerCountry,PartnerAddress,PartnerFocalPointName,PartnerFocalPointNameAr,PartnerFocalPointEmail,PartnerFocalPointPhone,PartnerWebsite,PartnerLogo,PartnerDes,PartnerDesAr,PartnerSort,status")] OurPartners ourPartners)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                db.Entry(ourPartners).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ourPartners);
        }

        // GET: Admin/OurPartners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurPartners ourPartners = db.OurPartners.Find(id);
            if (ourPartners == null)
            {
                return HttpNotFound();
            }
            return View(ourPartners);
        }

        // POST: Admin/OurPartners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            OurPartners ourPartners = db.OurPartners.Find(id);
            db.OurPartners.Remove(ourPartners);
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
    }
}
