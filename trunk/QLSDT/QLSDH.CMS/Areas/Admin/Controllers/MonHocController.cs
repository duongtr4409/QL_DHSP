using CoreAPI.Entity;
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
    public class MonHocController : Controller
    {
        // GET: Admin/MonHoc

        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            List<HocPhan> list = _unitOfWork.GetRepositoryInstance<HocPhan>().GetAllRecords().ToList();
            ViewBag.ListHocPhan = list;
            var lstMonHoc = _unitOfWork.GetRepositoryInstance<MonHoc>().GetAllRecords().OrderBy(o => o.Id).OrderByDescending(o => o.Id).ToList();
            return View(lstMonHoc);
        }

        public ActionResult GetListMonHoc(int mahocphan)
        {
            List<MonHoc> listMonHoc = new List<MonHoc>();
            if (mahocphan > 0)
            {
                listMonHoc = _unitOfWork.GetRepositoryInstance<MonHoc>().GetListByParameter(x => x.HocPhanId == mahocphan).ToList();
            }
            else
            {
                listMonHoc = _unitOfWork.GetRepositoryInstance<MonHoc>().GetAllRecords().ToList();
            }

            return PartialView("_PartialListMonHoc", listMonHoc);
        }
        public async Task<JsonResult> ThemMoi(string mamonhoc, string tenmonhoc, string mahocphan)
        {
            try
            {
                MonHoc monhoc = _unitOfWork.GetRepositoryInstance<MonHoc>().GetFirstOrDefaultByParameter(o => o.MaMon == mamonhoc);
                if (monhoc != null)
                {
                    TempData["error"] = "Mã môn học đã  tồn tại trên hệ thống";
                    return Json("Mã môn học đã  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                int id = int.Parse(mahocphan);
                HocPhan hocphan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (hocphan == null)
                {
                    TempData["error"] = "Không tìm thấy học phần";
                    return Json("Không tìm thấy học phần", JsonRequestBehavior.AllowGet);
                }
                monhoc = new MonHoc();
                monhoc.MaMon = mamonhoc.Trim();
                monhoc.TenMon = tenmonhoc.Trim();
                monhoc.HocPhanId = int.Parse(mahocphan);
                monhoc.KhoaHocId = hocphan.KhoaHocId;
                monhoc.KhoaId = hocphan.KhoaId;
                monhoc.KhoaHocId = hocphan.KhoaHocId;
                monhoc.NganhId = hocphan.NganhId;
                monhoc.ChuyenNganhId = hocphan.ChuyenNganhId;
                monhoc.CreatedAt = DateTime.Now;
                monhoc.UpdatedAt = DateTime.Now;
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                monhoc.UpdatedBy = user.Username;
                monhoc.CreatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<MonHoc>().Add(monhoc);
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
        public async Task<JsonResult> Sua(long id, string mamonhoc, string tenmonhoc, string mahocphan)
        {
            try
            {
                MonHoc monhoc = _unitOfWork.GetRepositoryInstance<MonHoc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                int idhocphan = int.Parse(mahocphan);
                HocPhan hocphan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(o => o.Id == idhocphan);
                if (hocphan == null)
                {
                    TempData["error"] = "Không tìm thấy học phần";
                    return Json("Không tìm thấy học phần", JsonRequestBehavior.AllowGet);
                }
                if (monhoc != null)
                {
                    monhoc.MaMon = mamonhoc.Trim();
                    monhoc.TenMon = tenmonhoc.Trim();
                    monhoc.HocPhanId = int.Parse(mahocphan);
                    monhoc.KhoaHocId = hocphan.KhoaHocId;
                    monhoc.KhoaId = hocphan.KhoaId;
                    monhoc.KhoaHocId = hocphan.KhoaHocId;
                    monhoc.ChuyenNganhId = hocphan.ChuyenNganhId;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    monhoc.UpdatedBy = user.Username;
                    monhoc.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<MonHoc>().Update(monhoc);
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
                MonHoc monhoc = new MonHoc();
                monhoc = _unitOfWork.GetRepositoryInstance<MonHoc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (monhoc != null)
                {
                    _unitOfWork.GetRepositoryInstance<MonHoc>().Remove(monhoc);
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