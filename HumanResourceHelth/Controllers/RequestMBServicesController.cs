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

namespace HumanResourceHelth.Web.Controllers
{
    public class RequestMBServicesController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();
        private UnitOfWork _uow = new UnitOfWork();

        // GET: RequestMBServices
        public ActionResult Index()
        {
            return View(db.RequestMBService.ToList());
        }

        // GET: RequestMBServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestMBService requestMBService = db.RequestMBService.Find(id);
            if (requestMBService == null)
            {
                return HttpNotFound();
            }
            return View(requestMBService);
        }

        // GET: RequestMBServices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestMBServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserID,ServiceID,OrganizationName,NumberOFEmployees,IndustryId,CountryId,ContactPerson,ContactPEmail,ContactPersonPhone,HaveExistingHRMP,ExistingHRMPPath")] RequestMBService requestMBService)
        {
            if (ModelState.IsValid)
            {
                db.RequestMBService.Add(requestMBService);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestMBService);
        }

        // GET: RequestMBServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestMBService requestMBService = db.RequestMBService.Find(id);
            if (requestMBService == null)
            {
                return HttpNotFound();
            }
            return View(requestMBService);
        }

        // POST: RequestMBServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserID,ServiceID,OrganizationName,NumberOFEmployees,IndustryId,CountryId,ContactPerson,ContactPEmail,ContactPersonPhone,HaveExistingHRMP,ExistingHRMPPath")] RequestMBService requestMBService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestMBService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestMBService);
        }

        // GET: RequestMBServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestMBService requestMBService = db.RequestMBService.Find(id);
            if (requestMBService == null)
            {
                return HttpNotFound();
            }
            return View(requestMBService);
        }

        // POST: RequestMBServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestMBService requestMBService = db.RequestMBService.Find(id);
            db.RequestMBService.Remove(requestMBService);
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
        //create function that recive request and return json
        [HttpPost]
        public JsonResult anythingtosave()
        {
            try
            {
                int countryId = 0, industryId = 0, employeesNumber = 0, serviceId = 0, existingHRMP = 0;
                string contactPerson = "", contactEmail = "", contactPhone = "", organizationName = "", existingHRMPFileName = "";

                //get user id from session
                int userId = Convert.ToInt32(Request.Form["userID"]);

                //get country and convert it into int if it's not empty
                countryId = Request.Form["Country"] == "" ? 0 : Convert.ToInt32(Request.Form["Country"]);
                //get industry and convert it into int if it's not empty
                industryId = Request.Form["industry"] == "" ? 0 : Convert.ToInt32(Request.Form["industry"]);
                //get employees number and convert it into int if it's not empty
                employeesNumber = Request.Form["employees"] == "" ? 0 : Convert.ToInt32(Request.Form["employees"]);
                //get contact person
                contactPerson = Request.Form["contact_name"];
                //get contact email
                contactEmail = Request.Form["contact_email"];
                //get contact phone
                contactPhone = Request.Form["contact_phone"];
                //get organization name
                organizationName = Request.Form["company_name"];
                // get service id
                serviceId = Convert.ToInt32(Request.Form["serviceId"]);
                //get existing HRMP and convert it into int if it's not empty
                existingHRMP = Request.Form["isFileExistVal"] == "" ? 0 : Convert.ToInt32(Request.Form["isFileExistVal"]);
                if (existingHRMP == 1)
                {
                    //get existing HRMP file
                    var existingHRMPFile = Request.Files[0];
                    //save file
                    existingHRMPFile.SaveAs(Server.MapPath("~/Uploads/ExistingHRMP/" + organizationName + "_" + existingHRMPFile.FileName));
                    //get file name
                    existingHRMPFileName = organizationName + "_" + existingHRMPFile.FileName;
                }
                //create new object from RequestMBService
                RequestMBService requestMBService = new RequestMBService();
                //set values
                requestMBService.UserID = userId;
                requestMBService.ServiceID = serviceId;
                requestMBService.OrganizationName = organizationName;
                requestMBService.NumberOFEmployees = employeesNumber;
                requestMBService.IndustryId = industryId;
                requestMBService.CountryId = countryId;
                requestMBService.ContactPerson = contactPerson;
                requestMBService.ContactPEmail = contactEmail;
                requestMBService.ContactPersonPhone = contactPhone;
                requestMBService.HaveExistingHRMP = existingHRMP == 0 ? false : true;
                requestMBService.ExistingHRMPPath = existingHRMPFileName;
                //add object to database
                db.RequestMBService.Add(requestMBService);
                //save changes
                db.SaveChanges();
                //return json
                return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
            
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Your message failed to send!<br> "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}

