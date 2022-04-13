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
using Newtonsoft.Json;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class UpdatesController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();
        private UnitOfWork _uow = new UnitOfWork();
        private static bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
        private List<string> folders = new List<string>();
        private List<int> subfolders = new List<int>();
        private List<Tuple<List<string>, List<int>>> tuples = new List<Tuple<List<string>, List<int>>>();

        public object CountryId { get; private set; }

        // GET: Admin/Updates
        public ActionResult Index()
        {
            return View(db.Updates.ToList());
        }

        // GET: Admin/Updates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Updates updates = db.Updates.Find(id);
            if (updates == null)
            {
                return HttpNotFound();
            }
            return View(updates);
        }

        // GET: Admin/Updates/Create
        public ActionResult Create()
        {
            Getcountries();
            return View();
        }

        private void Getcountries()
        {
            List<Country> countries = _uow.CountryRepo.GetAll().OrderByDescending(a => a.IsArabCountry).ToList();
            List<SelectListItem> TermsDomain = new List<SelectListItem>();
            TermsDomain.Add(new SelectListItem() { Text = HumanResourceHelth.Model.Resources.AppResource.TargetGroupSubStatAll, Value = "0" });
            foreach (Country country in countries)
            {
                TermsDomain.Add(new SelectListItem() { Text = isArabic ? country.NameAr : country.Name, Value = country.Id.ToString() });
            }

            ViewBag.Countries = new SelectList(TermsDomain, "Value", "Text");
        }

        // POST: Admin/Updates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,ArabicTitle,EnglisTitle,ArabicText,EnglisText")] Updates updates)
        {
            Getcountries();
            var SubscriptionStat = Request["inlineRadioOptions"];
            var Plan = int.Parse(Request["palns"]);
            int Country = int.Parse(Request["Country"]);
            List<User> users = _uow.UserRepo.Search(a => a.Id == 1171).ToList();
            //List<User> users = _uow.UserRepo.GetAll();
            List<UserPlan> userPlans = _uow.UserPlanRepo.GetAll();
            List<User> users2 = new List<User>();
            List<User> users3 = new List<User>();

            if (Country != 0)
                users2 = users.Where(a => a.CountryId == Country && !a.IsAdmin).ToList();
            else
                users2 = users;
            if (Plan != 0)
            {
                if (SubscriptionStat != "All")
                {
                    userPlans = userPlans.Where(z => z.PlanId == Plan && z.IsActive).ToList();
                }
                else
                {
                    userPlans = userPlans.Where(z => z.PlanId == Plan).ToList();
                }
                users3 = users2.Join(userPlans, Uinfo => Uinfo.Id, Up => Up.UserId, (Uinfo, Up) => new User
                {
                    Id = Uinfo.Id,
                    Email = Uinfo.Email,
                    NameAr = Uinfo.NameAr,
                    Name = Uinfo.Name
                }).ToList();
                users2 = users3;
            }
            users = users2;

            //List<int> UsersId = _uow.UserPlanRepo.Search(a => a.PlanId == 1).Select(z => z.Id).ToList();
            if (ModelState.IsValid)
            {
                updates.IssuedDate = DateTime.Now;
                db.Updates.Add(updates);
                db.SaveChanges();
                int UpdateId = updates.Id;
                foreach (User user in users)
                {
                    List<int> unreadlist = new List<int>();
                    SemiNotifications notifications = _uow.SemiNotificationRepo.Search(a => a.UserId == user.Id).FirstOrDefault();
                    if (notifications != null)
                    {
                        string unread = notifications.UnReadNotifi;
                        unreadlist = JsonConvert.DeserializeObject<List<int>>(unread);
                        unreadlist.Add(UpdateId);
                        notifications.UnReadNotifi = JsonConvert.SerializeObject(unreadlist);
                        _uow.SemiNotificationRepo.Update(notifications);
                        _uow.SaveChanges();
                    }
                    else
                    {
                        unreadlist.Add(UpdateId);
                        SemiNotifications semi = new SemiNotifications();
                        semi.UserId = user.Id;
                        semi.UnReadNotifi = JsonConvert.SerializeObject(unreadlist);
                        _uow.SemiNotificationRepo.Add(semi);
                        _uow.SaveChanges();
                    }
                    string header = "<div style='display:block;text-align:center;min-height: 206px;background-color: #f7f7f7!important;'>"
                    + "<img align='center' alt='' src='https://ci6.googleusercontent.com/proxy/xHSe0qjGm62SNldD4-ckoRX36jQkG9r2A3X_FRZlCiZvBSBtU-Ddzdt8aY0BfoFpOFS6TAMGiZxM6NjSWjOfFMyqnDEElecJtl3uVy8tx7fZ84fZGGMUqH48CLyFpRrO0duGzHCxfT0BeN4WlOWQifSXGnPicw=s0-d-e1-ft#https://mcusercontent.com/a8f1f5e51eea26a2e904db148/images/a28aaab1-0be6-5259-1e88-1147d3d97605.jpg' width='196' style='max-width:196px;padding-bottom:0;display:inline!important;vertical-align:bottom;border:0;height:auto;outline:none;text-decoration:none;margin-top:51px' class='m_-140627277912390187mcnImage CToWUd'/></div>";
                    string ar_html = "<div style='width: 50%; padding:0; display:block; float:right !important;text-align:center !important'>" +
                            "<div style='text-align:right;display:block' dir='rtl'>" +
                            "<h4>الفاضل/الأفاضل: " + user.NameAr + " </h4>" +
                            updates.ArabicText +
                            "</div></div>";
                    string en_html = "<div style='width: 50%; padding:0; display:block;float:left !important;text-align:center !important'>" +
                        "<div style='text-align:left;display:block'  dir='ltr'>" +
                        "<h4>Dear: " + user.Name + "</h4>" +
                        updates.EnglisText +
                        "</div></div>";
                    string footer = "<div style='display:block;background-color:#333333;border-top:0;border-bottom:0;margin-top: 450px; padding-top:45px;padding-bottom:45px;text-align:center'><div style='display:block; margin: 0 -10px;text-align:center; color:#fff'><a href='#' target='_blank' style='margin: 20px;'>"
                        + "<img src='https://ci6.googleusercontent.com/proxy/iZE-48sXvszGHh6MUoqCYHnlP8ohfGJI6V1fj23YRaJHEaKjOb2V7stez03tl97kcCY9ebD52HlFfqGKcTQbPlQaysAL26ZKjUSa5NGX7CU3WUodCbzb-vFMkIXxvIREY4PT879oIw=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-facebook-48.png' alt='Facebook' height='24' width='24' style='text-decoration:none;' />" +
                        "</a><a href='#' target='_blank'><img src='https://ci4.googleusercontent.com/proxy/GvgjS4VPlhMl8idO5upbHzEV4AqTNut4mbrm7tN9t-0Y_Os_vvAtMqPaBL6LxSdD50M0_WvdYOaRkeRE25HbR815TslhhzsjoZMzzpKYLiG8MFqu6VDbzkb2JbyH4IjCPWiYy3cT=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-twitter-48.png' alt='Twitter' height='24' width='24' style='text-decoration:none;' />" +
                        "</a><a href='#' target='_blank' style='margin: 20px;'><img src='https://ci5.googleusercontent.com/proxy/Ihh9hEwk_36d3lzL_tLmGaqmGhc-dLqZP-II9LpKgUDCh37Kvw1N4-DJsrxuyAA9V1NNx3975BQO5w7DNVWvFHpPM4gkDm8eMVCLYy_PtGWEZAxMuaULgOR-6W0K_1sgXOcwNMtgGVE=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-instagram-48.png' alt='Link' style='text-decoration:none;'" +
                        " height='24' width='24' /></a></div><hr style='min-width:100%;border-top:2px solid #505050;border-collapse:collapse' /><div style='padding-top:0;padding-right:18px;padding-bottom:9px;padding-left:18px;word-break:break-word;color:#ffffff;font-family:Helvetica;font-size:12px;line-height:150%;text-align:center;background-color:#333333'><em>Copyright © 2021 HR Factory, All rights reserved." +
                        "</em><br>You are receiving this email because you opted in via our website.<br><br><strong>Our mailing address is:</strong><br><div><span>HR Factory</span><div><div>MUSCAT,AL-SEEB STREET99</div><span>AL-SEEB</span><span> 121</span><div>Oman</div></div></div></div></div>";

                    string body = header + "<div style='margin: 0 -10px; background-color:#fffff; display:block'>"
                        + en_html + ar_html + "</div>" + footer;
                    string subject = updates.EnglisTitle + "/" + updates.ArabicTitle;
                    SendMail(user.Email, body, subject);
                }
                return RedirectToAction("Index");
            }

            return View(updates);
        }

        // GET: Admin/Updates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Updates updates = db.Updates.Find(id);
            if (updates == null)
            {
                return HttpNotFound();
            }
            return View(updates);
        }

        // POST: Admin/Updates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArabicTitle,EnglisTitle,ArabicText,EnglisText,IssuedDate")] Updates updates)
        {
            if (ModelState.IsValid)
            {
                db.Entry(updates).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updates);
        }

        // GET: Admin/Updates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Updates updates = db.Updates.Find(id);
            if (updates == null)
            {
                return HttpNotFound();
            }
            return View(updates);
        }

        // POST: Admin/Updates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Updates updates = db.Updates.Find(id);
            db.Updates.Remove(updates);
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
        public ActionResult GetDirectories()
        {
            getFolders(Server.MapPath("/"));
            foreach (string file in Directory.GetFiles(Server.MapPath("/")))
            {
                if (ImageExtensions.Contains(Path.GetExtension(file).ToUpperInvariant()))
                {
                    folders.Add(Server.MapPath("/"));

                    break;
                }
            }
            return Json(folders, JsonRequestBehavior.AllowGet);
        }
        private void getFolders(string path)
        {
            var directories = Directory.GetDirectories(path);
            foreach (string k in directories)
            {
                if (Directory.GetDirectories(k).Length > 0)
                    getFolders(k);

                foreach (string f in Directory.GetFiles(k))
                {
                    if (ImageExtensions.Contains(Path.GetExtension(f).ToUpperInvariant()))
                    {
                        folders.Add(k);

                        break;
                    }

                }
            }

        }
        public ActionResult GetImages()
        {
            List<string> images = new List<string>();
            var folder = Request["folderName"];
            foreach (string f in Directory.GetFiles(Server.MapPath(folder)))
            {
                if (ImageExtensions.Contains(Path.GetExtension(f).ToUpperInvariant()))
                {
                    images.Add(Path.GetFullPath(f));
                }
            }
            return Json(images, JsonRequestBehavior.AllowGet);
        }
        public void SendMail(string userEmail, string body, string subject)
        {
            try
            {

                MailMessage msgs = new MailMessage();
                msgs.To.Add(userEmail);
                MailAddress address = new MailAddress("care@hrfactoryapp.com");
                msgs.From = address;
                msgs.Subject = subject;
                msgs.Body = body;
                msgs.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = "relay-hosting.secureserver.net";
                //client.Host = "smtpout.asia.secureserver.net";
                client.Port = 25;
               // client.Port = 80;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.EnableSsl = false;
                client.Credentials = new NetworkCredential("care@hrfactoryapp.com", "Wahb@1985");
                //Send the msgs  
                client.Send(msgs);
                ViewBag.Status = "Email Sent Successfully.";
                Session["Successfull"] = "Email Sent Successfully.";
            }
            catch (Exception er)
            {
                Session["Successfull"] = er.Message + "<br> " + er.StackTrace;
                //new HttpStatusCodeResult(HttpStatusCode.BadRequest, er.Message);
            }



        }
        [ValidateInput(false)]
        public ActionResult SendTestEmail(string email, string ArabicText, string EnglisText, string ArabicTitle, string EnglisTitle)
        {
            try
            {
                string header = "<div style='display:block;text-align:center;min-height: 206px;background-color: #f7f7f7!important;'>"
                    + "<img align='center' alt='' src='https://ci6.googleusercontent.com/proxy/xHSe0qjGm62SNldD4-ckoRX36jQkG9r2A3X_FRZlCiZvBSBtU-Ddzdt8aY0BfoFpOFS6TAMGiZxM6NjSWjOfFMyqnDEElecJtl3uVy8tx7fZ84fZGGMUqH48CLyFpRrO0duGzHCxfT0BeN4WlOWQifSXGnPicw=s0-d-e1-ft#https://mcusercontent.com/a8f1f5e51eea26a2e904db148/images/a28aaab1-0be6-5259-1e88-1147d3d97605.jpg' width='196' style='max-width:196px;padding-bottom:0;display:inline!important;vertical-align:bottom;border:0;height:auto;outline:none;text-decoration:none;margin-top:51px' class='m_-140627277912390187mcnImage CToWUd'/></div>";
                string ar_html = "<div style='width: 50%; padding:0; display:block; float:right !important;text-align:center !important'>" +
                        "<div style='text-align:right;display:block' dir='rtl'>" +
                        "<h4>الفاضل/الأفاضل: تجربة </h4>" +
                        ArabicText +
                        "</div></div>";
                string en_html = "<div style='width: 50%; padding:0; display:block;float:left !important;text-align:center !important'>" +
                    "<div style='text-align:left;display:block'  dir='ltr'>" +
                    "<h4>Dear: TEST</h4>" +
                    EnglisText +
                    "</div></div>";
                string footer = "<div style='display:block;background-color:#333333;border-top:0;border-bottom:0;margin-top: 450px; padding-top:45px;padding-bottom:45px;text-align:center'><div style='display:block; margin: 0 -10px;text-align:center; color:#fff'><a href='#' target='_blank' style='margin: 20px;'>"
                    + "<img src='https://ci6.googleusercontent.com/proxy/iZE-48sXvszGHh6MUoqCYHnlP8ohfGJI6V1fj23YRaJHEaKjOb2V7stez03tl97kcCY9ebD52HlFfqGKcTQbPlQaysAL26ZKjUSa5NGX7CU3WUodCbzb-vFMkIXxvIREY4PT879oIw=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-facebook-48.png' alt='Facebook' height='24' width='24' style='text-decoration:none;' />" +
                    "</a><a href='#' target='_blank'><img src='https://ci4.googleusercontent.com/proxy/GvgjS4VPlhMl8idO5upbHzEV4AqTNut4mbrm7tN9t-0Y_Os_vvAtMqPaBL6LxSdD50M0_WvdYOaRkeRE25HbR815TslhhzsjoZMzzpKYLiG8MFqu6VDbzkb2JbyH4IjCPWiYy3cT=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-twitter-48.png' alt='Twitter' height='24' width='24' style='text-decoration:none;' />" +
                    "</a><a href='#' target='_blank' style='margin: 20px;'><img src='https://ci5.googleusercontent.com/proxy/Ihh9hEwk_36d3lzL_tLmGaqmGhc-dLqZP-II9LpKgUDCh37Kvw1N4-DJsrxuyAA9V1NNx3975BQO5w7DNVWvFHpPM4gkDm8eMVCLYy_PtGWEZAxMuaULgOR-6W0K_1sgXOcwNMtgGVE=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-instagram-48.png' alt='Link' style='text-decoration:none;'" +
                    " height='24' width='24' /></a></div><hr style='min-width:100%;border-top:2px solid #505050;border-collapse:collapse' /><div style='padding-top:0;padding-right:18px;padding-bottom:9px;padding-left:18px;word-break:break-word;color:#ffffff;font-family:Helvetica;font-size:12px;line-height:150%;text-align:center;background-color:#333333'><em>Copyright © 2021 HR Factory, All rights reserved." +
                    "</em><br>You are receiving this email because you opted in via our website.<br><br><strong>Our mailing address is:</strong><br><div><span>HR Factory</span><div><div>MUSCAT,AL-SEEB STREET99</div><span>AL-SEEB</span><span> 121</span><div>Oman</div></div></div></div></div>";

                string body = header + "<div style='margin: 0 -10px; background-color:#fffff; display:block'>"
                    + en_html + ar_html + "</div>" + footer;
                string subject = EnglisTitle + "/" + ArabicTitle;
                SendMail(email, body, subject);
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
