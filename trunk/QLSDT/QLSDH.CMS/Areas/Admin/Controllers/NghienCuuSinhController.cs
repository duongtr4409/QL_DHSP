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
    public class NghienCuuSinhController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public ActionResult Index()
        {
            var lstModule = _unitOfWork.GetRepositoryInstance<NCS>().GetAllRecords().OrderBy(o => o.HoTen).ToList();
            return View(lstModule);
        }
       
        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                NCS module = new NCS();
                module = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (module != null)
                {
                    _unitOfWork.GetRepositoryInstance<NCS>().Remove(module);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Xóa thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi xóa: " + ex.Message;
                return Json("Lỗi xóa", JsonRequestBehavior.AllowGet);
            }
        }
    }
}