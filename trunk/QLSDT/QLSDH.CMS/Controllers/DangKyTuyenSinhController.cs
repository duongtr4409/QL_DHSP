using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
using System.Globalization;
using CoreAPI.Entity;
using System.IO.Compression;

namespace TEMIS.CMS.Controllers
{
    public class DangKyTuyenSinhController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: DangKyTuyenSinh
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            List<City> listCity = _unitOfWork.GetRepositoryInstance<City>().GetAllRecords().OrderBy(x => x.Name).ToList();
            ViewBag.ListCity = listCity;
            List<DanToc> lstDanToc = _unitOfWork.GetRepositoryInstance<DanToc>().GetAllRecords().OrderBy(x => x.TenDanToc).ToList();
            ViewBag.ListDanToc = lstDanToc;
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;
            List<ChuyenNganhDaoTao> list = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListChuyenNganhDaoTao = list;
            return View();
        }

        public ActionResult Dangkythanhcong()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> DangKy(FormCollection form)
        {
            try
            {
                string HoTen = form["hovaten"] != null ? form["hovaten"] : "";
                string GioiTinh = form["gioitinh"] != null ? form["gioitinh"] : "";
                DateTime NgaySinh = form["ngaysinh"] != null ? DateTime.ParseExact(form["ngaysinh"].Trim(), "dd-MM-yyyy", CultureInfo.CurrentCulture) : DateTime.Now;
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
                    if (item != null)
                    {
                        nguoihuongdan1 = item.Name;
                    }
                }
                int khoaId_NHD2 = 0;
                int Id_NHD2 = 0;
                string loaiGV_2 = form["loaiGV_2"] != null && form["ddlKhoa_2"] != "0" ? form["loaiGV_2"] : "";
                string coquancongtac = form["coquancongtacGV_2"] != null ? form["coquancongtacGV_2"] : "";
                if (loaiGV_2 == "0")
                {
                    khoaId_NHD2 = form["ddlKhoa_2"] != null ? int.Parse(form["ddlKhoa_2"]) : 0;
                    Id_NHD2 = form["ddlGV_2"] != null ? int.Parse(form["ddlGV_2"]) : 0;
                    listGV = await CoreAPI.CoreAPI.GetListGiangVien(khoaId_NHD2);
                    if (listGV != null)
                    {
                        var item = listGV.Where(x => x.Id == Id_NHD2).FirstOrDefault();
                        if (item != null)
                        {
                            nguoihuongdan2 = item.Name;
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
                if (Email == "") { TempData["error"] = "Email không được để trống"; return RedirectToAction("Index"); }
                else
                {
                    var checkUser = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(x => x.Email.Equals(Email));
                    if (checkUser != null)
                    {
                        TempData["error"] = "Email đã tồn tại. Vui lòng nhập email khác"; return RedirectToAction("Index");
                    }
                }
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
                if (Url_FileUpload_DaiHoc == "") { TempData["error"] = "Bạn chưa đính kèm thông tin đại học"; return RedirectToAction("Index"); }

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

                if (NgoaiNgu == "") { TempData["error"] = "Ngoại ngữ không được để trống"; return RedirectToAction("Index"); }
                if (LoaiVanBangNgoaiNgu == "") { TempData["error"] = "Loại văn bằng chứng chỉ ngoại ngữ không được để trống"; return RedirectToAction("Index"); }
                if (Url_ChungChiNgoaiNgu == "") { TempData["error"] = "Bạn chưa đính kèm văn bằng, chứng chỉ ngoại ngữ"; return RedirectToAction("Index"); }
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
                    if (Url_FileUpload_ThacSi == "") { TempData["error"] = "Bạn chưa đính kèm thông tin thạc sỹ"; return RedirectToAction("Index"); }
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
                    //if (!filevbvb2.ContentType.Contains("image") && !filevbvb2.ContentType.Contains("pdf")) { TempData["error"] = "File văn bằng 2 không đúng định dạng"; return RedirectToAction("Index"); }
                    filevbvb2.SaveAs(Server.MapPath("~/Upload/" + PublicConstant.TaiLieuTuyenSinh + "/" + foldername + "/" + Url_FileUpload_VB2));
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
                dk.DanToc = DanToc;
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
                    dk.CoQuanCongTac_NHD2 = coquancongtac;
                }

                dk.CoQuanCongTac_NHD2 = coquancongtac2;

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
                dk.NganhId = cndt.NganhId;
                dk.TenKhoa = cndt.TenKhoa;
                dk.KhoaId = cndt.KhoaId;
                dk.TenChuyenNghanhDuTuyen = TenChuyenNghanhDuTuyen;
                dk.DoiTuongDuTuyen = DoiTuongDuTuyen;
                dk.ThoiGianHinhThucDaoTao = ThoiGianHinhThucDaoTao;
                dk.CreatedAt = DateTime.Now;
                dk.UpdatedAt = DateTime.Now;
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                if (user != null)
                {
                    dk.CreatedBy = user.Username;
                    dk.UpdatedBy = user.Username;
                }
                dk.Status = 0;
                ZipFile.CreateFromDirectory(folder, zipPath);
                dk.Url_FileUpload_Zip = foldername;
                KhoaHoc khoahoc = _unitOfWork.GetRepositoryInstance<Model.KhoaHoc>().GetAllRecords().OrderByDescending(o => o.Id).Take(1).SingleOrDefault();
                DotTuyenSinh dotTS = khoahoc != null ? _unitOfWork.GetRepositoryInstance<DotTuyenSinh>().GetFirstOrDefaultByParameter(x => x.idKhoahoc == khoahoc.Id && x.Status == 1) : null;
                dk.IdDotTS = dotTS?.Id;
                dk.TrangThaiTuyenSinh = PublicConstant.STT_DANGKYMOI;

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
                hocphi.Khoa = cndt.KhoaId;
                hocphi.TenKhoa = cndt.TenKhoa;
                hocphi.HoTen = HoTen;
                hocphi.TrangThai = PublicConstant.CHUA_NOP;
                hocphi.CreatedAt = DateTime.Now;
                hocphi.CreatedBy = HoTen;
                hocphi.Type = 1;
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
                return RedirectToAction("Dangkythanhcong");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi đăng ký: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async System.Threading.Tasks.Task<JsonResult> LoadGVByKhoa(int khoaid, int? id_GV = -1)
        {
            string str = "";
            try
            {
                List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();
                List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
                if (listGVtrong.Count > 0)
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                    foreach (var item in listGVtrong)
                    {
                        var hocham = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault().Name;

                        if (item.Id == id_GV)
                        {
                            str += "<option selected value=\"" + item.Id + "\">" + item.Name +" (" + hocham +")</option>";

                        }
                        else
                        {
                            str += "<option value=\"" + item.Id + "\">" + item.Name + " (" + hocham + ")</option>";

                        }
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDistrictByCity(string CityCode, string NameDistrict = "")
        {
            string str = "";
            try
            {
                List<District> listData = _unitOfWork.GetRepositoryInstance<District>().GetListByParameter(x => x.CityCode == CityCode).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                    foreach (var item in listData)
                    {
                        if(NameDistrict != "")
                        {
                            if (NameDistrict.Equals(item.DistrictCode))
                            {
                                str += "<option selected value=\"" + item.DistrictCode + "\">" + item.Type + " " + item.Name + "</option>";
                                ViewBag.IdDistrict = item.DistrictCode;
                            }
                                else
                            {
                                str += "<option value=\"" + item.DistrictCode + "\">" + item.Type + " " + item.Name + "</option>";
                            }
                        }
                        else
                        {
                            str += "<option value=\"" + item.DistrictCode + "\">" + item.Type + " " + item.Name + "</option>";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DanToc(string key)
        {
            //<li onClick = "selectCountry('<?php echo $country["country_name"]; ?>');"><?php echo $country["country_name"]; ?></li>

            string str = "";
            try
            {
                List<DanToc> listData = _unitOfWork.GetRepositoryInstance<DanToc>().GetAllRecords().ToList();
                if (listData.Count > 0)
                {
                    str += "";
                    foreach (var item in listData)
                    {
                        string[] lstTenKhac = item.TenKhac.Split(',');
                        foreach (string itemTenKhac in lstTenKhac){
                            if (itemTenKhac.Contains(key)||item.TenDanToc.Contains(key))
                            {
                                //str += "li onClick = "\"" + item.DistrictCode + "\">" + item.Type + " " + item.Name + "</option>";
                                //<li onClick = "selectCountry('<?php echo $country["country_name"]; ?>');" ><? php echo $country["country_name"]; ?></ li >
                            }
                        }                       
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadWardByDistrict(string DistrictCode, string NameWard="")
        {
            string str = "";
            try
            {
                District district = _unitOfWork.GetRepositoryInstance<District>().GetFirstOrDefaultByParameter(x => DistrictCode.Contains(x.Type) && DistrictCode.Contains(x.Name));
                List<Ward> listData = new List<Ward>();
                if (district != null)
                {
                    listData = _unitOfWork.GetRepositoryInstance<Ward>().GetListByParameter(x => x.DistrictCode == district.DistrictCode).ToList();
                }
                else
                {
                    listData = _unitOfWork.GetRepositoryInstance<Ward>().GetListByParameter(x => x.DistrictCode == DistrictCode ).ToList();
                }
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                    foreach (var item in listData)
                    {
                        if (NameWard != "")
                        {
                            if (NameWard.Equals(item.WardCode))
                            {
                                str += "<option selected value=\"" + item.WardCode + "\">" + item.Type + " " + item.Name + "</option>";
                            }
                            else
                            {
                                str += "<option value=\"" + item.WardCode + "\">" + item.Type + " " + item.Name + "</option>";
                            }
                        }
                        else
                        {
                            str += "<option value=\"" + item.WardCode + "\">" + item.Type + " " + item.Name + "</option>";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadChuyenNganhByKhoa(int khoaid)
        {
            string str = "";
            try
            {
                List<ChuyenNganhDaoTao> listData = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                    foreach (var item in listData)
                    {
                        str += "<option value=\"" + item.Id + "\">" + item.TenChuyenNganh + "</option>";
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadChuyenNganhByKhoaCapNhat(int khoaid, int chuyennganhdutuyenid)
        {
            string str = "";
            try
            {
                List<ChuyenNganhDaoTao> listData = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                    foreach (var item in listData)
                    {
                        if (item.Id == chuyennganhdutuyenid)
                        {
                            str += "<option selected value=\"" + item.Id + "\">" + item.TenChuyenNganh + "</option>";
                        }
                        else
                        {
                            str += "<option value=\"" + item.Id + "\">" + item.TenChuyenNganh + "</option>";
                        }
                       
                    }
                }
                else
                {
                    str += "<option value=\"0\">--------- chọn --------</option>";
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
    }
}