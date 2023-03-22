using CoreAPI.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.Model;
using TEMIS.CMS.Common;
namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    //[AuthorizeRoles(PublicConstant.ROLE_CB_PHONG_SDH)]
    public class KhoaController : Controller
    {
        // GET: Admin/Khoa
        // GET: Admin/ChucDanh
        public async Task<ActionResult> Index()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            return View(list);
        }


        public async Task<JsonResult> Sua(long id, string tenkhoa)
        {
            try
            {
                //ChuongTrinhDaoTao ctdt = new ChuongTrinhDaoTao();
                //ctdt = _unitOfWork.GetRepositoryInstance<ChuongTrinhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == id);
                //if (ctdt != null)
                //{
                //    ctdt.NghanhHoc = machucdanh.Trim();
                //    ctdt.TenVietTat = tenchucdanh.Trim();
                //    _unitOfWork.GetRepositoryInstance<ChuongTrinhDaoTao>().Update(ctdt);
                //    _unitOfWork.SaveChanges();
                //}
                //else
                //{
                //    TempData["error"] = "bản ghi không tồn tại";
                //    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                //}

                TempData["message"] = "Cập nhật thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi Cập nhật: " + ex.Message;
                return Json("Lỗi Cập nhật", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                //ChuongTrinhDaoTao ctdt = new ChuongTrinhDaoTao();
                //ctdt = _unitOfWork.GetRepositoryInstance<ChuongTrinhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == id);
                //if (ctdt != null)
                //{
                //    _unitOfWork.GetRepositoryInstance<ChuongTrinhDaoTao>().Remove(ctdt);
                //    _unitOfWork.SaveChanges();
                //}
                //else
                //{
                //    TempData["error"] = "bản ghi không tồn tại";
                //    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                //}

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