using HumanResourceHelth.DataAccess;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class PluginRequestsController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        public ActionResult Index()
        {
            return View(_uow.PluginRequestRepo.GetAll());
        }
    }
}