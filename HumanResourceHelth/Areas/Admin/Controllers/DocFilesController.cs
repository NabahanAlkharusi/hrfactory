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
    public class DocFilesController : Controller
    {
        private UnitOfWork _uow = new UnitOfWork();
        private bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        // GET: Admin/DocFiles
        public ActionResult Index()
        {
            SetUpForm();
            return View(_uow.DocFileRepo.GetAll().ToList());
        }

        // GET: Admin/DocFiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocFile docFile = _uow.DocFileRepo.FindById(id);
            if (docFile == null)
            {
                return HttpNotFound();
            }
            return View(docFile);
        }

        // GET: Admin/DocFiles/Create
        public ActionResult Create()
        {
            if (Session["PlansForFiles"] == null) return Redirect("/Admin/Login");
            if (_uow.CategoryRepo.GetAll().ToList().Count == 0) return Redirect("/Admin/Categories/Create");


            return View();

        }

        private void SetUpForm()
        {
            List<Country> countries = _uow.CountryRepo.GetAll().OrderByDescending(a => a.IsArabCountry).ToList();
            List<SelectListItem> CountriesSLI = new List<SelectListItem>();
            CountriesSLI.Add(new SelectListItem() { Text = isArabic ? "لجميع الدول" : "For All Countries", Value = "0" });
            foreach (Country country in countries)
            {
                CountriesSLI.Add(new SelectListItem() { Text = isArabic ? country.NameAr : country.Name, Value = country.Id.ToString() });
            }
            Session["CountriesForFiles"] = new SelectList(CountriesSLI, "Value", "Text");
            List<Plan> plans = _uow.PlanRepo.GetAll().ToList();
            List<SelectListItem> plansSLI = new List<SelectListItem>();
            plansSLI.Add(new SelectListItem() { Text = "Select", Value = "" });
            foreach (Plan plan in plans)
            {
                plansSLI.Add(new SelectListItem() { Text = isArabic ? plan.NameAR : plan.Name, Value = plan.Id.ToString() });
            }
            Session["PlansForFiles"] = new SelectList(plansSLI, "Value", "Text");
            //List<Category> Categories = _uow.CategoryRepo.Search(a => a.CategoryPlan == plans.FirstOrDefault().Id).ToList();
            //List<SelectListItem> CategoriesSLI = new List<SelectListItem>();
            //foreach (Category category in Categories)
            //{
            //    CategoriesSLI.Add(new SelectListItem() { Text = isArabic ? category.NameAr : category.Name, Value = category.Id.ToString() });
            //}
            //Session["Categories"] = new SelectList(CategoriesSLI, "Value", "Text");

        }

        // POST: Admin/DocFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NameAr,EngDocPath,ArbDocPath,EngVedio,ArbVedio,isFileFree,isFileActive,isvedioYouTube,CountryId,CategoryId")] DocFile docFile)
        {
            string Name = Request["Name"];
            string NameAr = Request["NameAr"];
            int CountryId = int.Parse(Request["CountryId"]);
            string CountryName = CountryId == 0 ? "All" : _uow.CountryRepo.FindById(CountryId).Name;
            int plans = int.Parse(Request["plans"]);
            string PlanName = Enum.GetName(typeof(SubscriptionPlan), plans);
            string MainPath = "/Areas/PlansData1/";
            CreatFolders(Server.MapPath(MainPath));
            string CountryPath = MainPath + CountryName + "/";
            CreatFolders(Server.MapPath(CountryPath));
            string PlanPath = CountryPath + PlanName;
            CreatFolders(Server.MapPath(PlanPath));
            string ArbFolderPath = PlanPath + "/ar";
            CreatFolders(Server.MapPath(ArbFolderPath));
            string EngFolderPath = PlanPath + "/en";
            CreatFolders(Server.MapPath(EngFolderPath));
            string ArbDocPath = ArbFolderPath + "/Docs";
            CreatFolders(Server.MapPath(ArbDocPath));
            string ArbVideosPath = ArbFolderPath + "/Videos";
            CreatFolders(Server.MapPath(ArbVideosPath));
            string EngDocPath = EngFolderPath + "/Docs";
            CreatFolders(Server.MapPath(EngDocPath));
            string EngVideosPath = EngFolderPath + "/Videos";
            CreatFolders(Server.MapPath(EngVideosPath));
            HttpPostedFileBase EngDoc = Request.Files[0];
            if (EngDoc.ContentLength > 0)
            {
                string EngDocFullPath = EngDocPath + "/" + EngDoc.FileName;
                EngDoc.SaveAs(Server.MapPath(EngDocFullPath));
                docFile.EngDocPath = EngDocFullPath;
            }
            HttpPostedFileBase ArbDoc = Request.Files[1];
            if (ArbDoc.ContentLength > 0)
            {
                string ArabDocFullPath = ArbDocPath + "/" + ArbDoc.FileName;
                ArbDoc.SaveAs(Server.MapPath(ArabDocFullPath));
                docFile.ArbDocPath = ArabDocFullPath;
            }
            //string EngVedio = "not setteed";
            //string ArbVedio = "not setted";
            if (!bool.Parse(Request["isvedioYouTube"]))
            {
                HttpPostedFileBase EngV = Request.Files[2];
                if (EngV.ContentLength > 0)
                {
                    string EngVedioFullPath = EngVideosPath + "/" + EngV.FileName;
                    EngV.SaveAs(Server.MapPath(EngVedioFullPath));
                    docFile.EngVedio = EngVedioFullPath;
                }
                HttpPostedFileBase ArbV = Request.Files[3];
                if (ArbV.ContentLength > 0)
                {
                    string ArabVedioFullPath = ArbVideosPath + "/" + ArbV.FileName;
                    ArbV.SaveAs(Server.MapPath(ArabVedioFullPath));
                    docFile.ArbVedio = ArabVedioFullPath;
                }
            }
            //return ("EngDoc: "+EngDoc.FileName+ " ArbDoc: "+ArbDoc.FileName+ " EngVedio: " + EngVedio+ " ArbVedio:"+ ArbVedio);
            if (ModelState.IsValid)
            {
                _uow.DocFileRepo.Add(docFile);
                _uow.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(docFile);
        }

        private static void CreatFolders(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        // GET: Admin/DocFiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["PlansForFiles"] == null) return Redirect("/Admin/Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocFile docFile = _uow.DocFileRepo.FindById(id);
            if (docFile == null)
            {
                return HttpNotFound();
            }
            Category category = _uow.CategoryRepo.FindById(docFile.CategoryId);
            Session["CategoryIdForUpdate"] = category.Id;
            Session["PlanIdForUpdate"] = category.CategoryPlan;
            return View(docFile);
        }

        // POST: Admin/DocFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NameAr,EngDocPath,ArbDocPath,EngVedio,ArbVedio,isFileFree,isFileActive,isvedioYouTube,CountryId,CategoryId")] DocFile docFile)
        {
            int CountryId = int.Parse(Request["CountryId"]);
            string CountryName = CountryId == 0 ? "All" : _uow.CountryRepo.FindById(CountryId).Name;
            int plans = int.Parse(Request["plans"]);
            string PlanName = Enum.GetName(typeof(SubscriptionPlan), plans);
            string MainPath = "/Areas/PlansData1/";
            CreatFolders(Server.MapPath(MainPath));
            string CountryPath = MainPath + CountryName + "/";
            CreatFolders(Server.MapPath(CountryPath));
            string PlanPath = CountryPath + PlanName;
            CreatFolders(Server.MapPath(PlanPath));
            string ArbFolderPath = PlanPath + "/ar";
            CreatFolders(Server.MapPath(ArbFolderPath));
            string EngFolderPath = PlanPath + "/en";
            CreatFolders(Server.MapPath(EngFolderPath));
            string ArbDocPath = ArbFolderPath + "/Docs";
            CreatFolders(Server.MapPath(ArbDocPath));
            string ArbVideosPath = ArbFolderPath + "/Videos";
            CreatFolders(Server.MapPath(ArbVideosPath));
            string EngDocPath = EngFolderPath + "/Docs";
            CreatFolders(Server.MapPath(EngDocPath));
            string EngVideosPath = EngFolderPath + "/Videos";
            CreatFolders(Server.MapPath(EngVideosPath));
            HttpPostedFileBase EngDoc = Request.Files[0];
            if (EngDoc.ContentLength > 0)
            {
                string EngDocFullPath = EngDocPath + "/" + EngDoc.FileName;
                EngDoc.SaveAs(Server.MapPath(EngDocFullPath));
                docFile.EngDocPath = EngDocFullPath;
            }
            HttpPostedFileBase ArbDoc = Request.Files[1];
            if (ArbDoc.ContentLength > 0)
            {
                string ArbDocFullPath = ArbDocPath + "/" + ArbDoc.FileName;
                EngDoc.SaveAs(Server.MapPath(ArbDocFullPath));
                docFile.ArbDocPath = ArbDocFullPath;
            }
            HttpPostedFileBase EngVideo = Request.Files[2];
            if (EngVideo.ContentLength > 0)
            {
                string EngVideoFullPath = EngVideosPath + "/" + EngVideo.FileName;
                EngDoc.SaveAs(Server.MapPath(EngVideoFullPath));
                docFile.EngVedio = EngVideoFullPath;
            }
            HttpPostedFileBase ArbVideo = Request.Files[3];
            if (ArbVideo.ContentLength > 0)
            {
                string ArbVideoFullPath = ArbVideosPath + "/" + ArbVideo.FileName;
                EngDoc.SaveAs(Server.MapPath(ArbVideoFullPath));
                docFile.ArbVedio = ArbVideoFullPath;
            }
            if (ModelState.IsValid)
            {
                _uow.DocFileRepo.Update(docFile);
                _uow.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(docFile);
        }

        // GET: Admin/DocFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocFile docFile = _uow.DocFileRepo.FindById(id);
            if (docFile == null)
            {
                return HttpNotFound();
            }
            return View(docFile);
        }

        // POST: Admin/DocFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocFile docFile = _uow.DocFileRepo.FindById(id);
            _uow.DocFileRepo.Delete(docFile);
            _uow.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _uow.c();
        //    }
        //    base.Dispose(disposing);
        //}
        public ActionResult GetCategories()
        {

            try
            {
                if (Request["id"] == null || Request["id"] == "") return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "error");
                var id = int.Parse(Request["id"]);
                List<Category> categories = _uow.CategoryRepo.Search(a => a.CategoryPlan == id && a.isActive).ToList();
                return Json(categories, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
