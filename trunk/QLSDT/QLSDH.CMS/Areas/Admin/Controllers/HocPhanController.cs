using CoreAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using TEMIS.Model;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class HocPhanController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/HocPhan
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {

            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;

            List<KhoaHoc> listKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().ToList();
            ViewBag.ListKhoaHoc = listKhoaHoc;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;

            var lstHocPHan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetAllRecords().OrderByDescending(o => o.Id).ToList();
            return View(lstHocPHan);
        }
        public ActionResult ChuyenDe()
        {
            return View();
        }
        public ActionResult GetListHocPhan(int khoahocid = 0, int khoaid = 0, int nganhid = 0, int chuyennganhid = 0, int loaihp = 0)
        {
            List<HocPhan> listHocPhan = new List<HocPhan>();

            bool tuchon = false;
            if (loaihp > 0)
            {
                tuchon = true;
            }

            listHocPhan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(o => (khoaid == 0 || o.KhoaId == khoaid) && (khoahocid == 0 || o.KhoaHocId == khoahocid) && (nganhid == 0 || o.NganhId == nganhid) && (chuyennganhid == 0 || o.ChuyenNganhId == chuyennganhid) && o.TuChon == tuchon).OrderByDescending(o => o.Id).ToList();
            
            return PartialView("_PartialListHocPhan", listHocPhan);
        }
        public async Task<JsonResult> ThemMoi(string mahocphan, string tenhocphan, int sodonvihoctrinh, int tuchon, int sotietlythuyet, int sotietthuchanh, int khoahocid, int khoaid, int nganhid, int chuyennganhid, int sotinchi, int thuochp = 1)
        {
            try
            {
                HocPhan hocPhan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(x => x.MaHocPhan == mahocphan);
                if (hocPhan != null)
                {
                    TempData["message"] = "Học phần đã tồn tại!";
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    hocPhan = new HocPhan();
                    hocPhan.MaHocPhan = mahocphan;
                    hocPhan.TenHocPhan = tenhocphan;
                    hocPhan.SoDVHT = sodonvihoctrinh;
                    if (tuchon == 0) hocPhan.TuChon = false;
                    else hocPhan.TuChon = true;
                    hocPhan.SoTietLyThuyet = sotietlythuyet;
                    hocPhan.SoTietThucHanh = sotietthuchanh;
                    hocPhan.NganhId = nganhid;
                    hocPhan.KhoaHocId = khoahocid;
                    hocPhan.KhoaId = khoaid;
                    hocPhan.NganhId = nganhid;
                    hocPhan.ChuyenNganhId = chuyennganhid;
                    hocPhan.CreatedAt = DateTime.Now;
                    hocPhan.UpdatedAt = DateTime.Now;
                    hocPhan.SoTinChi = sotinchi;
                    hocPhan.LoaiHP = thuochp;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    hocPhan.UpdatedBy = user.Username;
                    hocPhan.CreatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<HocPhan>().Add(hocPhan);
                    _unitOfWork.SaveChanges();
                }
                TempData["message"] = "Thêm  mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> SuaHocPhan(int id, string mahocphan, string tenhocphan, int sodonvihoctrinh, int tuchon, int sotietlythuyet, int sotietthuchanh, int khoahocid, int khoaid, int nganhid, int chuyennganhid, int sotinchi, int thuochp)
        {
            try
            {
                HocPhan hocPhan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (hocPhan == null)
                {
                    TempData["message"] = "Học phần không tồn tại!";
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    hocPhan.MaHocPhan = mahocphan;
                    hocPhan.TenHocPhan = tenhocphan;
                    hocPhan.SoDVHT = sodonvihoctrinh;
                    if (tuchon == 0) hocPhan.TuChon = false;
                    else hocPhan.TuChon = true;
                    hocPhan.SoTietLyThuyet = sotietlythuyet;
                    hocPhan.SoTietThucHanh = sotietthuchanh;
                    hocPhan.NganhId = nganhid;
                    hocPhan.KhoaHocId = khoahocid;
                    hocPhan.KhoaId = khoaid;
                    hocPhan.NganhId = nganhid;
                    hocPhan.ChuyenNganhId = chuyennganhid;
                    hocPhan.UpdatedAt = DateTime.Now;
                    hocPhan.SoTinChi = sotinchi;
                    hocPhan.LoaiHP = thuochp;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    hocPhan.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<HocPhan>().Update(hocPhan);
                    _unitOfWork.SaveChanges();
                }
                TempData["message"] = "Cập nhật thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                HocPhan hphan = new HocPhan();
                hphan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (hphan != null)
                {
                    _unitOfWork.GetRepositoryInstance<HocPhan>().Remove(hphan);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Bản ghi không tồn tại";
                    return Json("Bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
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