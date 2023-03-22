using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class BaoCaoThongKeModel
    {
        public int ID { get; set; }
        public string HangMuc { get; set; }
        public List<BCThongKeNhiemVuKHModel> NhiemVuKhoaHoc { get; set; }
    }
    public class BCThongKeNhiemVuKHModel
    {
        public int ID { get; set; }
        public int NghiepVuID { get; set; }
        public string HangMuc { get; set; }
        public string MaSo { get; set; }
        public int TongSo { get; set; }
        public int SoChuNhiemLaNu { get; set; }
        public int SoPheDuyet { get; set; }
        public int SoChuyenTiep { get; set; }
        public int SoDuocNghiemThu { get; set; }
        public int SoDaUngDung { get; set; }
    }
    public class BCDanhSachNhiemVuKHModel
    {
        public int NhiemVuID { get; set; }
        public int ChuNhiemDeTaiID { get; set; }
        public string TenNhiemVu { get; set; }
        public string TenChuNhiemDeTai { get; set; }
        public string MucTieu { get; set; }
        public string KetQuaSanPham { get; set; }
        public string ThoiGianThucHien { get; set; }
        public decimal TongKinhPhi { get; set; }
        public decimal NSNN { get; set; }
        public decimal NguonKhac { get; set; }
        public decimal TongDaCap { get; set; }
        public decimal NSNNDaCap { get; set; }
        public decimal NguonKhacDaCap { get; set; }
        public decimal Tong_NamHienTai { get; set; }
        public decimal NSNN_NamHienTai { get; set; }
        public decimal NguonKhac_NamHienTai { get; set; }
        public string GhiChu { get; set; }
    }
    public class ChiTietTinhHinhKetQuaThucHienModel
    {
        public int CapQuanLy { get; set; }
        public int ChuNhiemDeTaiID { get; set; }
        public string TenCapQuanLy { get; set; }
        public int NhiemVuID { get; set; }
        public string TenNhiemVu { get; set; }
        public string DonViChuTri { get; set; }
        public string ThoiGianThucHien { get; set; }
        public decimal KinhPhiNSNN { get; set; }
        public decimal NguonKhac { get; set; }
        public decimal SoDaCap { get; set; }
        public string KetQuaDatDuoc { get; set; }
        public string GhiChu { get; set; }
    }
    public class BCTinhHinhKetQuaModel
    {
        public int CapQuanLy { get; set; }
        public string TenCapQuanLy { get; set; }
        public List<ChiTietTinhHinhKetQuaThucHienModel> NhiemVuKhoaHoc { get; set; }
    }
    public class BCThongKeKetQuaNghienCuuModel
    {
        public int? KetQuaNghienCuuID { get; set; }
        public int? CTNhaKhoaHocID { get; set; }
        public string TacGia { get; set; }
        public string TenCongTrinhKhoaHoc { get; set; }
        public string TenTapChiSachHoiThao { get; set; }
        public string So { get; set; }
        public string Trang { get; set; }
        public string NhaXuatBan { get; set; }
        public string ThoiGian { get; set; }
        public Boolean CongBo { get; set; }
    }
    public class BCThongKeHoatDongNghienCuuModel
    { 
        public string MaDeTai { get; set; }
        public string TenNhiemVu { get; set; }
        public string NguoiChuTri { get; set; }
        public string NguoiChuTriIDStr { get; set; }
        public string DoiTac { get; set; }
        public string ThoiGianThucHien { get; set; }
        public string TrangThaiThucHien { get; set; }
        public decimal KinhPhiThucHien { get; set; }
        public string BaiBao { get; set; }
        public string SachChuyenKhao { get; set; }
        public string DaoTao { get; set; }
        public string SanPhamKhac { get; set; }
    }

    public class BCKeKhaiBaiBaoKhoaHocModel
    {
        public string TenBaiBao { get; set; }
        public string MaDeTai { get; set; }
        public string TenCacTacGia { get; set; }
        public string TenTapChiHoiThao { get; set; }
        public string HeSoAnhHuong { get; set; }
        public string ISI_SCOPUS { get; set; }
        public string Rank_SCIMAGO { get; set; }
        public string DiemTapChi { get; set; }
        public int LoaiBaiBao { get; set; }
        public int CapHoiThao { get; set; }
        public string LoaiHoiThao { get; set; }
        public string ISSN { get; set; }
        public string Tap { get; set; }
        public string So { get; set; }
        public string NamDangTai { get; set; }
        public string TuTrangDenTrang { get; set; }
        public string LinkBaiBao { get; set; }
        public string LinhVucNganhKhoaHoc { get; set; }
    }
}
