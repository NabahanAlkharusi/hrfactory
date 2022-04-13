using HumanResourceHelth.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanResourceHelth.Model;
using System.Net;
using HumanResourceHelth.Web.Data;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class TermsConditionsController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();
        UnitOfWork _uow = new UnitOfWork();
        bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        // GET: Admin/TermsConditions
        public ActionResult Index()
        {
            TermsConditionsViewModel terms = new TermsConditionsViewModel()
            {
                TermsConditions = _uow.TermsConditionsRepo.GetAll().ToList(),
                Countries = _uow.CountryRepo.GetAll().ToList()
            };
            return View(terms);
        }

        // GET: Admin/TermsConditions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermsConditions termsConditions = db.TermsConditions.Find(id);
            if (termsConditions == null)
            {
                return HttpNotFound();
            }
            TermsConditionsViewModel termsConditionsView = new TermsConditionsViewModel()
            {
                TermCondition = termsConditions,
                Countries = _uow.CountryRepo.GetAll()
            };
            return View(termsConditionsView);
        }

        // GET: Admin/TermsConditions/Create
        public ActionResult Create()
        {
            string TermsName = "";
            List<Country> countries = _uow.CountryRepo.GetAll().OrderByDescending(a => a.IsArabCountry).ToList();
            List<SelectListItem> TermsDomain = new List<SelectListItem>();
            List<SelectListItem> TermsType = new List<SelectListItem>();
            TermsDomain.Add(new SelectListItem() { Text = Resources.General.PublicTerms, Value = "0" });
            foreach (Country country in countries)
            {
                TermsDomain.Add(new SelectListItem() { Text = isArabic ? country.NameAr : country.Name, Value = country.Id.ToString() });
            }

            ViewBag.Countries = new SelectList(TermsDomain, "Value", "Text");

            var values = Enum.GetValues(typeof(TermsConditionType));
            foreach (int value in values)
            {
                switch ((int)value)
                {
                    case 1:
                        TermsName = Resources.General.RegisterationTerms;
                        break;
                    case 2:
                        TermsName = Resources.General.StartUpPlanMonthlyTerms;
                        break;
                    case 3:
                        TermsName = Resources.General.StartUpPlanAnnuallyTerms;
                        break;
                    case 4:
                        TermsName = Resources.General.ManualBuilderPlanMonthlyTerms;
                        break;
                    case 5:
                        TermsName = Resources.General.ManualBuilderPlanAnnuallyTerms;
                        break;
                    case 6:
                        TermsName = Resources.General.HrCheckUpTerms;
                        break;
                }
                TermsType.Add(new SelectListItem() { Text = TermsName, Value = value.ToString() });
            }
            ViewBag.TermsType = new SelectList(TermsType, "Value", "Text");
            return View();
        }

        // POST: Admin/TermsConditions/Create
        [HttpPost]
        [ValidateAntiForgeryToken
            , ValidateInput(false)]
        public ActionResult Create([Bind(Include = "TermsConditionType,CountryId,ArabicText,EnglishText,ArabicTitle,EnglishTitle")] TermsConditions terms)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    _uow.TermsConditionsRepo.Add(terms);
                    _uow.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    string TermsName = "";
                    List<Country> countries = _uow.CountryRepo.GetAll().OrderByDescending(a => a.IsArabCountry).ToList();
                    List<SelectListItem> TermsDomain = new List<SelectListItem>();
                    List<SelectListItem> TermsType = new List<SelectListItem>();
                    TermsDomain.Add(new SelectListItem() { Text = Resources.General.PublicTerms, Value = "0" });
                    foreach (Country country in countries)
                    {
                        TermsDomain.Add(new SelectListItem() { Text = isArabic ? country.NameAr : country.Name, Value = country.Id.ToString() });
                    }

                    ViewBag.Countries = new SelectList(TermsDomain, "Value", "Text");

                    var values = Enum.GetValues(typeof(TermsConditionType));
                    foreach (int value in values)
                    {
                        switch ((int)value)
                        {
                            case 1:
                                TermsName = Resources.General.RegisterationTerms;
                                break;
                            case 2:
                                TermsName = Resources.General.StartUpPlanMonthlyTerms;
                                break;
                            case 3:
                                TermsName = Resources.General.StartUpPlanAnnuallyTerms;
                                break;
                            case 4:
                                TermsName = Resources.General.ManualBuilderPlanMonthlyTerms;
                                break;
                            case 5:
                                TermsName = Resources.General.ManualBuilderPlanAnnuallyTerms;
                                break;
                            case 6:
                                TermsName = Resources.General.HrCheckUpTerms;
                                break;
                        }
                        TermsType.Add(new SelectListItem() { Text = TermsName, Value = value.ToString() });
                    }
                    ViewBag.TermsType = new SelectList(TermsType, "Value", "Text");
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TermsConditions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermsConditions termsConditions = _uow.TermsConditionsRepo.FindById(id);
            if (termsConditions == null)
            {
                return HttpNotFound();
            }

            string TermsName = "";
            List<Country> countries = _uow.CountryRepo.GetAll().OrderByDescending(a => a.IsArabCountry).ToList();
            List<SelectListItem> TermsDomain = new List<SelectListItem>();
            List<SelectListItem> TermsType = new List<SelectListItem>();
            TermsDomain.Add(new SelectListItem() { Text = Resources.General.PublicTerms, Value = "0" });
            foreach (Country country in countries)
            {
                TermsDomain.Add(new SelectListItem() { Text = isArabic ? country.NameAr : country.Name, Value = country.Id.ToString() });
            }

            ViewBag.Countries = new SelectList(TermsDomain, "Value", "Text");

            var values = Enum.GetValues(typeof(TermsConditionType));
            foreach (int value in values)
            {
                switch ((int)value)
                {
                    case 1:
                        TermsName = Resources.General.RegisterationTerms;
                        break;
                    case 2:
                        TermsName = Resources.General.StartUpPlanMonthlyTerms;
                        break;
                    case 3:
                        TermsName = Resources.General.StartUpPlanAnnuallyTerms;
                        break;
                    case 4:
                        TermsName = Resources.General.ManualBuilderPlanMonthlyTerms;
                        break;
                    case 5:
                        TermsName = Resources.General.ManualBuilderPlanAnnuallyTerms;
                        break;
                    case 6:
                        TermsName = Resources.General.HrCheckUpTerms;
                        break;
                }
                TermsType.Add(new SelectListItem() { Text = TermsName, Value = value.ToString() });
            }
            ViewBag.TermsType = new SelectList(TermsType, "Value", "Text");
            return View(termsConditions);
        }

        // POST: Admin/TermsConditions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken
            , ValidateInput(false)]
        public ActionResult Edit(int id,
            FormCollection formCollection,
            [Bind(Include = "TermsConditionType,CountryId,ArabicText,EnglishText,ArabicTitle,EnglishTitle")] TermsConditions terms)
        {
            string TermsName = "";
            List<Country> countries = _uow.CountryRepo.GetAll().OrderByDescending(a => a.IsArabCountry).ToList();
            List<SelectListItem> TermsDomain = new List<SelectListItem>();
            List<SelectListItem> TermsType = new List<SelectListItem>();
            TermsDomain.Add(new SelectListItem() { Text = Resources.General.PublicTerms, Value = "0" });
            foreach (Country country in countries)
            {
                TermsDomain.Add(new SelectListItem() { Text = isArabic ? country.NameAr : country.Name, Value = country.Id.ToString() });
            }

            ViewBag.Countries = new SelectList(TermsDomain, "Value", "Text");

            var values = Enum.GetValues(typeof(TermsConditionType));
            foreach (int value in values)
            {
                switch ((int)value)
                {
                    case 1:
                        TermsName = Resources.General.RegisterationTerms;
                        break;
                    case 2:
                        TermsName = Resources.General.StartUpPlanMonthlyTerms;
                        break;
                    case 3:
                        TermsName = Resources.General.StartUpPlanAnnuallyTerms;
                        break;
                    case 4:
                        TermsName = Resources.General.ManualBuilderPlanMonthlyTerms;
                        break;
                    case 5:
                        TermsName = Resources.General.ManualBuilderPlanAnnuallyTerms;
                        break;
                    case 6:
                        TermsName = Resources.General.HrCheckUpTerms;
                        break;
                }
                TermsType.Add(new SelectListItem() { Text = TermsName, Value = value.ToString() });
            }
            ViewBag.TermsType = new SelectList(TermsType, "Value", "Text");
            try
            {
                string Termsid = formCollection["Id"];
                terms.Id = Int32.Parse(Termsid);
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    if (id == terms.Id)
                    {
                        _uow.TermsConditionsRepo.Update(terms);
                        _uow.TermsConditionsRepo.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {

                    return View();
                }
            }
            catch
            {

                return View();
            }
        }

        // GET: Admin/TermsConditions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermsConditions termsConditions = _uow.TermsConditionsRepo.FindById(id);
            if (termsConditions == null)
            {
                return HttpNotFound();
            }
            if(termsConditions.CountryId==158)
                return RedirectToAction("Index");
            List<Country> countries = _uow.CountryRepo.GetAll().ToList();
            ViewBag.Countries = countries;
            return View(termsConditions);
        }

        // POST: Admin/TermsConditions/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                TermsConditions terms_ = _uow.TermsConditionsRepo.FindById(id); 
                // TODO: Add delete logic here
                _uow.TermsConditionsRepo.Delete(terms_);
                _uow.TermsConditionsRepo.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
