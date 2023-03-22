using CoreAPI.Entity;
using Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.CMS.Models;
using TEMIS.CMS.Repository;
using TEMIS.Model;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    [AuthorizeRoles(PublicConstant.ROLE_NCS)]
    public class HoSoController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public HoSoController()
        {
        }

        // GET: Admin/GiangVien
        public async Task<ActionResult> Index(int edit = 0)
        {
            NCSViewModel ncsmodel = new NCSViewModel();
            TaiKhoan userLogin = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            var ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Ma == userLogin.Username);
            //if (ncs == null)
            //{
            //    return RedirectToAction("CapNhatHoSo");
            //}
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
            DangKyTuyenSinh dkts = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(x=>x.Email == userLogin.Username || x.Email == userLogin.Email);
            if((dkts != null && dkts.Status == PublicConstant.STT_CHODUYET) || edit !=0 )
            {
                return RedirectToAction("CapNhatHoSo", edit);
            }
            if (dkts != null)
            {
                ncsmodel.Ma = ncs != null ? ncs.Ma : "";
                ncsmodel.HoTen = dkts.HoTen;
                ncsmodel.NgaySinh = dkts.NgaySinh;
                ncsmodel.NoiSinh = dkts.NoiSinh;
                ncsmodel.HoKhau = ncs != null? ncs.HoKhau : "";
                ncsmodel.DiaChi = dkts.DiaChiLienLac;
                ncsmodel.DienThoai = dkts.SoDienThoai;
                ncsmodel.Email = dkts.Email;
                ncsmodel.GioiTinh = dkts.GioiTinh;
                ncsmodel.DanToc = dkts.DanToc;
                ncsmodel.QuocTich = ncs != null ? ncs.QuocTich :"";
                ncsmodel.NgheNghiep = dkts.NgheNghiep;
                ncsmodel.CoQuanCongTacHienNay = dkts.CoQuanCongTacHienNay;
                ncsmodel.NamBatDauCongTac = dkts.NamBatDauCongTac;
                ncsmodel.HienLaCanBo = dkts.HienLaCanBo;
                ncsmodel.ViTriConViecHienTai = dkts.ViTriConViecHienTai;
                ncsmodel.ThamNiemNghieNghiep = dkts.ThamNiemNghieNghiep;
                ncsmodel.ChuyenMon = dkts.ChuyenMon;
                ncsmodel.Truong_TN_DaiHoc = dkts.Truong_TN_DaiHoc;
                ncsmodel.Nam_TN_DaiHoc = dkts.Nam_TN_DaiHoc.ToString();
                ncsmodel.HeDaoTao_DaiHoc = dkts.HeDaoTao_DaiHoc;
                ncsmodel.Nghanh_TN_DaiHoc = dkts.Nghanh_TN_DaiHoc;
                ncsmodel.DiemTrungBinh_DaiHoc = dkts.DiemTrungBinh_DaiHoc;
                ncsmodel.LoaiTotNghiep_DaiHoc = dkts.LoaiTotNghiep_DaiHoc;
                ncsmodel.Url_FileUpload_DaiHoc = dkts.Url_FileUpload_DaiHoc;
                ncsmodel.Truong_TN_ThacSi = dkts.Truong_TN_ThacSi;
                ncsmodel.Nam_TN_ThacSi = dkts.Nam_TN_ThacSi;
                ncsmodel.HeDaoTao_ThacSi = dkts.HeDaoTao_ThacSi;
                ncsmodel.Nghanh_TN_ThacSi = dkts.Nghanh_TN_ThacSi;
                ncsmodel.DiemTrungBinh_ThacSi = dkts.DiemTrungBinh_ThacSi;
                ncsmodel.Url_FileUpload_ThacSi = dkts.Url_FileUpload_ThacSi;
                ncsmodel.NgoaiNgu = dkts.NgoaiNgu;
                ncsmodel.LoaiVanBangNgoaiNgu = dkts.LoaiVanBangNgoaiNgu;
                ncsmodel.Url_ChungChiNgoaiNgu = dkts.Url_ChungChiNgoaiNgu;
                ncsmodel.BoTucKienThuc = dkts.BoTucKienThuc;
                ncsmodel.TenChuyenNghanhDuTuyen = dkts.TenChuyenNghanhDuTuyen;
                ncsmodel.DoiTuongDuTuyen = dkts.DoiTuongDuTuyen;
                ncsmodel.ThoiGianHinhThucDaoTao = dkts.ThoiGianHinhThucDaoTao;
                ncsmodel.TenDeTai = dkts.TenDeTai;
                ncsmodel.NHD1 = dkts.NHD1;
                ncsmodel.NHD2 = dkts.NHD2;
                ncsmodel.NoiDungPhanHoi = dkts.NoiDungPhanHoi;
                ncsmodel.TepFilePhanHoi = dkts.TepFilePhanHoi;
                ncsmodel.Truong_TN_VB2 = dkts.Truong_TN_VB2;
                ncsmodel.Nam_TN_VB2 = dkts.Nam_TN_VB2.ToString();
                ncsmodel.HeDaoTao_VB2 = dkts.HeDaoTao_VB2;
                ncsmodel.Nghanh_TN_VB2 = dkts.Nganh_TN_VB2;
                ncsmodel.LoaiTotNghiep_VB2 = dkts.LoaiTotNghiep_VB2;
                ncsmodel.Url_FileUpload_VB2 = dkts.Url_FileUpload_VB2;
                ncsmodel.DiemTrungBinh_VB2 = dkts.DiemTrungBinh_VB2;
                if(dkts.Status == 0 && ncs !=null)
                {
                    ChucDanhAPI cd = listChucDanh.Where(o => o.Id == ncs.ChucDanhId).SingleOrDefault();
                    if (cd != null)
                    {
                        ncsmodel.ChucDanh = cd.Name;
                    }

                    OrganizationInfo khoa = listKhoa.Where(o => o.Id == ncs.KhoaId).SingleOrDefault();
                    if (khoa != null)
                    {
                        ncsmodel.Khoa = khoa.Name;
                    }
                }                
            }
            
            return View(ncsmodel);
        }

        public async Task<ActionResult> CapNhatHoSo(int edit = 0)
        {
            List<City> listCity = _unitOfWork.GetRepositoryInstance<City>().GetAllRecords().OrderBy(x => x.Name).ToList();
            ViewBag.ListCity = listCity;
            List<DanToc> listDanToc = _unitOfWork.GetRepositoryInstance<DanToc>().GetAllRecords().OrderBy(x => x.TenDanToc).ToList();
            ViewBag.ListDanToc = listDanToc;
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;
            List<ChuyenNganhDaoTao> list = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListChuyenNganhDaoTao = list;
            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            var dkts = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Email.Equals(user.Username) || x.Email.Equals(user.Email));
            ViewBag.IdCity = listCity.Where(x => dkts.TinhThanh_CMND.Contains(x.CityCode)).FirstOrDefault().CityCode;
            if (dkts.Status != 0 && edit == 0)
            {
                return RedirectToAction("Index");
            }
            return View(dkts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> CapNhatHoSoCaNhan(FormCollection form)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var dk = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Email == user.Username);
                if (dk == null)
                {
                    TempData["error"] = "Lỗi cập nhật hồ sơ: ";
                    return RedirectToAction("CapNhatHoSo");
                }

                string HoTen = form["hovaten"] != null ? form["hovaten"] : "";
                string GioiTinh = form["gioitinh"] != null ? form["gioitinh"] : "";
                string ngay = form["ngaysinh"] != null ? form["ngaysinh"].Trim() : "";
                DateTime NgaySinh = form["ngaysinh"] != null ? DateTime.ParseExact(ngay, "dd-MM-yyyy", CultureInfo.CurrentCulture) : DateTime.Now;
                string SoDienThoai = form["sdt"] != null ? form["sdt"] : "";
                //string Email = form["email"] != null ? form["email"] : "";
                string NoiSinh = form["noisinh"] != null ? form["noisinh"] : "noisinh";
                string DiaChiLienLac = form["diachi"] != null ? form["diachi"] : "";
                string NgheNghiep = form["nghenghiep"] != null ? form["nghenghiep"] : "";
                string CoQuanCongTacHienNay = form["coquan"] != null ? form["coquan"] : "";
                string NamBatDauCongTac = form["namct"] != null ? form["namct"] : "";
                string HienLaCanBo = form["canbo"] != null ? form["canbo"] : "";
                string ViTriConViecHienTai = form["vitricv"] != null ? form["vitricv"] : "";
                string ThamNiemNghieNghiep = form["thamnien"] != null ? form["thamnien"] : "";
                string ChuyenMon = form["chuyenmon"] != null ? form["chuyenmon"] : "";
                string DanToc = form["dantoc"] != null ? form["dantoc"] : "";

                string Truong_TN_DaiHoc = form["tentruongct"] != null ? form["tentruongct"] : "";
                int Nam_TN_DaiHoc = form["namtn"] != null ? int.Parse(form["namtn"]) : 0;
                string HeDaoTao_DaiHoc = form["hedt"] != null ? form["hedt"] : "";
                string Nghanh_TN_DaiHoc = form["nganhtn"] != null ? form["nganhtn"] : "";
                string DiemTrungBinh_DaiHoc = form["diemTb"] != null ? form["diemTb"] : "";
                string LoaiTotNghiep_DaiHoc = form["loaitn"] != null ? form["loaitn"] : "";

                HttpPostedFileBase filevbdh = Request.Files["filevbdh"];
                string Url_FileUpload_DaiHoc = filevbdh.FileName;


                string Truong_TN_VB2 = form["tentruong_vb2"] != null ? form["tentruong_vb2"] : "";
                int Nam_TN_VB2 = form["namtn_vb2"] != null && form["namtn_vb2"] != "" ? int.Parse(form["namtn_vb2"]) : 0;
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

                HttpPostedFileBase filevbvb2 = Request.Files["filevb_vb2"];
                string Url_FileUpload_VB2 = filevbvb2 != null ? filevbvb2.FileName : "";


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
                string TenDeTai = form["tendetai"] != null ? form["tendetai"] : "";

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
                if (listGV != null)
                {
                    var item = listGV.Where(x => x.Id == Id_NHD1).FirstOrDefault();
                    if(item != null)
                    {
                        nguoihuongdan1 = listGV.Where(x => x.Id == Id_NHD1).FirstOrDefault().Name;
                    }
                }
                int khoaId_NHD2 = 0;
                int Id_NHD2 = 0;
                string loaiGV_2 = form["loaiGV_2"] != null ? form["loaiGV_2"] : "";

                if (loaiGV_2 == "0")
                {
                    khoaId_NHD2 = form["ddlKhoa_2"] != null && form["ddlKhoa_2"] != "0" ? int.Parse(form["ddlKhoa_2"]) : 0;
                    Id_NHD2 = form["ddlGV_2"] != null && form["ddlGV_2"] != "0" ? int.Parse(form["ddlGV_2"]) : 0;
                    listGV = await CoreAPI.CoreAPI.GetListGiangVien(khoaId_NHD2);
                    if (listGV != null)
                    {
                        var item = listGV.Where(x => x.Id == Id_NHD2).FirstOrDefault();
                        if (item != null)
                        {
                            nguoihuongdan2 = listGV.Where(x => x.Id == Id_NHD2).FirstOrDefault().Name;
                        }
                    }
                }
                else
                {
                    khoaId_NHD2 = 0;
                    Id_NHD2 = 0;
                    nguoihuongdan2 = form["tenGV_2"] != null ? form["tenGV_2"] : "";
                }
                string coquancongtac2 = form["coquancongtacGV_2"] != null && form["coquancongtacGV_2"] != "" ? form["coquancongtacGV_2"] : "";



                if (HoTen == "") { TempData["error"] = "Họ tên không được để trống"; return RedirectToAction("Index"); }
                if (GioiTinh == "") { TempData["error"] = "Bạn chưa chọn giới tính"; return RedirectToAction("Index"); }
                if (SoDienThoai == "") { TempData["error"] = "Điện thoại không được để trống"; return RedirectToAction("Index"); }
                //if (Email == "") { TempData["error"] = "Email không được để trống"; return RedirectToAction("Index"); }
                //else
                //{
                //    var checkUser = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Email.Equals(Email));
                //    if (checkUser != null)
                //    {
                //        TempData["error"] = "Email đã tồn tại. Vui lòng nhập email khác"; return RedirectToAction("Index");
                //    }
                //}
                if (NoiSinh == "") { TempData["error"] = "Nơi sinh không được để trống"; return RedirectToAction("Index"); }
                if (DiaChiLienLac == "") { TempData["error"] = "Địa chỉ không được để trống"; return RedirectToAction("Index"); }
                if (NgheNghiep == "") { TempData["error"] = "Bạn chưa chọn nghề nghiệp"; return RedirectToAction("Index"); }
                if (CoQuanCongTacHienNay == "") { TempData["error"] = "Cơ quan đang công tác không được để trống"; return RedirectToAction("Index"); }
                if (NamBatDauCongTac == "") { TempData["error"] = "Năm bắt đầu công tác không được để trống"; return RedirectToAction("Index"); }
                if (HienLaCanBo == "") { TempData["error"] = "Bạn chưa chọn cán bộ"; return RedirectToAction("Index"); }
                if (ViTriConViecHienTai == "") { TempData["error"] = "Vị trí công việc hiện tại không được để trống"; return RedirectToAction("Index"); }
                if (ThamNiemNghieNghiep == "") { TempData["error"] = "Thâm niên nghề nghiệp không được để trống"; return RedirectToAction("Index"); }
                if (ChuyenMon == "") { TempData["error"] = "Chuyên môn không được để trống"; return RedirectToAction("Index"); }
                if (Truong_TN_DaiHoc == "") { TempData["error"] = "Tên trường tốp nghiệp không được để trống"; return RedirectToAction("Index"); }
                if (Nam_TN_DaiHoc == 0) { TempData["error"] = "Năm tốt nghiệp đại học không được để trống"; return RedirectToAction("Index"); }
                if (HeDaoTao_DaiHoc == "") { TempData["error"] = "Hệ đào tạo đại học không được để trống"; return RedirectToAction("Index"); }
                if (Nghanh_TN_DaiHoc == "") { TempData["error"] = "Ngành tốt nghiệp đại học không được để trống"; return RedirectToAction("Index"); }
                if (DiemTrungBinh_DaiHoc == "") { TempData["error"] = "Điểm trung bình đại học không được để trống"; return RedirectToAction("Index"); }
                if (LoaiTotNghiep_DaiHoc == "") { TempData["error"] = "Loại tốt nghiệp đại học không được để trống"; return RedirectToAction("Index"); }
                //if (Url_FileUpload_DaiHoc == "") { TempData["error"] = "Bạn chưa đính kèm thông tin đại học"; return RedirectToAction("Index"); }

                //if (Url_FileUpload_AnhSoYeuLyLich == "") { TempData["error"] = "Bạn chưa đính kèm ảnh sơ yếu lý lịch"; return RedirectToAction("Index"); }
                //if (Url_FileUpload_CongVanGioiThieu == "") { TempData["error"] = "Bạn chưa đính kèm công văn giới thiệu"; return RedirectToAction("Index"); }
                //if (Url_FileUpload_GiaySucKhoe == "") { TempData["error"] = "Bạn chưa đính kèm giấy khám sức khoẻ"; return RedirectToAction("Index"); }
                //if (Url_FileUpload_HopDongLaoDong == "") { TempData["error"] = "Bạn chưa đính kèm quyết định tuyển dụng hoặc hợp đồng lao động"; return RedirectToAction("Index"); }
                //if (Url_FileUpload_ThuGioiThieu == "") { TempData["error"] = "Bạn chưa đính kèm thư giới thiệu"; return RedirectToAction("Index"); }
                //if (Url_FileUpload_BaiBaoKhoaHoc == "") { TempData["error"] = "Bạn chưa đính kèm bài báo khoa học hoặc báo cáo"; return RedirectToAction("Index"); }
                //if (Url_FileUpload_DeCuongNghienCuu == "") { TempData["error"] = "Bạn chưa đính kèm đề cương nghiên cứu"; return RedirectToAction("Index"); }

                //cmnd
                if (cmnd == "") { TempData["error"] = "Số CMND/CCCD không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_ngaycap == DateTime.Now) { TempData["error"] = "Chọn sai ngày cấp CMND/CCCD"; return RedirectToAction("Index"); }
                if (cmnd_noicap == "") { TempData["error"] = "Nơi cấp CMND/CCCD không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_tinhthanh == "" || cmnd_tinhthanh == null) { TempData["error"] = "Tỉnh thành phố không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_huyen == "" || cmnd_huyen == null) { TempData["error"] = "Huyện không được để trống"; return RedirectToAction("Index"); }
                if (cmnd_xa == "" || cmnd_xa == null) { TempData["error"] = "Xã không được để trống"; return RedirectToAction("Index"); }
                if (TenDeTai == "") { TempData["error"] = "Tên đề tài không được để trống"; return RedirectToAction("ThemMoi"); }

                if (NgoaiNgu == "") { TempData["error"] = "Ngoại ngữ không được để trống"; return RedirectToAction("Index"); }
                if (LoaiVanBangNgoaiNgu == "") { TempData["error"] = "Loại văn bằng chứng chỉ ngoại ngữ không được để trống"; return RedirectToAction("Index"); }
                //if (Url_ChungChiNgoaiNgu == "") { TempData["error"] = "Bạn chưa đính kèm văn bằng, chứng chỉ ngoại ngữ"; return RedirectToAction("Index"); }
                //if (BoTucKienThuc == "") { TempData["error"] = "Bổ túc kiến thức không được để trống"; return RedirectToAction("Index"); }
                if (ChuyenNghanhDuTuyenId == 0) { TempData["error"] = "Bạn chưa chọn chuyên nghành dự tuyển"; return RedirectToAction("Index"); }
                if (DoiTuongDuTuyen == "") { TempData["error"] = "Bạn chưa chọn đối tường dự tuyển"; return RedirectToAction("Index"); }
                if (ThoiGianHinhThucDaoTao == "") { TempData["error"] = "Bạn chưa chọn thời gian và hình thức đào tạo"; return RedirectToAction("Index"); }
                if (ThoiGianHinhThucDaoTao == "36") // chọn 3 năm thì bắt buộc nhập thạc sĩ
                {
                    if (Truong_TN_ThacSi == "") { TempData["error"] = "Tên trường tốt nghiệp thạc sĩ không được để trống"; return RedirectToAction("Index"); }
                    if (Nam_TN_ThacSi == "") { TempData["error"] = "Năm tốt nghiệp thạc sĩ không được để trống"; return RedirectToAction("Index"); }
                    if (HeDaoTao_ThacSi == "") { TempData["error"] = "Hệ đào tạo thạc sĩ không được để trống"; return RedirectToAction("Index"); }
                    if (Nghanh_TN_ThacSi == "") { TempData["error"] = "Ngành tốt nghiệp thạc sĩ không được để trống"; return RedirectToAction("Index"); }
                    if (DiemTrungBinh_ThacSi == "") { TempData["error"] = "Điểm trung bình thạc sĩ không được để trống"; return RedirectToAction("Index"); }
                    //if (Url_FileUpload_ThacSi == "") { TempData["error"] = "Bạn chưa đính kèm thông tin thạc sỹ"; return RedirectToAction("Index"); }
                }

                //folder
                var foldername = dk.Url_FileUpload_Zip;
                var folder = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername);
                //var zipPath = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + ".zip");
                var zipPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + ".zip");
                var temp = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh);
                //string[] files = Directory.GetFiles(@""+temp, "*.zip");
                //foreach (string s in files)
                //{
                //    if(s == zipPath)
                //    {
                //        System.IO.File.Delete(s);
                //    }
                //}
                System.IO.File.Delete(zipPath);

                if (!System.IO.Directory.Exists(folder))
                    System.IO.Directory.CreateDirectory(folder);



                //check file đính kèm
                if (Url_FileUpload_DaiHoc != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_DaiHoc);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_DaiHoc !="")
                    {
                        System.IO.File.Delete(name);
                    }
                    filevbdh.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_DaiHoc));
                    dk.Url_FileUpload_DaiHoc = Url_FileUpload_DaiHoc;
                }
                if (Url_FileUpload_VB2 != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_VB2);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_VB2 != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filevbvb2.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_VB2));
                    dk.Url_FileUpload_VB2 = Url_FileUpload_VB2;
                }
                if (Url_FileUpload_ThacSi != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_ThacSi);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_ThacSi != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filevbts.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_ThacSi));
                    dk.Url_FileUpload_ThacSi = Url_FileUpload_ThacSi;
                }
                if (Url_ChungChiNgoaiNgu != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_ChungChiNgoaiNgu);
                    if (System.IO.File.Exists(name) && dk.Url_ChungChiNgoaiNgu != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filenn.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_ChungChiNgoaiNgu));
                    dk.Url_ChungChiNgoaiNgu = Url_ChungChiNgoaiNgu;
                }
                if (Url_FileUpload_AnhSoYeuLyLich != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_AnhSoYeuLyLich);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_AnhSoYeuLyLich != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filesyll.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_AnhSoYeuLyLich));
                    dk.Url_FileUpload_AnhSoYeuLyLich = Url_FileUpload_AnhSoYeuLyLich;
                }
                if (Url_FileUpload_CongVanGioiThieu != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_CongVanGioiThieu);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_CongVanGioiThieu != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filecvgt.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_CongVanGioiThieu));
                    dk.Url_FileUpload_CongVanGioiThieu = Url_FileUpload_CongVanGioiThieu;
                }
                if (Url_FileUpload_GiaySucKhoe != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_GiaySucKhoe);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_GiaySucKhoe != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filegksk.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_GiaySucKhoe));
                    dk.Url_FileUpload_GiaySucKhoe = Url_FileUpload_GiaySucKhoe;
                }
                if (Url_FileUpload_HopDongLaoDong != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_HopDongLaoDong);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_HopDongLaoDong != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filehdld.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_HopDongLaoDong));
                    dk.Url_FileUpload_HopDongLaoDong = Url_FileUpload_HopDongLaoDong;
                }
                if (Url_FileUpload_ThuGioiThieu != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_ThuGioiThieu);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_ThuGioiThieu != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filetgt.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_ThuGioiThieu));
                    dk.Url_FileUpload_ThuGioiThieu = Url_FileUpload_ThuGioiThieu;
                }
                if (Url_FileUpload_BaiBaoKhoaHoc != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_BaiBaoKhoaHoc);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_BaiBaoKhoaHoc != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filebbkh.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_BaiBaoKhoaHoc));
                    dk.Url_FileUpload_BaiBaoKhoaHoc = Url_FileUpload_BaiBaoKhoaHoc;
                }
                if (Url_FileUpload_DeCuongNghienCuu != "")
                {
                    var name = Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + dk.Url_FileUpload_DeCuongNghienCuu);
                    if (System.IO.File.Exists(name) && dk.Url_FileUpload_DeCuongNghienCuu != "")
                    {
                        System.IO.File.Delete(name);
                    }
                    filedcnc.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_DeCuongNghienCuu));
                    dk.Url_FileUpload_DeCuongNghienCuu = Url_FileUpload_DeCuongNghienCuu;
                }

                dk.HoTen = HoTen;
                dk.GioiTinh = GioiTinh;
                dk.NgaySinh = NgaySinh;
                dk.SoDienThoai = SoDienThoai;
                //dk.Email = Email;
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
                dk.TenDeTai = TenDeTai;

                //CMNd
                dk.SoCMND = cmnd;
                dk.Ngaycap_CMND = cmnd_ngaycap;
                dk.Noicap_CMND = cmnd_noicap;

                var tinhthanh = _unitOfWork.GetRepositoryInstance<City>().GetFirstOrDefaultByParameter(x => x.CityCode == cmnd_tinhthanh);
                var quanhuyen = _unitOfWork.GetRepositoryInstance<District>().GetFirstOrDefaultByParameter(x => x.DistrictCode == cmnd_huyen);
                var xaphuong = _unitOfWork.GetRepositoryInstance<Ward>().GetFirstOrDefaultByParameter(x => x.WardCode == cmnd_xa);
                //dk.TinhThanh_CMND = tinhthanh != null ? tinhthanh.Type + " " + tinhthanh.Name : "";
                //dk.Quan_CMND = quanhuyen != null ? quanhuyen.Type + " " + quanhuyen.Name : "";
                //dk.Xa_CMND = xaphuong != null ? xaphuong.Type + " " + xaphuong.Name : "";

                dk.TinhThanh_CMND = tinhthanh != null ? tinhthanh.CityCode : "";
                dk.Quan_CMND = quanhuyen != null ? quanhuyen.DistrictCode : "";
                dk.Xa_CMND = xaphuong != null ? xaphuong.WardCode : "";

                // văn bằng 2
                dk.Truong_TN_VB2 = Truong_TN_VB2;
                dk.Nam_TN_VB2 = Nam_TN_VB2;
                dk.HeDaoTao_VB2 = HeDaoTao_VB2;
                dk.Nganh_TN_VB2 = Nghanh_TN_VB2;
                dk.DiemTrungBinh_VB2 = DiemTrungBinh_VB2;
                dk.LoaiTotNghiep_VB2 = LoaiTotNghiep_VB2;

                dk.CoQuanCongTac_NHD2 = coquancongtac2;


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
                    dk.KhoaId_NHD2 = 0;
                    dk.Id_NHD2 = 0;
                    dk.NHD2 = nguoihuongdan2;
                }
                

                dk.Truong_TN_ThacSi = Truong_TN_ThacSi;
                dk.Nam_TN_ThacSi = Nam_TN_ThacSi;
                dk.HeDaoTao_ThacSi = HeDaoTao_ThacSi;
                dk.Nghanh_TN_ThacSi = Nghanh_TN_ThacSi;
                dk.DiemTrungBinh_ThacSi = DiemTrungBinh_ThacSi;

                dk.NgoaiNgu = NgoaiNgu;
                dk.LoaiVanBangNgoaiNgu = LoaiVanBangNgoaiNgu;

                dk.BoTucKienThuc = BoTucKienThuc;
                dk.ChuyenNghanhDuTuyenId = ChuyenNghanhDuTuyenId;
                var cndt = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == ChuyenNghanhDuTuyenId);
                dk.TenNganh = cndt.TenNganh;
                dk.TenChuyenNghanhDuTuyen = TenChuyenNghanhDuTuyen;
                dk.DoiTuongDuTuyen = DoiTuongDuTuyen;
                dk.ThoiGianHinhThucDaoTao = ThoiGianHinhThucDaoTao;
                dk.CreatedAt = DateTime.Now;
                dk.UpdatedAt = DateTime.Now;
                if (user != null)
                {
                    dk.CreatedBy = user.Username;
                    dk.UpdatedBy = user.Username;
                }
                dk.Status = 0;
                ZipFile.CreateFromDirectory(folder, zipPath);
                dk.Url_FileUpload_Zip = foldername;
                dk.DanToc = DanToc;
                _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().Update(dk);

                SysNotification noti = new SysNotification();
                noti.UserName = dk.MaNCS != null ? dk.MaNCS : dk.Email;
                noti.Email = dk.Email;
                noti.Title = dk.Email + " cập nhật thông tin cá nhân!";
                noti.CreatedAt = DateTime.Now;
                noti.CreatedBy = dk.Email;
                _unitOfWork.GetRepositoryInstance<SysNotification>().Add(noti);

                _unitOfWork.SaveChanges();
                TempData["message"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật hồ sơ: " + ex.Message;
                return RedirectToAction("CapNhatHoSo");
            }
        }
    }
}