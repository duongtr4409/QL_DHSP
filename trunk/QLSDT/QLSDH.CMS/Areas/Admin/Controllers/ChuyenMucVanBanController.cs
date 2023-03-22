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
    public class ChuyenMucVanBanController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            var lstData = _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().GetAllRecords().OrderBy(o => o.TenChuyenMuc).ToList();
            return View(lstData);
        }

        public async Task<JsonResult> ThemMoi(string name)
        {
            try
            {
                ChuyenMucVanBan chuyenmucvanban = _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().GetFirstOrDefaultByParameter(o => o.TenChuyenMuc == name);
                if (chuyenmucvanban != null)
                {
                    TempData["error"] = "Key đã  tồn tại trên hệ thống";
                    return Json("Key đã  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                chuyenmucvanban = new ChuyenMucVanBan();
                chuyenmucvanban.TenChuyenMuc = name;
                chuyenmucvanban.CreatedAt = DateTime.Now;
                _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().Add(chuyenmucvanban);
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
                ChuyenMucVanBan chuyenmucvanban = _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (chuyenmucvanban != null)
                {
                    chuyenmucvanban.TenChuyenMuc = name;
                    chuyenmucvanban.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().Update(chuyenmucvanban);
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
                List<VanBan> listvanban = _unitOfWork.GetRepositoryInstance<VanBan>().GetListByParameter(o => o.ChuyenMuc == id).ToList();
                if (listvanban.Count > 0)
                {
                    TempData["error"] = "Danh mục không thể xóa  do vẫn còn văn bản trong danh mục";
                    return Json("Danh mục không thể xóa  do vẫn còn văn bản trong danh mục", JsonRequestBehavior.AllowGet);
                }

                ChuyenMucVanBan chuyemuc = _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (chuyemuc != null)
                {
                    _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().Remove(chuyemuc);
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