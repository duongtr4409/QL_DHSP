using CoreAPI.Entity;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    [AuthorizeRoles(PublicConstant.ROLE_ADMINSTRATOR, PublicConstant.ROLE_CB_PHONG_TAI_CHINH)]
    public class QuanLyHocPhiController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: Admin/QuanLyHocPhi
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            List<HocPhiNCS> listHP = _unitOfWork.GetRepositoryInstance<HocPhiNCS>().GetAllRecords().ToList();
            ViewBag.ListKhoa = list;
            List<KhoaHoc> ListKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().ToList();
            ViewBag.ListKhoaHoc = ListKhoaHoc;
            return View(listHP);
        }
        public async System.Threading.Tasks.Task<ActionResult> ApproveList()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;
            List<KhoaHoc> ListKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().ToList();
            ViewBag.ListKhoaHoc = ListKhoaHoc;
            List<HocPhi> lstHP = _unitOfWork.GetRepositoryInstance<HocPhi>().GetListByParameter(x=>x.Type != 1).ToList();
            return View(lstHP);
        }

        public async System.Threading.Tasks.Task<ActionResult> MucHocPhi()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;
            List<NganhDaoTao> listNganhDaoTao = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetAllRecords().ToList();
            ViewBag.ListNganhDaoTao = listNganhDaoTao;
            List<KhoaHoc> ListKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().ToList();
            ViewBag.ListKhoaHoc = ListKhoaHoc;
            return View();
        }
        public ActionResult LoadDataHocPhi(int khoaid = -1, int khoahocid = -1)
        {
            try
            {
                List<HocPhi> list_data = new List<HocPhi>();
                
                if (khoaid <= 0 && khoahocid <= 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<HocPhi>().GetAllRecords().ToList();
                }
                else if (khoahocid != 0 && khoaid != 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<HocPhi>().GetListByParameter(x => x.KhoaHoc == khoahocid && x.Khoa == khoaid).ToList();
                }
                else if (khoahocid > 0 && khoaid <= 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<HocPhi>().GetListByParameter(x => x.KhoaHoc == khoahocid).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<HocPhi>().GetListByParameter(x =>x.Khoa == khoaid).ToList();
                }
                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                return PartialView("_PartialHocPhi", list_data);

            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                return null;
            }
        }
        public JsonResult LoadNganhByKhoa(int khoaid)
        {
            string str = "";
            try
            {
                List<NganhDaoTao> listData = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.MaNganh + "\">" + item.TenNganh + "</option>";
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadMonHocByNganh(int nganhid)
        {
            string str = "";
            try
            {
                List<MonHoc> listData = _unitOfWork.GetRepositoryInstance<MonHoc>().GetListByParameter(x => x.NganhId == nganhid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.Id + "\">" + item.TenMon + "</option>";
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> LoadData(string khoaid, string nganhid, string khoahocid)
        {
            try
            {
                List<MucHocPhi> list_data = new List<MucHocPhi>();
                if (khoaid == "0")
                {
                    list_data = _unitOfWork.GetRepositoryInstance<MucHocPhi>().GetAllRecords().ToList();
                }
                else if (nganhid == "0" || nganhid == "")
                {
                    list_data = _unitOfWork.GetRepositoryInstance<MucHocPhi>().GetListByParameter(x => x.MaKhoa == khoaid).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<MucHocPhi>().GetListByParameter(x => x.MaNganh == nganhid).ToList();
                }
                if (khoahocid != "0")
                {
                    list_data = list_data.Where(x => x.NamHoc.Equals(khoahocid)).ToList();
                }
                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                return PartialView("_PartialMucHocPhi", list_data);

            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                throw;
            }
        }

        public JsonResult ThemMoiMucHocPhi(string khoa, string tenkhoa, string manganh, string tennganh, string namhoc, float hocphi)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                MucHocPhi muchocphi = new MucHocPhi();
                muchocphi.MaKhoa = khoa;
                muchocphi.TenKhoa = tenkhoa;
                muchocphi.NamHoc = namhoc;
                muchocphi.HocPhi = hocphi;
                muchocphi.MaNganh = manganh;
                muchocphi.TenNganh = tennganh;
                muchocphi.CreatedAt = DateTime.Now;
                muchocphi.CreatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<MucHocPhi>().Add(muchocphi);
                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateMucHocPhi(int id, string khoa, string manganh, string tennganh, string tenkhoa, string namhoc, float hocphi)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var muchocphi = _unitOfWork.GetRepositoryInstance<MucHocPhi>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (muchocphi != null)
                {
                    muchocphi.MaKhoa = khoa;
                    muchocphi.TenKhoa = tenkhoa;
                    muchocphi.NamHoc = namhoc;
                    muchocphi.HocPhi = hocphi;
                    muchocphi.MaNganh = manganh;
                    muchocphi.TenNganh = tennganh;
                    muchocphi.UpdatedAt = DateTime.Now;
                    muchocphi.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<MucHocPhi>().Update(muchocphi);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Cập nhật bản ghi lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateStatus(int id, int status)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var hocphi = _unitOfWork.GetRepositoryInstance<HocPhi>().GetFirstOrDefaultByParameter(x => x.Id == id);
                var hocphiNCS = new HocPhiNCS();
                if (hocphi != null)
                {
                    hocphi.UpdatedAt = DateTime.Now;
                    hocphi.UpdatedBy = user.Username;
                    hocphi.TrangThai = status;
                    hocphiNCS.MaHV = hocphi.MaNCS;
                    hocphiNCS.HoTen = hocphi.HoTen;
                    hocphiNCS.TongTien = hocphi.MucNop;
                    hocphiNCS.Email = hocphi.UserName;
                    ThongBao thongbao = new ThongBao();
                    thongbao.Email = hocphiNCS.Email;
                    thongbao.MaNCS = hocphiNCS.MaHV;
                    thongbao.CreatedAt = DateTime.Now;
                    thongbao.UpdatedAt = DateTime.Now;
                    thongbao.CreatedBy = user.Username;
                    thongbao.UpdatedBy = user.Username;
                    if (status == PublicConstant.DA_NOP)
                    {
                        hocphiNCS.DaTra = hocphi.MucNop;
                        hocphiNCS.HoanThanh = true;
                        thongbao.Title = "Nộp học phí thành công";
                    }
                    else
                    {
                        hocphiNCS.HoanThanh = false;
                        thongbao.Title = "Nộp học phí lỗi";
                    }
                    _unitOfWork.GetRepositoryInstance<HocPhiNCS>().Add(hocphiNCS);
                    _unitOfWork.GetRepositoryInstance<HocPhi>().Update(hocphi);
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Cập nhật trạng thái lỗi", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("Cập nhật trạng thái lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult XoaMucHocPhi(int id)
        {
            string mesage = "";
            try
            {
                MucHocPhi muchocphi = _unitOfWork.GetRepositoryInstance<MucHocPhi>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (muchocphi != null)
                {
                    _unitOfWork.GetRepositoryInstance<MucHocPhi>().Remove(muchocphi);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi xóa bản ghi: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadExcelFile(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var xs = Request.Form["Ftth"];
                    try
                    {
                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;

                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "File không đúng địng dạng");
                            TempData["error"] = "File không đúng địng dạng";
                            return View();
                        }
                        var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                        reader.IsFirstRowAsColumnNames = true;
                        DataSet result = reader.AsDataSet();
                        DataTable dt = result.Tables[0];
                        string fileExelName = upload.FileName;
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    HocPhiNCS item = new HocPhiNCS();
                                    item.MaHV = row.ItemArray[1].ToString();
                                    item.HoTen = row.ItemArray[2].ToString();
                                    item.TongTien = (double)row.ItemArray[3];
                                    item.DaTra = (double)row.ItemArray[4];
                                    item.CreatedAt = DateTime.Now;
                                    item.UpdatedAt = DateTime.Now;
                                    item.CreatedBy = user.Username;
                                    item.UpdatedBy = user.Username;
                                    if ((double)(row.ItemArray[6]) == 1)
                                    {
                                        item.HoanThanh = true;
                                    }
                                    else if ((double)(row.ItemArray[6]) == 0)
                                    {
                                        item.HoanThanh = false;
                                    }
                                    //qua han la Q
                                    if (row.ItemArray[7].ToString().Equals("Q") || row.ItemArray[7].ToString().Equals("q"))
                                    {
                                        item.QuaHan = true;
                                    }
                                    else if (row.ItemArray[7].ToString().Equals("K") || row.ItemArray[67].ToString().Equals("k"))
                                    {
                                        item.QuaHan = false;
                                    }
                                    _unitOfWork.GetRepositoryInstance<HocPhiNCS>().Add(item);
                                    _unitOfWork.SaveChanges();
                                }
                                TempData["message"] = "Import danh sách thành công!";
                                return RedirectToAction("Index");
                            }
                            catch (Exception ex)
                            {
                                TempData["error"] = "File dữ liệu không phù hợp với bản mẫu!";
                            }

                        }
                        else
                        {
                            TempData["error"] = "File không có dữ liệu!";
                        }
                        reader.Close();
                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "Lỗi xảy ra trong quá trình import file excel:" + ex.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "File không có dữ liệu !";
                }
            }
            return RedirectToAction("Index");
        }

    }
}