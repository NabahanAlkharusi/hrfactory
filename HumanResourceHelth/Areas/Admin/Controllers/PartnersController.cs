using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using Microsoft.AspNet.Identity;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class PartnersController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();

        // GET: Admin/Partners
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View(db.Partners.ToList());
        }

        // GET: Admin/Partners/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = db.Partners.Find(id);
            if (partners == null)
            {
                return HttpNotFound();
            }
            return View(partners);
        }

        // GET: Admin/Partners/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            return View();
        }

        // POST: Admin/Partners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PartnerName,PartnerNameAr,PartnerofficalEmail,PartnerofficalPhone,PartnerCountry,PartnerAddress,PartnerFocalPointName,PartnerFocalPointNameAr,PartnerFocalPointEmail,PartnerFocalPointPhone,PartnerSubDomain,status")] Partners partners)
        {
            //return "company size: " + Request.Form["PartnerSize"] + " Partner Industry: " + Request.Form["Industry"];
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                if (Request.Files["PartnerLogo"].ContentLength <= 0)
                {
                    ModelState.AddModelError("PartnerLogo", "Please Upload Partner Logo");
                    if (Request.Files["PartnerBanner"].ContentLength <= 0)
                        ModelState.AddModelError("PartnerBanner", "Please Upload Partner Banner");
                    else
                        ModelState.AddModelError("PartnerBanner", "Partner Banner Could be losted please reselect the file again");
                    return View(partners);
                }
                else if (Request.Files["PartnerBanner"].ContentLength <= 0)
                {
                    ModelState.AddModelError("PartnerBanner", "Please Upload Partner Banner");
                    ModelState.AddModelError("PartnerLogo", "Partner Logo Could be losted please reselect the file again");

                    return View(partners);
                }
                else
                {

                    string fileName = Path.GetFileName(Request.Files["PartnerLogo"].FileName);
                    string fileName2 = Path.GetFileName(Request.Files["PartnerBanner"].FileName);
                    string path = Server.MapPath("~/Areas/PartnersFiles/");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    Request.Files["PartnerLogo"].SaveAs(path + fileName);
                    Request.Files["PartnerBanner"].SaveAs(path + fileName2);
                    partners.PartnerLogo = "/Areas/PartnersFiles/" + fileName;
                    partners.PartnerBanner = "/Areas/PartnersFiles/" + fileName2;


                }
                User myUser = new User();
                myUser.Email = partners.PartnerofficalEmail;
                myUser.Name = partners.PartnerName;
                myUser.Password = "123456";
                myUser.NameAr = partners.PartnerNameAr;
                myUser.ContactInformation = partners.PartnerFocalPointPhone;
                myUser.ContactPerson = partners.PartnerFocalPointName;
                myUser.IsAdmin = false;
                myUser.IsDeleted = false;
                myUser.DocumentName = null;
                myUser.DocumentNameAr = null;
                myUser.ThawaniKey = null;
                myUser.CountryId = partners.PartnerCountry;
                myUser.IndustryId = Convert.ToInt32(Request.Form["Industry"]);
                myUser.NumberOFEmployees = Convert.ToInt32(Request.Form["PartnerSize"]);
                myUser.LastLoginDate = DateTime.Now;
                db.Users.Add(myUser);
                db.SaveChanges();
                partners.UserID = myUser.Id;
                db.Partners.Add(partners);
                db.SaveChanges();
                SendMail(myUser.Email, myUser.Name, myUser.NameAr);
                return RedirectToAction("Index");
            }

            return View(partners);
        }

        // GET: Admin/Partners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = db.Partners.Find(id);
            if (partners == null)
            {
                return HttpNotFound();
            }
            return View(partners);
        }

        // POST: Admin/Partners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PartnerName,PartnerNameAr,PartnerofficalEmail,PartnerofficalPhone,PartnerCountry,PartnerAddress,PartnerFocalPointName,PartnerFocalPointNameAr,PartnerFocalPointEmail,PartnerFocalPointPhone,PartnerSubDomain,status")] Partners partners)
        {
            //return "logo: " + Request.Form["PartnerLogo"] + " banner:" + Request.Form["PartnerBanner"];
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Areas/PartnersFiles/");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                if (Request.Files["EPartnerLogo"].ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Request.Files["EPartnerLogo"].FileName);
                    Request.Files["EPartnerLogo"].SaveAs(path + fileName);
                    partners.PartnerLogo = "/Areas/PartnersFiles/" + fileName;
                }
                else
                {
                    partners.PartnerLogo = Request.Form["PartnerLogo"];
                }
                if (Request.Files["EPartnerBanner"].ContentLength > 0)
                {
                    string fileName2 = Path.GetFileName(Request.Files["EPartnerBanner"].FileName);
                    Request.Files["EPartnerBanner"].SaveAs(path + fileName2);
                    partners.PartnerBanner = "/Areas/PartnersFiles/" + fileName2;
                }
                else
                {
                    partners.PartnerBanner = Request.Form["PartnerBanner"];
                }
                db.Entry(partners).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partners);
        }

        // GET: Admin/Partners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partners partners = db.Partners.Find(id);
            if (partners == null)
            {
                return HttpNotFound();
            }
            return View(partners);
        }

        // POST: Admin/Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            Partners partners = db.Partners.Find(id);
            db.Partners.Remove(partners);
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
        public void SendMail(string userEmail, string en_name, string ar_name)
        {
            try
            {

                MailMessage msgs = new MailMessage();
                msgs.To.Add(userEmail);
                MailAddress address = new MailAddress("care@hrfactoryapp.com");
                msgs.From = address;
                msgs.Subject = "Welcome to HR FACTORY";
                string htmlBody = "<h4>Dear " + en_name + "</h4><br>Greetings from HR Factory.<br><br>"
                                    + "Please find below videos that explain the concept of the HR platform(they are in Arabic).<br>"
                                    + "<a href='https://www.youtube.com/watch?v=Q31WLrcsARg'> https://www.youtube.com/watch?v=Q31WLrcsARg </a><br>"
                                    + "<a href='https://www.youtube.com/watch?v=Q8sn6AyrOKQ'> https://www.youtube.com/watch?v=Q8sn6AyrOKQ </a><br><br><br>"
                                    + "As noted, you may kickstart your journey by:<ul><li> Taking the free online HR health check.</li>"
                                    + "<li> Learning HR online.</li><li> Enjoying  the HR Startup plan : to enjoy the use of HR resources.  <a href='https://hrfactoryapp.com' > HR FACTORY(hrfactoryapp.com) </a></li>"
                                    + "<li> Enjoying to the HR manual builder plan: to enjoy a user-friendly and dynamic builder of HR policy manual and receive updates. <a href='https://hrfactoryapp.com'> HR FACTORY(hrfactoryapp.com) <a></li>"
                                    + "<li> Subscribing to the HR Doctor plan to provide you with HR mentoring and advisory support.</li></ul><br><br>"
                                    + "Looking forward and we are happy to be part of your success journey.<br><br><br>"
                                    + "<div dir='rtl'><h4>أهلا بك " + ar_name + " في منصة HR Factory </h4><br><br><br>"
                                    + "نرحب بك و نتمنى لك تحقيق الاستفادة القصوى من المنصة.<br><br><br>"
                                    + "يمكنك الآن الاستفادة من التالي:<br><br><ul>"
                                    + "<li> عمل تشخيص مجاني إلكتروني. (إن أردت حفظ التقارير يمكنك التسجيل في باقة الانطلاق والقيام بالتشخيص بشكل مستمر أثناء فترة الاشتراك)"
                                    + "<li>دورات عن بعد في الموارد البشرية.</li>"
                                    + "<li>الاستفادة من باقة الانطلاق والاستمتاع بالتالي:"
                                    + "<ul><li> باقة مدفوعة تهدف إلى تزويد رواد الأعمال بأهم الأدوات والممارسات الأساسية بشكل أعمق وأشمل في HR.</li>"
                                    + "<li> احصل على همسات ونصائح دورية في HR. </li>"
                                    + "<li> عمل تشخيص مستمر وغير محدود لمستوى ممارسات الموارد البشرية.</li></ul></ul><br><br><br>"
                                    + "<a href='https://www.youtube.com/watch?v=Ft7WVAuQ3T0' > https://www.youtube.com/watch?v=Ft7WVAuQ3T0 </a><br><br><br>"
                                    + "الاستفادة في باقة صانع لائحة سياسات العمل والحصول على نموذج ديناميكي يتيح لك إنشاء لائحة نظام العمل الخاصة بك.<br><br>"
                                    + "<a href='hrfactoryapp.com' > HR FACTORY(hrfactoryapp.com) </a></div>"
                                    + "<img src='https://www.hrfactoryapp.com/Images/emailfooter.jpg'/>";
                msgs.Body = htmlBody;
                msgs.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                //client.Host = "smtpout.asia.secureserver.net";
                client.Host = "relay-hosting.secureserver.net";
                //client.Port = 80;
                client.Port = 25;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.EnableSsl = false;
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "Wahb@1985");
                //Send the msgs  
                client.Send(msgs);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception er)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest, er.Message);
            }



        }

    }
}
