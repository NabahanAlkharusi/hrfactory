using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
   // [SessionAdminAuthorize]
    public class SpecialtiesController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork();
        public SpecialtiesController()
        {
           
        }

        // GET: Specialties
        public ActionResult Index()
        {
            return View(_unitOfWork.SpecialityRepo.GetAll().ToList());
        }

        // GET: Specialties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var specialties = _unitOfWork.SpecialityRepo.FindById(id);
               
            if (specialties == null)
            {
                return HttpNotFound();
            }

            return View(specialties);
        }

        // GET: Specialties/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Specialties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="SpecialtyName")] Specialties specialties)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SpecialityRepo.Add(specialties);
                _unitOfWork.SpecialityRepo.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(specialties);
        }

        // GET: Specialties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var specialties = _unitOfWork.SpecialityRepo.FindById(id);
            if (specialties == null)
            {
                return HttpNotFound();
            }
            return View(specialties);
        }

        // POST: Specialties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include ="SpecialtyID,SpecialtyName")] Specialties specialties)
        {
            if (id != specialties.SpecialtyID)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.SpecialityRepo.Update(specialties);
                    _unitOfWork.SpecialityRepo.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    if (!SpecialtiesExists(specialties.SpecialtyID))
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
            return View(specialties);
        }

        // GET: Specialties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var specialties = _unitOfWork.SpecialityRepo.GetAll().
                FirstOrDefault(x => x.SpecialtyID == id);
            if (specialties == null)
            {
                return HttpNotFound();
            }

            return View(specialties);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var specialties = _unitOfWork.SpecialityRepo.FindById(id);
            _unitOfWork.SpecialityRepo.Delete(specialties);
            _unitOfWork.SpecialityRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtiesExists(int id)
        {
            return _unitOfWork.SpecialityRepo.GetAll().Any(x => x.SpecialtyID == id);
        }
    }
}
