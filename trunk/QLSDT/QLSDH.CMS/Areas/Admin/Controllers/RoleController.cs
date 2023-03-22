using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class RoleController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            var lstData = _unitOfWork.GetRepositoryInstance<Roles>().GetAllRecords().OrderBy(o => o.Id).ToList();
            return View(lstData);
        }
    }
}