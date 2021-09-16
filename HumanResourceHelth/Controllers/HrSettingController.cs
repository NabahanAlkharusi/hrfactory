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
    public class HrSettingController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork();
        public HrSettingController()
        {

        }
        // GET: HrSetting
        public ActionResult Index()
        {
            var HrSetting = _unitOfWork.HrSettingRepo.FindById(1);

            if (HrSetting == null)
            {
                return HttpNotFound();
            }
            return View(HrSetting);

        }


        // GET: HrSetting/Edit/5
        public ActionResult Edit()
        {
           
            var hrSetting = _unitOfWork.HrSettingRepo.FindById(1);
            if (hrSetting == null)
            {
                return HttpNotFound();
            }
            return View(hrSetting);
        }

        private ActionResult NotFound()
        {
            throw new NotImplementedException();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "ID,LearnOnlineVideo,LearnOnlineText")] HrSetting hrSetting)
        {
            if (id != hrSetting.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.HrSettingRepo.Update(hrSetting);

                    _unitOfWork.HrSettingRepo.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HrSettingExists(hrSetting.ID))
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
            return View(hrSetting);
        }

        private bool HrSettingExists(int id)
        {
            return _unitOfWork.HrSettingRepo.GetAll().Any(e => e.ID == id);
        }

    }
}