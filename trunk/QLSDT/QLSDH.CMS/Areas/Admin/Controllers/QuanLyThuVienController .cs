using CoreAPI.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.Model;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using TEMIS.CMS.Areas.Admin.Models;
using System.IO;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class QuanLyThuVienController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<KhoaHoc> listKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().ToList();
            ViewBag.ListKhoaHoc = listKhoaHoc;

            var lstHocPHan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetAllRecords().OrderByDescending(o => o.Id).ToList();
            return View(lstHocPHan);
        }


        public async Task<ActionResult> GetListData(int khoahocid = 0, int chuyennganhid = 0, string key = "")
        {
            List<ThuVien> listTV = new List<ThuVien>();
            if (khoahocid > 0)
            {
                listTV = _unitOfWork.GetRepositoryInstance<ThuVien>().GetListByParameter(o => o.KhoaHocId == khoahocid).OrderByDescending(o => o.Id).ToList();
            }
            else if (chuyennganhid > 0 && key.Trim() != "")
            {
                listTV = _unitOfWork.GetRepositoryInstance<ThuVien>().GetListByParameter(o => o.ChuyenNganhId == chuyennganhid && (o.HoTen.Contains(key) || o.MaNCS.Contains(key))).OrderByDescending(o => o.Id).ToList();
            }
            else if (chuyennganhid > 0 && key.Trim() == "")
            {
                listTV = _unitOfWork.GetRepositoryInstance<ThuVien>().GetListByParameter(o => o.ChuyenNganhId == chuyennganhid).OrderByDescending(o => o.Id).ToList();
            }
            else if (chuyennganhid == 0 && key.Trim() != "")
            {
                listTV = _unitOfWork.GetRepositoryInstance<ThuVien>().GetListByParameter(o => o.HoTen.Contains(key) || o.MaNCS.Contains(key)).OrderByDescending(o => o.Id).ToList();
            }
            else
            {
                listTV = _unitOfWork.GetRepositoryInstance<ThuVien>().GetAllRecords().Take(200).OrderByDescending(o => o.Id).ToList();
            }

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<ThuVienViewModel> lisThuVien = new List<ThuVienViewModel>();
            foreach (var item in listTV)
            {
                ThuVienViewModel tv = new ThuVienViewModel();
                tv.Id = item.Id;
                tv.KhoaId = int.Parse(item.KhoaId.ToString());
                tv.TenKhoa = listKhoa.Count > 0 ? listKhoa.Where(o => o.Id == item.KhoaId).SingleOrDefault().Name : "";
                tv.KhoaHocId = int.Parse(item.KhoaHocId.ToString());
                tv.TenKhoahoc = item.KhoaHocId > 0 ? _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == item.KhoaHocId).MaKhoa : "";
                tv.NganhId = int.Parse(item.NganhId.ToString());
                tv.TenNganh = item.NganhId > 0 ? _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == item.NganhId).TenNganh : "";
                tv.ChuyenNganhId = int.Parse(item.ChuyenNganhId.ToString());
                tv.TenChuyenNganh = item.ChuyenNganhId > 0 ? _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == item.ChuyenNganhId).TenChuyenNganh : "";
                tv.HoTen = item.HoTen;
                tv.MaNCS = item.MaNCS;
                if (item.NgaySinh != null)
                {
                    tv.NgaySinh = DateTime.Parse(item.NgaySinh.ToString());
                }

                tv.NopLan1 = bool.Parse(item.NopLan1.ToString());
                tv.UrlFileLan1 = item.UrlFileLan1;
                tv.NopLan2 = bool.Parse(item.NopLan2.ToString());
                tv.UrlFileLan2 = item.UrlFileLan2;
                if (item.QDBV_CapTruong != null)
                {
                    tv.QDBV_CapTruong = DateTime.Parse(item.QDBV_CapTruong.ToString());
                }
                lisThuVien.Add(tv);
            }

            return PartialView("_PartialListData", lisThuVien);
        }

        public JsonResult LoadNganhByKhoa(int khoaid)
        {
            string str = "";
            try
            {
                List<NganhDaoTao> listData = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.Id + "\">" + item.TenNganh + "</option>";
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadNganhEdit(int nganhid)
        {
            string str = "";
            try
            {
                NganhDaoTao data = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == nganhid);
                if (data != null)
                {

                    str += "<option value=\"" + data.Id + "\">" + data.TenNganh + "</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadChuyenNganhByNganh(int nganhid)
        {
            string str = "";
            try
            {
                List<ChuyenNganhDaoTao> listData = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetListByParameter(x => x.NganhId == nganhid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.Id + "\">" + item.TenChuyenNganh + "</option>";
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadChuyenNganhByCNid(int chuyennganhid)
        {
            string str = "";
            try
            {
                ChuyenNganhDaoTao data = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == chuyennganhid);
                if (data != null)
                {
                    str += "<option value=\"" + data.Id + "\">" + data.TenChuyenNganh + "</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        //public async Task<JsonResult> ThemMoi(string khoahocid, string khoaid, string nganhid, string chuyennghanhid, string maNCS, string hoten, string ngaysinh, string quyetdinhngaybaove)
        //{
        //    try
        //    {
        //        ThuVien thuvien = _unitOfWork.GetRepositoryInstance<ThuVien>().GetFirstOrDefaultByParameter(x => x.MaNCS == maNCS);
        //        if (thuvien != null)
        //        {
        //            TempData["message"] = "NCS tồn tại trên hệ thống!";
        //            return Json("error", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            thuvien = new ThuVien();
        //            thuvien.KhoaHocId = int.Parse(khoahocid);
        //            thuvien.KhoaId = int.Parse(khoaid);
        //            thuvien.NganhId = int.Parse(nganhid);
        //            thuvien.ChuyenNganhId = int.Parse(chuyennghanhid);
        //            thuvien.HoTen = hoten;
        //            thuvien.MaNCS = maNCS;
        //            if (quyetdinhngaybaove != null || quyetdinhngaybaove != "")
        //            {
        //                thuvien.QDBV_CapTruong = DateTime.Parse(quyetdinhngaybaove);
        //            }
        //            if (ngaysinh != null || ngaysinh != "")
        //            {
        //                thuvien.NgaySinh = DateTime.Parse(ngaysinh);
        //            }

        //            if (Request.Files["fnoplan1"] != null)
        //            {
        //                HttpPostedFileBase fnoplan1 = Request.Files["fnoplan1"];
        //                string Url_FileUpload_ThuVien1 = fnoplan1.FileName;
        //                fnoplan1.SaveAs(Server.MapPath("~/Upload//Thuvien/" + Url_FileUpload_ThuVien1));
        //                thuvien.UrlFileLan1 = Url_FileUpload_ThuVien1;
        //                thuvien.NopLan1 = true;
        //            }
        //            else
        //            {
        //                thuvien.NopLan1 = false;
        //            }


        //            if (Request.Files["fnoplan2"] != null)
        //            {
        //                HttpPostedFileBase fnoplan2 = Request.Files["fnoplan2"];
        //                string Url_FileUpload_ThuVien2 = fnoplan2.FileName;
        //                fnoplan2.SaveAs(Server.MapPath("~/Upload//Thuvien/" + Url_FileUpload_ThuVien2));
        //                thuvien.UrlFileLan2 = Url_FileUpload_ThuVien2;
        //                thuvien.NopLan2 = true;
        //            }
        //            else
        //            {
        //                thuvien.NopLan2 = false;
        //            }

        //            thuvien.CreatedAt = DateTime.Now;
        //            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
        //            thuvien.CreatedBy = user.Username;
        //            _unitOfWork.GetRepositoryInstance<ThuVien>().Add(thuvien);
        //            _unitOfWork.SaveChanges();
        //        }
        //        TempData["message"] = "Thêm  mới thành công!";
        //        return Json("OK", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = "Lỗi thêm mới: " + ex.Message;
        //        return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public async Task<JsonResult> Sua(long id, string khoahocid, string khoaid, string nganhid, string chuyennghanhid, string maNCS, string hoten, string ngaysinh, string quyetdinhngaybaove)
        //{
        //    try
        //    {
        //        ThuVien thuvien = _unitOfWork.GetRepositoryInstance<ThuVien>().GetFirstOrDefaultByParameter(x => x.Id == id);
        //        if (thuvien == null)
        //        {
        //            TempData["message"] = "Bản ghi không tồn tại!";
        //            return Json("error", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            thuvien.KhoaHocId = int.Parse(khoahocid);
        //            thuvien.KhoaId = int.Parse(khoaid);
        //            thuvien.NganhId = int.Parse(nganhid);
        //            thuvien.ChuyenNganhId = int.Parse(chuyennghanhid);
        //            thuvien.HoTen = hoten;
        //            thuvien.MaNCS = maNCS;
        //            if (quyetdinhngaybaove != null || quyetdinhngaybaove != "")
        //            {
        //                thuvien.QDBV_CapTruong = DateTime.Parse(quyetdinhngaybaove);
        //            }
        //            if (ngaysinh != null || ngaysinh != "")
        //            {
        //                thuvien.NgaySinh = DateTime.Parse(ngaysinh);
        //            }

        //            if (Request.Files["fnoplan1"] != null)
        //            {
        //                // delete file 
        //                if (System.IO.File.Exists("~/Upload//Thuvien/" + thuvien.UrlFileLan1))
        //                {
        //                    System.IO.File.Delete("~/Upload//Thuvien/" + thuvien.UrlFileLan1);
        //                }
        //                HttpPostedFileBase fnoplan1 = Request.Files["fnoplan1"];
        //                string Url_FileUpload_ThuVien1 = fnoplan1.FileName;
        //                fnoplan1.SaveAs(Server.MapPath("~/Upload//Thuvien/" + Url_FileUpload_ThuVien1));
        //                thuvien.UrlFileLan1 = Url_FileUpload_ThuVien1;
        //                thuvien.NopLan1 = true;
        //            }
        //            else
        //            {
        //                thuvien.NopLan1 = false;
        //            }

        //            if (Request.Files["fnoplan2"] != null)
        //            {
        //                // delete file 
        //                if (System.IO.File.Exists("~/Upload//Thuvien/" + thuvien.UrlFileLan2))
        //                {
        //                    System.IO.File.Delete("~/Upload//Thuvien/" + thuvien.UrlFileLan2);
        //                }
        //                HttpPostedFileBase fnoplan2 = Request.Files["fnoplan2"];
        //                string Url_FileUpload_ThuVien2 = fnoplan2.FileName;
        //                fnoplan2.SaveAs(Server.MapPath("~/Upload//Thuvien/" + Url_FileUpload_ThuVien2));
        //                thuvien.UrlFileLan2 = Url_FileUpload_ThuVien2;
        //                thuvien.NopLan2 = true;
        //            }
        //            else
        //            {
        //                thuvien.NopLan2 = false;
        //            }

        //            thuvien.CreatedAt = DateTime.Now;
        //            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
        //            thuvien.CreatedBy = user.Username;
        //            _unitOfWork.GetRepositoryInstance<ThuVien>().Update(thuvien);
        //            _unitOfWork.SaveChanges();
        //        }
        //        TempData["message"] = "Cập nhật thành công!";
        //        return Json("OK", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = "Lỗi thêm mới: " + ex.Message;
        //        return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public async Task<JsonResult> Xoa(long id)
        //{
        //    try
        //    {
        //        ThuVien thuvien = new ThuVien();
        //        thuvien = _unitOfWork.GetRepositoryInstance<ThuVien>().GetFirstOrDefaultByParameter(o => o.Id == id);
        //        if (thuvien != null)
        //        { // delete file 
        //            if (System.IO.File.Exists("~/Upload//Thuvien/" + thuvien.UrlFileLan1))
        //            {
        //                System.IO.File.Delete("~/Upload//Thuvien/" + thuvien.UrlFileLan1);
        //            }
        //            if (System.IO.File.Exists("~/Upload//Thuvien/" + thuvien.UrlFileLan2))
        //            {
        //                System.IO.File.Delete("~/Upload//Thuvien/" + thuvien.UrlFileLan2);
        //            }

        //            _unitOfWork.GetRepositoryInstance<ThuVien>().Remove(thuvien);
        //            _unitOfWork.SaveChanges();
        //        }
        //        else
        //        {
        //            TempData["error"] = "Bản ghi không tồn tại";
        //            return Json("Bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
        //        }

        //        TempData["message"] = "Xóa thành công!";
        //        return Json("OK", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = "Lỗi xóa: " + ex.Message;
        //        return Json("Lỗi xóa", JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> UploadSoLanNop()
        {
            try
            {
                string fileUrl = "";
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                int id = Request.Form["hdfId"] != null ? int.Parse(Request.Form["hdfId"].ToString()) : 0;
                string mancs = Request.Form["hdfMaNCS"] != null ? Request.Form["hdfMaNCS"].ToString() : "";
                int lan = Request.Form["hdfLan"] != null ? int.Parse(Request.Form["hdfLan"].ToString()) : 0;
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] ffiles = file.FileName.Split(new char[] { '\\' });
                            fname = user.Username + "_" + ffiles[ffiles.Length - 1];
                        }
                        else
                        {
                            fname = user.Username + "_" + file.FileName;
                        }
                        fname = Path.Combine(Server.MapPath("~/Upload/Thuvien/"), fname);
                        file.SaveAs(fname);

                        fileUrl = fname;
                    }
                }
                else
                {
                    TempData["error"] = "Bạn chưa chọn file upload";
                    return Json("Bạn chưa chọn file upload", JsonRequestBehavior.AllowGet);
                }

                ThuVien thuvien = _unitOfWork.GetRepositoryInstance<ThuVien>().GetFirstOrDefaultByParameter(o => o.Id == id && o.MaNCS == mancs);
                if (thuvien != null)
                {
                    if (lan == 1)
                    {
                        thuvien.NopLan1 = true;
                        thuvien.UrlFileLan1 = fileUrl;
                    }
                    else if (lan == 2)
                    {
                        thuvien.NopLan2 = true;
                        thuvien.UrlFileLan2 = fileUrl;
                    }
                    thuvien.UpdatedAt = DateTime.Now;
                    thuvien.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<ThuVien>().Update(thuvien);
                    _unitOfWork.SaveChanges();
                }

                TempData["message"] = "Upload file thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi upload file: " + ex.Message;
                return Json("Lỗi upload", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult XoaSoLanNop(int id, int lan)
        {
            string mesage = "";
            try
            {
                ThuVien tv = _unitOfWork.GetRepositoryInstance<ThuVien>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (tv != null)
                {
                    if (lan == 1)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Thuvien/" + tv.UrlFileLan1));
                        tv.NopLan1 = false;
                        tv.UrlFileLan1 = "";
                    }
                    else if (lan == 2)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Thuvien/" + tv.UrlFileLan2));
                        tv.NopLan2 = false;
                        tv.UrlFileLan2 = "";
                    }

                    _unitOfWork.GetRepositoryInstance<ThuVien>().Update(tv);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi xóa số lần nộp: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }
    }
}