using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class CountriesController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork();

        public CountriesController()
        {
        }

        // GET: Countries
        public ActionResult Index()
        {
            return View(_unitOfWork.CountryRepo.GetAll().ToList());
        }

        // GET: Countries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var countries = _unitOfWork.CountryRepo.GetAll()
                .FirstOrDefault(m => m.Id == id);
            if (countries == null)
            {
                return HttpNotFound();
            }

            return View(countries);
        }

        // GET: Countries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CountryNameAr,CountryNameEn")] Country countries)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CountryRepo.Add(countries);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(countries);
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var countries = _unitOfWork.CountryRepo.FindById(id);
            if (countries == null)
            {
                return HttpNotFound();
            }
            return View(countries);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Id,CountryNameAr,CountryNameEn")] Country countries)
        {
            if (id != countries.Id)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CountryRepo.Update(countries);
                    _unitOfWork.CountryRepo.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountriesExists(countries.Id))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(countries);
        }

        // GET: Countries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var countries = _unitOfWork.CountryRepo.GetAll()
               .FirstOrDefault(m => m.Id == id);
            if (countries == null)
            {
                return HttpNotFound();
            }

            return View(countries);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var countries = _unitOfWork.CountryRepo.FindById(id);
            _unitOfWork.CountryRepo.Delete(countries);
            _unitOfWork.CountryRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CountriesExists(int id)
        {
            return _unitOfWork.CountryRepo.GetAll().Any(e => e.Id == id);
        }
    }
}