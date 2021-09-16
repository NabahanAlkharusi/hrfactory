using HumanResourceHelth.DataAccess;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;


namespace HumanResourceHelth.Web
{
    public class Global : System.Web.HttpApplication
    {
        private UnitOfWork _uow = new UnitOfWork();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(5000);
            UnitOfWork _uow = new UnitOfWork();
            if (Session["Content"] == null)
                Session["Content"] = _uow.ContentRepo.GetAll();

            if (Session["CMS"] == null)
                Session["CMS"] = false;

            string culture;
            if (Session["lang"] == null)
            {
                culture = "ar-OM";
                Session["lang"] = "ar-OM";
            }
            else
                culture = Session["lang"].ToString();

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        }
    }
}