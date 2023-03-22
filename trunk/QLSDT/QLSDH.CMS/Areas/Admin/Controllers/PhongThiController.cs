using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Common;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class PhongThiController : Controller
    {
        // GET: Admin/PhongThi
        public ActionResult Index()
        {
            return View();
        }
    }
}