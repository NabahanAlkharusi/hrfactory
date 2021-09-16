using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class CultureController : Controller
    {
        [HttpPost]
        public ActionResult SetLanguage(string culture, string returnUrl)
        {
            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            //);

            return LocalRedirect(returnUrl);
        }

        private ActionResult LocalRedirect(string returnUrl)
        {
            throw new NotImplementedException();
        }
    }
}