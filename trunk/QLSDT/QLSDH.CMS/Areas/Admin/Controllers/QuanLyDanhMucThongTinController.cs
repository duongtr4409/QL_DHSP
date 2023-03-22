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
    [AuthorizeRoles(PublicConstant.ROLE_ADMINSTRATOR)]
    public class QuanLyDanhMucThongTinController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/QuanLyDanhMucThongTin
        public ActionResult Index()
        {
            var lstDanhMuc = _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.LISTDANHMUC = lstDanhMuc;

            return View();
        }
        public ActionResult QuanLyDanhMuc()
        {
            var lstDanhMuc = _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().GetAllRecords().OrderBy(x => x.Id).ToList();
            return View(lstDanhMuc);
        }
        public JsonResult ThemMoiDanhMuc(string tendanhmuc)
        {
            try
            {
                if (string.IsNullOrEmpty(tendanhmuc))
                {
                    TempData["error"] = "Tên danh mục không hợp lệ!";
                    return Json("Tên danh mục không hợp lệ!", JsonRequestBehavior.AllowGet);
                }
                DanhMucThongTin danhmuc = _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().GetFirstOrDefaultByParameter(o => o.TenDanhMuc == tendanhmuc);
                if (danhmuc != null)
                {
                    TempData["error"] = "Danh mục đã  tồn tại trên hệ thống";
                    return Json("Danh mục đã  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                danhmuc = new DanhMucThongTin();
                danhmuc.TenDanhMuc = tendanhmuc.Trim();
                danhmuc.Status = true;
                danhmuc.CreatedAt = DateTime.Now;
                danhmuc.UpdatedAt = DateTime.Now;
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                danhmuc.UpdatedBy = user.Username;
                danhmuc.CreatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().Add(danhmuc);
                _unitOfWork.SaveChanges();

                TempData["message"] = "Thêm  mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SuaDanhMuc(long id, string tendanhmuc)
        {
            try
            {
                if (string.IsNullOrEmpty(tendanhmuc))
                {
                    TempData["error"] = "Tên danh mục không hợp lệ!";
                    return Json("Tên danh mục không hợp lệ!", JsonRequestBehavior.AllowGet);
                }
                DanhMucThongTin danhmuc = _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (danhmuc != null)
                {
                    danhmuc.TenDanhMuc = tendanhmuc.Trim();
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    danhmuc.UpdatedBy = user.Username;
                    danhmuc.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().Update(danhmuc);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Bản ghi không tồn tại";
                    return Json("Bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Cập nhật thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                TempData["error"] = "Lỗi Cập nhật: " + ex.Message;
                return Json("Lỗi Cập nhật", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult XoaDanhMuc(long id)
        {
            try
            {
                DanhMucThongTin danhmuc = new DanhMucThongTin();
                danhmuc = _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (danhmuc != null)
                {
                    _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().Remove(danhmuc);
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
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                TempData["error"] = "Lỗi xóa: " + ex.Message;
                return Json("Lỗi xóa", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ThemMoiTruongTT(int idDM, string tenthongtin, string loaithongtin)
        {
            try
            {
                if (string.IsNullOrEmpty(tenthongtin))
                {
                    TempData["error"] = "Tên trường thông tin không hợp lệ!";
                    return Json("Tên trường thông tin không hợp lệ!", JsonRequestBehavior.AllowGet);
                }
                TruongThongTin danhmuc = _unitOfWork.GetRepositoryInstance<TruongThongTin>().GetFirstOrDefaultByParameter(o => o.TenTruongThongTin == tenthongtin && o.IdDanhMuc == idDM);
                if (danhmuc != null)
                {
                    TempData["error"] = "Trường thông tin đã  tồn tại trên hệ thống";
                    return Json("Trường thông tin đã  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                danhmuc = new TruongThongTin();
                danhmuc.TenTruongThongTin = tenthongtin.Trim();
                danhmuc.LoaiTruongThongTin = loaithongtin;
                danhmuc.IdDanhMuc = idDM;
                danhmuc.Status = true;
                danhmuc.CreatedAt = DateTime.Now;
                danhmuc.UpdatedAt = DateTime.Now;
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                danhmuc.UpdatedBy = user.Username;
                danhmuc.CreatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<TruongThongTin>().Add(danhmuc);
                _unitOfWork.SaveChanges();

                TempData["message"] = "Thêm  mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SuaTruongTT(long id, int idDM, string tenthongtin, string loaithongtin)
        {
            try
            {
                if (string.IsNullOrEmpty(tenthongtin))
                {
                    TempData["error"] = "Tên trường thông tin không hợp lệ!";
                    return Json("Tên trường thông tin không hợp lệ!", JsonRequestBehavior.AllowGet);
                }
                TruongThongTin danhmuc = _unitOfWork.GetRepositoryInstance<TruongThongTin>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (danhmuc != null)
                {
                    danhmuc.TenTruongThongTin = tenthongtin.Trim();
                    danhmuc.IdDanhMuc = idDM;
                    danhmuc.LoaiTruongThongTin = loaithongtin;

                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    danhmuc.UpdatedBy = user.Username;
                    danhmuc.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<TruongThongTin>().Update(danhmuc);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Bản ghi không tồn tại";
                    return Json("Bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Cập nhật thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                TempData["error"] = "Lỗi Cập nhật: " + ex.Message;
                return Json("Lỗi Cập nhật", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult XoaTruongTT(long id)
        {
            try
            {
                TruongThongTin truongtt = new TruongThongTin();
                truongtt = _unitOfWork.GetRepositoryInstance<TruongThongTin>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (truongtt != null)
                {
                    _unitOfWork.GetRepositoryInstance<TruongThongTin>().Remove(truongtt);
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
        public ActionResult GetListTruongThongTin(int iddanhmuc)
        {
            List<TruongThongTin> listTruongThongTin = new List<TruongThongTin>();

            listTruongThongTin = _unitOfWork.GetRepositoryInstance<TruongThongTin>().GetListByParameter(x => x.Status == true && x.IdDanhMuc == iddanhmuc).Take(200).OrderByDescending(o => o.Id).ToList();
            return PartialView("_PartialListTruongThongtin", listTruongThongTin);
        }
    }
}