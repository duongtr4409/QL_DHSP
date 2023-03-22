using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class QuanLyThongBaoModel
    {
        public int? ThongBaoID { get; set; }
        public string TenThongBao { get; set; }
        public string NoiDung { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public Boolean? HienThi { get; set; }
        public string TenDoiTuongThongBao { get; set; }
        public int? LoaiThongBao { get; set; }
        public int? CapQuanLy { get; set; }
        public string TenCapQuanLy { get; set; }
        public int? NamBatDau { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
        public List<DoiTuongThongBaoModel> DoiTuongThongBao { get; set; }
        public List<LichSuChinhSuaThongBaoModel> LichSuChinhSuaThongBao { get; set; }
    }

    public class DoiTuongThongBaoModel
    {
        public int DoiTuongID { get; set; }
        public int? ThongBaoID { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public string TenCoQuan { get; set; }
        public string TenCanBo { get; set; }
        public string MaCanBo { get; set; }
    }

    public class DoiTuongDeTaiModel
    {
        public int? ParentId { get; set; }
        public int? CapQuanLy { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public string TenCanBo { get; set; }
        public string TenCoQuan { get; set; }
    }


    public class ChiTietThongBaoModel : QuanLyThongBaoModel
    {
        public int DoiTuongID { get; set; }
        public int CanBoID { get; set; }
        public int CoQuanID { get; set; }

        public int FileDinhKemID { get; set; }
        public string TenFileGoc { get; set; }
        public string TenFileHeThong { get; set; }
        public int LoaiFile { get; set; }
        public string FileUrl { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NgayTaoStr { get; set; }
        public Boolean? CoBaoMat { get; set; }
        public int? NguoiTaoID { get; set; }
        public string TenNguoiTao { get; set; }
        public int? ThongTinVaoRaID { get; set; }
        public string Base64File { get; set; }
        public int LoaiUpload { get; set; }
        public int NghiepVuID { get; set; }
        public string NoiDungFile { get; set; }

        public int? ID { get; set; }
        public string TenThongBaoLog { get; set; }
        public string NoiDungLog { get; set; }
        public DateTime? ThoiGianBatDauLog { get; set; }
        public DateTime? ThoiGianKetThucLog { get; set; }
        public Boolean? HienThiLog { get; set; }
        public int CapQuanLyLog { get; set; }
        public string TenCapQuanLyLog { get; set; }
        public int NamBatDauLog { get; set; }
        public int CanBoChinhSuaID { get; set; }
        public int CoQuanChinhSuaID { get; set; }
        public DateTime? NgayChinhSua { get; set; }
        public string TenNguoiChinhSua { get; set; }

        public int DoiTuongIDLog { get; set; }
        public int CanBoIDLog { get; set; }
        public int CoQuanIDLog { get; set; }

    }
    public class ChiTietHienThiThongBaoModel : QuanLyThongBaoModel
    {
        public int DoiTuongID { get; set; }
    }
    public class LichSuChinhSuaThongBaoModel
    {
        public int? ID { get; set; }
        public int? ThongBaoID { get; set; }
        public string TenThongBao { get; set; }
        public string NoiDung { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public Boolean? HienThi { get; set; }
        public string TenDoiTuongThongBao { get; set; }
        public int? LoaiThongBao { get; set; }
        public int? CapQuanLy { get; set; }
        public string TenCapQuanLy { get; set; }
        public int? NamBatDau { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public string TenNguoiChinhSua { get; set; }
        public DateTime? NgayChinhSua { get; set; }
        public List<DoiTuongThongBaoModel> DoiTuongThongBao { get; set; }
        //public List<FileDinhKemModel> FileDinhKem { get; set; }
    }
}
