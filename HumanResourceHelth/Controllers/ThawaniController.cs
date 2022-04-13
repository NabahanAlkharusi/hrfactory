using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.WebHooks;
using System.Web.Http;
using HumanResourceHelth.Web.Data;
using System.Linq;

namespace HumanResourceHelth.Web.Controllers
{
    public class ThawaniController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        bool isArabic = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        public ActionResult Index()
        {
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);
            Plan plan = _uow.PlanRepo.FindById(int.Parse(HttpContext.Request.QueryString["planId"]));
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://checkout.thawani.om/api/v1/checkout/session");
            var request = new RestRequest(Method.POST);
            request.AddHeader("thawani-api-key", "8AoY3m4ahzYGuWtSfDW8YUa736DZvJ");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined",
                "{\n\t\"client_reference_id\": \""
                + Session["UserId"] + "\",\n\t\"products\": [\n\t\t{\n\t\t\t\"name\": \""
                + plan.Name + "\",\n\t\t\t\"unit_amount\":" + HttpContext.Request.QueryString["Amount"]
                + ",\n\t\t\t\"quantity\": 1\n\t\t}],\n\t\"success_url\": \"https://hrfactoryapp.com/Plans/Subscribe?planId="
                + HttpContext.Request.QueryString["planId"] + "&subscriptionPeriod="
                + HttpContext.Request.QueryString["subscriptionPeriod"] + "&UserId="
                + Session["UserId"] + "\",\n\t\"cancel_url\": \"https://hrfactoryapp.com/#subscriptionPlans\",\n\t\"metadata\": {\n\t\t\"customer\": \""
                + Session["UserId"] + "\",\n\t\t\"name\": \"" + user.Name + "\",\n\t\t\"contact\": "
                + user.ContactInformation + ",\n\t\t\"email\": \"" + user.Email
                + "\"}\n}\n", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var v = response.Content.Split('"');
            return Redirect("http://checkout.thawani.om/pay/" + v[13] + "?key=AE3Ac7wGc8tB13XuoXGw98KlwtkV5j");
        }

        public ActionResult Learn()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            UnitOfWork _uow = new UnitOfWork();
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);
            Model.Courses course = _uow.coursesRepository.FindById(int.Parse(HttpContext.Request.QueryString["CID"]));
            double Price = double.Parse(HttpContext.Request.QueryString["Price"]) * 1000;
            int Priceint = int.Parse(Price.ToString());
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://checkout.thawani.om/api/v1/checkout/session");
            var request = new RestRequest(Method.POST);
            request.AddHeader("thawani-api-key", "8AoY3m4ahzYGuWtSfDW8YUa736DZvJ");
            request.AddHeader("Content-Type", "application/json");
            // course.CoursePrice.ToString().Replace(".","").Remove(course.CoursePrice.ToString().Length - 2,1)
            request.AddParameter("undefined", "{\n\t\"client_reference_id\": \"" + Session["UserId"] + "\",\n\t\"products\": [\n\t\t{\n\t\t\t\"name\": \"" + course.CourseName + "\",\n\t\t\t\"unit_amount\":" + Priceint + ",\n\t\t\t\"quantity\": 1\n\t\t}],\n\t\"success_url\": \"https://hrfactoryapp.com/TrainingHome/CheckOut?CID=" + HttpContext.Request.QueryString["CID"] + "&UserId=" + Session["UserId"] + "\",\n\t\"cancel_url\": \"https://hrfactoryapp.com/TrainingHome#learnOnline\",\n\t\"metadata\": {\n\t\t\"customer\": \"" + Session["UserId"] + "\",\n\t\t\"name\": \"" + user.Name + "\",\n\t\t\"contact\": " + user.ContactInformation + ",\n\t\t\"email\": \"" + user.Email + "\"}\n}\n", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var v = response.Content.Split('"');
            return Redirect("http://checkout.thawani.om/pay/" + v[13] + "?key=AE3Ac7wGc8tB13XuoXGw98KlwtkV5j");
        }
        public ActionResult Payment()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");
            if (HttpContext.Request.QueryString["planId"] == null || HttpContext.Request.QueryString["Amount"] == null || HttpContext.Request.QueryString["subscriptionPeriod"] == null)
                return RedirectToAction("Index", "Home");
            var planID = int.Parse(HttpContext.Request.QueryString["planId"]);
            Plan plan = _uow.PlanRepo.FindById(planID);
            ViewBag.plan = isArabic ? plan.NameAR : plan.Name;
            ViewBag.planId = planID;
            var Amount = HttpContext.Request.QueryString["Amount"];
            ViewBag.Amount = Amount;
            var subscriptionPeriod = HttpContext.Request.QueryString["subscriptionPeriod"];
            ViewBag.subscriptionPeriod = subscriptionPeriod == "Month" ? Resources.General.subscriptionPeriodMonth : Resources.General.subscriptionPeriodYear;
            ViewBag.DateFrom = DateTime.Now.ToShortDateString();
            ViewBag.DateTo = DateTime.Now.AddDays(subscriptionPeriod == "Month" ? 30 : 365).ToShortDateString();
            User user = _uow.UserRepo.FindById((int)Session["UserId"]);
            PayTabs PaymentDetails = new PayTabs();
            PaymentDetails.cart_amount = double.Parse(Amount);
            PaymentDetails.Email = user.Email;
            PaymentDetails.Country = user.Country.Name;
            PaymentDetails.Fullname = user.Name;
            DateTime dateTime = DateTime.Now;
            PaymentDetails.cart_id = user.Id.ToString() + "-" + planID.ToString() + "-" + subscriptionPeriod + "-" + dateTime.Day.ToString() + "-" + dateTime.Month.ToString() + "-"
                + dateTime.Year.ToString() + "-" + dateTime.Hour.ToString() + ":" + dateTime.Minute.ToString() + ":" + dateTime.Second.ToString();
            PaymentDetails.cart_description = subscriptionPeriod != "Month" ? "Subscription in " + Enum.GetName(typeof(SubscriptionPlan), planID) + " Plan for One" + subscriptionPeriod :
                "upgrade Subscription in " + Enum.GetName(typeof(SubscriptionPlan), planID) + " Plan for One year";
            Session["Pay"] = PaymentDetails;
            return View();
        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create()
        {
            if (Session["Pay"] == null)
                return Redirect("/");
            PayTabs PaymentDetails = new PayTabs();
            PaymentDetails = (PayTabs)Session["Pay"];
            try
            {
                PaymentDetails.returnurl = new Uri(Request.Url, Url.Action("Webhook", "Thawani")).ToString();
                PaymentDetails.callback = new Uri(Request.Url, Url.Action("Ipn", "Thawani")).ToString();
                PaymentDetails.framed = false;
                PaypageResponse res = await PTConnector.CreatePayment(PaymentDetails.cart_amount, PaymentDetails.cart_currency, PaymentDetails.cart_id,
                    PaymentDetails.cart_description, PaymentDetails.framed, PaymentDetails.returnurl, PaymentDetails.callback);
                if (res.IsSuccessful())
                {
                    ViewData["Frammed"] = PaymentDetails.framed;

                    return View("Paypage", res);
                }

                ViewBag.msg = res.message;
                ViewData["Message"] = res.message;

                return View();
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View();
            }

        }
        [System.Web.Http.HttpPost]
        public ActionResult Webhook(PaytabsReturnResponse response)
        {

            string url = "https://hrfactoryapp.com/Plans/";
            string cartID = response.cartId;
            string[] cartIDinfo = cartID.Split('-');
            string userID = cartIDinfo[0];
            string planID = cartIDinfo[1];
            string Period = cartIDinfo[2];

            string plan = planID == "2" ? "StartUp" : "ManualBuilder";
            User user = _uow.UserRepo.FindById(int.Parse(userID));
            if (response.respStatus == "A")
            {
                SubscriptionPeriod subscriptionPeriod = Period == "Month" ? SubscriptionPeriod.Month : SubscriptionPeriod.Year;
                Subscribe(int.Parse(planID), subscriptionPeriod, user.Id);
                url = url + "Subscribe?planId="
                    + planID + "&subscriptionPeriod=" + Period
                    + "&UserId=" + userID;

            }
            else
            {
                GetSubscription(user);
            }
            FillSessions(user);
            return RedirectToAction(plan, "Plans");

        }
        [System.Web.Mvc.HttpPost]
        public void Ipn([FromBody] PaytabsIPNResponse ipn)
        {
            System.Console.WriteLine(ipn);
            return;
        }
        public void Subscribe(int planId, SubscriptionPeriod subscriptionPeriod, int UserId, bool isTrail = false)
        {
            bool isUpgrade = false;
            Session["UserId"] = UserId;

            User user = _uow.UserRepo.FindById((int)Session["UserId"]);

            int userId = (int)Session["UserId"];

            if (user.UserPlans.Where(x => x.PlanId == planId && x.IsActive).Count() == 1)
            {
                UserPlan userPlan =
                    user.UserPlans.Where(x => x.PlanId == planId && x.IsActive && x.IsFreeActive).Count() == 1 ?
                    user.UserPlans.Where(x => x.PlanId == planId && x.IsActive && x.IsFreeActive).FirstOrDefault() : null;
                if (userPlan != null)
                {
                    isUpgrade = true;
                    userPlan.IsFreeActive = false;
                    userPlan.IsActive = false;
                    _uow.UserPlanRepo.Update(userPlan);
                    _uow.UserPlanRepo.SaveChanges();
                }

            }

            if (planId == (int)SubscriptionPlan.ManualBuilder)
            {
                CopySections(user.Id);
                SetUpManual(user.Id, isTrail, isUpgrade);
            }
            double subscriptionPrice = 0;
            Plan plan = _uow.PlanRepo.FindById(planId);
            DateTime subscriptionEndDate = DateTime.Today;
            switch (subscriptionPeriod)
            {
                case SubscriptionPeriod.Month:
                    subscriptionEndDate = DateTime.Today.AddMonths(1).AddDays(-1);
                    subscriptionPrice = plan.ManthlyPrice;
                    break;
                case SubscriptionPeriod.Year:
                    subscriptionEndDate = DateTime.Today.AddYears(1).AddDays(-1);
                    subscriptionPrice = plan.AnnualPrice;
                    break;
                case SubscriptionPeriod.UpgradeToYear:
                    subscriptionEndDate = DateTime.Today.AddYears(1).AddDays(-1);
                    subscriptionPrice = plan.AnnualPrice;
                    break;
            }


            UserPlan myPlan = new UserPlan()
            {
                StartDate = DateTime.Now,
                EndDate = subscriptionEndDate,
                PlanId = planId,
                IsActive = true,
                UserId = userId,
                Price = subscriptionPrice
            };



            _uow.UserPlanRepo.Add(myPlan);
            _uow.SaveChanges();

            SubscriptionViewModel mySubscription = new SubscriptionViewModel()
            {
                Plan = _uow.PlanRepo.FindById(planId),
                UserPlan = myPlan,
                SubscriptionPeriod = subscriptionPeriod
            };
            Session["User"] = _uow.UserRepo.FindById((int)Session["UserId"]);

            GetSubscription(user);
            Session["eligableForFree"] = false;

        }
        private void SetUpManual(int userId, bool isTrail, bool isUpgrade = false)
        {
            List<Section> sections = _uow.SectionRepo.Search(a => a.UserId == userId).ToList();
            int looper;
            if (isTrail && !isUpgrade)
            {
                foreach (Section section in sections)
                {
                    if (section.ParenId == null)
                    {
                        looper = 1;
                        foreach (Section ChildSection in section.Childs.OrderBy(x => x.Ordering))
                        {
                            if (looper > 3)
                            {
                                ChildSection.IsActive = false;
                                _uow.SectionRepo.Update(ChildSection);
                                _uow.SectionRepo.SaveChanges();
                            }
                            looper++;
                        }
                    }
                }
            }
            else
            {
                foreach (Section section in sections)
                {
                    if (section.ParenId == null)
                    {

                        foreach (Section ChildSection in section.Childs.OrderBy(x => x.Ordering))
                        {

                            ChildSection.IsActive = section.IsActive;
                            _uow.SectionRepo.Update(ChildSection);
                            _uow.SectionRepo.SaveChanges();

                        }
                    }
                }
            }
        }
        public void CopySections(int userId, bool isTrial = false)
        {
            List<Section> userSections = new List<Section>();
            if (_uow.SectionRepo.Search(x => x.UserId == userId).Count() > 0) return;
            int adminId = _uow.UserRepo.Search(x => x.IsAdmin).Single().Id;
            User user = _uow.UserRepo.FindById(userId);
            List<Section> adminSections = _uow.SectionRepo.Search(a => a.UserId == adminId && a.CountryID == user.CountryId).ToList();
            if (adminSections.Count == 0)
                adminSections = _uow.SectionRepo.Search(a => a.UserId == adminId && a.CountryID == 158).ToList();
            List<Section> parentAdminSections = adminSections.Where(x => x.ParenId == null).ToList();
            foreach (Section parent in parentAdminSections)
            {
                Section parentUserSection = new Section()
                {
                    Description = parent.Description,
                    Title = parent.Title,
                    Ordering = parent.Ordering,
                    UserId = userId,
                    LanguageId = parent.LanguageId,
                    IsActive = parent.IsActive,
                    Content = parent.Content,
                    CountryID = parent.CountryID,
                    SectionId = parent.SectionId,
                    Childs = new List<Section>(),
                };

                foreach (Section child in parent.Childs)
                {
                    Section childUserSection = new Section()
                    {
                        Description = child.Description,
                        Title = child.Title,
                        Ordering = child.Ordering,
                        UserId = int.Parse(Session["UserId"].ToString()),
                        LanguageId = parent.LanguageId,
                        IsActive = parent.IsActive,
                        Content = child.Content,
                        CountryID = child.CountryID,
                        SectionId = child.SectionId,
                    };
                    parentUserSection.Childs.Add(childUserSection);
                }
                userSections.Add(parentUserSection);
            }
            foreach (Section section in userSections)
            {
                _uow.SectionRepo.Add(section);
            }
            _uow.SaveChanges();
        }
        public void GetSubscription(User user)
        {
            bool havingMBSub = false;
            bool havingSUSub = false;
            Session["eligableForFreeSU"] = false;
            Session["eligableForFreeMB"] = false;
            List<UserPlan> subscriptionPlan = _uow.UserPlanRepo.Search(x => x.UserId == user.Id && x.IsActive).ToList();
            List<UserPlan> subscriptionPlan1 = _uow.UserPlanRepo.Search(x => x.UserId == user.Id).ToList();
            if (subscriptionPlan1.Count == 0)
            {
                Session["eligableForFree"] = true;
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
            }
            else
            {
                foreach (UserPlan Plansub in subscriptionPlan1)
                {
                    if (Plansub.IsFreeActive)
                    {
                        DateTime dateToday = DateTime.Now;
                        if ((Plansub.EndDate.Date - dateToday.Date).Days <= 0)
                        {
                            Plansub.IsActive = false;
                            Plansub.IsFreeActive = false;
                            _uow.SaveChanges();
                        }
                    }
                    if (Plansub.PlanId == (int)SubscriptionPlan.Startup)
                        havingSUSub = true;
                    if (Plansub.PlanId == (int)SubscriptionPlan.ManualBuilder)
                        havingMBSub = true;
                }
                if (!havingMBSub)
                    Session["eligableForFreeMB"] = true;
                else
                    Session["eligableForFreeMB"] = false;
                if (!havingSUSub)
                    Session["eligableForFreeSU"] = true;
                else
                    Session["eligableForFreeSU"] = false;
                if (!havingMBSub || !havingSUSub)
                    Session["eligableForFree"] = true;
                else
                    Session["eligableForFree"] = false;
            }
            Session["userPlans"] = subscriptionPlan;

        }
        private void FillSessions(User user)
        {
            Session["UserId"] = user.Id;
            Session["User"] = user;
            if (user.IsAdmin)
            {
                Session["CMS"] = true;
                Session["IsAdmin"] = true;
            }
            else
            {
                Session["CMS"] = false;
                Session["IsAdmin"] = false;
            }
        }
        //public ActionResult Paytabs()
        //{
        //    Dictionary<string, dynamic> list = new Dictionary<string, dynamic>();
        //    list.Add("profile_id", 92253);
        //    list.Add("tran_type", "sale");
        //    list.Add("tran_class", "ecom");
        //    list.Add("cart_id", "4244b9fd-c7e9-4f16-8d3c-4fe7bf6c48ca");
        //    list.Add("cart_description", "Dummy Order 35925502061445345");
        //    list.Add("cart_currency", "OMR");
        //    list.Add("cart_amount", 1);
        //    //list.Add("callback", "https://www.hrfactoryapp.com/Thawani/Test");
        //    list.Add("return", "https://www.hrfactoryapp.com/Thawani/Test1");
        //    string test = JsonConvert.SerializeObject(list);
        //    var client = new RestClient("https://secure-oman.paytabs.com/payment/request");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("authorization", "SZJN2RT2TM-JDGJT62RJM-NDHJKNJR2D");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddParameter("data", test, ParameterType.RequestBody);
        //    //IRestResponse response1 = client.Execute(request);
        //    IRestResponse response = client.Execute(request);
        //    var v = response.Content.Split('"');
        //    URL = v[31];
        //    return Redirect(v[31]);
        //    //return Redirect(v[35]);
        //}
        //[HttpPost]
        //public string Test()
        //{
        //    var result = "";
        //    using (var Client = new HttpClient())
        //    {
        //        Client.BaseAddress = new Uri("https://secure-oman.paytabs.com/payment/page");
        //        var responseTask = Client.GetAsync("result");
        //        responseTask.Wait();
        //        result = responseTask.Result.ToString();
        //    }
        //    return "the result is " + result;
        //}

        //public ActionResult Paytabs1()
        //{
        //    //return View();
        //    return Pay();
        //}

        //private ActionResult Pay()
        //{
        //    Dictionary<string, dynamic> list = new Dictionary<string, dynamic>();
        //    list.Add("profile_id", 92253);
        //    list.Add("tran_type", "sale");
        //    list.Add("tran_class", "ecom");
        //    list.Add("cart_id", "4244b9fd-c7e9-4f16-8d3c-4fe7bf6c48ca");
        //    list.Add("cart_description", "Dummy Order 35925502061445345");
        //    list.Add("cart_currency", "OMR");
        //    list.Add("cart_amount", 1);
        //    list.Add("callback", "https://www.hrfactoryapp.com/Thawani/Test");
        //    //list.Add("return", "https://www.hrfactoryapp.com/Thawani/Test1");
        //    string test = JsonConvert.SerializeObject(list);
        //    var client = new RestClient("https://secure-oman.paytabs.com/payment/request");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("authorization", "SZJN2RT2TM-JDGJT62RJM-NDHJKNJR2D");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddParameter("data", test, ParameterType.RequestBody);
        //    //IRestResponse response1 = client.Execute(request);
        //    IRestResponse response = client.Execute(request);
        //    var test2 = JsonConvert.DeserializeObject(response.Content);
        //    var v = response.Content.Split('"');
        //    //return Redirect(v[31]);
        //    URL = v[35];
        //    HttpResponse httpResponse = new HttpResponse();
        //    System.Web.HttpContext.Current.Response.Redirect(v[35]);
        //    var x = httpResponse;
        //    var test1 = Response.StatusCode;
        //    var data = Response.Output;
        //    return Redirect(v[35]);
        //}
    }

}
