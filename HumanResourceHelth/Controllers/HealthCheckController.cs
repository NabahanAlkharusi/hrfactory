using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class HealthCheckController : Controller
    {
        // GET: HealthCheck
        public ActionResult Index()
        {
            return View();
        }
    }
}