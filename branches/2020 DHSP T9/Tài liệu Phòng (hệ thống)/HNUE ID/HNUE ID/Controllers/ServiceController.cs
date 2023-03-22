using System.Web.Mvc;
using Ums.App.Base;

namespace Ums.Website.Controllers
{
    public class ServiceController : BaseController
    {
        public ActionResult OAuth(string authToken, string returnUrl)
        {
            return View();
        }
    }
}