using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using RestSharp;
using System.Net;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class ThawaniController : Controller
    {
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
            request.AddParameter("undefined", "{\n\t\"client_reference_id\": \"" + Session["UserId"] + "\",\n\t\"products\": [\n\t\t{\n\t\t\t\"name\": \"" + plan.Name + "\",\n\t\t\t\"unit_amount\":" + HttpContext.Request.QueryString["Amount"] + ",\n\t\t\t\"quantity\": 1\n\t\t}],\n\t\"success_url\": \"https://hrfactoryapp.com/Plans/Subscribe?planId=" + HttpContext.Request.QueryString["planId"] + "&subscriptionPeriod=" + HttpContext.Request.QueryString["subscriptionPeriod"] + "&UserId="+Session["UserId"] +"\",\n\t\"cancel_url\": \"https://hrfactoryapp.com/#subscriptionPlans\",\n\t\"metadata\": {\n\t\t\"customer\": \"" + Session["UserId"] + "\",\n\t\t\"name\": \"" + user.Name + "\",\n\t\t\"contact\": " + user.ContactInformation + ",\n\t\t\"email\": \"" + user.Email + "\"}\n}\n", ParameterType.RequestBody);
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
            double Price=double.Parse(HttpContext.Request.QueryString["Price"]) * 1000;
            int Priceint = int.Parse (Price.ToString());
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://checkout.thawani.om/api/v1/checkout/session");
            var request = new RestRequest(Method.POST);
            request.AddHeader("thawani-api-key", "8AoY3m4ahzYGuWtSfDW8YUa736DZvJ");
            request.AddHeader("Content-Type", "application/json");
            // course.CoursePrice.ToString().Replace(".","").Remove(course.CoursePrice.ToString().Length - 2,1)
            request.AddParameter("undefined", "{\n\t\"client_reference_id\": \"" + Session["UserId"] + "\",\n\t\"products\": [\n\t\t{\n\t\t\t\"name\": \"" + course.CourseName + "\",\n\t\t\t\"unit_amount\":" +Priceint + ",\n\t\t\t\"quantity\": 1\n\t\t}],\n\t\"success_url\": \"https://hrfactoryapp.com/TrainingHome/CheckOut?CID=" + HttpContext.Request.QueryString["CID"] +"&UserId=" + Session["UserId"] + "\",\n\t\"cancel_url\": \"https://hrfactoryapp.com/TrainingHome#learnOnline\",\n\t\"metadata\": {\n\t\t\"customer\": \"" + Session["UserId"] + "\",\n\t\t\"name\": \"" + user.Name + "\",\n\t\t\"contact\": " + user.ContactInformation + ",\n\t\t\"email\": \"" + user.Email + "\"}\n}\n", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var v = response.Content.Split('"');
            return Redirect("http://checkout.thawani.om/pay/" + v[13] + "?key=AE3Ac7wGc8tB13XuoXGw98KlwtkV5j");
        }

    }

}
