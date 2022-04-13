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
    public class CategoriesController : Controller
    {
        private UnitOfWork _uow = new UnitOfWork();
        private bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(_uow.CategoryRepo.GetAll().ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _uow.CategoryRepo.FindById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            GetPlans();
            return View();
        }

        private void GetPlans()
        {
            List<Plan> plans = _uow.PlanRepo.GetAll().ToList();
            List<SelectListItem> PlansSelectItems = new List<SelectListItem>();
            //int count = 0;
            foreach (Plan plan in plans)
            {

                PlansSelectItems.Add(new SelectListItem() { Text = isArabic ? plan.NameAR : plan.Name, Value = plan.Id.ToString() });
            }
            ViewBag.Palns = new SelectList(PlansSelectItems, "Value", "Text");
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NameAr,isActive,CategoryPlan")] Category category)
        {
            GetPlans();
            Category category1 = _uow.CategoryRepo.Search(a => a.CategoryPlan == category.CategoryPlan && (a.Name == category.Name || a.NameAr==category.NameAr)).SingleOrDefault();
            if (category1 != null)
                return View();
            if (ModelState.IsValid)
            {
                _uow.CategoryRepo.Add(category);
                _uow.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            GetPlans();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _uow.CategoryRepo.FindById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NameAr,isActive,CategoryPlan")] Category category)
        {
            if (ModelState.IsValid)
            {
                _uow.CategoryRepo.Update(category);
                _uow.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _uow.CategoryRepo.FindById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _uow.CategoryRepo.FindById(id);
            _uow.CategoryRepo.Delete(category);
            _uow.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _uow.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
