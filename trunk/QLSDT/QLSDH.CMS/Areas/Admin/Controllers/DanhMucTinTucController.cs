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
    public class DanhMucTinTucController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            var lstData = _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetAllRecords().OrderBy(o => o.TenDanhMuc).ToList();
            return View(lstData);
        }

        public async Task<JsonResult> ThemMoi(string name)
        {
            try
            {
                DanhMucTinTuc danhmucTin = _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetFirstOrDefaultByParameter(o => o.TenDanhMuc == name);
                if (danhmucTin != null)
                {
                    TempData["error"] = "Key đã  tồn tại trên hệ thống";
                    return Json("Key đã  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                danhmucTin = new DanhMucTinTuc();
                danhmucTin.TenDanhMuc = name;
                danhmucTin.CreatedAt = DateTime.Now;
                _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().Add(danhmucTin);
                _unitOfWork.SaveChanges();

                TempData["message"] = "Thêm mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Sua(int id, string name)
        {
            try
            {
                DanhMucTinTuc danhmucTin = _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (danhmucTin != null)
                {
                    danhmucTin.TenDanhMuc = name;
                    danhmucTin.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().Update(danhmucTin);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

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
                List<TinTuc> listTin = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == id).ToList();
                if (listTin.Count > 0)
                {
                    TempData["error"] = "Danh mục không thể xóa  do vẫn còn bài viết trong danh mục";
                    return Json("Danh mục không thể xóa  do vẫn còn bài viết trong danh mục", JsonRequestBehavior.AllowGet);
                }

                DanhMucTinTuc danhmuc = _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (danhmuc != null)
                {
                    _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().Remove(danhmuc);
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