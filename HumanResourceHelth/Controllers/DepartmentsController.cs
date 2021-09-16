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
   // [SessionAdminAuthorizeAttribute]
    public class DepartmentsController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork();
        public DepartmentsController()
        {
           
        }

        // GET: Departments
        public ActionResult Index()
        {
            return View(_unitOfWork.DepartmentRepo.GetAll().ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var departments = _unitOfWork.DepartmentRepo.FindById(id);
             
            if (departments == null)
            {
                return HttpNotFound();
            }

            return View(departments);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="DepartmentID,Name")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.DepartmentRepo.Add(departments);
                _unitOfWork.DepartmentRepo.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var departments = _unitOfWork.DepartmentRepo.FindById(id);
            if (departments == null)
            {
                return HttpNotFound();
            }
            return View(departments);
        }

        private ActionResult NotFound()
        {
            throw new NotImplementedException();
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include ="DepartmentID,Name")] Departments departments)
        {
            if (id != departments.DepartmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepo.Update(departments);

                    _unitOfWork.DepartmentRepo.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentsExists(departments.DepartmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = _unitOfWork.DepartmentRepo.FindById(id);
            if (departments == null)
            {
                return NotFound();
            }

            return View(departments);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var departments = _unitOfWork.DepartmentRepo.FindById(id);
            _unitOfWork.DepartmentRepo.Remove(departments);
            _unitOfWork.DepartmentRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentsExists(int id)
        {
            return _unitOfWork.DepartmentRepo.GetAll().Any(e => e.DepartmentID == id);
        }
    }
}
