using CoreAPI.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using AWord = Microsoft.Office.Interop.Word;
using System.Web.UI.WebControls;
using System.Net.Mail;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;
using System.Data;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using FontSize = DocumentFormat.OpenXml.Wordprocessing.FontSize;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using TableCellProperties = DocumentFormat.OpenXml.Wordprocessing.TableCellProperties;
using System.Net;
using System.Globalization;
using TEMIS.CMS.Areas.Admin.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO.Compression;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class ThiSinhTuyenSinhController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private TEMIS_systemEntities db = new TEMIS_systemEntities();
        public string urlFile = "theme_admin\\FileBieuMau\\";
        public string parthdowload = "Upload\\FileBMDowload\\";
        //public string url_download = "http://14.225.5.64:8765/upload/FileBMDowload/";
        public string url_download = "http://qlncs.hnue.edu.vn/upload/FileBMDowload/";
        public string EmailSendTest = WebConfigurationManager.AppSettings["Email_Send_Test"].ToString();
        public string url_domain = "http://localhost:50498/";
        //public string url_domain = "http://qlncs.hnue.edu.vn/";

        // GET: Admin/ThiSinhTuyenSinh
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;


            var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            int staffid = int.Parse(loginInfo.StaffId.ToString());
            var user = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
            int department = user.DepartmentId;
            var idkhoa = listKhoa.Where(x => x.Id == department).FirstOrDefault();
            if (idkhoa != null)
            {
                ViewBag.IDKHOA = idkhoa;
            }
            else
            {
                ViewBag.IDKHOA = -1;
            }
            ViewBag.IDKHOA = 78;
            return View();
        }

        public async Task<ActionResult> GetListDangKyTuyenSinh(int chuyennghanhid = 0)
        {
            var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            var roleinfo = (UserRoles)Session[PublicConstant.ROLE_INFO];

            int staffid = int.Parse(loginInfo.StaffId.ToString());
            var user = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
            int department = user.DepartmentId;
            var keyKhoacheck = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(x => x.SKey.Equals("TestKhoa"));
            if (keyKhoacheck != null && !string.IsNullOrEmpty(keyKhoacheck.Value))
            {
                department = int.Parse(keyKhoacheck.Value);
            }
            List<Model.DangKyTuyenSinh> listDangKyTuyenSinh = new List<DangKyTuyenSinh>();
            if (chuyennghanhid > 0)
            {
                listDangKyTuyenSinh = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetListByParameter(o => o.ChuyenNghanhDuTuyenId == chuyennghanhid && o.Status == PublicConstant.STT_CHODUYET).OrderBy(o => o.HoTen).ToList();
            }
            else
            {
                listDangKyTuyenSinh = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetListByParameter(x => x.Status == PublicConstant.STT_CHODUYET).Take(200).OrderBy(o => o.HoTen).ToList();
            }
            if (roleinfo.Role.Equals(PublicConstant.ROLE_TRUONG_KHOA_DT))
            {
                listDangKyTuyenSinh = listDangKyTuyenSinh.Where(x => x.KhoaId == department).ToList();
            }
            return PartialView("_PartialListDanhSachDangKyTuyenSinh", listDangKyTuyenSinh);
        }

        public async Task<ActionResult> GetListDangKyTuyenSinhXetDuyet(int chuyennghanhid = 0)
        {
            List<Model.DangKyTuyenSinh> listDangKyTuyenSinh = new List<DangKyTuyenSinh>();
            if (chuyennghanhid > 0)
            {
                listDangKyTuyenSinh = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetListByParameter(o => o.ChuyenNghanhDuTuyenId == chuyennghanhid && (o.Status == PublicConstant.STT_DUYET || o.Status == PublicConstant.STT_XETTUYEN)).OrderBy(o => o.HoTen).ToList();
            }
            else
            {
                listDangKyTuyenSinh = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetListByParameter(x => x.Status == PublicConstant.STT_DUYET || x.Status == PublicConstant.STT_XETTUYEN).Take(200).OrderBy(o => o.HoTen).ToList();
            }

            return PartialView("_PartialListXetDuyetTuyenSinh", listDangKyTuyenSinh);
        }

        public JsonResult LoadNganhByKhoa(int khoaid)
        {
            string str = "";
            try
            {
                List<NganhDaoTao> listData = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn Ngành --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.Id + "\">" + item.TenNganh + "</option>";
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- Chọn Ngành --------</option>";
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
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.Id + "\">" + item.TenChuyenNganh + "</option>";
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
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

        public async System.Threading.Tasks.Task<JsonResult> LoadGiangVienByKhoa(int khoaid, int id_NHD)
        {
            string str = "";
            try
            {
                List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);

                if (listGVtrong.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listGVtrong)
                    {
                        if (item.Id == id_NHD)
                        {
                            str += "<option selected value=\"" + item.Id + "\">" + item.Name + "</option>";
                        }
                        else
                        {
                            str += "<option value=\"" + item.Id + "\">" + item.Name + "</option>";
                        }
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public async System.Threading.Tasks.Task<JsonResult> LoadGiangVienByKhoaWithHHHV(int khoaid, int? id_NHD = -1)
        {
            string str = "";
            try
            {
                List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
                List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();

                if (listGVtrong.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listGVtrong)
                    {
                        var hocham = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault();
                        if (hocham!=null)
                        {
                            if (item.Id == id_NHD)
                            {
                                str += "<option selected value=\"" + item.Id + "\">" + item.Name + " (" + hocham.Name + ")</option>";
                            }
                            else
                            {
                                str += "<option value=\"" + item.Id + "\">" + item.Name + " (" + hocham.Name + ")</option>";
                            }
                        }                       
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> LoadGiangVienByNganh(int nganhid)
        {
            string str = "";
            try
            {
                var chuyennganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == nganhid);
                if (chuyennganh == null)
                {

                }
                else
                {
                    int khoaid = chuyennganh.KhoaId.Value;
                    List<GiangVienAPI> listGV = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
                    if (listGV.Count > 0)
                    {
                        str += "<option value=\"0\">--------- Chọn --------</option>";
                        foreach (var item in listGV)
                        {
                            str += "<option value=\"" + item.Id + "\">" + item.Name + "</option>";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDotTSByKhoahoc(int IDkhoahoc)
        {
            string str = "";
            try
            {
                var khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(x => x.Id == IDkhoahoc);
                if (khoahoc == null)
                {

                }
                else
                {
                    List<DotTuyenSinh> listDotTS = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetListByParameter(x => x.idKhoahoc == IDkhoahoc && x.Status == PublicConstant.DANGTUYENSINH).ToList();
                    if (listDotTS.Count > 0)
                    {
                        //str += "<option value=\"0\">--- Chọn đợt TS ---</option>";
                        foreach (var item in listDotTS)
                        {
                            str += "<option value=\"" + item.Id + "\">" + item.TenDot + "</option>";
                        }
                    }
                    else
                    {
                        str += "<option value=\"0\">Không có đợt TS cho khóa này</option>";
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> LoadKhoaByDot(int khoahocid, int dotid)
        {
            string str = "";
            try
            {
                List<int> lstKhoa = new List<int>();
                List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                List<DanhSachCanBoAddForm> listData = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdKhoahoc == khoahocid && x.IdDotTS == dotid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn khoa --------</option>";
                    foreach (var item in listData)
                    {
                        var khoa = listKhoa.Where(x => x.Id == item.IdKhoa).FirstOrDefault();
                        string name = khoa != null ? khoa.Name : item.CoQuanCongTac;
                        if (!lstKhoa.Contains(item.IdKhoa.Value))
                        {
                            str += "<option value=\"" + item.IdKhoa + "\">" + name + "</option>";
                            lstKhoa.Add(item.IdKhoa.Value);
                        }

                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDotByKhoaHoc(int khoahocId)
        {
            string str = "";
            try
            {
                List<DotTuyenSinh> listData = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetListByParameter(x => x.idKhoahoc == khoahocId).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.Id + "\">" + item.TenDot + "</option>";
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HoiDongDanhGia()
        {
            return View();
        }
        public ActionResult QDHoiDongDanhGia()
        {
            return View();
        }
        public ActionResult UpadteHoiDongDG()
        {
            return View();
        }
        public ActionResult YKienDanhGia()
        {
            return View();
        }
        public async Task<ActionResult> ThemMoi()
        {
            List<City> listCity = _unitOfWork.GetRepositoryInstance<City>().GetAllRecords().OrderBy(x => x.Name).ToList();
            ViewBag.ListCity = listCity;
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;
            List<ChuyenNganhDaoTao> list = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListChuyenNganhDaoTao = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThemMoiTS(FormCollection form)
        {
            try
            {
                string HoTen = form["hovaten"] != null ? form["hovaten"] : "";
                string GioiTinh = form["gioitinh"] != null ? form["gioitinh"] : "";
                DateTime NgaySinh = form["ngaysinh"] != null ? DateTime.ParseExact(form["ngaysinh"],"dd-MM-yyyy", CultureInfo.CurrentCulture) : DateTime.Now;
                string SoDienThoai = form["sdt"] != null ? form["sdt"] : "";
                string Email = form["email"] != null ? form["email"] : "";
                string NoiSinh = form["noisinh"] != null ? form["noisinh"] : "noisinh";
                string DiaChiLienLac = form["diachi"] != null ? form["diachi"] : "";
                string NgheNghiep = form["nghenghiep"] != null ? form["nghenghiep"] : "";
                string CoQuanCongTacHienNay = form["coquan"] != null ? form["coquan"] : "";
                string NamBatDauCongTac = form["namct"] != null ? form["namct"] : "";
                string HienLaCanBo = form["canbo"] != null ? form["canbo"] : "";
                string ViTriConViecHienTai = form["vitricv"] != null ? form["vitricv"] : "";
                string ThamNiemNghieNghiep = form["thamnien"] != null ? form["thamnien"] : "";
                string ChuyenMon = form["chuyenmon"] != null ? form["chuyenmon"] : "";
                string TenDeTai = form["tendetai"] != null ? form["tendetai"] : "";
                string Truong_TN_DaiHoc = form["tentruongct"] != null ? form["tentruongct"] : "";
                int Nam_TN_DaiHoc = form["namtn"] != null ? int.Parse(form["namtn"]) : 0;
                string HeDaoTao_DaiHoc = form["hedt"] != null ? form["hedt"] : "";
                string Nghanh_TN_DaiHoc = form["nganhtn"] != null ? form["nganhtn"] : "";
                string DiemTrungBinh_DaiHoc = form["diemTb"] != null ? form["diemTb"] : "";
                string LoaiTotNghiep_DaiHoc = form["loaitn"] != null ? form["loaitn"] : "";

                HttpPostedFileBase filevbdh = Request.Files["filevbdh"];
                string Url_FileUpload_DaiHoc = filevbdh.FileName;

                // văn bằng 2
                string Truong_TN_VB2 = form["tentruong_vb2"] != null ? form["tentruong_vb2"] : "";
                int Nam_TN_VB2 = form["namtn_vb2"] != "" && form["namtn_vb2"] != null ? int.Parse(form["namtn_vb2"]) : 0;
                string HeDaoTao_VB2 = form["hedt_vb2"] != null ? form["hedt_vb2"] : "";
                string Nghanh_TN_VB2 = form["nganhtn_vb2"] != null ? form["nganhtn_vb2"] : "";
                string DiemTrungBinh_VB2 = form["diemTb_vb2"] != null ? form["diemTb_vb2"] : "";
                string LoaiTotNghiep_VB2 = form["loaitn_vb2"] != null ? form["loaitn_vb2"] : "";

                //CMND
                string cmnd = form["cmnd"] != null ? form["cmnd"] : "";
                string cmnd_noicap = form["cmnd_noicap"] != null ? form["cmnd_noicap"] : "";
                DateTime cmnd_ngaycap = form["cmnd_ngaycap"] != null ? DateTime.ParseExact(form["cmnd_ngaycap"].Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture) : DateTime.Now;

                string cmnd_tinhthanh = form["cmnd_tinhthanh"] != null ? form["cmnd_tinhthanh"] : "";
                string cmnd_huyen = form["cmnd_huyen"] != null ? form["cmnd_huyen"] : "";
                string cmnd_xa = form["cmnd_xa"] != null ? form["cmnd_xa"] : "";

                HttpPostedFileBase filevb_vb2 = Request.Files["filevb_vb2"];
                string Url_FileUpload_VB2 = filevb_vb2.FileName;


                string Truong_TN_ThacSi = form["tentruongts"] != null ? form["tentruongts"] : "";
                string Nam_TN_ThacSi = form["namtnts"] != null ? form["namtnts"] : "";
                string HeDaoTao_ThacSi = form["hedtts"] != null ? form["hedtts"] : "";
                string Nghanh_TN_ThacSi = form["nganhts"] != null ? form["nganhts"] : "";
                string DiemTrungBinh_ThacSi = form["diemTbts"] != null ? form["diemTbts"] : "";

                HttpPostedFileBase filevbts = Request.Files["filevbts"];
                string Url_FileUpload_ThacSi = filevbts.FileName;

                string NgoaiNgu = form["ngoaingu"] != null ? form["ngoaingu"] : "";
                string LoaiVanBangNgoaiNgu = form["vanbang"] != null ? form["vanbang"] : "";

                HttpPostedFileBase filenn = Request.Files["filenn"];
                string Url_ChungChiNgoaiNgu = filenn.FileName;

                string BoTucKienThuc = form["botuckthuc"] != null ? form["botuckthuc"] : "";
                int ChuyenNghanhDuTuyenId = form["chuyennganhdt"] != null ? int.Parse(form["chuyennganhdt"]) : 0;
                string tenchuyennganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == ChuyenNghanhDuTuyenId).TenChuyenNganh;
                string TenChuyenNghanhDuTuyen = tenchuyennganh;
                string DoiTuongDuTuyen = form["dtdt"] != null ? form["dtdt"] : "";
                string ThoiGianHinhThucDaoTao = form["thoigiandt"] != null ? form["thoigiandt"] : "";

                //Văn bằng đính kèm
                HttpPostedFileBase filesyll = Request.Files["filesyll"];
                string Url_FileUpload_AnhSoYeuLyLich = filesyll.FileName;
                HttpPostedFileBase filecvgt = Request.Files["filecvgt"];
                string Url_FileUpload_CongVanGioiThieu = filecvgt.FileName;
                HttpPostedFileBase filegksk = Request.Files["filegksk"];
                string Url_FileUpload_GiaySucKhoe = filegksk.FileName;
                HttpPostedFileBase filehdld = Request.Files["filehdld"];
                string Url_FileUpload_HopDongLaoDong = filehdld.FileName;
                HttpPostedFileBase filetgt = Request.Files["filetgt"];
                string Url_FileUpload_ThuGioiThieu = filetgt.FileName;
                HttpPostedFileBase filebbkh = Request.Files["filebbkh"];
                string Url_FileUpload_BaiBaoKhoaHoc = filebbkh.FileName;
                HttpPostedFileBase filedcnc = Request.Files["filedcnc"];
                string Url_FileUpload_DeCuongNghienCuu = filedcnc.FileName;

                //Người hướng dẫn
                int khoaId_NHD1 = form["ddlKhoa_1"] != null ? int.Parse(form["ddlKhoa_1"]) : 0;
                int Id_NHD1 = form["ddlGV_1"] != null ? int.Parse(form["ddlGV_1"]) : 0;
                List<GiangVienAPI> listGV = await CoreAPI.CoreAPI.GetListGiangVien(khoaId_NHD1);
                string nguoihuongdan1 = "", nguoihuongdan2 = "";
                if (Id_NHD1 != 0)
                {
                    nguoihuongdan1 = listGV.Where(x => x.Id == Id_NHD1).FirstOrDefault().Name;
                }
                int khoaId_NHD2 = 0;
                int Id_NHD2 = 0;
                string loaiGV_2 = form["loaiGV_2"] != null ? form["loaiGV_2"] : "";
                if (loaiGV_2 == "0")
                {
                    khoaId_NHD2 = form["ddlKhoa_2"] != null ? int.Parse(form["ddlKhoa_2"]) : 0;
                    Id_NHD2 = form["ddlGV_2"] != null ? int.Parse(form["ddlGV_2"]) : 0;
                    listGV = await CoreAPI.CoreAPI.GetListGiangVien(khoaId_NHD2);
                    if (Id_NHD2 != 0)
                    {
                        nguoihuongdan2 = listGV.Where(x => x.Id == Id_NHD2).FirstOrDefault().Name;
                    }
                }
                else
                {
                    khoaId_NHD2 = 0;
                    Id_NHD2 = 0;
                    nguoihuongdan2 = form["tenGV_2"] != null ? form["tenGV_2"] : "";
                }


                if (HoTen == "") { TempData["error"] = "Họ tên không được để trống"; return RedirectToAction("ThemMoi"); }
                if (GioiTinh == "") { TempData["error"] = "Bạn chưa chọn giới tính"; return RedirectToAction("ThemMoi"); }
                if (SoDienThoai == "") { TempData["error"] = "Điện thoại không được để trống"; return RedirectToAction("ThemMoi"); }
                if (Email == "") { TempData["error"] = "Email không được để trống"; return RedirectToAction("ThemMoi"); }
                else
                {
                    var checkUser = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Email.Equals(Email));
                    if (checkUser != null)
                    {
                        TempData["error"] = "Email đã tồn tại. Vui lòng nhập email khác"; return RedirectToAction("ThemMoi");
                    }
                }
                if (NoiSinh == "") { TempData["error"] = "Nơi sinh không được để trống"; return RedirectToAction("ThemMoi"); }
                if (DiaChiLienLac == "") { TempData["error"] = "Địa chỉ không được để trống"; return RedirectToAction("ThemMoi"); }
                if (NgheNghiep == "") { TempData["error"] = "Bạn chưa chọn nghề nghiệp"; return RedirectToAction("ThemMoi"); }
                if (CoQuanCongTacHienNay == "") { TempData["error"] = "Cơ quan đang công tác không được để trống"; return RedirectToAction("ThemMoi"); }
                if (NamBatDauCongTac == "") { TempData["error"] = "Năm bắt đầu công tác không được để trống"; return RedirectToAction("ThemMoi"); }
                if (HienLaCanBo == "") { TempData["error"] = "Bạn chưa chọn cán bộ"; return RedirectToAction("ThemMoi"); }
                if (ViTriConViecHienTai == "") { TempData["error"] = "Vị trí công việc hiện tại không được để trống"; return RedirectToAction("ThemMoi"); }
                if (ThamNiemNghieNghiep == "") { TempData["error"] = "Thâm niên nghề nghiệp không được để trống"; return RedirectToAction("ThemMoi"); }
                if (ChuyenMon == "") { TempData["error"] = "Chuyên môn không được để trống"; return RedirectToAction("ThemMoi"); }
                if (Truong_TN_DaiHoc == "") { TempData["error"] = "Tên trường tốp nghiệp không được để trống"; return RedirectToAction("ThemMoi"); }
                if (Nam_TN_DaiHoc == 0) { TempData["error"] = "Năm tốt nghiệp đại học không được để trống"; return RedirectToAction("ThemMoi"); }
                if (HeDaoTao_DaiHoc == "") { TempData["error"] = "Hệ đào tạo đại học không được để trống"; return RedirectToAction("ThemMoi"); }
                if (Nghanh_TN_DaiHoc == "") { TempData["error"] = "Ngành tốt nghiệp đại học không được để trống"; return RedirectToAction("ThemMoi"); }
                if (DiemTrungBinh_DaiHoc == "") { TempData["error"] = "Điểm trung bình đại học không được để trống"; return RedirectToAction("ThemMoi"); }
                if (LoaiTotNghiep_DaiHoc == "") { TempData["error"] = "Loại tốt nghiệp đại học không được để trống"; return RedirectToAction("ThemMoi"); }
                if (Url_FileUpload_DaiHoc == "") { TempData["error"] = "Bạn chưa đính kèm thông tin đại học"; return RedirectToAction("ThemMoi"); }

                if (Url_FileUpload_AnhSoYeuLyLich == "") { TempData["error"] = "Bạn chưa đính kèm ảnh sơ yếu lý lịch"; return RedirectToAction("Index"); }
                if (Url_FileUpload_CongVanGioiThieu == "") { TempData["error"] = "Bạn chưa đính kèm công văn giới thiệu"; return RedirectToAction("Index"); }
                if (Url_FileUpload_GiaySucKhoe == "") { TempData["error"] = "Bạn chưa đính kèm giấy khám sức khoẻ"; return RedirectToAction("Index"); }
                if (Url_FileUpload_HopDongLaoDong == "") { TempData["error"] = "Bạn chưa đính kèm quyết định tuyển dụng hoặc hợp đồng lao động"; return RedirectToAction("Index"); }
                if (Url_FileUpload_ThuGioiThieu == "") { TempData["error"] = "Bạn chưa đính kèm thư giới thiệu"; return RedirectToAction("Index"); }
                if (Url_FileUpload_BaiBaoKhoaHoc == "") { TempData["error"] = "Bạn chưa đính kèm bài báo khoa học hoặc báo cáo"; return RedirectToAction("Index"); }
                if (Url_FileUpload_DeCuongNghienCuu == "") { TempData["error"] = "Bạn chưa đính kèm đề cương nghiên cứu"; return RedirectToAction("Index"); }

                //cmnd
                if (cmnd == "") { TempData["error"] = "Số CMND/CCCD không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_ngaycap == DateTime.Now) { TempData["error"] = "Chọn sai ngày cấp CMND/CCCD"; return RedirectToAction("Index"); }
                if (cmnd_noicap == "") { TempData["error"] = "Nơi cấp CMND/CCCD không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_tinhthanh == "" || cmnd_tinhthanh == null) { TempData["error"] = "Tỉnh thành phố không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_huyen == "" || cmnd_huyen == null) { TempData["error"] = "Huyện không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_xa == "" || cmnd_xa == null) { TempData["error"] = "Xã không được để trống"; return RedirectToAction("Index"); }
                if (TenDeTai == "") { TempData["error"] = "Tên đề tài không được để trống"; return RedirectToAction("ThemMoi"); }

                if (NgoaiNgu == "") { TempData["error"] = "Ngoại ngữ không được để trống"; return RedirectToAction("ThemMoi"); }
                if (LoaiVanBangNgoaiNgu == "") { TempData["error"] = "Loại văn bằng chứng chỉ ngoại ngữ không được để trống"; return RedirectToAction("ThemMoi"); }
                if (Url_ChungChiNgoaiNgu == "") { TempData["error"] = "Bạn chưa đính kèm văn bằng, chứng chỉ ngoại ngữ"; return RedirectToAction("ThemMoi"); }
                if (BoTucKienThuc == "") { TempData["error"] = "Bổ túc kiến thức không được để trống"; return RedirectToAction("ThemMoi"); }
                if (ChuyenNghanhDuTuyenId == 0) { TempData["error"] = "Bạn chưa chọn chuyên nghành dự tuyển"; return RedirectToAction("ThemMoi"); }
                if (DoiTuongDuTuyen == "") { TempData["error"] = "Bạn chưa chọn đối tường dự tuyển"; return RedirectToAction("ThemMoi"); }
                if (ThoiGianHinhThucDaoTao == "") { TempData["error"] = "Bạn chưa chọn thời gian và hình thức đào tạo"; return RedirectToAction("ThemMoi"); }
                if (ThoiGianHinhThucDaoTao == "36") // chọn 3 năm thì bắt buộc nhập thạc sĩ
                {
                    if (Truong_TN_ThacSi == "") { TempData["error"] = "Tên trường tốt nghiệp thạc sĩ không được để trống"; return RedirectToAction("ThemMoi"); }
                    if (Nam_TN_ThacSi == "") { TempData["error"] = "Năm tốt nghiệp thạc sĩ không được để trống"; return RedirectToAction("ThemMoi"); }
                    if (HeDaoTao_ThacSi == "") { TempData["error"] = "Hệ đào tạo thạc sĩ không được để trống"; return RedirectToAction("ThemMoi"); }
                    if (Nghanh_TN_ThacSi == "") { TempData["error"] = "Ngành tốt nghiệp thạc sĩ không được để trống"; return RedirectToAction("ThemMoi"); }
                    if (DiemTrungBinh_ThacSi == "") { TempData["error"] = "Điểm trung bình thạc sĩ không được để trống"; return RedirectToAction("ThemMoi"); }
                    if (Url_FileUpload_ThacSi == "") { TempData["error"] = "Bạn chưa đính kèm thông tin thạc sỹ"; return RedirectToAction("ThemMoi"); }
                }

                //tạo folder
                var foldername = HoTen + $"_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}";
                var folder = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername);
                var zipPath = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + ".zip");
                if (!System.IO.Directory.Exists(folder))
                    System.IO.Directory.CreateDirectory(folder);

                //check định dạng file

                if (Url_FileUpload_DaiHoc != "")
                {
                    //if (!filevbdh.ContentType.Contains("image") && !filevbdh.ContentType.Contains("pdf")) { TempData["error"] = "File văn bằng đại học không đúng định dạng"; return RedirectToAction("Index"); }
                    filevbdh.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_DaiHoc));
                }
                if (Url_FileUpload_VB2 != "")
                {
                    //if (!filevb_vb2.ContentType.Contains("image") && !filevb_vb2.ContentType.Contains("pdf")) { TempData["error"] = "File văn bằng 2 không đúng định dạng"; return RedirectToAction("Index"); }
                    filevb_vb2.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_VB2));
                }
                if (Url_FileUpload_ThacSi != "")
                {
                    //if (!filevbts.ContentType.Contains("image") && !filevbts.ContentType.Contains("pdf")) { TempData["error"] = "File văn bằng thạc sĩ không đúng định dạng"; return RedirectToAction("Index"); }
                    filevbts.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_ThacSi));
                }
                if (Url_ChungChiNgoaiNgu != "")
                {
                    //if (!filenn.ContentType.Contains("image") && !filenn.ContentType.Contains("pdf")) { TempData["error"] = "File ngoại ngữ không đúng định dạng"; return RedirectToAction("Index"); }
                    filenn.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_ChungChiNgoaiNgu));
                }

                //check file đính kèm
                if (Url_FileUpload_AnhSoYeuLyLich != "")
                {
                    //if (!filesyll.ContentType.Contains("image") && !filesyll.ContentType.Contains("pdf")) { TempData["error"] = "File sơ yếu lý lịch không đúng định dạng"; return RedirectToAction("Index"); }
                    filesyll.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_AnhSoYeuLyLich));
                }
                if (Url_FileUpload_CongVanGioiThieu != "")
                {
                    //if (!filecvgt.ContentType.Contains("image") && !filecvgt.ContentType.Contains("pdf")) { TempData["error"] = "File công văn giới thiệu không đúng định dạng"; return RedirectToAction("Index"); }
                    filecvgt.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_CongVanGioiThieu));
                }
                if (Url_FileUpload_GiaySucKhoe != "")
                {
                    //if (!filegksk.ContentType.Contains("image") && !filegksk.ContentType.Contains("pdf")) { TempData["error"] = "File giấy khám sức khoẻ không đúng định dạng"; return RedirectToAction("Index"); }
                    filegksk.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_GiaySucKhoe));
                }
                if (Url_FileUpload_HopDongLaoDong != "")
                {
                    //if (!filehdld.ContentType.Contains("image") && !filehdld.ContentType.Contains("pdf")) { TempData["error"] = "File thư giới thiệu không đúng định dạng"; return RedirectToAction("Index"); }
                    filehdld.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_HopDongLaoDong));
                }
                if (Url_FileUpload_ThuGioiThieu != "")
                {
                    //if (!filetgt.ContentType.Contains("image") && !filetgt.ContentType.Contains("pdf")) { TempData["error"] = "File bài báo khoa học không đúng định dạng"; return RedirectToAction("Index"); }
                    filetgt.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_ThuGioiThieu));
                }
                if (Url_FileUpload_BaiBaoKhoaHoc != "")
                {
                    //if (!filebbkh.ContentType.Contains("image") && !filebbkh.ContentType.Contains("pdf")) { TempData["error"] = "File đề cương nghiên cứu không đúng định dạng"; return RedirectToAction("Index"); }
                    filebbkh.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_BaiBaoKhoaHoc));
                }
                if (Url_FileUpload_DeCuongNghienCuu != "")
                {
                    //if (!filedcnc.ContentType.Contains("image") && !filedcnc.ContentType.Contains("pdf")) { TempData["error"] = "File quyết định tuyển dụng không đúng định dạng"; return RedirectToAction("Index"); }
                    filedcnc.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_DeCuongNghienCuu));
                }


                DangKyTuyenSinh dk = new DangKyTuyenSinh();
                dk.HoTen = HoTen;
                dk.GioiTinh = GioiTinh;
                dk.NgaySinh = NgaySinh;
                dk.SoDienThoai = SoDienThoai;
                dk.Email = Email;
                dk.NoiSinh = NoiSinh;
                dk.DiaChiLienLac = DiaChiLienLac;
                dk.NgheNghiep = NgheNghiep;
                dk.CoQuanCongTacHienNay = CoQuanCongTacHienNay;
                dk.NamBatDauCongTac = NamBatDauCongTac;
                dk.HienLaCanBo = HienLaCanBo;
                dk.ViTriConViecHienTai = ViTriConViecHienTai;
                dk.ThamNiemNghieNghiep = ThamNiemNghieNghiep;
                dk.ChuyenMon = ChuyenMon;
                dk.Truong_TN_DaiHoc = Truong_TN_DaiHoc;
                dk.Nam_TN_DaiHoc = Nam_TN_DaiHoc;
                dk.HeDaoTao_DaiHoc = HeDaoTao_DaiHoc;
                dk.Nghanh_TN_DaiHoc = Nghanh_TN_DaiHoc;
                dk.DiemTrungBinh_DaiHoc = DiemTrungBinh_DaiHoc;
                dk.LoaiTotNghiep_DaiHoc = LoaiTotNghiep_DaiHoc;
                dk.Url_FileUpload_DaiHoc = Url_FileUpload_DaiHoc;
                dk.TenDeTai = TenDeTai;

                //file đính kèm
                dk.Url_FileUpload_AnhSoYeuLyLich = Url_FileUpload_AnhSoYeuLyLich;
                dk.Url_FileUpload_CongVanGioiThieu = Url_FileUpload_CongVanGioiThieu;
                dk.Url_FileUpload_GiaySucKhoe = Url_FileUpload_GiaySucKhoe;
                dk.Url_FileUpload_HopDongLaoDong = Url_FileUpload_HopDongLaoDong;
                dk.Url_FileUpload_ThuGioiThieu = Url_FileUpload_ThuGioiThieu;
                dk.Url_FileUpload_BaiBaoKhoaHoc = Url_FileUpload_BaiBaoKhoaHoc;
                dk.Url_FileUpload_DeCuongNghienCuu = Url_FileUpload_DeCuongNghienCuu;

                //CMNd
                dk.SoCMND = cmnd;
                dk.Ngaycap_CMND = cmnd_ngaycap;
                dk.Noicap_CMND = cmnd_noicap;

                var tinhthanh = _unitOfWork.GetRepositoryInstance<City>().GetFirstOrDefaultByParameter(x => x.CityCode == cmnd_tinhthanh);
                var quanhuyen = _unitOfWork.GetRepositoryInstance<District>().GetFirstOrDefaultByParameter(x => x.DistrictCode == cmnd_huyen);
                var xaphuong = _unitOfWork.GetRepositoryInstance<Ward>().GetFirstOrDefaultByParameter(x => x.WardCode == cmnd_xa);
                dk.TinhThanh_CMND = tinhthanh != null ? tinhthanh.Type + " " + tinhthanh.Name : "";
                dk.Quan_CMND = quanhuyen != null ? quanhuyen.Type + " " + quanhuyen.Name : "";
                dk.Xa_CMND = xaphuong != null ? xaphuong.Type + " " + xaphuong.Name : "";

                // văn bằng 2
                dk.Truong_TN_VB2 = Truong_TN_VB2;
                dk.Nam_TN_VB2 = Nam_TN_VB2;
                dk.HeDaoTao_VB2 = HeDaoTao_VB2;
                dk.Nganh_TN_VB2 = Nghanh_TN_VB2;
                dk.DiemTrungBinh_VB2 = DiemTrungBinh_VB2;
                dk.LoaiTotNghiep_VB2 = LoaiTotNghiep_VB2;
                dk.Url_FileUpload_VB2 = Url_FileUpload_VB2;


                //Người hướng dẫn
                if (khoaId_NHD1 != 0 && Id_NHD1 != 0)
                {
                    dk.KhoaId_NHD1 = khoaId_NHD1;
                    dk.Id_NHD1 = Id_NHD1;
                    dk.NHD1 = nguoihuongdan1;
                }
                if (khoaId_NHD2 != 0 && Id_NHD2 != 0)
                {
                    dk.KhoaId_NHD2 = khoaId_NHD2;
                    dk.Id_NHD2 = Id_NHD2;
                    dk.NHD2 = nguoihuongdan2;
                }
                else if (nguoihuongdan2 != "")
                {
                    dk.NHD2 = nguoihuongdan2;
                }

                dk.Truong_TN_ThacSi = Truong_TN_ThacSi;
                dk.Nam_TN_ThacSi = Nam_TN_ThacSi;
                dk.HeDaoTao_ThacSi = HeDaoTao_ThacSi;
                dk.Nghanh_TN_ThacSi = Nghanh_TN_ThacSi;
                dk.DiemTrungBinh_ThacSi = DiemTrungBinh_ThacSi;
                dk.Url_FileUpload_ThacSi = Url_FileUpload_ThacSi;
                dk.NgoaiNgu = NgoaiNgu;
                dk.LoaiVanBangNgoaiNgu = LoaiVanBangNgoaiNgu;
                dk.Url_ChungChiNgoaiNgu = Url_ChungChiNgoaiNgu;
                dk.BoTucKienThuc = BoTucKienThuc;
                dk.ChuyenNghanhDuTuyenId = ChuyenNghanhDuTuyenId;
                var cndt = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == ChuyenNghanhDuTuyenId);
                dk.TenNganh = cndt.TenNganh;
                dk.TenNganh = cndt.TenNganh;
                dk.NganhId = cndt.NganhId;
                dk.TenKhoa = cndt.TenKhoa;
                dk.KhoaId = cndt.KhoaId;
                dk.TenChuyenNghanhDuTuyen = TenChuyenNghanhDuTuyen;
                dk.DoiTuongDuTuyen = DoiTuongDuTuyen;
                dk.ThoiGianHinhThucDaoTao = ThoiGianHinhThucDaoTao;
                dk.CreatedAt = DateTime.Now;
                dk.UpdatedAt = DateTime.Now;
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                dk.CreatedBy = user.Username;
                dk.UpdatedBy = user.Username;
                dk.Status = 0;
                ZipFile.CreateFromDirectory(folder, zipPath);
                dk.Url_FileUpload_Zip = foldername;

                KhoaHoc khoahoc = _unitOfWork.GetRepositoryInstance<Model.KhoaHoc>().GetAllRecords().OrderByDescending(o => o.Id).Take(1).SingleOrDefault();
                DotTuyenSinh dotTS = khoahoc != null ? _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.idKhoahoc == khoahoc.Id && x.Status == 1) : null;
                dk.IdDotTS = dotTS?.Id;

                _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().Add(dk);

                //insert bảng User bằng Email
                User user_tam = new User();
                user_tam.Email = Email;
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[6];
                var random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var password = new string(stringChars);
                user_tam.PassWord = Utility.Encrypt(password, true);
                user_tam.CreatedAt = DateTime.Now;
                _unitOfWork.GetRepositoryInstance<User>().Add(user_tam);

                //insert UserRoles
                UserRoles usrerole = new UserRoles();
                usrerole.Email = Email;
                usrerole.Role = PublicConstant.ROLE_NCS;
                usrerole.CreatedAt = DateTime.Now;
                _unitOfWork.GetRepositoryInstance<UserRoles>().Add(usrerole);

                //insert HocPhi
                HocPhi hocphi = new HocPhi();
                hocphi.UserName = Email;
                hocphi.ChuyennNghanh = ChuyenNghanhDuTuyenId;
                hocphi.TenChuyenNganh = TenChuyenNghanhDuTuyen;
                hocphi.Khoa = dk.KhoaId;
                hocphi.TenKhoa = dk.TenKhoa;
                hocphi.HoTen = HoTen;
                hocphi.TrangThai = PublicConstant.CHUA_NOP;
                hocphi.CreatedAt = DateTime.Now;
                hocphi.CreatedBy = HoTen;
                SysSetting mucDangKyThi = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(x => x.SKey.Equals("DangKyThi"));
                if (mucDangKyThi != null)
                {
                    hocphi.MucNop = double.Parse(mucDangKyThi.Value);
                    hocphi.NoiDung = "Đóng lệ phí " + mucDangKyThi.Name;
                }
                else
                {
                    hocphi.MucNop = 200000;
                    hocphi.NoiDung = "Đóng lệ phí đăng ký tuyển sinh!";
                }
                string title = "Đăng ký tuyển sinh";
                string content = "Bạn đã đăng kí thành công! Phí " + mucDangKyThi.Name + " là " + mucDangKyThi.Value + "\nTài khoản của bạn là:" + Email + "\nMật khẩu là: " + password;
                if (!Utility.SendMail(Email, title, content).Equals(""))
                {
                    TempData["error"] = "Lỗi gửi mail";
                }

                _unitOfWork.GetRepositoryInstance<HocPhi>().Add(hocphi);
                _unitOfWork.SaveChanges();
                TempData["message"] = "Đăng ký thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                TempData["error"] = "Lỗi đăng ký: " + ex.Message;
                return RedirectToAction("ThemMoi");
            }
        }

        public JsonResult LoadDistrictByCity(string CityCode)
        {
            string str = "";
            try
            {
                List<District> listData = _unitOfWork.GetRepositoryInstance<District>().GetListByParameter(x => x.CityCode == CityCode).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.DistrictCode + "\">" + item.Type + " " + item.Name + "</option>";
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadWardByDistrict(string DistrictCode)
        {
            string str = "";
            try
            {
                List<Ward> listData = _unitOfWork.GetRepositoryInstance<Ward>().GetListByParameter(x => x.DistrictCode == DistrictCode).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.WardCode + "\">" + item.Type + " " + item.Name + "</option>";
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ThemMoiExel()
        {
            return View();
        }
        #region UploadExcel old
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async System.Threading.Tasks.Task<ActionResult> UploadExcelFile(HttpPostedFileBase upload)
        //{
        //    string error = "";
        //    if (ModelState.IsValid)
        //    {
        //        if (upload != null && upload.ContentLength > 0)
        //        {
        //            try
        //            {
        //                Stream stream = upload.InputStream;
        //                IExcelDataReader reader = null;

        //                if (upload.FileName.EndsWith(".xls"))
        //                {
        //                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //                }
        //                else if (upload.FileName.EndsWith(".xlsx"))
        //                {
        //                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("File", "File không đúng địng dạng");
        //                    TempData["error"] = "File không đúng địng dạng";
        //                    return View();
        //                }

        //                reader.IsFirstRowAsColumnNames = true;

        //                DataSet ds = reader.AsDataSet();
        //                DataTable dt = ds.Tables[0];
        //                string fileExelName = upload.FileName;
        //                if (dt.Rows.Count > 0)
        //                {
        //                    if (dt.Rows.Count < 2)
        //                    {
        //                        TempData["error"] = "File excel phải có ít nhất từ 2 bản ghi trở lên";
        //                        return RedirectToAction("Index", "Home");
        //                    }

        //                    if (dt.Rows.Count > 1000)
        //                    {
        //                        TempData["error"] = "Số lượng tối đa 1000 bản ghi trong mỗi lần up";
        //                        return RedirectToAction("Index", "Home");
        //                    }

        //                    string userName = User.Identity.GetUserName();

        //                    Models.HocVien hv;

        //                    foreach (DataRow row in dt.Rows)
        //                    {
        //                        try
        //                        {
        //                            if (row["Họ Và Tên"].ToString() != "")
        //                            {
        //                                //var role = row["Nhóm Quyền"].ToString();
        //                                var user = new ApplicationUser
        //                                {
        //                                    UserName = row["Tên Truy Cập"].ToString(),
        //                                    TwoFactorEnabled = true,
        //                                    Email = row["Email"].ToString(),
        //                                    PrivateKey = TimeSensitivePassCode.GeneratePresharedKey()
        //                                };
        //                                hv = new Models.HocVien();
        //                                hv.ApplicationUserId = user.Id;
        //                                hv.Ho = row["Họ"].ToString();
        //                                hv.Ten = row["Tên"].ToString();
        //                                hv.NgaySinh = DateTime.Parse(row["Ngày Sinh"].ToString());
        //                                hv.DiaChi = row["Địa Chỉ"].ToString();
        //                                hv.GioiTinh = row["Giới Tính"].ToString();
        //                                hv.NoiSinh = row["Nơi Sinh"].ToString();
        //                                hv.HoKhau = row["Hộ Khẩu"].ToString();
        //                                hv.DiaChi = row["Địa Chỉ"].ToString();
        //                                hv.DienThoai = row["Số Điện Thoại"].ToString();
        //                                hv.Email = row["Email"].ToString();
        //                                hv.DanToc = row["Dân Tộc"].ToString();
        //                                hv.QuocTich = row["Quốc Tịch"].ToString();

        //                                hv.RoleId = 1;
        //                                user.HocVien = hv;


        //                                var message = await UserManager.CreateAsync(user, hv.Password);
        //                                if (message.Succeeded)
        //                                {
        //                                    UserManager.AddToRole(user.Id, PublicConstant.ROLE_GIAOVIEN.ToString());
        //                                }
        //                                else
        //                                {
        //                                    error = "Lỗi tạo tài khoản";
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            error = "Lỗi tạo tài khoản : " + ex.Message;
        //                            break;
        //                        }
        //                    }

        //                    if (error == "")
        //                    {
        //                        _unitOfWork.SaveChanges();
        //                        TempData["message"] = "Import danh sách thành công!";
        //                    }
        //                    else
        //                    {
        //                        error = "Lỗi upload : " + error;
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["error"] = "File không có dữ liệu!";
        //                }

        //                reader.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                TempData["error"] = "Lỗi xảy ra trong quá trình import file excel:" + ex.Message;
        //            }
        //        }
        //        else
        //        {
        //            TempData["error"] = "File không có dữ liệu !";
        //        }
        //    }


        //    return RedirectToAction("Index", "HocVien");
        //}
        #endregion


        public ActionResult Edit()
        {
            return View();
        }

        public async Task<ActionResult> Detail(long id)
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;
            List<City> listCity = _unitOfWork.GetRepositoryInstance<City>().GetAllRecords().OrderBy(x => x.Name).ToList();
            ViewBag.ListCity = listCity;
            List<District> listDistrict = _unitOfWork.GetRepositoryInstance<District>().GetAllRecords().OrderBy(x => x.Name).ToList();
            ViewBag.ListDistrict = listDistrict;
            List<Ward> listWard = _unitOfWork.GetRepositoryInstance<Ward>().GetAllRecords().OrderBy(x => x.Name).ToList();
            ViewBag.ListWard = listWard;
            Model.DangKyTuyenSinh dkts = new Model.DangKyTuyenSinh();
            dkts = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
            HocPhi phidangky = _unitOfWork.GetRepositoryInstance<HocPhi>().GetFirstOrDefaultByParameter(x => x.UserName == dkts.Email && x.Type == 1);
            ViewBag.PhiDangKy = phidangky;
            return View(dkts);
        }

        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                Model.DangKyTuyenSinh gv = new Model.DangKyTuyenSinh();
                gv = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (gv != null)
                {
                    _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().Remove(gv);
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
        public async Task<JsonResult> DuyetHoSo(long id, string tendetai, int ddlKhoa_1, int ddlGV_1, int loaiGV_2, int ddlKhoa_2, int ddlGV_2, string tenGV_2,string coquancongtacGV_2)
        {
            try
            {
                Model.DangKyTuyenSinh gv = new Model.DangKyTuyenSinh();
                gv = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (gv != null)
                {
                    gv.Status = PublicConstant.STT_DUYET;
                    gv.TenDeTai = tendetai;
                    List<GiangVienAPI> listGV = await CoreAPI.CoreAPI.GetListGiangVien(ddlKhoa_1);
                    if (listGV != null)
                    {
                        var giangvien = listGV.Where(x => x.Id == ddlGV_1).FirstOrDefault();
                        if (giangvien != null)
                        {
                            gv.KhoaId_NHD1 = ddlKhoa_1;
                            gv.Id_NHD1 = ddlGV_1;
                            gv.NHD1 = giangvien.Name;
                        }
                        else
                        {
                            TempData["error"] = "Giảng viên trong trường không tồn tại!";
                            return Json("Giảng viên trong trường không tồn tại!", JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (loaiGV_2 == 0)
                    {
                        listGV = await CoreAPI.CoreAPI.GetListGiangVien(ddlKhoa_2);
                        var giangvien = listGV.Where(x => x.Id == ddlGV_2).FirstOrDefault();
                        if (giangvien != null)
                        {
                            gv.KhoaId_NHD2 = ddlKhoa_2;
                            gv.Id_NHD2 = ddlGV_2;
                            gv.NHD2 = giangvien.Name;
                        }
                        else
                        {
                            TempData["error"] = "Giảng viên trong trường không tồn tại!";
                            return Json("Giảng viên trong trường không tồn tại!", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        gv.NHD2 = tenGV_2;
                        gv.CoQuanCongTac_NHD2 = coquancongtacGV_2;
                    }

                    _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().Update(gv);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Duyệt hồ sơ thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi duyệt hồ sơ: " + ex.Message;
                return Json("Lỗi duyệt hồ sơ", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GuiPhanHoi()
        {
            try
            {
                int id = Request.Form["id"] != null ? int.Parse(Request.Form["id"]) : 0;
                string noidungphanhoi = Request.Form["noidungphanhoi"] != null ? Request.Form["noidungphanhoi"] : "";

                Model.DangKyTuyenSinh dkts = new Model.DangKyTuyenSinh();
                dkts = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (dkts != null)
                {
                    if (noidungphanhoi == "")
                    {
                        TempData["error"] = "Nội dung phản hồi không được để trống";
                        return Json("Nội dung phản hồi không được để trống", JsonRequestBehavior.AllowGet);
                    }
                    dkts.TrangThaiTuyenSinh = PublicConstant.STT_DANGXULY;
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    if (files != null)
                    {
                        string Url_PhanHoi = file.FileName;
                        if (Url_PhanHoi != "")
                        {
                            file.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + Url_PhanHoi));
                            dkts.TepFilePhanHoi = Url_PhanHoi;
                        }
                    }

                    dkts.NoiDungPhanHoi = noidungphanhoi;
                    _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().Update(dkts);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Gửi phản hồi thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi gửi phản hồi: " + ex.Message;
                return Json("Lỗi gửi phản hồi", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GuiPhanHoiTuyenSinh(string email, string tieude, string noidungphanhoi)
        {
            try
            {
                string mess = Utility.SendMail(email, tieude, noidungphanhoi);
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                if (mess.Equals(""))
                {
                    SysComment cmt = new SysComment();
                    cmt.EmailNCS = email;
                    cmt.Category = tieude;
                    cmt.Description = noidungphanhoi;
                    cmt.Status = 1;
                    _unitOfWork.GetRepositoryInstance<SysComment>().Add(cmt);

                    ThongBao tb = new ThongBao();
                    tb.Email = email;
                    tb.Title = tieude;
                    tb.Description = noidungphanhoi;
                    tb.CreatedAt = DateTime.Now;
                    tb.UpdatedAt = DateTime.Now;
                    tb.CreatedBy = user.Name;
                    tb.UpdatedBy = user.Name;

                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(tb);

                    _unitOfWork.SaveChanges();
                    TempData["message"] = "Gửi phản hồi thành công!";
                }
                else
                {
                    TempData["error"] = mess;
                }
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi gửi phản hồi: " + ex.Message;
                return Json("Lỗi gửi phản hồi", JsonRequestBehavior.AllowGet);
            }
        }
        public async System.Threading.Tasks.Task<ActionResult> XetTuyen()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;
            return View();
        }
        public async Task<ActionResult> HoSoXetTuyen()
        {
            List<BieuMau> list_bieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListBieumau = list_bieumau;

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;

            return View();
        }
        public async Task<ActionResult> XinTieuBan()
        {
            List<BieuMau> list_bieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListBieumau = list_bieumau;

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;
            var list_data = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == 1005).ToList();

            List<KhoaHoc> list = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = list;

            return View(list_data);
        }
        public async Task<ActionResult> XinTieuBanKhoa()
        {
            var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            var roleInfo = (UserRoles)Session[PublicConstant.ROLE_INFO];

            int staffid = int.Parse(loginInfo.StaffId.ToString());
            var user = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
            int departmentID = user.DepartmentId;
            ViewBag.DEPARTEMENTID = departmentID;
            List<BieuMau> list_bieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListBieumau = list_bieumau;

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            var lstDotTuyensinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetAllRecords().OrderBy(o => o.Id).OrderByDescending(o => o.Id).ToList();
            List<KhoaHoc> list = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = list;

            ViewBag.IDKHOA = 78;
            return View();
        }
        public async Task<ActionResult> LoadGVByKhoa_XinTieuBan(int departmentID, int IdKhoahoc, int IdDotTS)
        {
            List<GiangVien_ChucVuView> lstGV = new List<GiangVien_ChucVuView>();
            List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(departmentID, "", 1, 200, 0);
            List<ChucVuAPI> listChucVu = await CoreAPI.CoreAPI.GetListChucVu();
            List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();
            try
            {
                if (listGVtrong.Count > 0)
                {
                    foreach (var item in listGVtrong)
                    {
                        GiangVien_ChucVuView gv = new GiangVien_ChucVuView();
                        List<string> chucvu = new List<string>();
                        gv.name = item.Name;
                        if (item.PositionIds != null)
                        {
                            foreach (var o in item.PositionIds)
                            {
                                var cv = listChucVu.Where(x => x.Id == o).SingleOrDefault();
                                if (cv != null)
                                {
                                    chucvu.Add(cv.Name);
                                }
                            }
                        }
                        gv.chucvu = chucvu;
                        gv.idgv = item.Id;
                        var hocham = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault();
                        if (hocham != null)
                        {
                            gv.hochamhocvi = hocham.Name;
                        }
                        else
                        {
                            gv.hochamhocvi = "";
                        }
                        gv.donvicongtac = item.Department;
                        var idcb = item.Id;
                        var check_cbdachon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb && x.IdBieuMau == 1005 && x.IdDotTS == IdDotTS && x.IdKhoahoc == IdKhoahoc);
                        if (check_cbdachon != null)
                        {
                            gv.vaitro = check_cbdachon.VaiTro;
                        }
                        lstGV.Add(gv);
                    }
                    return PartialView("_PartialGiangVienKhoaXTB", lstGV);
                }
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, loginInfo == null ? string.Empty : loginInfo.Username, ex);
            }
            return PartialView("_PartialGiangVienKhoaXTB", null);
        }
        public ActionResult LoadGVKhoa_Selected(int departmentID, int IdKhoahoc, int IdDotTS)
        {
            List<DanhSachCanBoAddForm> lstGV = new List<DanhSachCanBoAddForm>();
            try
            {
                lstGV = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdKhoa == departmentID && x.IdKhoahoc == IdKhoahoc && x.IdDotTS == IdDotTS).OrderBy(o => o.Id).ToList();
                return PartialView("_PartialGVSelected", lstGV);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, loginInfo == null ? string.Empty : loginInfo.Username, ex);
            }
            return PartialView("_PartialGVSelected", null);
        }
        public async Task<ActionResult> HoiDongTuyenSinh()
        {
            List<BieuMau> list_bieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListBieumau = list_bieumau;

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;
            var list_data = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == 1).ToList();
            return View(list_data);
        }
        #region ReplaceWordDocument old
        //public JsonResult ReplaceWordDocument(string id)
        //{
        //    List<int> processesbeforegen = getRunningProcesses();
        //    string isOk = string.Empty;
        //    try
        //    {
        //        string filename = "";
        //        string fileurl = "";
        //        object missing = Missing.Value;
        //        int id_bm = int.Parse(id);
        //        var get_url = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == id_bm);
        //        if (get_url != null)
        //        {
        //            fileurl = get_url.FileUrl;
        //            filename = get_url.Template;
        //        }
        //        if (!string.IsNullOrEmpty(fileurl))
        //        {
        //            var urlchecl = Server.MapPath("~/" + urlFile + fileurl);

        //            //Word.Document aDoc = wordApp.Documents.Open(urlchecl);

        //            if (System.IO.File.Exists(urlchecl))
        //            {
        //                AWord.Application wordApp;
        //                AWord.Document aDoc;
        //                wordApp = new AWord.Application();
        //                //Set animation status for word application  
        //                wordApp.Visible = false;
        //                isOk = urlchecl + "1-";
        //                aDoc = wordApp.Documents.Open(urlchecl, ref missing);
        //                isOk = isOk + "2-";
        //                DateTime today = DateTime.Now;

        //                object readOnly = false; //default
        //                object isVisible = false;

        //                wordApp.Visible = false;

        //                aDoc.Activate();
        //                isOk = isOk + "3-";

        //                var lstTxtReplace = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == id_bm).ToList();
        //                foreach (var item in lstTxtReplace)
        //                {
        //                    //Find and replace:
        //                    if (item.KieuDuLieu != PublicConstant.BM_BANG)
        //                    {
        //                        Utility.FindAndReplace(wordApp, item.TenThamSo, item.GiaTriThamSo);
        //                    }
        //                    else
        //                    {
        //                        int tableReplace = item.ThuTuHienThi != null ? item.ThuTuHienThi.Value : 0;
        //                        AWord.Table tab = aDoc.Tables[tableReplace];
        //                        AWord.Range range = tab.Range;
        //                        isOk = isOk + "4-";

        //                        //SET STYLE
        //                        tab.Borders.Enable = 0;

        //                        var lstDanhsach = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetAllRecords();
        //                        var rowCount = lstDanhsach.Count();

        //                        for (int i = 0; i < rowCount; i++)
        //                        {
        //                            var row = tab.Rows.Add(ref missing);

        //                            var objRow = lstDanhsach.ToList()[i];
        //                            row.Cells[1].Range.Text = (i + 1).ToString();
        //                            row.Cells[1].WordWrap = true;
        //                            row.Cells[1].Range.Underline = AWord.WdUnderline.wdUnderlineNone;
        //                            row.Cells[1].Range.Bold = 0;

        //                            row.Cells[2].Range.Text = objRow.TenCanBo.ToString();
        //                            row.Cells[2].WordWrap = true;
        //                            row.Cells[2].Range.Underline = AWord.WdUnderline.wdUnderlineNone;
        //                            row.Cells[2].Range.Bold = 0;

        //                            row.Cells[3].Range.Text = objRow.ChucVu.ToString();
        //                            row.Cells[3].WordWrap = true;
        //                            row.Cells[3].Range.Underline = AWord.WdUnderline.wdUnderlineNone;
        //                            row.Cells[3].Range.Bold = 0;

        //                            row.Cells[4].Range.Text = objRow.VaiTro.ToString();
        //                            row.Cells[4].WordWrap = true;
        //                            row.Cells[4].Range.Underline = AWord.WdUnderline.wdUnderlineNone;
        //                            row.Cells[4].Range.Bold = 0;

        //                        }
        //                    }
        //                }
        //                Object oMissed = aDoc.Paragraphs[1].Range; //the position you want to insert
        //                Object oLinkToFile = false;  //default
        //                Object oSaveWithDocument = true;//default


        //                var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //                string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_";
        //                var filePath = Path.Combine(systemPath, filename_new);
        //                //Save as: filename
        //                aDoc.SaveAs2(filePath);
        //            }
        //            else
        //            {
        //                return Json("Không tìm thấy file biểu mẫu", JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //        List<int> processesaftergen = getRunningProcesses();
        //        killProcesses(processesbeforegen, processesaftergen);

        //        return Json("OK", JsonRequestBehavior.AllowGet);
        //    }

        //    catch (Exception ex)
        //    {
        //        string actionName = ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
        //        var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
        //        ExceptionLogging.SendErrorToText(isOk, actionName, user == null ? string.Empty : user.Username, ex);
        //        List<int> processesaftergen = getRunningProcesses();
        //        killProcesses(processesbeforegen, processesaftergen);
        //        return Json("error", JsonRequestBehavior.AllowGet);
        //    }

        //}
        #endregion
        public JsonResult ReplaceWordDocument(string id)
        {
            List<int> processesbeforegen = getRunningProcesses();
            string isOk = string.Empty;
            try
            {
                string filename = "";
                string fileurl = "";
                object missing = Missing.Value;
                int id_bm = int.Parse(id);
                var get_url = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == id_bm);
                if (get_url != null)
                {
                    fileurl = get_url.FileUrl;
                    filename = get_url.Template;
                }
                if (!string.IsNullOrEmpty(fileurl))
                {
                    var urlchecl = Server.MapPath("~/" + urlFile + fileurl);
                    byte[] byteArray = System.IO.File.ReadAllBytes(urlchecl);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        stream.Write(byteArray, 0, (int)byteArray.Length);
                        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
                        {
                            var lstTxtReplace = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == id_bm).ToList();
                            var lstBang = lstTxtReplace.Where(x => x.KieuDuLieu == PublicConstant.BM_BANG);
                            string docText = null;
                            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                            {
                                docText = sr.ReadToEnd();
                            }
                            foreach (var item in lstTxtReplace)
                            {
                                //Find and replace:
                                if (item.KieuDuLieu != PublicConstant.BM_BANG)
                                {
                                    Regex regexText = new Regex(item.TenThamSo.Trim());
                                    docText = regexText.Replace(docText, item.GiaTriThamSo);
                                }
                            }

                            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                            {
                                sw.Write(docText);
                            }
                            foreach (var item in lstBang)
                            {
                                int tableReplace = item.ThuTuHienThi != null ? item.ThuTuHienThi.Value : 0;
                                var lstDanhsach = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == id_bm).ToList();
                                if (lstDanhsach.Count > 0)
                                {
                                    if (id_bm == 2003)
                                    {
                                        if (lstDanhsach.Where(x => x.VaiTro.Equals("UVTT, Trưởng ban")).ToList().Count != 1)
                                        {
                                            return Json("error Phải có 1 UVTT, Trưởng ban", JsonRequestBehavior.AllowGet);
                                        }
                                        else if (lstDanhsach.Where(x => x.VaiTro.Equals("Ủy viên")).ToList().Count < 4)
                                        {
                                            return Json("error Phải có ít nhất 4 uỷ viên", JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    if (id_bm == 1)
                                    {
                                        if (lstDanhsach.Where(x => x.VaiTro.Equals("Chủ tịch Hội đồng")).ToList().Count != 1)
                                        {
                                            return Json("error Phải có 1 Chủ tịch Hội đồng", JsonRequestBehavior.AllowGet);
                                        }
                                        else if (lstDanhsach.Where(x => x.VaiTro.Equals("Phó chủ tịch HĐ")).ToList().Count != 1)
                                        {
                                            return Json("error Phải có 1 Phó chủ tịch HĐ", JsonRequestBehavior.AllowGet);
                                        }
                                        else if (lstDanhsach.Where(x => x.VaiTro.Equals("Ủy viên thường trực")).ToList().Count != 1 || lstDanhsach.Where(x => x.Equals("UVTT")).ToList().Count != 1)
                                        {
                                            return Json("error Phải có 1 Ủy viên thường trực", JsonRequestBehavior.AllowGet);
                                        }
                                        else if (lstDanhsach.Where(x => x.VaiTro.Equals("Ủy viên")).ToList().Count < 2)
                                        {
                                            return Json("error Phải có ít nhất 2 uỷ viên", JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                                else
                                {
                                    return Json("error Không tìm thấy file", JsonRequestBehavior.AllowGet);
                                }
                                Table table = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(item.ThuTuHienThi.Value - 1);
                                var count = lstDanhsach.Count;
                                for (var idx = 0; idx < count; idx++)
                                {
                                    var cb = lstDanhsach[idx];
                                    if (idx == 0)
                                    {
                                        TableRow rowFirst = table.Elements<TableRow>().First();
                                        // Find the third cell in the row.
                                        TableCell cell1 = rowFirst.Elements<TableCell>().ElementAt(0);
                                        // Find the first paragraph in the table cell.
                                        Paragraph p1 = cell1.Elements<Paragraph>().First();
                                        p1.Append(new Run(new Text("1")));
                                        //// Find the first run in the paragraph.
                                        //Run r1 = p1.Elements<Run>().First();
                                        //// Set the text for the run.
                                        //Text t1 = r1.Elements<Text>().First();
                                        //t1.Text = "1";

                                        TableCell cell2 = rowFirst.Elements<TableCell>().ElementAt(1);
                                        Paragraph p2 = cell2.Elements<Paragraph>().First();
                                        p2.Append(new Run(new Text(cb.TenCanBo)));

                                        TableCell cell3 = rowFirst.Elements<TableCell>().ElementAt(2);
                                        Paragraph p3 = cell3.Elements<Paragraph>().First();
                                        p3.Append(new Run(new Text(cb.ChucVu)));

                                        TableCell cell4 = rowFirst.Elements<TableCell>().ElementAt(3);
                                        Paragraph p4 = cell4.Elements<Paragraph>().First();
                                        p4.Append(new Run(new Text(cb.VaiTro)));
                                    }
                                    else
                                    {
                                        TableRow row = new TableRow();
                                        int stt = idx + 1;
                                        TableCell cell1 = new TableCell();
                                        cell1.Append(new Paragraph(new Run(new Text((stt).ToString()))));
                                        row.Append(cell1);


                                        TableCell cell2 = new TableCell();
                                        cell2.Append(new Paragraph(new Run(new Text(cb.TenCanBo))));
                                        row.Append(cell2);

                                        TableCell cell3 = new TableCell();
                                        cell3.Append(new Paragraph(new Run(new Text(cb.ChucVu))));
                                        row.Append(cell3);

                                        TableCell cell4 = new TableCell();
                                        cell4.Append(new Paragraph(new Run(new Text(cb.VaiTro))));
                                        row.Append(cell4);
                                        table.Append(row);
                                    }
                                }

                            }


                        }

                        byteArray = stream.ToArray();

                        //var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        //string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}.docx";
                        //// var filePath = System.IO.Path.Combine(systemPath, filename_new);
                        //var filePath = Server.MapPath("~/" + urlFile + filename_new);

                        //System.IO.File.WriteAllBytes(filePath, byteArray);
                        stream.Close();
                    }
                    string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.docx";
                    var filePath = Server.MapPath("~/" + parthdowload + filename_new);
                    //File(byteArray, "", $"{filePath}");
                    System.IO.File.WriteAllBytes(filePath, byteArray);

                    var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();

                    // Download the Web resource and save it into the current filesystem folder.
                    myWebClient.DownloadFile(filePath, filePathdowload);

                    return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<int> processesaftergen = getRunningProcesses();
                    killProcesses(processesbeforegen, processesaftergen);
                    return Json("error Không tìm thấy file", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(isOk, actionName, user == null ? string.Empty : user.Username, ex);
                List<int> processesaftergen = getRunningProcesses();
                killProcesses(processesbeforegen, processesaftergen);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }
        //public JsonResult BieuMauXinTieuBan(string id, int dotId)
        //{
        //    List<int> processesbeforegen = getRunningProcesses();
        //    string isOk = string.Empty;
        //    try
        //    {
        //        string filename = "";
        //        string fileurl = "";
        //        object missing = Missing.Value;
        //        int id_bm = int.Parse(id);
        //        var get_url = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == id_bm);
        //        DotTuyenSinh dot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == dotId);
        //        var danhsachNCS = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(dk => dk.IdDotTS == dot.Id && dk.Status != 0).ToList();
        //        var soluongNCS = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(dk => dk.IdDotTS == dot.Id && dk.Status != 0).Count();
        //        if (get_url != null)
        //        {
        //            fileurl = get_url.FileUrl;
        //            filename = get_url.Template;
        //        }
        //        if (!string.IsNullOrEmpty(fileurl))
        //        {
        //            var urlchecl = Server.MapPath("~/" + urlFile + fileurl);
        //            byte[] byteArray = System.IO.File.ReadAllBytes(urlchecl);
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                stream.Write(byteArray, 0, (int)byteArray.Length);
        //                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
        //                {
        //                    string docText = null;
        //                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
        //                    {
        //                        docText = sr.ReadToEnd();
        //                    }
        //                    var lstTxtReplace = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == id_bm).ToList();
        //                    var lstBang = lstTxtReplace.Where(x => x.KieuDuLieu == PublicConstant.BM_BANG);
        //                    foreach (var item in lstTxtReplace)
        //                    {
        //                        //Find and replace:
        //                        if (item.KieuDuLieu != PublicConstant.BM_BANG)
        //                        {
        //                            Regex regexText = new Regex(item.TenThamSo.Trim());
        //                            docText = regexText.Replace(docText, item.GiaTriThamSo);
        //                        }
        //                    }
        //                    Regex SOLUONGNCS = new Regex("{SOLUONGNCS}");
        //                    docText = SOLUONGNCS.Replace(docText, soluongNCS.ToString());
        //                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
        //                    {
        //                        sw.Write(docText);
        //                    }
        //                    foreach (var item in lstTxtReplace)
        //                    {
        //                        if (item.KieuDuLieu == PublicConstant.BM_BANG)
        //                        {
        //                            int tableReplace = item.ThuTuHienThi != null ? item.ThuTuHienThi.Value : 0;
        //                            var lstDanhsach = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == id_bm).ToList();
        //                            Table table = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(item.ThuTuHienThi.Value - 1);

        //                            var count = lstDanhsach.Count;
        //                            TableRow rowFirst = table.Elements<TableRow>().First();
        //                            // Find the third cell in the row.
        //                            TableCell cell1 = rowFirst.Elements<TableCell>().ElementAt(0);
        //                            // Find the first paragraph in the table cell.
        //                            Paragraph p1 = cell1.Elements<Paragraph>().First();
        //                            p1.Append(new Run(new Text("STT")));

        //                            TableCell cell2 = rowFirst.Elements<TableCell>().ElementAt(1);
        //                            Paragraph p2 = cell2.Elements<Paragraph>().First();
        //                            p2.Append(new Run(new Text("HỌC HÀM, HỌC VỊ, HỌ VÀ TÊN")));

        //                            TableCell cell3 = rowFirst.Elements<TableCell>().ElementAt(2);
        //                            Paragraph p3 = cell3.Elements<Paragraph>().First();
        //                            p3.Append(new Run(new Text("CƠ QUAN CÔNG TÁC")));

        //                            TableCell cell4 = rowFirst.Elements<TableCell>().ElementAt(3);
        //                            Paragraph p4 = cell4.Elements<Paragraph>().First();
        //                            p4.Append(new Run(new Text("TRÁCH NHIỆM TRONG HĐ")));

        //                            TableCell cell5 = rowFirst.Elements<TableCell>().ElementAt(4);
        //                            Paragraph p5 = cell5.Elements<Paragraph>().First();
        //                            p5.Append(new Run(new Text("GHI CHÚ")));
        //                            if (tableReplace == 4)
        //                            {
        //                                for (var idx = 0; idx < count; idx++)
        //                                {
        //                                    var cb = lstDanhsach[idx];
        //                                    TableRow row = new TableRow();
        //                                    TableCell cell11_ = new TableCell();
        //                                    cell11_.Append(new Paragraph(new Run(new Text((idx + 1).ToString()))));
        //                                    row.Append(cell11_);


        //                                    TableCell cell21 = new TableCell();
        //                                    cell21.Append(new Paragraph(new Run(new Text(cb.HocHamHocVi + " " + cb.TenCanBo))));
        //                                    row.Append(cell21);

        //                                    TableCell cell31 = new TableCell();
        //                                    cell31.Append(new Paragraph(new Run(new Text(cb.CoQuanCongTac))));
        //                                    row.Append(cell31);

        //                                    TableCell cell41 = new TableCell();
        //                                    cell41.Append(new Paragraph(new Run(new Text(cb.VaiTro))));
        //                                    row.Append(cell41);

        //                                    TableCell cell51 = new TableCell();
        //                                    cell51.Append(new Paragraph(new Run(new Text(""))));
        //                                    row.Append(cell51);

        //                                    table.Append(row);
        //                                }
        //                            }

        //                            //Tạo bảng 2 và 6 - list danh sách thisinhtuyensinh
        //                            Table table_kinhgui = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(1);

        //                            List<ChuyenNganhDuTuyenViewModel> lst = (from dk in db.DangKyTuyenSinh
        //                                                                     where dk.IdDotTS == dot.Id && dk.Status != 0
        //                                                                     group dk by dk.ChuyenNghanhDuTuyenId into dkts
        //                                                                     select new ChuyenNganhDuTuyenViewModel
        //                                                                     {
        //                                                                         ChuyenNghanhDuTuyenId = dkts.Key,
        //                                                                         SoLuong = dkts.Count()
        //                                                                     }
        //                                                                     ).ToList();
        //                            foreach (ChuyenNganhDuTuyenViewModel item1 in lst)
        //                            {
        //                                var x = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(i => i.Id == item1.ChuyenNghanhDuTuyenId);
        //                                if (x != null)
        //                                {
        //                                    item1.TenNganh = x.TenNganh;
        //                                    item1.TenChuyenNganhDuTuyen = x.TenChuyenNganh;
        //                                }
        //                            }
        //                            var tenkhoa = danhsachNCS.Select(x => x.TenKhoa).Distinct();


        //                            var tennganh = lst.Select(x => x.TenNganh).Distinct();
        //                            int stt = 0, tongsoluong = 0;
        //                            foreach (var x in tenkhoa)
        //                            {
        //                                if (stt == 0)
        //                                {
        //                                    TableRow rowFirst_k = table_kinhgui.Elements<TableRow>().First();
        //                                    TableCell cell1_k = rowFirst_k.Elements<TableCell>().ElementAt(0);
        //                                    Paragraph p1_k = cell1_k.Elements<Paragraph>().First();
        //                                    p1_k.Append(new Run(new Text("Kính gửi")));

        //                                    TableCell cell2_k = rowFirst_k.Elements<TableCell>().ElementAt(1);
        //                                    Paragraph p2_k = cell2_k.Elements<Paragraph>().First();
        //                                    p2_k.Append(new Run(new Text("-" + x)));
        //                                    stt++;
        //                                }
        //                                else
        //                                {
        //                                    TableRow row = new TableRow();
        //                                    TableCell cell1_k = new TableCell();
        //                                    cell1_k.Append(new Paragraph(new Run(new Text())));
        //                                    row.Append(cell1_k);

        //                                    TableCell cell2_k = new TableCell();
        //                                    cell2_k.Append(new Paragraph(new Run(new Text("-" + x))));
        //                                    row.Append(cell2_k);

        //                                    table_kinhgui.Append(row);
        //                                }
        //                            }

        //                            stt = 1;
        //                            Table table_soluong = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(5);
        //                            foreach (ChuyenNganhDuTuyenViewModel x in lst)
        //                            {

        //                                TableRow row = new TableRow();
        //                                TableCell cell1_k = new TableCell();
        //                                cell1_k.Append(new Paragraph(new Run(new Text(stt.ToString()))));
        //                                row.Append(cell1_k);

        //                                TableCell cell2_k = new TableCell();
        //                                cell2_k.Append(new Paragraph(new Run(new Text(x.TenNganh))));
        //                                row.Append(cell2_k);

        //                                TableCell cell3_k = new TableCell();
        //                                cell3_k.Append(new Paragraph(new Run(new Text(x.TenChuyenNganhDuTuyen))));
        //                                row.Append(cell3_k);

        //                                TableCell cell4_k = new TableCell();
        //                                cell4_k.Append(new Paragraph(new Run(new Text(x.SoLuong.ToString()))));
        //                                row.Append(cell4_k);

        //                                tongsoluong = tongsoluong + x.SoLuong;
        //                                stt++;
        //                                table_soluong.Append(row);
        //                            }

        //                            TableRow row1 = new TableRow();
        //                            TableCell cell11 = new TableCell();
        //                            cell11.Append(new Paragraph(new Run(new Text())));
        //                            row1.Append(cell11);

        //                            TableCell cell12 = new TableCell();
        //                            cell12.Append(new Paragraph(new Run(new Text())));
        //                            row1.Append(cell12);

        //                            TableCell cell13 = new TableCell();
        //                            cell13.Append(new Paragraph(new Run(new Text("Tổng :"))));
        //                            row1.Append(cell13);

        //                            TableCell cell14 = new TableCell();
        //                            cell14.Append(new Paragraph(new Run(new Text(tongsoluong.ToString()))));
        //                            row1.Append(cell14);
        //                            table_soluong.Append(row1);
        //                        }

        //                    }

        //                }

        //                byteArray = stream.ToArray();

        //                //var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //                //string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}.docx";
        //                //// var filePath = System.IO.Path.Combine(systemPath, filename_new);
        //                //var filePath = Server.MapPath("~/" + urlFile + filename_new);

        //                //System.IO.File.WriteAllBytes(filePath, byteArray);
        //                stream.Close();
        //            }
        //            string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.docx";
        //            var filePath = Server.MapPath("~/" + parthdowload + filename_new);
        //            //File(byteArray, "", $"{filePath}");
        //            System.IO.File.WriteAllBytes(filePath, byteArray);

        //            var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //            var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
        //            // Create a new WebClient instance.
        //            WebClient myWebClient = new WebClient();

        //            // Download the Web resource and save it into the current filesystem folder.
        //            myWebClient.DownloadFile(filePath, filePathdowload);

        //            return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            List<int> processesaftergen = getRunningProcesses();
        //            killProcesses(processesbeforegen, processesaftergen);
        //            return Json("Không tìm thấy file", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
        //        var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
        //        ExceptionLogging.SendErrorToText(isOk, actionName, user == null ? string.Empty : user.Username, ex);
        //        List<int> processesaftergen = getRunningProcesses();
        //        killProcesses(processesbeforegen, processesaftergen);
        //        return Json("error", JsonRequestBehavior.AllowGet);
        //    }

        //}

        public JsonResult ReplaceWordDocumentNCS(string idBM_canhan, int idDotTuyenSinh, int idKhoaHoc)
        {
            int id_bm = 2004;
            int id_bmcanhan = int.Parse(idBM_canhan);
            List<int> processesbeforegen = getRunningProcesses();
            string isOk = string.Empty;
            try
            {
                string filename = "";
                string fileurl = "";
                object missing = Missing.Value;
                var get_url = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == id_bm);
                if (get_url != null)
                {
                    fileurl = get_url.FileUrl;
                    filename = get_url.Template;
                }
                if (!string.IsNullOrEmpty(fileurl))
                {
                    var urlchecl = Server.MapPath("~/" + urlFile + fileurl);
                    byte[] byteArray = System.IO.File.ReadAllBytes(urlchecl);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        stream.Write(byteArray, 0, (int)byteArray.Length);
                        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
                        {
                            var lstTxtReplace = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == id_bmcanhan).ToList();
                            var lstBang = lstTxtReplace.Where(x => x.KieuDuLieu == PublicConstant.BM_BANG);
                            string docText = null;
                            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                            {
                                docText = sr.ReadToEnd();
                            }
                            foreach (var item in lstTxtReplace)
                            {
                                //Find and replace:
                                if (item.KieuDuLieu != PublicConstant.BM_BANG)
                                {
                                    Regex regexText = new Regex(item.TenThamSo.Trim());
                                    docText = regexText.Replace(docText, item.GiaTriThamSo);
                                }
                            }

                            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                            {
                                sw.Write(docText);
                            }
                                var lstDanhsach = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == id_bm).ToList();
                                Table table = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(3);

                            #region
                            string tendot = "";
                            DotTuyenSinh dot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == idDotTuyenSinh && x.idKhoahoc == idKhoaHoc);
                            var lst = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x => x.IdDotTS == dot.Id).ToList();
                            List<DotTuyenSinh> lstDot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetListByParameter(x => x.idKhoahoc == idKhoaHoc).ToList();
                            var lstDkts = new List<DangKyTuyenSinh>();
                            foreach (var item in lstDot)
                            {
                                var lstDotTS = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x => x.IdDotTS == item.Id).ToList();
                                if (lstDotTS.Count() > 0)
                                {
                                    foreach (var x in lstDotTS)
                                    {
                                        lstDkts.Add(x);
                                    }
                                }
                                tendot = tendot + item.TenDot + " ";
                            }
                            var tennganh = lst.Select(x => x.TenNganh).Distinct();
                            var tenkhoa = lst.Select(x => x.TenKhoa).Distinct();
                            string[] arrColumnHeader = {"STT","Họ và", "tên","Giới tính",
                    "Ngày sinh","Nơi sinh","Chuyên ngành đào tạo","Mã số","Hình thức đào tạo"};

                            var countColHeader = arrColumnHeader.Count();
                            TableRow rowFirst = table.Elements<TableRow>().First();
                            // Find the third cell in the row.
                            for(int i = 0; i < countColHeader; i++)
                            {
                                TableCell cell1 = rowFirst.Elements<TableCell>().ElementAt(i);
                                // Find the first paragraph in the table cell.
                                Paragraph p1 = cell1.Elements<Paragraph>().First();
                                p1.Append(new Run(new Text(arrColumnHeader[i])));
                            }
                            int stt=1;
                            foreach (DangKyTuyenSinh dkts in lstDkts)
                            {
                                string[] hovaten = dkts.HoTen.Split(' ');
                                string hoten = "";
                                for (int k = 0; k < hovaten.Count() - 1; k++)
                                {
                                    hoten += hovaten[k];
                                }

                                TableRow row = new TableRow();
                                TableCell cell1 = new TableCell();
                                cell1.Append(new Paragraph(new Run(new Text((stt++).ToString()))));
                                row.Append(cell1);

                                TableCell cell2 = new TableCell();
                                cell2.Append(new Paragraph(new Run(new Text((hoten).ToString()))));
                                row.Append(cell2);

                                TableCell cell3 = new TableCell();
                                cell3.Append(new Paragraph(new Run(new Text((hovaten[hovaten.Count() - 1]).ToString()))));
                                row.Append(cell3);

                                TableCell cell4 = new TableCell();
                                cell4.Append(new Paragraph(new Run(new Text(dkts.GioiTinh.ToString()))));
                                row.Append(cell4);

                                TableCell cell5 = new TableCell();
                                cell5.Append(new Paragraph(new Run(new Text(DateTime.Parse(dkts.NgaySinh.ToString()).ToString("dd/MM/yyyy").ToString()))));
                                row.Append(cell5);

                                TableCell cell6 = new TableCell();
                                cell6.Append(new Paragraph(new Run(new Text(dkts.NoiSinh.ToString()))));
                                row.Append(cell6);

                                TableCell cell7 = new TableCell();
                                cell7.Append(new Paragraph(new Run(new Text(dkts.TenChuyenNghanhDuTuyen.ToString()))));
                                row.Append(cell7);


                                var chuyennganhdutuyen = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(cndt => cndt.Id == dkts.ChuyenNghanhDuTuyenId) != null ?
                                   _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(cndt => cndt.Id == dkts.ChuyenNghanhDuTuyenId).MaChuyenNganh : "";
                                var thoigian = dkts.ThoiGianHinhThucDaoTao == "48" ? "Chính quy tập chung 4 năm" : "Chính quy tập chung 3 năm";

                                TableCell cell8 = new TableCell();
                                cell8.Append(new Paragraph(new Run(new Text(chuyennganhdutuyen.ToString()))));
                                row.Append(cell8);

                                TableCell cell9 = new TableCell();
                                cell9.Append(new Paragraph(new Run(new Text(thoigian.ToString()))));
                                row.Append(cell9);

                                table.Append(row);
                            }

                            #endregion


                        }

                        byteArray = stream.ToArray();

                        //var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        //string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}.docx";
                        //// var filePath = System.IO.Path.Combine(systemPath, filename_new);
                        //var filePath = Server.MapPath("~/" + urlFile + filename_new);

                        //System.IO.File.WriteAllBytes(filePath, byteArray);
                        stream.Close();
                    }
                    string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.docx";
                    var filePath = Server.MapPath("~/" + parthdowload + filename_new);
                    //File(byteArray, "", $"{filePath}");
                    System.IO.File.WriteAllBytes(filePath, byteArray);

                    var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();

                    // Download the Web resource and save it into the current filesystem folder.
                    myWebClient.DownloadFile(filePath, filePathdowload);

                    return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<int> processesaftergen = getRunningProcesses();
                    killProcesses(processesbeforegen, processesaftergen);
                    return Json("error Không tìm thấy file", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(isOk, actionName, user == null ? string.Empty : user.Username, ex);
                List<int> processesaftergen = getRunningProcesses();
                killProcesses(processesbeforegen, processesaftergen);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> BieuMauXinTieuBan(string id, int dotId)
        {
            try
            {
                int idBieuMau = int.Parse(id);
                DotTuyenSinh dot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == dotId);
                List<DanhSachCanBoAddForm> lstDS = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == idBieuMau).OrderBy(x => x.IdKhoa).ToList();
                if(lstDS != null && lstDS.Count>2 && lstDS.Count < 6)
                {
                    var lstTenKhoa = lstDS.Select(x => x.TenKhoa).Distinct();
                    //tạo file excel

                    using (ExcelPackage p = new ExcelPackage())
                    {

                        p.Workbook.Properties.Author = "Admin";
                        p.Workbook.Properties.Title = $"Danh sách tiểu ban {dot.NgayBatDau.Value.Year}";
                        int j = 1;

                        #region Tạo sheet 1
                        if (j == 1)
                        {
                            p.Workbook.Worksheets.Add($"DS DU TUYEN {dot.MaKhoaHoc}");
                            ExcelWorksheet ws = p.Workbook.Worksheets[j];

                            string[] arrColumnHeader = { "Khoa", "Tiêu ban chuyên ngành", "Danh sách các thành viên", "Cơ quan công tác", "Trách nhiệm" };
                            var countColHeader = arrColumnHeader.Count();

                            ws.Column(1).Style.Font.Bold = true;
                            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[1, 1].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
                            ws.Cells[1, 1, 1, 3].Merge = true;
                            ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[2, 1].Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI";
                            ws.Cells[2, 1, 2, 3].Merge = true;
                            ws.Cells[2, 1, 2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            ws.Cells[1, 4].Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                            ws.Cells[1, 4, 1, 5].Merge = true;
                            ws.Cells[1, 4, 1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[2, 4].Value = "Độc lập - Tự do - Hạnh phúc";
                            ws.Cells[2, 4, 2, 5].Merge = true;
                            ws.Cells[2, 4, 2, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[4, 1].Value = $"DANH SÁCH CÁC TIỂU BAN CHUYÊN MÔN XÉT TUYỂN NCS - NĂM {dot.NgayBatDau.Value.Year}";
                            ws.Cells[4, 1, 4, countColHeader].Merge = true;
                            ws.Cells[4, 1, 4, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[5, 1].Value = $"( {dot.TenDot} )";
                            ws.Cells[5, 1, 5, countColHeader].Merge = true;
                            ws.Cells[5, 1, 5, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[6, 1].Value = $"(Kèm theo Quyết định số:            /QĐ-ĐHSPHN ngày       tháng     năm {dot.NgayBatDau.Value.Year})";
                            ws.Cells[6, 1, 6, countColHeader].Merge = true;
                            ws.Cells[6, 1, 6, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[6, 1, 6, countColHeader].Style.Font.Italic = true;
                            ws.Cells[6, 1, 6, countColHeader].Style.Font.Bold = false;

                            int colIndex = 1;
                            int rowIndex = 7;
                            foreach (var item in arrColumnHeader)
                            {
                                var cell = ws.Cells[rowIndex, colIndex];
                                var fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                var colFromHex = System.Drawing.ColorTranslator.FromHtml("#78746b");
                                fill.BackgroundColor.SetColor(colFromHex);
                                cell.Value = item;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                colIndex++;
                            }
                            foreach (var tenKhoa in lstTenKhoa)
                            {
                                var lstDSByKhoa = lstDS.Where(x => x.TenKhoa == tenKhoa).ToList();
                                ws.Cells[rowIndex + 1, 1].Value = tenKhoa;
                                if (lstDSByKhoa.Count > 1)
                                {
                                    ws.Cells[rowIndex + 1, 1, rowIndex + lstDSByKhoa.Count, 1].Merge = true;
                                }
                                foreach (DanhSachCanBoAddForm cb in lstDSByKhoa)
                                {
                                    colIndex = 2;
                                    rowIndex++;
                                    ws.Cells[rowIndex, 1].Value = tenKhoa;
                                    ws.Cells[rowIndex, colIndex++].Value = "";
                                    ws.Cells[rowIndex, colIndex++].Value = cb.HocHamHocVi + " " + cb.TenCanBo;
                                    ws.Cells[rowIndex, colIndex++].Value = cb.CoQuanCongTac;
                                    string vaitro = "Trưởng tiểu ban";
                                    if (cb.VaiTro.Trim().Equals(vaitro))
                                    {
                                        ws.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                                    }
                                    ws.Cells[rowIndex, colIndex++].Value = cb.VaiTro;
                                }

                            }

                            rowIndex++;
                            ws.Cells[rowIndex, 1].Value = "Ghi chú:  \n - Thư kí của từng Tiểu ban nhận Hồ sơ 01 ngày trước ngày xét tuyển tại Phòng Sau đại học(Phòng 405).";
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Merge = true;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Font.Bold = false;
                            ws.Row(rowIndex).Height = 48;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            #region set style
                            ws.Cells.Style.WrapText = true;
                            ws.Row(1).Style.Font.Bold = true;
                            ws.Row(2).Style.Font.Bold = true;
                            ws.Row(4).Style.Font.Bold = true;
                            ws.Row(7).Style.Font.Bold = true;

                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                            ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                            ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                            ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                            ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                            ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                            ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));

                            ws.Cells.Style.Font.Name = "Times New Roman";
                            ws.Row(1).Style.Font.Size = ws.Row(2).Style.Font.Size = 10;
                            ws.Row(4).Style.Font.Size = 14;
                            ws.Cells[5, 1, rowIndex, countColHeader].Style.Font.Size = 12;
                            ws.Column(1).Width = 7.5;
                            ws.Column(2).Width = 14;
                            ws.Column(3).Width = 30;
                            ws.Column(4).Width = 25;
                            ws.Column(5).Width = 19;
                            #endregion
                        }
                        #endregion
                        j++;

                        #region Sheet 2
                        if (j == 2)
                        {
                            p.Workbook.Worksheets.Add($"DS DU TUYEN {dot.MaKhoaHoc}");
                            ExcelWorksheet ws = p.Workbook.Worksheets[j];

                            string[] arrColumnHeader = { "Khoa", "Tiêu ban chuyên ngành", "Danh sách các thành viên", "Cơ quan công tác", "Trách nhiệm", "Số tiền", "Kí nhận (ký và ghi rõ họ tên)", "Số thí sinh" };
                            var countColHeader = arrColumnHeader.Count();

                            ws.Column(1).Style.Font.Bold = true;
                            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[1, 1].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
                            ws.Cells[1, 1, 1, 3].Merge = true;
                            ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[2, 1].Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI";
                            ws.Cells[2, 1, 2, 3].Merge = true;
                            ws.Cells[2, 1, 2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            ws.Cells[1, 4].Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                            ws.Cells[1, 4, 1, 5].Merge = true;
                            ws.Cells[1, 4, 1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[2, 4].Value = "Độc lập - Tự do - Hạnh phúc";
                            ws.Cells[2, 4, 2, 5].Merge = true;
                            ws.Cells[2, 4, 2, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[3, 1].Value = $"DANH SÁCH NHẬN KINH PHÍ CỦA CÁC TIỂU BAN CHUYÊN MÔN XÉT TUYỂN NCS - NĂM {dot.NgayBatDau.Value.Year} ( {dot.TenDot})";
                            ws.Cells[3, 1, 3, countColHeader].Merge = true;
                            ws.Cells[3, 1, 3, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[4, 1].Value = $"(Kèm theo Quyết định số:            /QĐ-ĐHSPHN ngày       tháng     năm {dot.NgayBatDau.Value.Year})";
                            ws.Cells[4, 1, 4, countColHeader].Merge = true;
                            ws.Cells[4, 1, 4, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[4, 1, 4, countColHeader].Style.Font.Italic = true;
                            ws.Cells[4, 1, 4, countColHeader].Style.Font.Bold = false;

                            int colIndex = 1;
                            int rowIndex = 5;
                            foreach (var item in arrColumnHeader)
                            {
                                var cell = ws.Cells[rowIndex, colIndex];
                                var fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                var colFromHex = System.Drawing.ColorTranslator.FromHtml("#78746b");
                                fill.BackgroundColor.SetColor(colFromHex);
                                cell.Value = item;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                colIndex++;
                            }
                            var tientieuban1 = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(x => x.SKey.Equals("TienTieuBan1"));
                            var tientieuban2 = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(x => x.SKey.Equals("TienTieuBan2"));
                            foreach (var tenKhoa in lstTenKhoa)
                            {
                                var lstdkts = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x=>x.IdDotTS == dotId && x.TenKhoa == tenKhoa).Count();
                                int tien;
                                if(lstdkts > 1)
                                {
                                    tien = int.Parse(tientieuban2.Value);
                                }
                                else
                                {
                                    tien = int.Parse(tientieuban1.Value);
                                }
                                if(lstdkts > 0)
                                {
                                    var lstDSByKhoa = lstDS.Where(x => x.TenKhoa == tenKhoa).ToList();
                                    ws.Cells[rowIndex + 1, 1].Value = tenKhoa;
                                    if (lstDSByKhoa.Count > 1)
                                    {
                                        ws.Cells[rowIndex + 1, 1, rowIndex + lstDSByKhoa.Count + 1, 1].Merge = true;
                                        ws.Cells[rowIndex + 1, 7, rowIndex + lstDSByKhoa.Count + 1, 7].Merge = true;
                                        ws.Cells[rowIndex + 1, 8, rowIndex + lstDSByKhoa.Count + 1, 8].Merge = true;
                                        ws.Cells[rowIndex + 1, 8, rowIndex + lstDSByKhoa.Count + 1, 8].Value = lstdkts;
                                    }
                                    int tongtien = 0;
                                    foreach (DanhSachCanBoAddForm cb in lstDSByKhoa)
                                    {
                                        colIndex = 2;
                                        rowIndex++;
                                        ws.Cells[rowIndex, 1].Value = tenKhoa;
                                        ws.Cells[rowIndex, colIndex++].Value = "";
                                        ws.Cells[rowIndex, colIndex++].Value = cb.HocHamHocVi + " " + cb.TenCanBo;
                                        ws.Cells[rowIndex, colIndex++].Value = cb.CoQuanCongTac;
                                        string vaitro = "Trưởng tiểu ban";
                                        if (cb.VaiTro.Trim().Equals(vaitro))
                                        {
                                            ws.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                                            ws.Cells[rowIndex, colIndex+1].Style.Font.Bold = true;
                                        }
                                        ws.Cells[rowIndex, colIndex++].Value = cb.VaiTro;
                                        ws.Cells[rowIndex, colIndex++].Value = tien;
                                        tongtien = tongtien + tien;
                                    }
                                    rowIndex++;
                                    ws.Cells[rowIndex, 4].Value = "Tổng số tiền của tiểu ban:";
                                    ws.Cells[rowIndex, 5].Value = tongtien.ToString();
                                }

                            }

                            rowIndex++;
                            ws.Cells[rowIndex, 1].Value = "Ghi chú:  \n - Thư kí của từng Tiểu ban nhận Hồ sơ 01 ngày trước ngày xét tuyển tại Phòng Sau đại học(Phòng 405).";
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Merge = true;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Font.Bold = false;
                            ws.Row(rowIndex).Height = 48;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            #region set style
                            ws.Cells.Style.WrapText = true;
                            ws.Row(1).Style.Font.Bold = true;
                            ws.Row(2).Style.Font.Bold = true;
                            ws.Row(4).Style.Font.Bold = true;
                            ws.Row(7).Style.Font.Bold = true;

                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[7, 1, rowIndex - 1, countColHeader].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                            ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                            ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                            ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                            ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                            ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                            ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                            ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));

                            ws.Cells.Style.Font.Name = "Times New Roman";
                            ws.Row(1).Style.Font.Size = ws.Row(2).Style.Font.Size = 10;
                            ws.Row(4).Style.Font.Size = 14;
                            ws.Cells[5, 1, rowIndex, countColHeader].Style.Font.Size = 12;
                            ws.Column(1).Width = 7.5;
                            ws.Column(2).Width = 14;
                            ws.Column(3).Width = 30;
                            ws.Column(4).Width = 25;
                            ws.Column(5).Width = 19;
                            #endregion
                        }
                        #endregion


                        byte[] bin = p.GetAsByteArray();
                        string name = "Danhsachtuyensinh";
                        string filename_new = $"{name}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.xls";
                        var filePath = Server.MapPath("~/" + parthdowload + filename_new);
                        //File(byteArray, "", $"{filePath}");
                        System.IO.File.WriteAllBytes(filePath, bin);

                        var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                        // Create a new WebClient instance.
                        WebClient myWebClient = new WebClient();

                        // Download the Web resource and save it into the current filesystem folder.
                        myWebClient.DownloadFile(filePath, filePathdowload);

                        return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Số lượng cán bộ trọn phải từ 3 đến 5 người!", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BieuMauQuyetDinhTrungTuyen(int idDotTuyenSinh)
        {
            List<int> processesbeforegen = getRunningProcesses();
            string isOk = string.Empty;
            try
            {
                string filename = "";
                string fileurl = "";
                object missing = Missing.Value;
                int id_bm = 2001;
                var get_url = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == id_bm);
                if (get_url != null)
                {
                    fileurl = get_url.FileUrl;
                    filename = get_url.Template;
                    //fileurl = "QD_CaNhanTrungTuyen.docx";
                    //filename = "QD CaNhanTrungTuyen";
                }

                DotTuyenSinh dot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == idDotTuyenSinh);
                var lst = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x => x.IdDotTS == dot.Id && x.Status == 2).ToList();

                string foldername = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}";
                var filePath1 = Server.MapPath("~/" + parthdowload + foldername);
                var zipPath = Server.MapPath("~/" + parthdowload + foldername + ".zip");
                if (!System.IO.Directory.Exists(filePath1))
                    System.IO.Directory.CreateDirectory(filePath1);
                foreach (DangKyTuyenSinh dkts in lst)
                {
                    if (!string.IsNullOrEmpty(fileurl))
                    {
                        var urlchecl = Server.MapPath("~/" + urlFile + fileurl);
                        byte[] byteArray = System.IO.File.ReadAllBytes(urlchecl);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            stream.Write(byteArray, 0, (int)byteArray.Length);
                            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
                            {
                                string docText = null;
                                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                                {
                                    docText = sr.ReadToEnd();
                                }
                                var lstTxtReplace = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == id_bm).ToList();
                                foreach (var item in lstTxtReplace)
                                {
                                    //Find and replace:
                                    if (item.KieuDuLieu != PublicConstant.BM_BANG)
                                    {
                                        Regex regexText = new Regex(item.TenThamSo.Trim());
                                        docText = regexText.Replace(docText, item.GiaTriThamSo);
                                    }
                                }

                                docText = docText.Replace("{HOVATEN}", dkts.HoTen);
                                docText = docText.Replace("{CHUYENNGANH}", dkts.TenChuyenNghanhDuTuyen);
                                docText = docText.Replace("{NGAYSINH}", DateTime.Parse(dkts.NgaySinh.ToString()).ToString("dd/MM/yyyy"));
                                docText = docText.Replace("{NOISINH}", dkts.NoiSinh);
                                docText = docText.Replace("{NGUOIHUONGDAN}", dkts.NHD1 + ", " + dkts.NHD2);
                                docText = docText.Replace("{TENDETAI}", dkts.TenDeTai);
                                docText = docText.Replace("{MACHUYENNGANH}", _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == dkts.ChuyenNghanhDuTuyenId).MaChuyenNganh);
                                if (dkts.ThoiGianHinhThucDaoTao == "36")
                                {
                                    docText = docText.Replace("{HINHTHUCDAOTAO}", "Tập chung " + 3 + " năm");
                                }
                                else
                                {
                                    docText = docText.Replace("{HINHTHUCDAOTAO}", "Tập chung " + 4 + " năm");
                                }
                                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                                {
                                    sw.Write(docText);
                                }

                            }

                            byteArray = stream.ToArray();

                            //var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            //string filename_new = $"{filename}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}.docx";
                            //// var filePath = System.IO.Path.Combine(systemPath, filename_new);
                            //var filePath = Server.MapPath("~/" + urlFile + filename_new);

                            //System.IO.File.WriteAllBytes(filePath, byteArray);
                            stream.Close();
                        }
                        string filename_new = $"{dkts.HoTen}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.docx";
                        var filePath = Server.MapPath("~/" + parthdowload + foldername + "/" + filename_new);
                        //File(byteArray, "", $"{filePath}");
                        System.IO.File.WriteAllBytes(filePath, byteArray);

                        var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                        // Create a new WebClient instance.
                        WebClient myWebClient = new WebClient();

                        // Download the Web resource and save it into the current filesystem folder.
                        myWebClient.DownloadFile(filePath, filePathdowload);

                    }
                    else
                    {
                        List<int> processesaftergen = getRunningProcesses();
                        killProcesses(processesbeforegen, processesaftergen);
                        return Json("Không tìm thấy file", JsonRequestBehavior.AllowGet);
                    }


                }

                ZipFile.CreateFromDirectory(filePath1, zipPath);

                //Xóa thư mục 
                DirectoryInfo di = new DirectoryInfo(filePath1);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
                di.Delete();
                return Json(url_download + foldername + ".zip", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(isOk, actionName, user == null ? string.Empty : user.Username, ex);
                List<int> processesaftergen = getRunningProcesses();
                killProcesses(processesbeforegen, processesaftergen);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }
        private static DocumentFormat.OpenXml.Wordprocessing.RunProperties GetRunPropertyFromTableCell(TableRow rowCopy, int cellIndex)
        {
            var runProperties = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            var fontname = "Calibri";
            var fontSize = "18";
            try
            {
                fontname =
                    rowCopy.Descendants<TableCell>()
                       .ElementAt(cellIndex)
                       .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                       .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties>()
                       .GetFirstChild<ParagraphMarkRunProperties>()
                       .GetFirstChild<RunFonts>()
                       .Ascii;
            }
            catch
            {
                //swallow
            }
            try
            {
                fontSize =
                       rowCopy.Descendants<TableCell>()
                          .ElementAt(cellIndex)
                          .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                          .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties>()
                          .GetFirstChild<ParagraphMarkRunProperties>()
                          .GetFirstChild<FontSize>()
                          .Val;
            }
            catch
            {
                //swallow
            }
            runProperties.AppendChild(new RunFonts() { Ascii = fontname });
            runProperties.AppendChild(new FontSize() { Val = fontSize });
            return runProperties;
        }
        public List<int> getRunningProcesses()
        {
            List<int> ProcessIDs = new List<int>();
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (Process.GetCurrentProcess().Id == clsProcess.Id)
                    continue;
                if (clsProcess.ProcessName.Contains("WINWORD"))
                {
                    ProcessIDs.Add(clsProcess.Id);
                }
            }
            return ProcessIDs;
        }
        private void killProcesses(List<int> processesbeforegen, List<int> processesaftergen)
        {
            foreach (int pidafter in processesaftergen)
            {
                bool processfound = false;
                foreach (int pidbefore in processesbeforegen)
                {
                    if (pidafter == pidbefore)
                    {
                        processfound = true;
                    }
                }

                if (processfound == false)
                {
                    Process clsProcess = Process.GetProcessById(pidafter);
                    clsProcess.Kill();
                }
            }
        }
        public async Task<ActionResult> LoadDataThamsoBM(string idBM)
        {
            try
            {
                List<ThamSoBieuMau> list_data = new List<ThamSoBieuMau>();
                int idbieumau = int.Parse(idBM);

                list_data = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == idbieumau).ToList();

                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }

                return PartialView("_PartialThamSoBieuMau", list_data);

            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return PartialView("_PartialThamSoBieuMau", null);
            }
        }
        public JsonResult SaveGiaTriThamSo(int idthamso, string giatrithamso, int idBM)
        {
            try
            {
                var bieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == idBM);
                if (bieumau == null)
                {
                    return Json("Không tìm thấy thông tin biểu mẫu", JsonRequestBehavior.AllowGet);
                }
                var tenbieumau = bieumau.Template;
                var thamso = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetFirstOrDefaultByParameter(x => x.Id == idthamso);
                if (thamso != null)
                {
                    thamso.GiaTriThamSo = giatrithamso;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    thamso.UpdatedBy = user.Username;
                    thamso.UpdatedAt = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().Update(thamso);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Giá tri không tồn tại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return Json("cập nhật giá tri lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ResetChonCanBo(int id)
        {
            try
            {
                var lstChon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == id).ToList();

                foreach (var item in lstChon)
                {
                    _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().Remove(item);
                    _unitOfWork.SaveChanges();
                }
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return Json("cập nhật giá tri lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> LoadGVByKhoa(int loaigv, int khoaid)
        {
            try
            {
                List<GiangVienView> lstGV = new List<GiangVienView>();
                if (loaigv == 0 || loaigv == -1)
                {
                    //Giảng viên trong trường
                    List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
                    List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();

                    if (listGVtrong.Count > 0)
                    {
                        foreach (var item in listGVtrong)
                        {
                            GiangVienView gv = new GiangVienView();
                            gv.name = item.Name;
                            gv.hocham = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault().Name;
                            lstGV.Add(gv);
                        }
                    }
                }
                else if (loaigv == 1)
                {
                    //Giảng viên ngoài trường
                    var lstGVngoai = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                    if (lstGVngoai.Count > 0)
                    {
                        foreach (var item in lstGVngoai)
                        {
                            GiangVienView gv = new GiangVienView();
                            gv.name = item.HoTen;
                            gv.hocham = item.HocHamHocViId.ToString();
                            lstGV.Add(gv);
                        }
                    }
                }

                return PartialView("_PartialViewGiangVien", lstGV);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
            }
            return PartialView("_PartialViewGiangVien", null);
        }
        public async Task<ActionResult> LoadGVByKhoa_Form2(int loaigv, int khoaid)
        {
            try
            {
                List<GiangVien_ChucVuView> lstGV = new List<GiangVien_ChucVuView>();
                List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
                List<ChucVuAPI> listChucVu = await CoreAPI.CoreAPI.GetListChucVu();
                List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();
                if (loaigv == 0 || loaigv == -1)
                {
                    if (listGVtrong.Count > 0)
                    {
                        foreach (var item in listGVtrong)
                        {
                            GiangVien_ChucVuView gv = new GiangVien_ChucVuView();
                            List<string> chucvu = new List<string>();
                            gv.name = item.Name;
                            if (item.PositionIds != null)
                            {
                                foreach (var o in item.PositionIds)
                                {
                                    var cv = listChucVu.Where(x => x.Id == o).SingleOrDefault();
                                    if (cv != null)
                                    {
                                        chucvu.Add(cv.Name);
                                    }
                                }
                            }
                            gv.chucvu = chucvu;
                            gv.idgv = item.Id;
                            gv.hochamhocvi = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault().Name;
                            gv.donvicongtac = "Trường ĐHSP Hà Nội";
                            var idcb = item.Id;
                            var check_cbdachon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb && x.IdBieuMau == 1005);
                            if (check_cbdachon != null)
                            {
                                gv.vaitro = check_cbdachon.VaiTro;
                            }
                            lstGV.Add(gv);
                        }
                    }
                }
                else if (loaigv == 1)
                {
                    //Giảng viên ngoài trường
                    var lstGVngoai = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                    if (lstGVngoai.Count > 0)
                    {
                        foreach (var item in lstGVngoai)
                        {
                            GiangVien_ChucVuView gvngoai = new GiangVien_ChucVuView();
                            gvngoai.name = item.HoTen;
                            int idhocham = item.HocHamHocViId.Value;
                            gvngoai.hochamhocvi = listHocHamHocViAPI.Where(o => o.Id == idhocham).SingleOrDefault().Name;
                            gvngoai.idgv = (int)item.Id;
                            gvngoai.donvicongtac = item.DiaChi;
                            lstGV.Add(gvngoai);
                        }
                    }
                }
                return PartialView("_PartialGiangVienChucVu", lstGV);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
            }
            return PartialView("_PartialViewGiangVien", null);
        }
        public async Task<ActionResult> LoadGVByKhoa_Form3(int loaigv, int khoaid, int? idBM = 1)
        {
            try
            {
                List<GiangVien_ChucVuView> lstGV = new List<GiangVien_ChucVuView>();
                List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
                List<ChucVuAPI> listChucVu = await CoreAPI.CoreAPI.GetListChucVu();
                List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();
                if (loaigv == 0 || loaigv == -1)
                {
                    if (listGVtrong.Count > 0)
                    {
                        foreach (var item in listGVtrong)
                        {
                            GiangVien_ChucVuView gv = new GiangVien_ChucVuView();
                            List<string> chucvu = new List<string>();
                            gv.name = item.Name;
                            if (item.PositionIds != null)
                            {
                                foreach (var o in item.PositionIds)
                                {
                                    var cv = listChucVu.Where(x => x.Id == o).SingleOrDefault();
                                    if (cv != null)
                                    {
                                        chucvu.Add(cv.Name);
                                    }
                                }
                            }
                            gv.chucvu = chucvu;
                            gv.idgv = item.Id;
                            gv.hochamhocvi = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault().Name;
                            gv.donvicongtac = item.Department;
                            var idcb = item.Id;
                            var check_cbdachon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb && x.IdBieuMau == idBM);

                            if (check_cbdachon != null)
                            {
                                gv.vaitro = check_cbdachon.VaiTro;
                            }
                            lstGV.Add(gv);
                        }
                    }
                }
                else if (loaigv == 1)
                {
                    //Giảng viên ngoài trường
                    var lstGVngoai = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                    if (lstGVngoai.Count > 0)
                    {
                        foreach (var item in lstGVngoai)
                        {
                            GiangVien_ChucVuView gvngoai = new GiangVien_ChucVuView();
                            gvngoai.name = item.HoTen;
                            int idhocham = item.HocHamHocViId.Value;
                            gvngoai.hochamhocvi = listHocHamHocViAPI.Where(o => o.Id == idhocham).SingleOrDefault().Name;
                            gvngoai.idgv = (int)item.Id;
                            gvngoai.donvicongtac = item.DiaChi;
                            lstGV.Add(gvngoai);
                        }
                    }
                }
                return PartialView("_PartialGiangVienChucVu3", lstGV);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
            }
            return PartialView("_PartialViewGiangVien3", null);
        }

        public JsonResult AddCanBoFormBieuMau(int type, int idcb, string tencb, string chucvu, string vaitro, string hocham, string coquan, int idbm)
        {
            try
            {
                var listCbChon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetAllRecords();
                SoLuongChon strResult = new SoLuongChon();
                if (type == 0)
                {
                    //Thêm mới vào bảng DanhSachCanBoAddForm
                    var checkcb = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb && x.IdBieuMau == idbm);
                    if (checkcb != null)
                    {
                        strResult.mess = "Cán bộ đã được chọn";
                    }
                    else
                    {
                        strResult.chutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_CHUTICH && x.IdBieuMau == idbm).Count();
                        strResult.phochutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_PHOCHUTICH && x.IdBieuMau == idbm).Count();
                        strResult.uvthuongtruc = listCbChon.Where(x => (x.VaiTro == PublicConstant.VT_UVTHUONGTRUC && x.IdBieuMau == idbm) || (x.VaiTro == PublicConstant.VT_UVTHUONGTRUC_TK && x.IdBieuMau == idbm)).Count();
                        strResult.uyvien = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_UV && x.IdBieuMau == idbm).Count();
                        var lstDanhsach = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == idbm).ToList();
                        if (idbm == 2003)
                        {
                            if(vaitro.Equals("UVTT, Trưởng ban") && lstDanhsach.Where(x => x.VaiTro.Equals("UVTT, Trưởng ban")).ToList().Count == 1)
                            {
                                strResult.mess = "error Chỉ có 1 UVTT, Trưởng ban";
                                return Json(strResult, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else if(idbm == 1)
                        {
                            if (vaitro.Equals("Chủ tịch Hội đồng") && lstDanhsach.Where(x => x.VaiTro.Equals("Chủ tịch Hội đồng") ).ToList().Count == 1 )
                            {
                                strResult.mess = "error Chỉ có 1 Chủ tịch Hội đồng";
                                return Json(strResult, JsonRequestBehavior.AllowGet);
                            }
                            else if (vaitro.Equals("Phó chủ tịch HĐ") && lstDanhsach.Where(x => x.VaiTro.Equals("Phó chủ tịch HĐ")).ToList().Count == 1)
                            {
                                strResult.mess = "error Chỉ có 1 Phó chủ tịch HĐ";
                                return Json(strResult, JsonRequestBehavior.AllowGet);
                            }
                            else if (vaitro.Equals("Ủy viên thường trực") && lstDanhsach.Where(x => x.VaiTro.Equals("Ủy viên thường trực")).ToList().Count == 1)
                            {
                                strResult.mess = "error Chỉ có 1 Ủy viên thường trực";
                                return Json(strResult, JsonRequestBehavior.AllowGet);
                            }
                        }
                        checkcb = new DanhSachCanBoAddForm();
                        checkcb.IdCanBo = idcb;
                        checkcb.TenCanBo = tencb;
                        checkcb.VaiTro = vaitro;
                        checkcb.ChucVu = chucvu;
                        checkcb.IdBieuMau = idbm;
                        checkcb.HocHamHocVi = hocham;
                        checkcb.CoQuanCongTac = coquan;
                        _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().Add(checkcb);
                        _unitOfWork.SaveChanges();
                        strResult.mess = "Cán bộ đã được chọn";
                    }
                }
                else if (type == 1)
                {
                    //xóa bỏ cán bộ ở bảng DanhSachCanBoAddForm
                    var checkcb = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb && x.IdBieuMau == idbm);
                    if (checkcb == null)
                    {
                        strResult.mess = "Cán bộ đã được xóa khỏi form chọn";
                    }
                    else
                    {
                        strResult.mess = "Cán bộ đã được xóa khỏi form chọn";
                        _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().Remove(checkcb);
                        _unitOfWork.SaveChanges();
                    }
                }
                listCbChon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetAllRecords();
                strResult.chutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_CHUTICH && x.IdBieuMau ==idbm).Count();
                strResult.phochutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_PHOCHUTICH && x.IdBieuMau == idbm).Count();
                strResult.uvthuongtruc = listCbChon.Where(x => (x.VaiTro == PublicConstant.VT_UVTHUONGTRUC && x.IdBieuMau == idbm) || (x.VaiTro == PublicConstant.VT_UVTHUONGTRUC_TK && x.IdBieuMau == idbm)).Count();
                strResult.uyvien = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_UV && x.IdBieuMau == idbm).Count();
                return Json(strResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AddCanBoXinTieuBan(int type, int idcb, string tencb, string chucvu, string vaitro)
        {
            try
            {
                if (type == 0)
                {
                    //Thêm mới vào bảng DanhSachCanBoAddForm
                    var checkcb = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb);
                    if (checkcb != null)
                    {
                        return Json("Cán bộ đã được chọn", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        checkcb = new DanhSachCanBoAddForm();
                        checkcb.IdCanBo = idcb;
                        checkcb.TenCanBo = tencb;
                        checkcb.VaiTro = vaitro;
                        checkcb.ChucVu = chucvu;
                        checkcb.IdBieuMau = 1005;
                        _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().Add(checkcb);
                        _unitOfWork.SaveChanges();
                    }
                }
                else if (type == 1)
                {
                    //xóa bỏ cán bộ ở bảng DanhSachCanBoAddForm
                    var checkcb = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb);
                    if (checkcb == null)
                    {
                        return Json("Cán bộ đã được xóa khỏi form chọn", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().Remove(checkcb);
                        _unitOfWork.SaveChanges();
                    }
                }
                var listCbChon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetAllRecords();
                SoLuongChon strResult = new SoLuongChon();
                strResult.chutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_CHUTICH).Count();
                strResult.phochutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_PHOCHUTICH).Count();
                strResult.uvthuongtruc = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_UVTHUONGTRUC).Count();
                strResult.uyvien = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_UV).Count();
                return Json(strResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> AddCanBoXinTieuBanCapKHOA(int type, int idcb, string tencb, string chucvu, string vaitro, string hocham, string coquan, int idbm, int idkhoahoc, int dotTS, int? typeupdate = 0)
        {
            SoLuongChon strResult = new SoLuongChon();
            strResult.mess = "";
            try
            {
                var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var roleInfo = (UserRoles)Session[PublicConstant.ROLE_INFO];

                int staffid = int.Parse(loginInfo.StaffId.ToString());
                var user = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
                int departmentID = user.DepartmentId;
               
                if (type == 0)
                {
                    var lst_canbochon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdKhoa == departmentID && x.IdDotTS == dotTS && x.IdKhoahoc == idkhoahoc).ToList();
                    if(lst_canbochon.Where(x=>x.VaiTro.Equals("Trưởng tiểu ban")).ToList().Count >=1 && vaitro.Equals("Trưởng tiểu ban")){
                        strResult.mess = "Error: Chỉ được tối đa 1 trưởng tiểu ban";
                        return Json(strResult, JsonRequestBehavior.AllowGet);
                    }
                    if (lst_canbochon.Where(x => x.VaiTro.Equals("Thư ký")).ToList().Count >= 1 && vaitro.Equals("Thư ký")){
                        strResult.mess = "Error: Chỉ được tối đa 1 thư ký";
                        return Json(strResult, JsonRequestBehavior.AllowGet);
                    }
                    if (lst_canbochon != null && lst_canbochon.Count >= 5)
                    {
                        strResult.mess = "Error: Số lượng cán bộ chọn chỉ được tối đa 5 người";
                        return Json(strResult, JsonRequestBehavior.AllowGet);
                    }
                    //Thêm mới vào bảng DanhSachCanBoAddForm
                    var checkcb = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb && x.IdBieuMau == 1005);
                    if (checkcb != null)
                    {
                        strResult.mess = "Error: Đã tồn tại";
                        return Json(strResult, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        checkcb = new DanhSachCanBoAddForm();
                        checkcb.IdCanBo = idcb;
                        checkcb.TenCanBo = tencb;
                        checkcb.VaiTro = vaitro;
                        checkcb.ChucVu = chucvu;
                        checkcb.IdBieuMau = idbm;
                        checkcb.HocHamHocVi = hocham;
                        checkcb.CoQuanCongTac = coquan.Trim();
                        checkcb.IdKhoahoc = idkhoahoc;
                        checkcb.IdDotTS = dotTS;
                        checkcb.Status = false;
                        checkcb.IdKhoa = departmentID;
                        List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                        var checkKhoa = listKhoa.Where(x => x.Id == departmentID).FirstOrDefault();
                        if (checkKhoa != null)
                        {
                            checkcb.TenKhoa = checkKhoa.Name;
                        }
                        else
                        {
                            checkcb.TenKhoa = "";
                        }
                        strResult.mess = "Thêm cán bộ thành công";
                        _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().Add(checkcb);
                        _unitOfWork.SaveChanges();
                    }
                }
                else if (type == 1)
                {
                    if (typeupdate == 1)
                    {
                        var lst_canbochon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdKhoa == departmentID && x.IdDotTS == dotTS && x.IdKhoahoc == idkhoahoc).ToList();
                        if (lst_canbochon != null && lst_canbochon.Count == 3)
                        {
                            strResult.mess = "Error: Số lượng cán bộ tối thiểu là 3 người!";
                            return Json(strResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //xóa bỏ cán bộ ở bảng DanhSachCanBoAddForm
                    var checkcb = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb);
                    if (checkcb == null)
                    {
                        strResult.mess = "Error";
                        return Json(strResult, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().Remove(checkcb);
                        _unitOfWork.SaveChanges();
                        strResult.mess = "Xóa cán bộ thành công";
                    }
                }
                var listCbChon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetAllRecords();
                strResult.chutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_CHUTICH).Count();
                strResult.phochutichhd = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_PHOCHUTICH).Count();
                strResult.uvthuongtruc = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_UVTHUONGTRUC).Count();
                strResult.uyvien = listCbChon.Where(x => x.VaiTro == PublicConstant.VT_UV).Count();
                return Json(strResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                strResult.mess = "Error";
                return Json(strResult, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> UpdateStatus(long id, int status,int? diemdecuong,int? diemtong)
        {
            try
            {
                Model.DangKyTuyenSinh dkts = new Model.DangKyTuyenSinh();
                dkts = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                DotTuyenSinh dotTS = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == dkts.IdDotTS);
                if (dkts != null)
                {
                    if (status == PublicConstant.STT_XETTUYEN) // khi trúng tuyển thì mới insert sang bảng ncs ở trang quản lý ncs
                    {
                        NCS ncs = new NCS();
                        var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                        KhoaHoc khoahoc = _unitOfWork.GetRepositoryInstance<Model.KhoaHoc>().GetAllRecords().OrderByDescending(o => o.Id).Take(1).SingleOrDefault();
                        if (dotTS != null)
                        {
                            ncs = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetListByParameter(o => o.Ma.Contains(dotTS.MaKhoaHoc.Trim())).OrderByDescending(o => o.Id).Take(1).SingleOrDefault();
                        }
                        else
                        {
                            ncs = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetAllRecords().OrderByDescending(o => o.Id).Take(1).SingleOrDefault();
                        }
                        string lastNumber = ncs != null ? ncs.Ma.Split('S')[1] : "000";
                        string lastKhoaHoc = khoahoc != null ? khoahoc.MaKhoa : "K01";
                        if (dotTS != null)
                        {
                            lastKhoaHoc = dotTS.MaKhoaHoc;
                        }
                        int curentMa = int.Parse(lastNumber);
                        curentMa = curentMa + 1;
                        if (curentMa < 10)
                        {
                            lastNumber = "00" + curentMa.ToString();
                        }
                        if (curentMa > 9 && curentMa < 100)
                        {
                            lastNumber = "0" + curentMa.ToString();
                        }
                        string maNCS = lastKhoaHoc + "NCS" + lastNumber;


                        ncs = new NCS();
                        ncs.Ma = maNCS; // ví dụ mã: K32NCS065
                        ncs.HoTen = dkts.HoTen;
                        ncs.NgaySinh = dkts.NgaySinh;
                        ncs.NoiSinh = dkts.NoiSinh;
                        ncs.HoKhau = dkts.DiaChiLienLac;
                        ncs.DiaChi = dkts.DiaChiLienLac;
                        ncs.DienThoai = dkts.SoDienThoai;
                        ncs.Email = dkts.Email;
                        ncs.DanToc = dkts.DanToc;
                        ncs.GioiTinh = dkts.GioiTinh;
                        ncs.CreatedAt = DateTime.Now;
                        ncs.UpdatedAt = DateTime.Now;
                        ncs.CreatedBy = user.Username;
                        ncs.UpdatedBy = user.Username;

                        ncs.KHoaHocId = int.Parse(khoahoc.Id.ToString());
                        ncs.NganhDaoTaoId = dkts.ChuyenNghanhDuTuyenId;
                        ChuyenNganhDaoTao chuyennganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == dkts.ChuyenNghanhDuTuyenId);
                        if (chuyennganh != null)
                        {
                            ncs.NganhId = chuyennganh.NganhId;
                            ncs.KhoaId = chuyennganh.KhoaId;
                        }
                        else
                        {
                            ncs.NganhId = 0;
                            ncs.KhoaId = 0;
                        }

                        ncs.ChucDanhId = 0;
                        ncs.Type = 0;
                        ncs.DanToc = "";
                        _unitOfWork.GetRepositoryInstance<Model.NCS>().Add(ncs);

                        dkts.MaNCS = ncs.Ma;

                        // insert học phí
                        int dem = int.Parse(dkts.ThoiGianHinhThucDaoTao) / 12;
                        for (int i = 1; i <= dem + 1; i++)
                        {
                            HocPhi hocphi = new HocPhi();
                            hocphi.UserName = ncs.Email;
                            hocphi.MaNCS = ncs.Ma;
                            hocphi.HoTen = ncs.HoTen;
                            hocphi.Khoa = ncs.KhoaId;
                            hocphi.TenKhoa = chuyennganh.TenKhoa;
                            hocphi.ChuyennNghanh = chuyennganh.Id;
                            hocphi.TenChuyenNganh = chuyennganh.TenChuyenNganh;
                            hocphi.KhoaHoc = ncs.KHoaHocId;
                            hocphi.TenKhoaHoc = khoahoc.MaKhoa;
                            hocphi.TrangThai = PublicConstant.CHUA_NOP;
                            MucHocPhi muchocphi = _unitOfWork.GetRepositoryInstance<MucHocPhi>().GetFirstOrDefaultByParameter(x => x.MaKhoa == ncs.KhoaId.ToString() && x.MaNganh == hocphi.ChuyennNghanh.ToString());
                            if (muchocphi != null)
                            {
                                hocphi.MucNop = muchocphi.HocPhi;
                            }
                            if (i > dem)
                            {
                                hocphi.NoiDung = "Học phí dự trù";
                            }
                            else
                            {
                                hocphi.NoiDung = "Học phí năm thứ " + i;
                            }
                            _unitOfWork.GetRepositoryInstance<HocPhi>().Add(hocphi);
                        }



                        // insert đăng ký các học phần bát buộc cua khoa trước
                        //List<HocPhan> listHocPhanBatBuoc = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(o => o.ChuyenNganhId == ncs.NganhDaoTaoId && o.TuChon == false).ToList();
                        List<HocPhan> listHocPhanBatBuoc = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(o => o.KhoaId == ncs.KhoaId && o.TuChon == false).ToList();
                        if (listHocPhanBatBuoc.Count > 0)
                        {
                            HocPhan_NCS hp_ncs = new HocPhan_NCS();
                            foreach (HocPhan hpbb in listHocPhanBatBuoc)
                            {
                                //var lstMonhocbatbuoc = _unitOfWork.GetRepositoryInstance<MonHoc>().GetListByParameter(x => x.HocPhanId == hpbb.Id).ToList();
                                //if (lstMonhocbatbuoc.Count > 0)
                                //{
                                //    foreach (var monhoc in lstMonhocbatbuoc)
                                //    {
                                hp_ncs = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetFirstOrDefaultByParameter(o => o.MaHocPhan == hpbb.MaHocPhan && o.MaNCS == ncs.Ma);
                                if (hp_ncs == null) // chưa insert -> insert mới
                                {
                                    hp_ncs = new HocPhan_NCS();
                                    hp_ncs.MaNCS = ncs.Ma;
                                    hp_ncs.MaHocPhan = hpbb.MaHocPhan;
                                    hp_ncs.TenHocPhan = hpbb.TenHocPhan;
                                    hp_ncs.TinChi = hpbb.SoDVHT;
                                    hp_ncs.Status = true;
                                    hp_ncs.CreatedAt = DateTime.Now;
                                    hp_ncs.UpdatedAt = DateTime.Now;
                                    hp_ncs.CreatedBy = user.Username;
                                    hp_ncs.UpdatedBy = user.Username;
                                    //hp_ncs.MaMonHoc = monhoc.MaMon;
                                    //hp_ncs.TenMonHoc = monhoc.TenMon;
                                    hp_ncs.TuChon = false;
                                    _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Add(hp_ncs);
                                }
                                //    }
                                //}
                            }
                        }

                        BaoVe_NCS baove_ncs = new BaoVe_NCS();
                        baove_ncs = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetFirstOrDefaultByParameter(o => o.MaNCS == ncs.Ma);
                        if (baove_ncs == null)
                        {
                            baove_ncs = new BaoVe_NCS();
                            baove_ncs.MaNCS = ncs.Ma;
                            baove_ncs.Buoc1 = 0;
                            baove_ncs.Buoc2 = 0;
                            baove_ncs.Buoc3 = 0;
                            baove_ncs.Buoc4 = 0;
                            baove_ncs.Buoc5 = 0;
                            baove_ncs.Buoc6 = 0;
                            baove_ncs.Buoc7 = 0;
                            baove_ncs.Status = 0;
                            baove_ncs.CreatedAt = DateTime.Now;
                            baove_ncs.UpdatedAt = DateTime.Now;
                            baove_ncs.CreatedBy = user.Username;
                            baove_ncs.UpdatedBy = user.Username;
                            _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().Add(baove_ncs);
                        }

                        // insert bảng thông tin đề tài

                        ThongTinDeTai detai = new ThongTinDeTai();
                        detai.MaNCS = ncs.Ma;
                        detai.TenDeTai = dkts.TenDeTai;
                        detai.NHD1 = dkts.NHD2;
                        detai.NHD2 = dkts.NHD1;
                        detai.CreatedAt = DateTime.Now;
                        detai.UpdatedAt = DateTime.Now;
                        detai.CreatedBy = user.Username;
                        detai.UpdatedBy = user.Username;
                        _unitOfWork.GetRepositoryInstance<ThongTinDeTai>().Add(detai);

                        // insert bảng thông báo
                        ThongBao thongbao = new ThongBao();
                        thongbao.MaNCS = ncs.Ma;
                        thongbao.CreatedAt = DateTime.Now;
                        thongbao.UpdatedAt = DateTime.Now;
                        thongbao.CreatedBy = user.Username;
                        thongbao.UpdatedBy = user.Username;
                        thongbao.Title = "Quyết định trúng tuyển NCS SĐH";
                        _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);
                    }

                    dkts.Status = status;
                    _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().Update(dkts);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }
                if (dkts != null)
                {
                    TaiKhoan info = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    User user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => x.Email == dkts.Email);
                    if (user != null)
                    {
                        user.UserName = dkts.MaNCS;
                        user.CreatedAt = DateTime.Now;
                        user.CreatedBy = info.Username;
                        user.IsLock = false;
                        _unitOfWork.GetRepositoryInstance<User>().Update(user);
                    }


                    UserRoles usrerole = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.Email == dkts.Email);
                    if (user != null)
                    {
                        usrerole.UserName = dkts.MaNCS;
                        usrerole.Role = PublicConstant.ROLE_NCS;
                        usrerole.CreatedAt = DateTime.Now;
                        usrerole.CreatedBy = info.Username;
                        _unitOfWork.GetRepositoryInstance<UserRoles>().Update(usrerole);
                    }
                    dkts.CapQuyenTruyCap = true;
                    _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().Update(dkts);



                    string title = "Thông báo trúng tuyển";
                    string content = "Bạn đã trúng tuyển! Bạn có thể truy cập vào hệ thống bằng Email hoặc mã NCS của bạn. Mã NCS của bạn là " + dkts.MaNCS;
                    ThongBao tb = new ThongBao();
                    tb.MaNCS = dkts.MaNCS;
                    tb.Email = dkts.Email;
                    tb.Title = "Cấp quyền truy cập";
                    tb.Description = content;
                    tb.CreatedAt = DateTime.Now;
                    tb.UpdatedAt = DateTime.Now;
                    tb.CreatedBy = info.Username;
                    tb.UpdatedBy = info.Username;
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(tb);

                    _unitOfWork.SaveChanges();

                    // Gửi email
                    try
                    {
                        string mess = Utility.SendMail(dkts.Email, title, content);
                        if (!mess.Equals(""))
                        {
                            TempData["error"] = "Lỗi gửi mail thông báo đến NCS";
                            return Json("Lỗi gửi mail thông báo đến NCS!", JsonRequestBehavior.AllowGet);
                        }
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    TempData["error"] = "Không tìm thấy thông tin trên hệ thống";
                    return Json("Không tìm thấy thông tin trên hệ thống", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Cập nhật dữ liệu thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật dữ liệu: " + ex.Message;
                return Json("Lỗi cập nhật dữ liệu", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// khi bấm nút cấp mã truy cập thì sẽ đăng kí tài khoản bằng mã học viên và mật khẩu mặc định từ 1->6
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> CapQuyenTruyCap(long id, string password)
        {
            try
            {
                Model.DangKyTuyenSinh dkts = new Model.DangKyTuyenSinh();
                dkts = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (dkts != null)
                {
                    TaiKhoan info = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    User user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => x.Email == dkts.Email);
                    if (user != null)
                    {
                        user.UserName = dkts.MaNCS;
                        user.CreatedAt = DateTime.Now;
                        user.CreatedBy = info.Username;
                        user.IsLock = false;
                        _unitOfWork.GetRepositoryInstance<User>().Update(user);
                    }


                    UserRoles usrerole = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.Email == dkts.Email);
                    if (user != null)
                    {
                        usrerole.UserName = dkts.MaNCS;
                        usrerole.Role = PublicConstant.ROLE_NCS;
                        usrerole.CreatedAt = DateTime.Now;
                        usrerole.CreatedBy = info.Username;
                        _unitOfWork.GetRepositoryInstance<UserRoles>().Update(usrerole);
                    }
                    dkts.CapQuyenTruyCap = true;
                    _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().Update(dkts);



                    string title = "Thông báo trúng tuyển";
                    string content = "Bạn đã trúng tuyển! Bạn có thể truy cập vào hệ thống bằng Email hoặc mã NCS của bạn. Mã NCS của bạn là " + dkts.MaNCS;
                    ThongBao tb = new ThongBao();
                    tb.MaNCS = dkts.MaNCS;
                    tb.Email = dkts.Email;
                    tb.Title = "Cấp quyền truy cập";
                    tb.Description = content;
                    tb.CreatedAt = DateTime.Now;
                    tb.UpdatedAt = DateTime.Now;
                    tb.CreatedBy = info.Username;
                    tb.UpdatedBy = info.Username;
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(tb);

                    _unitOfWork.SaveChanges();

                    // Gửi email
                    try
                    {
                        string mess = Utility.SendMail(dkts.Email, title, content);
                        if (!mess.Equals(""))
                        {
                            TempData["error"] = "Lỗi gửi mail thông báo đến NCS";
                            return Json("Lỗi gửi mail thông báo đến NCS!", JsonRequestBehavior.AllowGet);
                        }
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    TempData["error"] = "Không tìm thấy thông tin trên hệ thống";
                    return Json("Không tìm thấy thông tin trên hệ thống", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Cấp quyền truy cập thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cấp quyền truy cập: " + ex.Message;
                return Json("Lỗi cấp quyền truy cập", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CreateRandomPassword()
        {
            string password = "";
            try
            {
                password = Utility.RandomString(6);
            }
            catch (Exception ex)
            {
            }

            return Json(password, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DsCanboaddForm(int idBM)
        {
            try
            {
                string strResult = string.Empty;
                var lstDs = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == idBM).ToList();
                foreach (var item in lstDs)
                {
                    strResult += item.TenCanBo + " ; ";
                }
                return Json(strResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật dữ liệu: " + ex.Message;
                return Json("Lỗi cập nhật dữ liệu", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DanhSachNCS()
        {
            List<KhoaHoc> list_khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = list_khoahoc;
            //List<DotTuyenSinh> list_dottuyensinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            //ViewBag.ListDot = list_dottuyensinh;
            return View();
        }
        public ActionResult DanhSachNCSDuTuyen()
        {
            List<KhoaHoc> list_khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = list_khoahoc;
            //List<DotTuyenSinh> list_dottuyensinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            //ViewBag.ListDot = list_dottuyensinh;
            return View();
        }
        public async Task<ActionResult> DanhSachNCSTrungTuyen()
        {
            List<BieuMau> list_bieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListBieumau = list_bieumau;

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;

            //List<DotTuyenSinh> list_dottuyensinh = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            //ViewBag.ListDot = list_dottuyensinh;

            List<KhoaHoc> list_khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = list_khoahoc;

            var list_data = _unitOfWork.GetRepositoryInstance<ThamSoBieuMau>().GetListByParameter(x => x.BieuMauId == 2001).ToList();
            return View(list_data);
        }
        public JsonResult BieuMauExcel(string id, int idDotTuyenSinh, int idKhoaHoc)
        {
            try
            {
                string tendot = "";
                DotTuyenSinh dot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == idDotTuyenSinh && x.idKhoahoc == idKhoaHoc);
                var lst = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x => x.IdDotTS == dot.Id).ToList();
                List<DotTuyenSinh> lstDot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetListByParameter(x => x.idKhoahoc == idKhoaHoc).ToList();
                var lstDkts = new List<DangKyTuyenSinh>();
                foreach (var item in lstDot)
                {
                    var lstDotTS = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x => x.IdDotTS == item.Id).ToList();
                    if (lstDotTS.Count() > 0)
                    {
                        foreach (var x in lstDotTS)
                        {
                            lstDkts.Add(x);
                        }
                    }
                    tendot = tendot + item.TenDot + " ";
                }
                var tennganh = lst.Select(x => x.TenNganh).Distinct();
                var tenkhoa = lst.Select(x => x.TenKhoa).Distinct();

                //tạo file excel

                using (ExcelPackage p = new ExcelPackage())
                {

                    p.Workbook.Properties.Author = "Admin";
                    p.Workbook.Properties.Title = "Báo cáo thống kê";
                    int j = 1;

                    #region Tạo sheet 1 toàn trường (theo khoá)
                    if (j == 1)
                    {
                        p.Workbook.Worksheets.Add($"DS DU TUYEN {dot.MaKhoaHoc}");
                        ExcelWorksheet ws = p.Workbook.Worksheets[j];
                        ws.Cells.Style.WrapText = true;
                        string[] arrColumnHeader = {"STT","Họ và", "tên","Giới tính",
                    "Ngày sinh","Nơi sinh","Chuyên ngành đào tạo","Mã số","Hình thức đào tạo"};

                        var countColHeader = arrColumnHeader.Count();

                        ws.Cells[1, 1].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
                        ws.Cells[1, 1, 1, 3].Merge = true;
                        ws.Cells[2, 1].Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI";
                        ws.Cells[2, 1, 2, 5].Merge = true;

                        ws.Cells[1, 6].Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                        ws.Cells[1, 6, 1, 9].Merge = true;
                        ws.Cells[1, 6, 1, 9].Style.Font.Bold = true;
                        ws.Cells[1, 6, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 6].Value = "ĐỘC LẬP - TỰ DO - HẠNH PHÚC";
                        ws.Cells[2, 6, 2, 9].Merge = true;
                        ws.Cells[2, 6, 2, 9].Style.Font.Bold = true;
                        ws.Cells[2, 6, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[3, 1].Value = $"DANH SÁCH NGHIÊN CỨU SINH KHOÁ {dot.MaKhoaHoc} - NĂM {dot.NgayBatDau.Value.Year} ({tendot})";
                        ws.Cells[3, 1, 3, countColHeader].Merge = true;
                        ws.Cells[3, 1, 3, countColHeader].Style.Font.Bold = true;
                        ws.Cells[3, 1, 3, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[4, 1].Value = $"(Kèm theo QĐ số    / QĐ-ĐHSP HN, ngày  tháng   năm    của Hiệu trưởng trường ĐHSP Hà Nội";
                        ws.Cells[4, 1, 4, countColHeader].Merge = true;
                        ws.Cells[4, 1, 4, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[4, 1, 4, countColHeader].Style.Font.Italic = true;

                        int colIndex = 1;
                        int rowIndex = 5;
                        foreach (var item in arrColumnHeader)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            var fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            var colFromHex = System.Drawing.ColorTranslator.FromHtml("#78746b");
                            fill.BackgroundColor.SetColor(colFromHex);
                            cell.Value = item;

                            colIndex++;
                        }
                        int sothutu = 0;
                        foreach (DangKyTuyenSinh dkts in lstDkts)
                        {
                            colIndex = 1;
                            rowIndex++;
                            sothutu++;
                            ws.Cells[rowIndex, colIndex++].Value = sothutu;
                            string[] hovaten = dkts.HoTen.Split(' ');
                            string hoten = "";
                            for (int k = 0; k < hovaten.Count() - 1; k++)
                            {
                                hoten += hovaten[k];
                            }
                            ws.Cells[rowIndex, colIndex++].Value = hoten;
                            ws.Cells[rowIndex, colIndex++].Value = hovaten[hovaten.Count() - 1];
                            ws.Cells[rowIndex, colIndex++].Value = dkts.GioiTinh;
                            ws.Cells[rowIndex, colIndex++].Value = DateTime.Parse(dkts.NgaySinh.ToString()).ToString("dd/MM/yyyy");
                            ws.Cells[rowIndex, colIndex++].Value = dkts.NoiSinh;
                            ws.Cells[rowIndex, colIndex++].Value = dkts.TenChuyenNghanhDuTuyen;
                            ws.Cells[rowIndex, colIndex++].Value = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(cndt => cndt.Id == dkts.ChuyenNghanhDuTuyenId) != null ?
                               _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(cndt => cndt.Id == dkts.ChuyenNghanhDuTuyenId).MaChuyenNganh : "";
                            ws.Cells[rowIndex, colIndex++].Value = dkts.ThoiGianHinhThucDaoTao == "48" ? "Chính quy tập chung 4 năm" : "Chính quy tập chung 3 năm";
                        }
                        rowIndex++;
                        ws.Cells[rowIndex, 2].Value = "(Danh sách gồm có";
                        ws.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                        ws.Cells[rowIndex, 4].Value = lst.Count().ToString();
                        ws.Cells[rowIndex, 5].Value = "NCS)";
                        ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));



                        #region set style
                        ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[5, 2, rowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.None;
                        ws.Cells[5, 3, rowIndex, 3].Style.Border.Left.Style = ExcelBorderStyle.None;
                        ws.Cells.Style.Font.Name = "Times New Roman";
                        ws.Cells[1, 1, 2, countColHeader].Style.Font.Size = 12;
                        ws.Cells[3, 1, 3, countColHeader].Style.Font.Size = 14;
                        ws.Cells[4, 1, 4, countColHeader].Style.Font.Size = 11;
                        ws.Cells[4, 1, 4, countColHeader].Style.Font.Italic = true;
                        ws.Cells[5, 1, 5, countColHeader].Style.Font.Bold = true;
                        ws.Cells[5, 1, rowIndex, countColHeader].Style.Font.Size = 12;
                        ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(1).Width = 8;
                        ws.Column(2).Width = 18;
                        ws.Column(3).Width = 8;
                        ws.Column(4).Width = 9;
                        ws.Column(5).Width = 15;
                        ws.Column(6).Width = 12;
                        ws.Column(7).Width = 24;
                        ws.Column(8).Width = 10;
                        ws.Column(9).Width = 18;
                        #endregion

                        rowIndex++;
                        ws.Cells[rowIndex, 7].Value = "HIỆU TRƯỞNG";
                        ws.Cells[rowIndex, 7, rowIndex, 8].Merge = true;
                        ws.Cells[rowIndex, 7, rowIndex, 8].Style.Font.Bold = true;
                        ws.Cells[rowIndex, 7, rowIndex, 8].Style.Font.Size = 12;

                        ws.Cells[rowIndex + 5, 7].Value = "GS.TS Nguyễn Văn Minh";
                        ws.Cells[rowIndex + 5, 7, rowIndex + 5, 8].Merge = true;
                        ws.Cells[rowIndex + 5, 7, rowIndex + 5, 8].Style.Font.Bold = true;
                        ws.Cells[rowIndex + 5, 7, rowIndex + 5, 8].Style.Font.Size = 12;
                        j++;
                    }
                    #endregion

                    #region Tạo sheet 2 toàn trường (theo đợt)
                    if (j == 2)
                    {
                        p.Workbook.Worksheets.Add($"Danh sách toàn trường ({dot.TenDot})");
                        ExcelWorksheet ws = p.Workbook.Worksheets[j];

                        ws.Cells.Style.WrapText = true;
                        ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells.Style.Font.Name = "Times New Roman";
                        ws.Cells.Style.Font.Size = 10;
                        ws.Row(1).Style.Font.Size = 13;
                        ws.Row(2).Style.Font.Size = 13;
                        ws.Row(3).Style.Font.Size = 13;
                        ws.Row(4).Style.Font.Size = 14;
                        ws.Row(5).Style.Font.Italic = true;
                        ws.Row(5).Style.Font.Bold = true;
                        ws.Column(1).Width = 8;
                        ws.Column(2).Width = 15;
                        ws.Column(3).Width = 17;
                        ws.Column(4).Width = 9;
                        ws.Column(5).Width = 5.6;
                        ws.Column(6).Width = 12;
                        ws.Column(7).Width = 12;
                        ws.Column(8).Width = 18.5;
                        ws.Column(9).Width = 10.5;
                        ws.Column(10).Width = 10.5;
                        ws.Column(11).Width = 17;

                        string[] arrColumnHeader = {"STT","Khoa","Họ và", "tên","Giới tính",
                    "Ngày sinh","Nơi sinh","Chuyên ngành đào tạo","Điểm đề cương",
                    "Điểm tổng","Kết quả tuyển chọn (Đạt hay không)" };
                        var countColHeader = arrColumnHeader.Count();

                        ws.Cells[1, 1].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
                        ws.Cells[1, 1, 1, 4].Merge = true;
                        ws.Cells[2, 1].Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI";
                        ws.Cells[2, 1, 2, 6].Merge = true;
                        ws.Cells[3, 1].Value = "HỘI ĐỒNG TUYỂN SINH NCS NĂM 2020";
                        ws.Cells[3, 1, 3, 4].Merge = true;
                        ws.Cells[3, 1, 3, 4].Style.Font.Bold = true;

                        ws.Cells[1, 7].Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                        ws.Cells[1, 7, 1, 11].Merge = true;
                        ws.Cells[1, 7, 1, 11].Style.Font.Bold = true;
                        ws.Cells[1, 7, 1, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 7].Value = "ĐỘC LẬP - TỰ DO - HẠNH PHÚC";
                        ws.Cells[2, 7, 2, 11].Merge = true;
                        ws.Cells[2, 7, 2, 11].Style.Font.Bold = true;
                        ws.Cells[2, 7, 2, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[4, 1].Value = $"DANH SÁCH XÉT TUYỂN NGHIÊN CỨU SINH KHOÁ {dot.MaKhoaHoc} - NĂM {dot.NgayBatDau.Value.Year} ({dot.TenDot})";
                        ws.Cells[4, 1, 4, countColHeader].Merge = true;
                        ws.Cells[4, 1, 4, countColHeader].Style.Font.Bold = true;
                        ws.Cells[4, 1, 4, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        int colIndex = 1;
                        int rowIndex = 5;
                        foreach (var item in arrColumnHeader)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            var fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            var colFromHex = System.Drawing.ColorTranslator.FromHtml("#78746b");
                            fill.BackgroundColor.SetColor(colFromHex);
                            cell.Value = item;

                            colIndex++;
                        }
                        int sothutu = 0;
                        foreach (DangKyTuyenSinh dkts in lst)
                        {
                            colIndex = 1;
                            rowIndex++;
                            sothutu++;
                            ws.Cells[rowIndex, colIndex++].Value = sothutu;
                            ws.Cells[rowIndex, colIndex++].Value = dkts.TenKhoa;
                            string[] hovaten = dkts.HoTen.Split(' ');
                            string hoten = "";
                            for (int k = 0; k < hovaten.Count() - 1; k++)
                            {
                                hoten += hovaten[k];
                            }
                            ws.Cells[rowIndex, colIndex++].Value = hoten;
                            ws.Cells[rowIndex, colIndex++].Value = hovaten[hovaten.Count() - 1];
                            ws.Cells[rowIndex, colIndex++].Value = dkts.GioiTinh;
                            ws.Cells[rowIndex, colIndex++].Value = DateTime.Parse(dkts.NgaySinh.ToString()).ToString("dd/MM/yyyy");
                            ws.Cells[rowIndex, colIndex++].Value = dkts.NoiSinh;
                            ws.Cells[rowIndex, colIndex++].Value = dkts.TenChuyenNghanhDuTuyen;
                            ws.Cells[rowIndex, colIndex++].Value = "";
                            ws.Cells[rowIndex, colIndex++].Value = "";
                            if (dkts.Status == 2)
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Đạt";
                            }
                            else
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Không đạt";
                            }
                        }
                        rowIndex++;
                        ws.Cells[rowIndex, 3].Value = "(Danh sách gồm có";
                        ws.Cells[rowIndex, 3, rowIndex, 4].Merge = true;
                        ws.Cells[rowIndex, 5].Value = lst.Count().ToString();
                        ws.Cells[rowIndex, 6].Value = "NCS)";
                        ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));

                        ws.Column(3).Style.Border.Right.Style = ExcelBorderStyle.None;
                        ws.Column(4).Style.Border.Left.Style = ExcelBorderStyle.None;

                        j++;
                    }
                    #endregion

                    //Tạo sheet
                    #region Tạo các sheet danh sách NCS theo đợt / khoa
                    foreach (var x in tenkhoa)
                    {
                        p.Workbook.Worksheets.Add(x);
                        ExcelWorksheet ws = p.Workbook.Worksheets[j];

                        ws.Cells.Style.WrapText = true;
                        ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells.Style.Font.Name = "Times New Roman";
                        ws.Cells.Style.Font.Size = 10;
                        ws.Row(1).Style.Font.Size = 13;
                        ws.Row(2).Style.Font.Size = 13;
                        ws.Row(3).Style.Font.Size = 13;
                        ws.Row(4).Style.Font.Size = 14;
                        ws.Row(5).Style.Font.Italic = true;
                        ws.Row(5).Style.Font.Bold = true;
                        ws.Column(1).Width = 8;
                        ws.Column(2).Width = 15;
                        ws.Column(3).Width = 17;
                        ws.Column(4).Width = 9;
                        ws.Column(5).Width = 5.6;
                        ws.Column(6).Width = 12;
                        ws.Column(7).Width = 12;
                        ws.Column(8).Width = 18.5;
                        ws.Column(9).Width = 10.5;
                        ws.Column(10).Width = 10.5;
                        ws.Column(11).Width = 17;

                        string[] arrColumnHeader = {"STT","Khoa","Họ và", "tên","Giới tính",
                    "Ngày sinh","Nơi sinh","Chuyên ngành đào tạo","Điểm đề cương",
                    "Điểm tổng","Kết quả tuyển chọn (Đạt hay không)" };
                        var countColHeader = arrColumnHeader.Count();

                        ws.Cells[1, 1].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
                        ws.Cells[1, 1, 1, 4].Merge = true;
                        ws.Cells[2, 1].Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI";
                        ws.Cells[2, 1, 2, 6].Merge = true;
                        ws.Cells[3, 1].Value = "HỘI ĐỒNG TUYỂN SINH NCS NĂM 2020";
                        ws.Cells[3, 1, 3, 4].Merge = true;
                        ws.Cells[3, 1, 3, 4].Style.Font.Bold = true;

                        ws.Cells[1, 7].Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                        ws.Cells[1, 7, 1, 11].Merge = true;
                        ws.Cells[1, 7, 1, 11].Style.Font.Bold = true;
                        ws.Cells[1, 7, 1, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 7].Value = "ĐỘC LẬP - TỰ DO - HẠNH PHÚC";
                        ws.Cells[2, 7, 2, 11].Merge = true;
                        ws.Cells[2, 7, 2, 11].Style.Font.Bold = true;
                        ws.Cells[2, 7, 2, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[4, 1].Value = $"DANH SÁCH XÉT TUYỂN NGHIÊN CỨU SINH KHOÁ {dot.MaKhoaHoc} - NĂM {dot.NgayBatDau.Value.Year} (ĐỢT {dot.TenDot})";
                        ws.Cells[4, 1, 4, countColHeader].Merge = true;
                        ws.Cells[4, 1, 4, countColHeader].Style.Font.Bold = true;
                        ws.Cells[4, 1, 4, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        int colIndex = 1;
                        int rowIndex = 5;
                        foreach (var item in arrColumnHeader)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            var fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            var colFromHex = System.Drawing.ColorTranslator.FromHtml("#78746b");
                            fill.BackgroundColor.SetColor(colFromHex);
                            cell.Value = item;

                            colIndex++;
                        }
                        int sothutu = 0;
                        var lstDktsTheoKhoa = lst.Where(item => item.TenKhoa.Equals(x));
                        foreach (DangKyTuyenSinh dkts in lstDktsTheoKhoa)
                        {
                            colIndex = 1;
                            rowIndex++;
                            sothutu++;
                            ws.Cells[rowIndex, colIndex++].Value = sothutu;
                            ws.Cells[rowIndex, colIndex++].Value = dkts.TenKhoa;
                            string[] hovaten = dkts.HoTen.Split(' ');
                            string hoten = "";
                            for (int k = 0; k < hovaten.Count() - 1; k++)
                            {
                                hoten += hovaten[k];
                            }
                            ws.Cells[rowIndex, colIndex++].Value = hoten;
                            ws.Cells[rowIndex, colIndex++].Value = hovaten[hovaten.Count() - 1];
                            ws.Cells[rowIndex, colIndex++].Value = dkts.GioiTinh;
                            ws.Cells[rowIndex, colIndex++].Value = DateTime.Parse(dkts.NgaySinh.ToString()).ToString("dd/MM/yyyy");
                            ws.Cells[rowIndex, colIndex++].Value = dkts.NoiSinh;
                            ws.Cells[rowIndex, colIndex++].Value = dkts.TenChuyenNghanhDuTuyen;
                            ws.Cells[rowIndex, colIndex++].Value = "";
                            ws.Cells[rowIndex, colIndex++].Value = "";
                            if (dkts.Status == 2)
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Đạt";
                            }
                            else
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Không đạt";
                            }
                        }
                        rowIndex++;
                        ws.Cells[rowIndex, 3].Value = "(Danh sách gồm có";
                        ws.Cells[rowIndex, 3, rowIndex, 4].Merge = true;
                        ws.Cells[rowIndex, 5].Value = lstDktsTheoKhoa.Count().ToString();
                        ws.Cells[rowIndex, 6].Value = "NCS)";
                        ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[5, 1, rowIndex - 1, countColHeader].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));

                        ws.Column(3).Style.Border.Right.Style = ExcelBorderStyle.None;
                        ws.Column(4).Style.Border.Left.Style = ExcelBorderStyle.None;

                        j++;
                    }
                    #endregion



                    byte[] bin = p.GetAsByteArray();
                    string name = "Danhsachtuyensinh";
                    string filename_new = $"{name}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.xls";
                    var filePath = Server.MapPath("~/" + parthdowload + filename_new);
                    //File(byteArray, "", $"{filePath}");
                    System.IO.File.WriteAllBytes(filePath, bin);

                    var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();

                    // Download the Web resource and save it into the current filesystem folder.
                    myWebClient.DownloadFile(filePath, filePathdowload);

                    return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BieuMauExcel2(string id, int idDotTuyenSinh, int idKhoaHoc)
        {
            try
            {
                string tendot = "";
                DotTuyenSinh dot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == idDotTuyenSinh && x.idKhoahoc == idKhoaHoc);
                List<DotTuyenSinh> lstDot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetListByParameter(x => x.idKhoahoc == idKhoaHoc).OrderBy(x => x.TenDot).ToList();
                var cndt = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetAllRecords().ToList();
                var lstDkts = new List<DangKyTuyenSinh>();
                foreach (var item in lstDot)
                {
                    var lstDotTS = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x => x.IdDotTS == item.Id).ToList();
                    if (lstDotTS.Count() > 0)
                    {
                        foreach (var x in lstDotTS)
                        {
                            lstDkts.Add(x);
                        }
                    }
                    tendot = tendot + item.TenDot + " ";
                }

                var tennganh = cndt.Select(x => x.TenNganh).Distinct();

                //tạo file excel

                using (ExcelPackage p = new ExcelPackage())
                {

                    p.Workbook.Properties.Author = "Admin";
                    p.Workbook.Properties.Title = "Báo cáo thống kê";
                    int j = 1;

                    #region Tạo sheet 1 toàn trường (theo khoá)
                    if (j == 1)
                    {
                        p.Workbook.Worksheets.Add($"SL TS {dot.MaKhoaHoc}");
                        ExcelWorksheet ws = p.Workbook.Worksheets[j];
                        #region set style
                        ws.Cells.Style.WrapText = true;
                        ws.Cells.Style.Font.Name = "Times New Roman";
                        ws.Cells.Style.Font.Size = 12;
                        ws.Row(2).Style.Font.Size = 10;
                        ws.Column(1).Width = 4;
                        ws.Column(2).Width = 20;
                        ws.Column(3).Width = 30;
                        ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        #endregion
                        ws.Cells[1, 1].Value = $"SỐ LƯỢNG NGHIÊN CỨU SINH DỰ TUYỂN NĂM {dot.NgayBatDau.Value.Year}";


                        int rowIndex = 2;
                        int colIndex = 1;

                        #region Xét value hàng đầu
                        ws.Cells[rowIndex, colIndex++].Value = "STT";
                        ws.Cells[rowIndex, colIndex++].Value = "Ngành";
                        ws.Cells[rowIndex, colIndex++].Value = "Chuyên ngành";
                        foreach (DotTuyenSinh dts in lstDot)
                        {
                            ws.Column(colIndex).Width = 10;
                            ws.Cells[rowIndex, colIndex++].Value = $"Số lượng dự tuyển {dts.TenDot}";
                            ws.Column(colIndex).Width = 10;
                            ws.Cells[rowIndex, colIndex++].Value = $"Số lượng trúng tuyển {dts.TenDot}";
                        }
                        ws.Cells[1, 1, 1, colIndex].Merge = true;
                        ws.Cells[1, 1, 1, colIndex].Style.Font.Bold = true;
                        var colFromHex = System.Drawing.ColorTranslator.FromHtml("#78746b");
                        ws.Cells[1, 1, 1, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 1, colIndex].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        ws.Cells[1, 1, 1, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[rowIndex, colIndex++].Value = "Tổng số Đạt CN";
                        ws.Cells[rowIndex, colIndex++].Value = "Mã ngành";
                        ws.Cells[rowIndex, colIndex++].Value = "Khối Ngành";



                        #endregion

                        int sothutu = 0;
                        foreach (var ten in tennganh)
                        {
                            colIndex = 1;
                            rowIndex++;
                            colIndex++;
                            ws.Cells[rowIndex, colIndex++].Value = ten;
                            int rowIndex2 = rowIndex;
                            var chuyennganh = cndt.Where(x => x.TenNganh.Equals(ten)).ToList();
                            foreach (ChuyenNganhDaoTao cn in chuyennganh)
                            {
                                int tong = 0;
                                sothutu++;
                                colIndex = 3;
                                ws.Cells[rowIndex2, 1].Value = sothutu;
                                ws.Cells[rowIndex2, colIndex++].Value = cn.TenChuyenNganh;
                                foreach (DotTuyenSinh dts in lstDot)
                                {
                                    ws.Cells[rowIndex2, colIndex++].Value = lstDkts.Where(x => x.IdDotTS == dts.Id && x.ChuyenNghanhDuTuyenId == cn.Id).Count() > 0 ?
                                        lstDkts.Where(x => x.IdDotTS == dts.Id && x.ChuyenNghanhDuTuyenId == cn.Id).Count().ToString() : "";
                                    ws.Cells[rowIndex2, colIndex++].Value = lstDkts.Where(x => x.IdDotTS == dts.Id && x.Status == 2 && x.ChuyenNghanhDuTuyenId == cn.Id).Count() > 0 ?
                                        lstDkts.Where(x => x.IdDotTS == dts.Id && x.Status == 2 && x.ChuyenNghanhDuTuyenId == cn.Id).Count().ToString() : "";
                                    tong = lstDkts.Where(x => x.IdDotTS == dts.Id && x.Status == 2 && x.ChuyenNghanhDuTuyenId == cn.Id).Count() > 0 ?
                                        tong + lstDkts.Where(x => x.IdDotTS == dts.Id && x.Status == 2 && x.ChuyenNghanhDuTuyenId == cn.Id).Count() : tong;
                                }
                                ws.Cells[rowIndex2, colIndex++].Value = tong.ToString();
                                ws.Cells[rowIndex2, colIndex++].Value = cn.MaChuyenNganh;
                                rowIndex2++;
                            }

                            ws.Cells[rowIndex, 2, rowIndex2 - 1, 2].Merge = true;
                            rowIndex = rowIndex2 - 1;
                        }
                        ws.Cells[1, 1, rowIndex, colIndex].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, rowIndex, colIndex].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, rowIndex, colIndex].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, rowIndex, colIndex].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                        colIndex = 3;
                        ws.Cells[rowIndex, colIndex++].Value = "Tổng số: ";
                        foreach (DotTuyenSinh dts in lstDot)
                        {
                            ws.Cells[rowIndex, colIndex++].Value = lstDkts.Where(x => x.IdDotTS == dts.Id).Count() > 0 ?
                                    lstDkts.Where(x => x.IdDotTS == dts.Id).Count().ToString() : "0";
                            ws.Cells[rowIndex, colIndex].Style.Font.Size = 16;
                            ws.Cells[rowIndex, colIndex++].Value = lstDkts.Where(x => x.IdDotTS == dts.Id && x.Status == 2).Count() > 0 ?
                                    lstDkts.Where(x => x.IdDotTS == dts.Id && x.Status == 2).Count().ToString() : "0";
                        }
                        ws.Cells[rowIndex, colIndex].Value = lstDkts.Where(x => x.Status == 2).Count() > 0 ? lstDkts.Where(x => x.Status == 2).Count().ToString() : "0";
                        ws.Cells[rowIndex, colIndex].Style.Font.Size = 16;
                        rowIndex++;
                        ws.Cells[rowIndex, 3].Value = $"Tổng số: {dot.idKhoahoc} :";
                        ws.Cells[rowIndex, 4].Value = lstDkts.Where(x => x.Status == 2).Count() > 0 ? lstDkts.Where(x => x.Status == 2).Count().ToString() : "0";
                        ws.Cells[rowIndex, 4].Style.Font.Size = 16;

                        ws.Cells[1, 1, 1, colIndex].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, colIndex, rowIndex, colIndex].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        ws.Cells[rowIndex, 1, rowIndex, colIndex].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                        ws.Cells[1, 1, 1, colIndex].Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, colIndex, rowIndex, colIndex].Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[rowIndex, 1, rowIndex, colIndex].Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));


                        //rowIndex++;
                        //ws.Cells[rowIndex, 1].Value = "(Danh sách gồm có " + lstDkts.Count() + " NCS)";
                        //ws.Cells[rowIndex, 1, rowIndex, countColHeader].Merge = true;


                    }
                    #endregion



                    byte[] bin = p.GetAsByteArray();
                    string name = $"SL TS{dot.MaKhoaHoc} {dot.TenDot}";
                    string filename_new = $"{name}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.xls";
                    var filePath = Server.MapPath("~/" + parthdowload + filename_new);
                    //File(byteArray, "", $"{filePath}");
                    System.IO.File.WriteAllBytes(filePath, bin);

                    var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();

                    // Download the Web resource and save it into the current filesystem folder.
                    myWebClient.DownloadFile(filePath, filePathdowload);

                    return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GuiThongBaoXTB()
        {
            List<KhoaHoc> list = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = list;
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuiThongBaoXTB(ThongBaoXTBViewModel model)
        {
            try
            {
                List<KhoaHoc> list = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
                ViewBag.ListKhoaHoc = list;
                if (string.IsNullOrEmpty(model.NoiDungThongBao))
                {
                    TempData["error"] = "Nội dung thông báo không được để trống";
                    return View(model);
                }
                List<int> lstKhoa = new List<int>();
                List<string> lstEmail = new List<string>();
                List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                DotTuyenSinh dot = _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Id == model.ddlDotTS);
                var listNCS = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(dk => dk.IdDotTS == dot.Id && dk.Status != 0).ToList();
                foreach (var item in listNCS)
                {
                    int chuyennganhid = item.ChuyenNghanhDuTuyenId;
                    int khoaid = 0;
                    var chuyennganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == chuyennganhid);
                    if (chuyennganh != null)
                    {
                        khoaid = chuyennganh.KhoaId.Value;
                        if (!lstKhoa.Contains(khoaid))
                        {
                            lstKhoa.Add(khoaid);
                        }
                    }
                }
                foreach (var itemkhoa in lstKhoa)
                {
                    var checkkhoa = listKhoa.Where(x => x.Id == itemkhoa).FirstOrDefault();
                    if (checkkhoa != null && !string.IsNullOrEmpty(checkkhoa.Email))
                    {
                        lstEmail.Add(checkkhoa.Email);
                    }
                }
                foreach (var itemEmail in lstEmail)
                {
                    //var sendEmai = Utility.SendMail(itemEmail, tieude, noidungthongbao);

                }
                string noidung = model.NoiDungThongBao;
                if (noidung.Contains("/Images/files"))
                {
                    noidung = noidung.Replace("/Images/files", url_domain + "Images/files");
                }
                var sendEmai = Utility.SendMail(EmailSendTest, model.TieuDe, noidung);
                TempData["message"] = "Gửi thông báo thành công!";
                return View();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi gửi thông báo: " + ex.Message;
                return View(model);
            }
        }
        public async Task<ActionResult> DanhSachTieuBanKhoa(int Khoahocid, int dotid, int khoaid)
        {
            List<GiangVien_ChucVuView> lstGV = new List<GiangVien_ChucVuView>();
            List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid, "", 1, 200, 0);
            List<ChucVuAPI> listChucVu = await CoreAPI.CoreAPI.GetListChucVu();
            List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();
            try
            {
                try
                {
                    if (listGVtrong.Count > 0)
                    {
                        foreach (var item in listGVtrong)
                        {
                            GiangVien_ChucVuView gv = new GiangVien_ChucVuView();
                            List<string> chucvu = new List<string>();
                            gv.name = item.Name;
                            if (item.PositionIds != null)
                            {
                                foreach (var o in item.PositionIds)
                                {
                                    var cv = listChucVu.Where(x => x.Id == o).SingleOrDefault();
                                    if (cv != null)
                                    {
                                        chucvu.Add(cv.Name);
                                    }
                                }
                            }
                            gv.chucvu = chucvu;
                            gv.idgv = item.Id;
                            var hocham = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault();
                            if (hocham != null)
                            {
                                gv.hochamhocvi = hocham.Name;
                            }
                            else
                            {
                                gv.hochamhocvi = "";
                            }
                            gv.donvicongtac = item.Department;
                            var idcb = item.Id;
                            var check_cbdachon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetFirstOrDefaultByParameter(x => x.IdCanBo == idcb && x.IdBieuMau == 1005 && x.IdDotTS == dotid && x.IdKhoahoc == Khoahocid);
                            if (check_cbdachon != null)
                            {
                                gv.vaitro = check_cbdachon.VaiTro;
                                gv.id_select = check_cbdachon.Id;
                            }
                            else
                            {
                                gv.vaitro = "";
                                gv.id_select = 0;
                            }
                            lstGV.Add(gv);
                        }
                    }
                    var lstcheck_cbdachon = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(x => x.IdBieuMau == 1005 && x.IdDotTS == dotid && x.IdKhoahoc == Khoahocid).ToList();
                    foreach (var it in lstcheck_cbdachon)
                    {
                        int idcb = it.IdCanBo.Value;
                        var checkcbkhoa = lstGV.Where(o => o.idgv == idcb).FirstOrDefault();
                        if (checkcbkhoa == null)
                        {
                            GiangVien_ChucVuView gv = new GiangVien_ChucVuView();
                            gv.donvicongtac = it.CoQuanCongTac;
                            gv.hochamhocvi = it.HocHamHocVi;
                            gv.idgv = it.IdCanBo.Value;
                            gv.id_select = it.Id;
                            gv.name = it.TenCanBo;
                            gv.TenKhoa = it.TenKhoa;
                            gv.vaitro = it.VaiTro;
                            lstGV.Add(gv);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string actionName = ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                    var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    ExceptionLogging.SendErrorToText(controllerName, actionName, loginInfo == null ? string.Empty : loginInfo.Username, ex);
                }

                //var lstResult = _unitOfWork.GetRepositoryInstance<DanhSachCanBoAddForm>().GetListByParameter(o => o.IdKhoa == khoaid && o.IdDotTS == dotid && o.IdKhoahoc == Khoahocid).ToList();
                return PartialView("_PartialDanhSachTieuBanKhoa", lstGV.OrderByDescending(x => x.id_select).ToList());
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
            }
            return PartialView("_PartialDanhSachTieuBanKhoa", null);
        }

        public JsonResult LoadOption(int id)
        {
            string str = "";
            try
            {
                if(id == 0)
                {
                    str += " <option>Chủ tịch Hội đồng</option><option> Phó chủ tịch HĐ </option><option > Ủy viên thường trực </option><option> Ủy viên </option> ";
                }
                else
                {
                    str += " <option>UVTT, Trưởng ban</option><option>Ủy viên</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
    }

    public class GiangVienView
    {
        public string name { get; set; }
        public string hocham { get; set; }
        public int staffid { get; set; }
        public int idgv { get; set; }
    }
    public class GiangVien_ChucVuView
    {
        public int id_select { get; set; }
        public int idgv { get; set; }
        public string name { get; set; }
        public List<string> chucvu { get; set; }
        public string vaitro { get; set; }
        public string hochamhocvi { get; set; }
        public string donvicongtac { get; set; }
        public string TenKhoa { get; set; }
    }
    public class SoLuongChon
    {
        public int chutichhd { get; set; }
        public int phochutichhd { get; set; }
        public int uvthuongtruc { get; set; }
        public int uyvien { get; set; }

        public string mess { get; set; }
    }
}