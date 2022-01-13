using HumanResourceHelth.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class ExpertController : Controller
    {
        private HumanResourceContext db = new HumanResourceContext();
        // GET: Expert
        public ActionResult Index()
        {
            return View(db.Experts.ToList());
        }
    }
}