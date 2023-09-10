using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HtmlAgilityPack;
using HumanResourceHelth.DataAccess;
using HumanResourceHelth.DataAccess.Migrations;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Data;
using iTextSharp.tool.xml.html;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

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
                string contactPerson = "", contactEmail = "", contactPhone = "", organizationName = "", existingHRMPFileName = "", fullPath = "" ,servicePlan="";

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
                servicePlan = serviceId==2? " Legal Review " : " Customized HR Policy Manual ";
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
                    fullPath = "https://www.hrfactoryapp.com/Uploads/ExistingHRMP/" + existingHRMPFileName;
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
                MailMessage msgs = new MailMessage();
                msgs.To.Add(contactEmail);
                email email = new email();
                string  EmailMsg= "<br>Dear " + contactPerson + ",<br/><br/>Thank you for your interest in HR FACTORY. We will contact you shortly to discuss your requirements related to" + servicePlan + " plan.<br/><br/>Best regards,<br/>HR FACTORY Team"; ;
                MailAddress address = new MailAddress("care@hrfactoryapp.com");
                msgs.From = address;
                msgs.Subject = "Welcome to HRFactoryApp";
                msgs.Body = email.EmailHeader + EmailMsg + email.EmailBody;
                msgs.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                //client.Host = "smtpout.asia.secureserver.net";
                client.Host = "smtp.gmail.com";
                //client.Port = 80;
                client.Port = 587;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                //client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "HRFA@pp(@2023)");
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "vymtkpdcvkcwwzmm");
                //Send the msgs  
                //Send the msgs  
                client.Send(msgs);
                msgs = new MailMessage();

                if (serviceId == 2)
                {
                    msgs.To.Add("info@saifalrawahi.com");
                    msgs.CC.Add("muath@extramiles-om.com");
                }
                else
                {
                    msgs.To.Add("muath@extramiles-om.com");
                }
                msgs.CC.Add("nabahan@extramiles-om.com");
                msgs.CC.Add("wahb@hrfactoryapp.com");
                address = new MailAddress("care@hrfactoryapp.com");
                msgs.From = address;
                msgs.Subject = "New Service Request";
                if (existingHRMP == 1)
                    EmailMsg = "Dear HRFactoryApp Team,<br/><br/>You have a new service request (<b>"+ servicePlan + "plan</b>) from: <b>" + organizationName + " </b> they have an Existing HR Policy <a href='" + fullPath + "'> Click here to download it</a> ." +
                    "<br> You can contact them through their contact person: <b>"+ contactPerson + "</b> using the submited email: <b>"+contactEmail+ "</b> or to phone number: <b>" +contactPhone+"</b>"+
                    "<br/><br/>Best regards,<br/>HR FACTORY Team";
                else
                    EmailMsg = "Dear HRFactoryApp Team,<br/><br/>You have a new service request (<b>" + servicePlan + "plan</b>) from: <b>" + organizationName + "</b>. " +
                    "<br> You can contact them through their contact person: <b>" + contactPerson + "</b> using the submited email: <b>" + contactEmail + "</b> or to phone number: <b>" + contactPhone + "</b>" +
                    "<br/><br/>Best regards,<br/>HR FACTORY Team";
                msgs.Body = email.EmailHeader + EmailMsg + email.EmailBody;
                msgs.IsBodyHtml = true;
                client = new SmtpClient();
                //client.Host = "smtpout.asia.secureserver.net";
                client.Host = "smtp.gmail.com";
                //client.Port = 80;
                client.Port = 587;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "vymtkpdcvkcwwzmm");
                //Send the msgs  
                client.Send(msgs);
                return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Your message failed to send!<br> " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

