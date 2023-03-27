using CoreAPI.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class QuanLyDotTuyenSinhController : Controller
    {
        // GET: Admin/QuanLyDotTuyenSinh
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            var lstDotTuyensinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetAllRecords().OrderBy(o => o.Id).OrderByDescending(o => o.Id).ToList();
            ViewBag.lstDotTuyensinh = lstDotTuyensinh;
            List<KhoaHoc> list = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = list;
            return View();
        }
        public ActionResult LoadDotTuyenSinh(int? idKhoahoc = -1)
        {
            try
            {
                List<DotTuyenSinh> list_data = new List<DotTuyenSinh>();

                if (idKhoahoc > 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetListByParameter(o => o.idKhoahoc == idKhoahoc).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
                }

                return PartialView("_PartialDotTuyenSinh", list_data);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<JsonResult> ThemMoi(string makhoahoc, string tendot, string ngaybatdau, string ngayketthuc)
        {
            try
            {
                long idKH = long.Parse(makhoahoc);
                KhoaHoc khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == idKH);
                if (khoahoc == null)
                {
                    TempData["error"] = "Khóa học không  tồn tại trên hệ thống";
                    return Json("Khóa học không  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                var dotTuyenSinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.TenDot == tendot && x.idKhoahoc == idKH);
                if (dotTuyenSinh != null)
                {
                    TempData["error"] = "Đợt tuyển sinh đã tồn tại trên hệ thống";
                    return Json("Đợt tuyển sinh đã tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                dotTuyenSinh = new DotTuyenSinh();
                dotTuyenSinh.idKhoahoc = idKH;
                dotTuyenSinh.MaKhoaHoc = khoahoc.MaKhoa;
                dotTuyenSinh.NgayBatDau = DateTime.ParseExact(ngaybatdau.Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture);
                dotTuyenSinh.NgayKetThuc = DateTime.ParseExact(ngayketthuc.Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture);
                dotTuyenSinh.TenDot = tendot;
                dotTuyenSinh.CreatedAt = DateTime.Now;
                dotTuyenSinh.UpdatedAt = DateTime.Now;
                dotTuyenSinh.Status = 0;
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                dotTuyenSinh.UpdatedBy = user.Username;
                dotTuyenSinh.CreatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().Add(dotTuyenSinh);
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
        public async Task<JsonResult> Sua(long id, string makhoahoc, string tendot, string ngaybatdau, string ngayketthuc)
        {
            try
            {
                long idKH = long.Parse(makhoahoc);
                KhoaHoc khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == idKH);
                if (khoahoc == null)
                {
                    TempData["error"] = "Khoá học không tồn tại trên hệ thống";
                    return Json("Khoá học không tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                DotTuyenSinh dotTuyenSinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (dotTuyenSinh != null)
                {
                    dotTuyenSinh.TenDot = tendot.Trim();
                    //khoahoc.TenKhoa = tenkhoa.Trim();
                    dotTuyenSinh.NgayBatDau = DateTime.ParseExact(ngaybatdau.Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture);
                    dotTuyenSinh.NgayKetThuc = DateTime.ParseExact(ngayketthuc.Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture);

                    dotTuyenSinh.idKhoahoc = khoahoc.Id;
                    dotTuyenSinh.MaKhoaHoc = khoahoc.MaKhoa;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    dotTuyenSinh.UpdatedBy = user.Username;
                    dotTuyenSinh.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().Update(dotTuyenSinh);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Đợt tuyển sinh không tồn tại";
                    return Json("Đợt tuyển sinh không tồn tại", JsonRequestBehavior.AllowGet);
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
                DotTuyenSinh dotTuyenSinh = new DotTuyenSinh();
                dotTuyenSinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (dotTuyenSinh != null)
                {
                    if (dotTuyenSinh.Status == 1)
                    {
                        TempData["error"] = "Đang tuyển sinh không thể xoá đợt tuyển sinh!";
                        return Json("Đang tuyển sinh không thể xoá đợt tuyển sinh!", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().Remove(dotTuyenSinh);
                        _unitOfWork.SaveChanges();
                    }    
                }
                else
                {
                    TempData["error"] = "Đợt tuyển sinh không tồn tại";
                    return Json("Đợt tuyển sinh không tồn tại", JsonRequestBehavior.AllowGet);
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

        public async Task<JsonResult> UpdateStatus(long id, int status)
        {
            try
            {
                DotTuyenSinh dotTuyenSinh = new DotTuyenSinh();
                dotTuyenSinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                DotTuyenSinh DotTS = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.idKhoahoc == dotTuyenSinh.idKhoahoc && x.Id != dotTuyenSinh.Id && x.Status == 1);
                if (dotTuyenSinh != null)
                {
                    if(status == 1)
                    {
                        if (DotTS != null)
                        {
                            return Json("Không được tuyển sinh 2 đợt trong cùng 1 khoá!", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            dotTuyenSinh.Status = status;
                            _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().Update(dotTuyenSinh);
                            _unitOfWork.SaveChanges();
                            return Json("Mở đợt tuyển sinh thành công!", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        dotTuyenSinh.Status = status;
                        _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().Update(dotTuyenSinh);
                        _unitOfWork.SaveChanges();
                        return Json("Đóng đợt tuyển sinh thành công!", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("Đợt tuyển sinh không tồn tại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Lỗi mở đợt tuyển sinh", JsonRequestBehavior.AllowGet);
            }
        }
    }
}