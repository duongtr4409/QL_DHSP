using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class NCSViewModel
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Ma { get; set; }
        public string HoTen { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string NoiSinh { get; set; }
        public string HoKhau { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public string DanToc { get; set; }
        public string ChucDanh { get; set; }
        public string Khoa { get; set; }
        public string QuocTich { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<int> Type { get; set; }

        public string NgheNghiep { get; set; }
        public string CoQuanCongTacHienNay { get; set; }
        public string NamBatDauCongTac { get; set; }
        public string HienLaCanBo { get; set; }
        public string ViTriConViecHienTai { get; set; }
        public string ThamNiemNghieNghiep { get; set; }
        public string ChuyenMon { get; set; }
        public string Truong_TN_DaiHoc { get; set; }
        public string Nam_TN_DaiHoc { get; set; }
        public string HeDaoTao_DaiHoc { get; set; }
        public string Nghanh_TN_DaiHoc { get; set; }
        public string DiemTrungBinh_DaiHoc { get; set; }
        public string LoaiTotNghiep_DaiHoc { get; set; }
        public string Url_FileUpload_DaiHoc { get; set; }
        public string Truong_TN_ThacSi { get; set; }
        public string Nam_TN_ThacSi { get; set; }
        public string HeDaoTao_ThacSi { get; set; }
        public string Nghanh_TN_ThacSi { get; set; }
        public string DiemTrungBinh_ThacSi { get; set; }
        public string Url_FileUpload_ThacSi { get; set; }
        public string NgoaiNgu { get; set; }
        public string LoaiVanBangNgoaiNgu { get; set; }
        public string Url_ChungChiNgoaiNgu { get; set; }
        public string BoTucKienThuc { get; set; }
        public string TenChuyenNghanhDuTuyen { get; set; }
        public string DoiTuongDuTuyen { get; set; }
        public string ThoiGianHinhThucDaoTao { get; set; }
        public string TenDeTai { get; set; }
        public string NHD1 { get; set; }
        public string NHD2 { get; set; }
        public string NoiDungPhanHoi { get; set; }
        public string TepFilePhanHoi { get; set; }
        public string Truong_TN_VB2 { get; set; }
        public string Nam_TN_VB2 { get; set; }
        public string HeDaoTao_VB2 { get; set; }
        public string Nghanh_TN_VB2 { get; set; }
        public string LoaiTotNghiep_VB2 { get; set; }
        public string Url_FileUpload_VB2 { get; set; }
        public string DiemTrungBinh_VB2 { get; set; }
        public string ChuyenNghanhDuTuyen { get; set; }
        public float progress { get; set; }
    }
}