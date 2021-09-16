using HumanResourceHelth.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class TechRequestsController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Admin/TechRequests
        public ActionResult Index()
        {
            return View(_uow.TechRequestRepo.GetAll());
        }
    }
}