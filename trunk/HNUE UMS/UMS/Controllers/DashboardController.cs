using System.Web.Mvc;
using Ums.App.Base;

namespace Ums.Website.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Construction()
        {
            return View();
        }
    }
}