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

namespace HumanResourceHelth.Web.Controllers
{
    public class ThawaniController : Controller
    {
        string URL;
        public ActionResult Index()
        {
            UnitOfWork _uow = new UnitOfWork();
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
            ViewBag.plan = HttpContext.Request.QueryString["planId"];
            ViewBag.Amount = HttpContext.Request.QueryString["Amount"];
            ViewBag.subscriptionPeriod = HttpContext.Request.QueryString["subscriptionPeriod"];

            return View();
        }
        public ActionResult Paytabs()
        {
            Dictionary<string, dynamic> list = new Dictionary<string, dynamic>();
            list.Add("profile_id", 92253);
            list.Add("tran_type", "sale");
            list.Add("tran_class", "ecom");
            list.Add("cart_id", "4244b9fd-c7e9-4f16-8d3c-4fe7bf6c48ca");
            list.Add("cart_description", "Dummy Order 35925502061445345");
            list.Add("cart_currency", "OMR");
            list.Add("cart_amount", 1);
            //list.Add("callback", "https://www.hrfactoryapp.com/Thawani/Test");
            list.Add("return", "https://www.hrfactoryapp.com/Thawani/Test1");
            string test = JsonConvert.SerializeObject(list);
            var client = new RestClient("https://secure-oman.paytabs.com/payment/request");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "SZJN2RT2TM-JDGJT62RJM-NDHJKNJR2D");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("data", test, ParameterType.RequestBody);
            //IRestResponse response1 = client.Execute(request);
            IRestResponse response = client.Execute(request);
            var v = response.Content.Split('"');
            URL = v[31];
            return Redirect(v[31]);
            //return Redirect(v[35]);
        }
        public string Test()
        {
            var result = "";
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri("https://secure-oman.paytabs.com/payment/page");
                var responseTask = Client.GetAsync("result");
                responseTask.Wait();
                result = responseTask.Result.ToString();
            }
            return "the result is " + result;
        }

        public ActionResult Paytabs1()
        {
            return View();
            //return Pay();
        }

        private ActionResult Pay()
        {
            Dictionary<string, dynamic> list = new Dictionary<string, dynamic>();
            list.Add("profile_id", 92253);
            list.Add("tran_type", "sale");
            list.Add("tran_class", "ecom");
            list.Add("cart_id", "4244b9fd-c7e9-4f16-8d3c-4fe7bf6c48ca");
            list.Add("cart_description", "Dummy Order 35925502061445345");
            list.Add("cart_currency", "OMR");
            list.Add("cart_amount", 1);
            list.Add("callback", "https://www.hrfactoryapp.com/Thawani/Test");
            //list.Add("return", "https://www.hrfactoryapp.com/Thawani/Test1");
            string test = JsonConvert.SerializeObject(list);
            var client = new RestClient("https://secure-oman.paytabs.com/payment/request");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "SZJN2RT2TM-JDGJT62RJM-NDHJKNJR2D");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("data", test, ParameterType.RequestBody);
            //IRestResponse response1 = client.Execute(request);
            IRestResponse response = client.Execute(request);
            var v = response.Content.Split('"');
            //return Redirect(v[31]);
            URL = v[35];
            HttpResponse httpResponse = new HttpResponse();
            System.Web.HttpContext.Current.Response.Redirect(v[35]);
            var x = httpResponse;
            var test1 = Response.StatusCode;
            var data = Response.Output;
            return Redirect(v[35]);
        }

        public string Test1()
        {
            var result1 = "";
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri("https://secure-oman.paytabs.com/payment/page");
                var responseTask = Client.GetAsync("");
                responseTask.Wait();
                var result = responseTask.Result;
                var ResultContent = result.Content.ReadAsAsync<dynamic>();
                ResultContent.Wait();
                var tran_ref = ResultContent;
                return "the result is:  " + JsonConvert.SerializeObject(ResultContent.Result);
            }

            //return "the result is " + result1;
        }
    }

}
