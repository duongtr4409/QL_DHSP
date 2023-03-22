using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
using CoreAPI.Entity;
using System.Globalization;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    // [Roles(PublicConstant.ROLE_ADMIN, PublicConstant.ROLE_SUPPERADMIN)]
    //[Authorize]
    [AuditAction]
    public class KhoaHocController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            var lstKhoHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderBy(o => o.Id).OrderByDescending(o => o.Id).ToList();
            ViewBag.ListKhoaHoc = lstKhoHoc;
            return View();
        }
        public async Task<ActionResult> LoadKhoaHoc(int? idKhoahoc = -1)
        {
            try
            {
                List<KhoaHoc> list_data = new List<KhoaHoc>();

                if (idKhoahoc > 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetListByParameter(o => o.Id == idKhoahoc).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
                }

                return PartialView("_PartialKhoaHoc", list_data);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<JsonResult> ThemMoi(string makhoa, string ngaykhaigiang, string soluonghocvien)
        {
            try
            {
                KhoaHoc khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.MaKhoa == makhoa);
                if (khoahoc != null)
                {
                    TempData["error"] = "Mã khoa đã  tồn tại trên hệ thống";
                    return Json("Mã khoa đã  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                khoahoc = new KhoaHoc();
                khoahoc.MaKhoa = makhoa.Trim();
                khoahoc.NgayKhaiGiang = DateTime.ParseExact(ngaykhaigiang.Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture);
                khoahoc.SoLuongHocVien = int.Parse(soluonghocvien);
                khoahoc.CreatedAt = DateTime.Now;
                khoahoc.UpdatedAt = DateTime.Now;
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                khoahoc.UpdatedBy = user.Username;
                khoahoc.CreatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<KhoaHoc>().Add(khoahoc);
                _unitOfWork.SaveChanges();

                TempData["message"] = "Thêm  mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Sua(long id, string makhoa, string ngaykhaigiang, string soluonghocvien)
        {
            try
            {
                KhoaHoc khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (khoahoc != null)
                {
                    khoahoc.MaKhoa = makhoa.Trim();
                    //khoahoc.TenKhoa = tenkhoa.Trim();
                    khoahoc.NgayKhaiGiang = DateTime.ParseExact(ngaykhaigiang.Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture);
                    khoahoc.SoLuongHocVien = int.Parse(soluonghocvien);
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    khoahoc.UpdatedBy = user.Username;
                    khoahoc.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<KhoaHoc>().Update(khoahoc);
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
                KhoaHoc khoahoc = new KhoaHoc();
                khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (khoahoc != null)
                {
                    _unitOfWork.GetRepositoryInstance<KhoaHoc>().Remove(khoahoc);
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