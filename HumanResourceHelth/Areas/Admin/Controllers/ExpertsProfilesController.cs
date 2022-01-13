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
using Newtonsoft.Json;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class ExpertsProfilesController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();

        // GET: Admin/ExpertsProfiles
        public ActionResult Index()
        {
            return View(db.Experts.ToList());
        }

        // GET: Admin/ExpertsProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpertsProfile expertsProfile = db.Experts.Find(id);
            if (expertsProfile == null)
            {
                return HttpNotFound();
            }
            return View(expertsProfile);
        }

        // GET: Admin/ExpertsProfiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ExpertsProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,ImageUrl,LinkedInUrl,DescriptionEn,DescriptionAr,YouToubeUrl,InstagramUrl,NameEn,NameAr,SummaryEn,EducationEn,ExperiencesEN,CertificatesEN,LanguagesEn,SummaryAr,EducationAr,ExperiencesAr,CertificatesAr,LanguagesAr")] ExpertsProfile expertsProfile)
        {
            if (ModelState.IsValid)
            {
                HttpFileCollectionBase files = Request.Files;
                var ImageFileName = "";
                
                if (files.Count > 0)
                {

                    ImageFileName = Path.GetFileName(files[0].FileName);
                    
                    if (!Directory.Exists(Server.MapPath($"~/ExpertImages/")))
                        Directory.CreateDirectory(Server.MapPath($"~/ExpertImages/"));
                    files[0].SaveAs(Server.MapPath($"~/ExpertImages/{ImageFileName}"));
                    
                }
                expertsProfile.ImageUrl = ImageFileName;
                db.Experts.Add(expertsProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expertsProfile);
        }

        // GET: Admin/ExpertsProfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpertsProfile expertsProfile = db.Experts.Find(id);
            if (expertsProfile == null)
            {
                return HttpNotFound();
            }
            return View(expertsProfile);
        }

        // POST: Admin/ExpertsProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl,LinkedInUrl,DescriptionEn,DescriptionAr,YouToubeUrl,InstagramUrl,NameEn,NameAr,SummaryEn,EducationEn,ExperiencesEN,CertificatesEN,LanguagesEn,SummaryAr,EducationAr,ExperiencesAr,CertificatesAr,LanguagesAr")] ExpertsProfile expertsProfile)
        {
            if (ModelState.IsValid)
            {
                HttpFileCollectionBase files = Request.Files;
                var ImageFileName = "";

                if (files.Count > 0)
                {

                    ImageFileName = Path.GetFileName(files[0].FileName);

                    if (!Directory.Exists(Server.MapPath($"~/ExpertImages/")))
                        Directory.CreateDirectory(Server.MapPath($"~/ExpertImages/"));
                    files[0].SaveAs(Server.MapPath($"~/ExpertImages/{ImageFileName}"));

                }
                expertsProfile.ImageUrl = ImageFileName;
                db.Entry(expertsProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expertsProfile);
        }

        // GET: Admin/ExpertsProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpertsProfile expertsProfile = db.Experts.Find(id);
            if (expertsProfile == null)
            {
                return HttpNotFound();
            }
            return View(expertsProfile);
        }

        // POST: Admin/ExpertsProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpertsProfile expertsProfile = db.Experts.Find(id);
            db.Experts.Remove(expertsProfile);
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
        [HttpPost]
        public JsonResult GetExpert(FormCollection formCollection)
        {
            try
            {
                int id = int.Parse(formCollection["id"]);
                ExpertsProfile profile = db.Experts.Find(id);
                return Json(new { Content = (profile) });
            }
            catch(Exception ex)
            {
                return Json(new { err = "err: " + ex.Message });
            }
        }
    }
}
