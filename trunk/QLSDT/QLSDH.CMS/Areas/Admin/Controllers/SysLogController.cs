using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using TEMIS.Model;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class SysLogController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: Admin/SysLog
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ListLog(string startAt, string startWhen, string txtSearch)
        {
            try
            {
                DateTime today = DateTime.Today;
                int y = today.Year;
                int m = today.Month;
                int d = today.Day;
                DateTime start_At, start_When;
                if (startAt != "" && startWhen != "")
                {
                    start_At = Convert.ToDateTime(startAt);
                    start_When = Convert.ToDateTime(startWhen);
                }
                else
                {
                    start_At = new DateTime(y, m, d, 00, 00, 00);
                    start_When = new DateTime(y, m, d, 23, 59, 59);
                }
                List<SysLog> listLog = new List<SysLog>();
                listLog = _unitOfWork.GetRepositoryInstance<SysLog>().GetListByParameter(x=> x.CreatedAt > start_At && x.CreatedAt < start_When && x.CreatedBy.Contains(txtSearch)).ToList();
                return PartialView("_PartialListLog", listLog);
            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                throw;
            }

        }
        public ActionResult Crash()
        {
            ViewBag.LogFile = HttpContext.Server.MapPath("~/Log/") + "error.txt";
            return View();
        }
        public ActionResult RemoveLog()
        {
            return View();
        }
    }
}