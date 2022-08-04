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
using HumanResourceHelth.Web.Data;
using Newtonsoft.Json;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class DefaultMBsController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();
        UnitOfWork _uow = new UnitOfWork();
        bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        // GET: Admin/DefaultMBs
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            List<Country> countries = isArabic ? _uow.CountryRepo.GetAll().OrderBy(a => a.NameAr).ToList() : _uow.CountryRepo.GetAll().OrderBy(a => a.Name).ToList();
            int langId = isArabic ? 2 : 1;
            List<DefaultMB> _defaultMBs = _uow.DefaultMBRepo.GetAll().Where(a => a.CountryID == 158 && a.CompanySize==1 && a.LanguageId == langId).OrderBy(x => x.Ordering).ToList();
            List<DefaultMB> _defaultParentMBs = _uow.DefaultMBRepo.GetAll().Where(a => a.CountryID == 158 && a.LanguageId == langId && a.ParenId == null && a.CompanySize == 1).OrderBy(x => x.Ordering).ToList();
            DefaultMBViewModel defaultMBViewModel = new DefaultMBViewModel()
            {
                Countries = countries,
                DefaultMB = _defaultMBs,
                DefaultMB1 = _defaultParentMBs,
                ParentConter = _defaultParentMBs.Count,
                CurrentCountry = 158,
                CurrentCompanySize = 1

            };
            return View(defaultMBViewModel);
        }

        // GET: Admin/DefaultMBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultMB defaultMB = db.DefaultMB.Find(id);
            if (defaultMB == null)
            {
                return HttpNotFound();
            }
            return View(defaultMB);
        }
        [HttpPost]
        public ActionResult GetSection(int id)
        {

            try
            {
                DefaultMB defaultMB = _uow.DefaultMBRepo.FindById(id);
                if (defaultMB == null)
                {
                    return HttpNotFound();
                }
                Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
                keyValues.Add("id", defaultMB.Id);
                keyValues.Add("title", defaultMB.Title);
                keyValues.Add("explain", defaultMB.Description);
                keyValues.Add("content", defaultMB.Content);
                keyValues.Add("haveLine", defaultMB.IsHaveLineBefore);
                return Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ChangeOrder(int id, int NextID)
        {

            try
            {
                DefaultMB defaultMB = _uow.DefaultMBRepo.FindById(id);
                if (defaultMB == null)
                {
                    return HttpNotFound();
                }
                //return Json("ID: " + id.ToString() + " NextId: " + NextID.ToString(), JsonRequestBehavior.AllowGet);
                int currentOrder = (int)defaultMB.Ordering;
                DefaultMB NextMB = _uow.DefaultMBRepo.FindById(NextID);
                int NextOrder = (int)NextMB.Ordering;
                NextMB.Ordering = currentOrder;
                _uow.DefaultMBRepo.Update(NextMB);
                _uow.SaveChanges();
                defaultMB.Ordering = NextOrder;
                _uow.DefaultMBRepo.Update(defaultMB);
                _uow.SaveChanges();

                return Json("Done currentOrder= " + currentOrder.ToString() + " New= " + defaultMB.Ordering.ToString() + " == NextOrder= " + NextOrder.ToString() + "  New next= " + NextMB.Ordering.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetParent(int country, int size)
        {

            try
            {
                int langId = isArabic ? 2 : 1;
                List<DefaultMB> ParentMBSec = _uow.DefaultMBRepo.Search(a => a.CountryID == country && a.CompanySize == size && a.ParenId == null && a.LanguageId == langId).OrderBy(z => z.Ordering).ToList();
                Dictionary<string, dynamic> ParentSection = new Dictionary<string, dynamic>();
                List<string> ParentName = new List<string>();
                List<int> ParentId = new List<int>();
                foreach (DefaultMB Parent in ParentMBSec)
                {
                    ParentName.Add(Parent.Title);
                    ParentId.Add(Parent.Id);
                }
                ParentSection.Add("id", ParentId);
                ParentSection.Add("name", ParentName);
                return Json(ParentSection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveChanges(
            int? id, string title, string explain, string content,
                bool ischecked, int CountryID, int size, int? Parent
            )
        {
            try
            {
                if (id != null)
                {
                    DefaultMB defaultMB = _uow.DefaultMBRepo.FindById(id);
                    if (defaultMB != null)
                    {
                        defaultMB.Title = title;
                        defaultMB.Content = content;
                        defaultMB.Description = explain;
                        defaultMB.IsHaveLineBefore = ischecked;
                        _uow.DefaultMBRepo.Update(defaultMB);
                    }
                }
                else
                {
                    List<DefaultMB> defaultMBs = _uow.DefaultMBRepo.GetAll();
                    int DefMBID = defaultMBs.LastOrDefault().DefaultMBId;
                    DefMBID++;
                    int order = (int)defaultMBs.Where(a => a.CountryID == CountryID).LastOrDefault().Ordering;
                    order = order + 1;
                    DefaultMB NewdefaultMB = new DefaultMB();
                    NewdefaultMB.Title = title;
                    NewdefaultMB.Content = content;
                    NewdefaultMB.Description = explain;
                    NewdefaultMB.IsHaveLineBefore = ischecked;
                    NewdefaultMB.CompanyIndustry = 0;
                    NewdefaultMB.CompanySize = size;
                    NewdefaultMB.CountryID = CountryID;
                    NewdefaultMB.IsActive = true;
                    NewdefaultMB.LanguageId = isArabic ? 2 : 1;
                    NewdefaultMB.ParenId = Parent;
                    NewdefaultMB.UserId = 1;
                    NewdefaultMB.DefaultMBId = DefMBID;
                    NewdefaultMB.Ordering = order;
                    _uow.DefaultMBRepo.Add(NewdefaultMB);

                }
                _uow.SaveChanges();
                return Json("Done", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult ChildDown(int id, int order, int parent)
        //{

        //    try
        //    {

        //        DefaultMB defaultMB = _uow.DefaultMBRepo.FindById(id);
        //        if (defaultMB == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        int countryId = defaultMB.CountryID;
        //        int user = defaultMB.UserId;
        //        int companySize = defaultMB.CompanySize;
        //        int CompanyIndustry = defaultMB.CompanyIndustry;
        //        int nextOrder = order + 1;

        //        DefaultMB NextMB = _uow.DefaultMBRepo.Search(a => a.ParenId == parent && a.Ordering == nextOrder).FirstOrDefault();
        //        string result = "Current: " + defaultMB != null ? defaultMB.Id.ToString() : "Not";
        //        result = result + "Next: " + NextMB != null ? NextMB.Id.ToString() : "Not";
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //        NextMB.Ordering = order;
        //        _uow.DefaultMBRepo.Update(NextMB);
        //        _uow.SaveChanges();
        //        defaultMB.Ordering = nextOrder;
        //        _uow.DefaultMBRepo.Update(defaultMB);
        //        _uow.SaveChanges();

        //        return Json("Done", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //[HttpPost]
        //public ActionResult ChildUp(int id, int order, int parent)
        //{

        //    try
        //    {
        //        DefaultMB defaultMB = _uow.DefaultMBRepo.FindById(id);
        //        if (defaultMB == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        int countryId = defaultMB.CountryID;
        //        int user = defaultMB.UserId;
        //        int companySize = defaultMB.CompanySize;
        //        int CompanyIndustry = defaultMB.CompanyIndustry;
        //        int nextOrder = order - 1;
        //        DefaultMB NextMB = _uow.DefaultMBRepo.Search(a => a.ParenId == parent && a.Ordering == nextOrder).FirstOrDefault();
        //        NextMB.Ordering = order;
        //        _uow.DefaultMBRepo.Update(NextMB);
        //        _uow.SaveChanges();
        //        defaultMB.Ordering = nextOrder;
        //        _uow.DefaultMBRepo.Update(defaultMB);
        //        _uow.SaveChanges();

        //        return Json("Done", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        // GET: Admin/DefaultMBs/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Admin/DefaultMBs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Ordering,ParenId,Description,Content,UserId,IsActive,LanguageId,IsHaveLineBefore,CountryID,DefaultMBId,CompanySize,CompanyIndustry")] DefaultMB defaultMB)
        {
            if (ModelState.IsValid)
            {
                db.DefaultMB.Add(defaultMB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", defaultMB.UserId);
            return View(defaultMB);
        }

        // GET: Admin/DefaultMBs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultMB defaultMB = db.DefaultMB.Find(id);
            if (defaultMB == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", defaultMB.UserId);
            return View(defaultMB);
        }

        // POST: Admin/DefaultMBs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Ordering,ParenId,Description,Content,UserId,IsActive,LanguageId,IsHaveLineBefore,CountryID,DefaultMBId,CompanySize,CompanyIndustry")] DefaultMB defaultMB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(defaultMB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", defaultMB.UserId);
            return View(defaultMB);
        }

        // GET: Admin/DefaultMBs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultMB defaultMB = db.DefaultMB.Find(id);
            if (defaultMB == null)
            {
                return HttpNotFound();
            }
            return View(defaultMB);
        }

        // POST: Admin/DefaultMBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DefaultMB defaultMB = db.DefaultMB.Find(id);
            db.DefaultMB.Remove(defaultMB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // POST: Admin/DefaultMBs/Delete/5
        [HttpPost]

        public ActionResult DeleteSection(int id, bool isParent)
        {
            try
            {
                int DeletedSections = 1;
                if (isParent)
                {
                    List<DefaultMB> defaultMBs = _uow.DefaultMBRepo.Search(a => a.ParenId == id).ToList();
                    DeletedSections = DeletedSections + defaultMBs.Count;
                    if (defaultMBs.Count > 0)
                    {
                        foreach (DefaultMB mB in defaultMBs)
                        {
                            _uow.DefaultMBRepo.Delete(mB);
                            _uow.SaveChanges();
                        }
                    }

                }
                _uow.DefaultMBRepo.Delete(_uow.DefaultMBRepo.FindById(id));
                _uow.SaveChanges();
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
                response.Add("Message", isParent ? Resources.General.DeletedParentSectionMsg : Resources.General.DeletedChildSectionMsg);
                response.Add("Deleted", DeletedSections);
                response.Add("IsParent", isParent);
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json("error: " + ex.Message);
            }
        }
        public ActionResult CopySection()
        {
            CopySections();
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
        public void CopySections()
        {
            List<DefaultMB> userSections = new List<DefaultMB>();
            int CurrentDMBSID=_uow.DefaultMBRepo.GetAll().Max(a=>a.DefaultMBId);
            List<Section> adminSections = _uow.SectionRepo.Search(a => a.UserId == 1 && a.CountryID == 182).ToList();
            //if (adminSections.Count == 0)
            //    adminSections = _uow.SectionRepo.Search(a => a.UserId == adminId && a.CountryID == 158).ToList();
            List<Section> parentAdminSections = adminSections.Where(x => x.ParenId == null).ToList();
            foreach (Section parent in parentAdminSections)
            {
                DefaultMB parentUserSection = new DefaultMB()
                {
                    Description = parent.Description,
                    Title = parent.Title,
                    Ordering = parent.Ordering,
                    UserId = 1,
                    LanguageId = parent.LanguageId,
                    IsActive = parent.IsActive,
                    Content = parent.Content,
                    CountryID = parent.CountryID,
                    DefaultMBId = ++CurrentDMBSID,
                    CompanySize=1,
                    Childs = new List<DefaultMB>(),
                };

                foreach (Section child in parent.Childs)
                {
                    DefaultMB childUserSection = new DefaultMB()
                    {
                        Description = child.Description,
                        Title = child.Title,
                        Ordering = child.Ordering,
                        UserId = 1,
                        LanguageId = parent.LanguageId,
                        IsActive = parent.IsActive,
                        Content = child.Content,
                        CountryID = child.CountryID,
                        CompanySize = 1,
                        DefaultMBId = ++CurrentDMBSID,
                    };
                    parentUserSection.Childs.Add(childUserSection);
                }
                userSections.Add(parentUserSection);
            }
            foreach (DefaultMB section in userSections)
            {
                _uow.DefaultMBRepo.Add(section);
            }
            _uow.SaveChanges();
        }
        public ActionResult CheckBuilder(int countryID, int sizeCategory)
        {
            List<DefaultMB> defaultMBs = _uow.DefaultMBRepo.Search(a => a.CountryID == countryID && a.CompanySize == sizeCategory).ToList();
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            response.Add("DefaultM", defaultMBs.Select(s => s.CountryID).Distinct().ToList().Count);
            return Json(response);
        }
        public ActionResult GetCountries()
        {
            List<DefaultMB> defaultMBs = _uow.DefaultMBRepo.GetAll();
            List<int> countriesID = defaultMBs.Select(s => s.CountryID).Distinct().ToList();
            //Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            List<Dictionary<string, dynamic>> CountriesList = new List<Dictionary<string, dynamic>>();
            foreach (var c in countriesID)
            {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
                Country country = _uow.CountryRepo.FindById(c);
                response.Add("CountryName", isArabic ? country.NameAr : country.Name);
                response.Add("CountryID", country.Id);
                CountriesList.Add(response);
            }
            return Json(CountriesList);
        }
        public ActionResult GetCompaniesSizes(int CountryID)
        {
            List<DefaultMB> defaultMBs = _uow.DefaultMBRepo.Search(a => a.CountryID == CountryID).ToList();
            List<int> companiesSize = defaultMBs.Select(s => s.CompanySize).Distinct().ToList();
            //Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            List<Dictionary<string, dynamic>> companiesSizeList = new List<Dictionary<string, dynamic>>();
            foreach (var c in companiesSize)
            {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
                var optionText = "";
                if (isArabic)
                {
                    NumberOfEmployeesAr numberOfEmployeesAr = new NumberOfEmployeesAr();
                    optionText = numberOfEmployeesAr.EmployeesAr[c];
                }
                else
                {
                    NumberOfEmployees numberOfEmployees = new NumberOfEmployees();
                    optionText = numberOfEmployees.Employees[c];
                }
                response.Add("optionText", optionText);
                response.Add("optionID", c);
                companiesSizeList.Add(response);

            }
            return Json(companiesSizeList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RenderDefaultM(int NumberofEmpToShowValue, int countryToShowValue)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            //return "sizeCategory: " + NumberofEmpToShowValue.ToString()+" || "+ countryToShowValue.ToString();
            List<Country> countries = isArabic ? _uow.CountryRepo.GetAll().OrderBy(a => a.NameAr).ToList() : _uow.CountryRepo.GetAll().OrderBy(a => a.Name).ToList();
            int langId = isArabic ? 2 : 1;
            List<DefaultMB> _defaultMBs = _uow.DefaultMBRepo.GetAll().Where(a => a.CountryID == countryToShowValue && a.CompanySize==NumberofEmpToShowValue && a.LanguageId == langId).OrderBy(x => x.Ordering).ToList();
            List<DefaultMB> _defaultParentMBs = _uow.DefaultMBRepo.GetAll().Where(a => a.CountryID == countryToShowValue && a.LanguageId == langId && a.ParenId == null && a.CompanySize == NumberofEmpToShowValue).OrderBy(x => x.Ordering).ToList();
            DefaultMBViewModel defaultMBViewModel = new DefaultMBViewModel()
            {
                Countries = countries,
                DefaultMB = _defaultMBs,
                DefaultMB1 = _defaultParentMBs,
                ParentConter = _defaultParentMBs.Count,
                CurrentCountry = countryToShowValue,
                CurrentCompanySize = NumberofEmpToShowValue

            };
            return View("Index", defaultMBViewModel);
        }
        public ActionResult CpoyDefaultM(int NumberofEmpCopyFrom, int countryCopyFrom, int NumberofEmpNew, int countryNew)
        {
            try
            {
                int currentID = _uow.DefaultMBRepo.GetAll().Max(a => a.DefaultMBId);
                List<DefaultMB> _defaultParentMBs = _uow.DefaultMBRepo.GetAll().Where(a => a.CountryID == countryCopyFrom && a.CompanySize == NumberofEmpCopyFrom).OrderBy(x => x.Ordering).ToList();
                List<DefaultMB> _NewdefaultParentMBs = new List<DefaultMB>();
                foreach (DefaultMB Parent in _defaultParentMBs.Where(a => a.ParenId == null))
                {
                    DefaultMB defaultMBParent = new DefaultMB()
                    {
                        CountryID = countryNew,
                        CompanySize = NumberofEmpNew,
                        CompanyIndustry = Parent.CompanyIndustry,
                        Content = Parent.Content,
                        Description = Parent.Description,
                        IsActive = Parent.IsActive,
                        IsHaveLineBefore = Parent.IsHaveLineBefore,
                        LanguageId = Parent.LanguageId,
                        Ordering = Parent.Ordering,
                        Title = Parent.Title,
                        UserId = Parent.UserId,
                        DefaultMBId = ++currentID,
                        Childs = new List<DefaultMB>(),
                    };
                    foreach (DefaultMB Child in Parent.Childs)
                    {
                        DefaultMB newChild = new DefaultMB()
                        {
                            CountryID = countryNew,
                            CompanySize = NumberofEmpNew,
                            CompanyIndustry = Child.CompanyIndustry,
                            Content = Child.Content,
                            Description = Child.Description,
                            IsActive = Child.IsActive,
                            IsHaveLineBefore = Child.IsHaveLineBefore,
                            LanguageId = Child.LanguageId,
                            Ordering = Child.Ordering,
                            Title = Child.Title,
                            UserId = Child.UserId,
                            DefaultMBId = ++currentID,
                        };
                        defaultMBParent.Childs.Add(newChild);
                    }
                    _NewdefaultParentMBs.Add(defaultMBParent);
                }
                foreach(DefaultMB dMB in _NewdefaultParentMBs)
                {
                    _uow.DefaultMBRepo.Add(dMB);
                }
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend: " + ex.Message);
            }
        }

    }
}
