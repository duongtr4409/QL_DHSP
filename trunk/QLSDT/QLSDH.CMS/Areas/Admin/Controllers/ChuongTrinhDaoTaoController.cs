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
    // [Roles(PublicConstant.ROLE_ADMIN, PublicConstant.ROLE_SUPPERADMIN)]
    //[Authorize]
    [AuditAction]
    public class ChuongTrinhDaoTaoController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            var lstChucDanh = _unitOfWork.GetRepositoryInstance<ChuongTrinhDaoTao>().GetAllRecords().OrderBy(o => o.TenVietTat).ToList();
            return View(lstChucDanh);
        }

        public async Task<JsonResult> ThemMoi(string machucdanh, string tenchucdanh)
        {
            try
            {
                //ChuongTrinhDaoTao ctdt = new ChuongTrinhDaoTao();
                //ctdt.NghanhHoc = machucdanh.Trim();
                //ctdt.TenVietTat = tenchucdanh.Trim();
                //_unitOfWork.GetRepositoryInstance<ChuongTrinhDaoTao>().Add(ctdt);
                //_unitOfWork.SaveChanges();

                TempData["message"] = "Thêm  mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Sua(long id, string machucdanh, string tenchucdanh)
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