using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
using CoreAPI.Entity;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class QuanLyThamSoBieuMauController : Controller
    {

        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ThamSoBieuMau
        public async Task<ActionResult> Index()
        {
            List<BieuMau> list = _unitOfWork.GetRepositoryInstance<BieuMau>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListBieuMau = list;
            return View();
        }

        public async Task<ActionResult> LoadData()
        {
            try
            {
                List<ThamSoBieuMau> list_data = new List<ThamSoBieuMau>();

                list_data = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetAllRecords().OrderByDescending(x => x.TenThamSo).ToList();

                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                return PartialView("_PartialListData", list_data);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult ThemMoi(string tenthamso, string giatrithamso, int idbm, string kieudl, string cautruc, int thutuhienthi = 0)
        {
            try
            {
                var thamso = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetFirstOrDefaultByParameter(x => x.TenThamSo == tenthamso.Trim() && x.BieuMauId == idbm);
                if (thamso != null)
                {
                    return Json("Tên tham số đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var bieuMau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == idbm);
                    if (bieuMau == null)
                    {
                        return Json("Biểu mẫu chọn không tồn tại", JsonRequestBehavior.AllowGet);
                    }
                    thamso = new ThamSoBieuMau();
                    thamso.TenThamSo = tenthamso;
                    thamso.GiaTriThamSo = giatrithamso;
                    thamso.BieuMau = bieuMau.Template;
                    thamso.BieuMauId = bieuMau.Id;
                    thamso.KieuDuLieu = kieudl;
                    thamso.CauTrucHienThi = cautruc;
                    thamso.ThuTuHienThi = thutuhienthi;
                    thamso.CreatedAt = DateTime.Now;
                    thamso.UpdatedAt = DateTime.Now;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    thamso.CreatedBy = user.Username;
                    thamso.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().Add(thamso);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Thêm mới giá tri lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Update(string tenthamso, string giatrithamso, int idbm, string kieudl, string cautruc, int thutuhienthi)
        {
            try
            {
                var thamso = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetFirstOrDefaultByParameter(x => x.TenThamSo == tenthamso);
                if (thamso != null)
                {
                    var bieuMau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == idbm);
                    if (bieuMau == null)
                    {
                        return Json("Biểu mẫu chọn không tồn tại", JsonRequestBehavior.AllowGet);
                    }
                    thamso.TenThamSo = tenthamso;
                    thamso.GiaTriThamSo = giatrithamso;
                    thamso.BieuMau = bieuMau.Template;
                    thamso.BieuMauId = bieuMau.Id;
                    thamso.KieuDuLieu = kieudl;
                    thamso.CauTrucHienThi = cautruc;
                    thamso.ThuTuHienThi = thutuhienthi;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    thamso.UpdatedAt = DateTime.Now;
                    thamso.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().Update(thamso);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("giá tri không tồn tại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("cập nhật giá tri lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult XoaThamSo(string tenthamso)
        {
            string mesage = "";
            try
            {
                ThamSoBieuMau checkNganh = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetFirstOrDefaultByParameter(o => o.TenThamSo == tenthamso);
                if (checkNganh != null)
                {
                    _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().Remove(checkNganh);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi xóa bản ghi: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }
    }
}