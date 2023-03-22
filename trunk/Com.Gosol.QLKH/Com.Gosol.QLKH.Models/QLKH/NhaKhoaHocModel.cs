using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class NhaKhoaHocModel
    {
        public int CanBoID { get; set; }
        public string MaCB { get; set; }
        public string TenCanBo { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? GioiTinh { get; set; }
        public string GioiTinhStr { get; set; }
        public List<int> ChucDanhKhoaHoc { get; set; }
        public List<int> ChucDanhHanhChinh { get; set; }
        public string TenChucDanhKhoaHoc { get; set; }
        public string ChucDanhKhoaHocIDStr { get; set; }
        public string TenChucDanhHanhChinh { get; set; }
        public string ChucDanhHanhChinhIDStr { get; set; }
        public string CoQuanCongTac { get; set; }
        public string DiaChiCoQuan { get; set; }
        public int? PhongBanID { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public string DienThoaiDiDong { get; set; }
        public string Fax { get; set; }
        public int? TrangThaiID { get; set; }
        public Boolean? LaChuyenGia { get; set; }
        public string Url { get; set; }
        public List<NguoiGioiThieuModel> NguoiGioiThieu { get; set; }
        public List<QuaTrinhDaoTaoModel> QuaTrinhDaoTao { get; set; }
        public List<QuaTrinhCongTacModel> QuaTrinhCongTac { get; set; }
        public List<NgoaiNguModel> NgoaiNgu { get; set; }
        public List<VanBangChungChiModel> VanBangChungChi { get; set; }
        public List<GiaiThuongKhoaHocModel> GiaiThuongKhoaHoc { get; set; }
        public List<DuAnDeTaiModel> DuAnDeTai { get; set; }
        public List<BaiBaoTapChiModel> BaiBaoTapChi { get; set; }
        public List<SanPhamDaoTaoModel> SanPhamDaoTao { get; set; }
        public List<SanPhamKhacModel> SanPhamKhac { get; set; }
        public List<BaoCaoKhoaHocModel> BaoCaoKhoaHoc { get; set; }
        public List<KetQuaNghienCuuNKHModel> KetQuaNghienCuu { get; set; }
        public List<SachChuyenKhaoModel> SachChuyenKhao { get; set; }
        public List<CacMonGiangDayModel> CacMonGiangDay { get; set; }
        public List<HoatDongKhoaHocModel> HoatDongKhoaHoc { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
        public HuongNghienCuuChinhModel HuongNghienCuuChinh { get; set; }
        public string AnhHoSo { get; set; }
        public int CoQuanID { get; set; }
        public bool LaCanBoTrongTruong { get; set; } = false;
        public string FName { get; set; }
    }
    public class NguoiGioiThieuModel
    {
        public int? ID { get; set; }
        public int? CanBoID { get; set; }
        public int? NguoiGioiThieuID { get; set; }
        public int? CoQuanGioiThieuID { get; set; }
        public string TenNguoiGioiThieu { get; set; }
        public string Link { get; set; }
    }
    public class ChucDanhModel
    {
        public int ChucDanhID { get; set; }
        public int CanBoID { get; set; }
        public string TenChucDanh { get; set; }
        public int LoaiChucDanh { get; set; }
    }
    public class QuaTrinhDaoTaoModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string KhoangThoiGian { get; set; }
        public string CoSoDaoTao { get; set; }
        public string ChuyenNganh { get; set; }
        public string HocVi { get; set; }
    }
    public class QuaTrinhCongTacModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string CoQuanCongTac { get; set; }
        public string DiaChiDienThoai { get; set; }
        public string ChucVu { get; set; }
        public string KhoangThoiGian { get; set; }
    }
    public class NgoaiNguModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string TenNgoaiNgu { get; set; }
        public string Doc { get; set; }
        public string Viet { get; set; }
        public string Noi { get; set; }
    }
    public class VanBangChungChiModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string TieuDe { get; set; }
        public DateTime? NgayCap { get; set; }
        public string SoHieu { get; set; }
        public string TrinhDo { get; set; }
        public string NoiCap { get; set; }
    }
    public class GiaiThuongKhoaHocModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string TieuDe { get; set; }
        public string KhoangThoiGian { get; set; }
    }
    public class DuAnDeTaiModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string TenDuAn { get; set; }
        public string KhoangThoiGian { get; set; }
        public string CoQuanTaiTro { get; set; }
        public string VaiTroThamGia { get; set; }
        public int VaiTro { get; set; }
        public Boolean Disable { get; set; }
    }
    public class BaiBaoTapChiModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public int? ChiTietDeTaiID { get; set; }
        public int? DeTai { get; set; }
        public int? LoaiBaiBao { get; set; }
        public int? NhiemVu { get; set; }
        public int? LoaiNhiemVu { get; set; }
        public string KhoangThoiGian { get; set; }
        public string TieuDe { get; set; }
        public string TenTapChiSachHoiThao { get; set; }
        public string So { get; set; }
        public string Trang { get; set; }
        public string NhaXuatBan { get; set; }
        public string ISSN { get; set; }

        public string Tap { get; set; }
        public int? NamDangTai { get; set; }
        public string LinkBaiBao { get; set; }
        public int? LinhVucNganhKhoaHoc { get; set; }
        public string HeSoAnhHuong { get; set; }
        public int? ChiSo { get; set; }
        public int? RankSCIMAG { get; set; }
        public decimal? DiemTapChi { get; set; }
        public int? CapHoiThao { get; set; }
        public DateTime? NgayHoiThao { get; set; }
        public string DiaDiemToChuc { get; set; }

        public Boolean Disable { get; set; }
        public int? NguoiTaoID { get; set; }
        public List<TacGiaModel> ListTacGia { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }
    public class TacGiaModel
    {
        public int? TacGiaID { get; set; }
        public int? CTNhaKhoaHocID { get; set; }
        public int? ChiTietDeTaiID { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public string TenTacGia { get; set; }
    }
    public class BaoCaoKhoaHocModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string TieuDe { get; set; }
        public string TenHoiThao { get; set; }
        public string KhoangThoiGian { get; set; }
        public List<TacGiaModel> ListTacGia { get; set; }
    }

    public class SanPhamDaoTaoModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public int? ChiTietDeTaiID { get; set; }
        public int? DeTai { get; set; }
        public int? NhiemVu { get; set; }
        public int? LoaiNhiemVu { get; set; }
        public int? LoaiDaoTao { get; set; }
        public string TenHocVien { get; set; }
        public string TenLuanVan { get; set; }
        public string NguoiHuongDan { get; set; }
        public int? NamBaoVe { get; set; }
        public int? NguoiTaoID { get; set; }
        public Boolean Disable { get; set; }
        public string CoSoDaoTao { get; set; }
        public string KhoangThoiGian { get; set; }
        public int? CapHoiThao { get; set; }
    }

    public class SanPhamKhacModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public int? DeTai { get; set; }
        public int? NhiemVu { get; set; }
        public int? LoaiNhiemVu { get; set; }
        public string TieuDe { get; set; }
        public string KhoangThoiGian { get; set; }
        public string GhiChu { get; set; }
        public List<TacGiaModel> ListTacGia { get; set; }
    }

    public class KetQuaNghienCuuNKHModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public int? ChiTietDeTaiID { get; set; }
        public int? DeTai { get; set; }
        public string KhoangThoiGian { get; set; }
        public string TieuDe { get; set; }
        public string TacGia { get; set; }
        public string TenTapChiSachHoiThao { get; set; }
        public string So { get; set; }
        public string Trang { get; set; }
        public string NhaXuatBan { get; set; }
        public int? NamXuatBan { get; set; }
        public int? NhiemVu { get; set; }
        public int? LoaiNhiemVu { get; set; }
        public string GhiChu { get; set; }
        public List<TacGiaModel> ListTacGia { get; set; }

        public int? NguoiTaoID { get; set; }
        public Boolean Disable { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }
    public class SachChuyenKhaoModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public int? ChiTietDeTaiID { get; set; }
        public int? DeTai { get; set; }
        public string KhoangThoiGian { get; set; }
        public string TieuDe { get; set; }
        public string TenTapChiSachHoiThao { get; set; }
        public string So { get; set; }
        public string Trang { get; set; }
        public string NhaXuatBan { get; set; }

        public int? NamXuatBan { get; set; }
        public int? ChuBienID { get; set; }
        public int? CoQuanChuBienID { get; set; }
        public int? NhiemVu { get; set; }
        public int? LoaiNhiemVu { get; set; }

        public int? NguoiTaoID { get; set; }
        public Boolean Disable { get; set; }
        public List<TacGiaModel> ListTacGia { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
        public string ISSN { get; set; }
    }
    public class CacMonGiangDayModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string TieuDe { get; set; }
        public string DeCuong { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }
    public class HoatDongKhoaHocModel
    {
        public int? HoatDongKhoaHocID { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public int? NhiemVu { get; set; }
        public string HoatDongKhoaHoc { get; set; }
        public Boolean? PublicCV { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
        public List<int> DeleteFileDinhKem { get; set; }
        public int LoaiFile { get; set; }
        public int? NamThucHien { get; set; }
    }
    public class ChiTietHoatDongKhoaHocModel : HoatDongKhoaHocModel
    {
        public int FileDinhKemID { get; set; }
        public string TenFileGoc { get; set; }
        public string TenFileHeThong { get; set; }
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
    }

    public class ThongTinCTNhaKhoaHocModel : NhaKhoaHocModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public int? ChiTietDeTaiID { get; set; }
        public string KhoangThoiGian { get; set; }
        public string CoSoDaoTao { get; set; }
        public string ChuyenNganh { get; set; }
        public string HocVi { get; set; }
        public string DiaChiDienThoai { get; set; }
        public string ChucVu { get; set; }
        public string TenNgoaiNgu { get; set; }
        public string Doc { get; set; }
        public string Viet { get; set; }
        public string Noi { get; set; }
        public string TieuDe { get; set; }
        public DateTime? NgayCap { get; set; }
        public string SoHieu { get; set; }
        public string TrinhDo { get; set; }
        public string NoiCap { get; set; }
        public string NoiCapCC { get; set; }
        public string TenDuAn { get; set; }
        public int? DeTai { get; set; }
        public string CoQuanTaiTro { get; set; }
        public string VaiTroThamGia { get; set; }
        public string TacGia { get; set; }
        public string TenTapChiSachHoiThao { get; set; }
        public string So { get; set; }
        public string Trang { get; set; }
        public string NhaXuatBan { get; set; }
        public string DeCuong { get; set; }
        public int LoaiThongTin { get; set; }
        public string CoQuanCongTacCT { get; set; }
        public int? LoaiBaiBao { get; set; }
        public int? NhiemVuBaiBao { get; set; }
        public int? LoaiNhiemVu { get; set; }
        public string ISSN { get; set; }
        public string TenHoiThao { get; set; }
        public List<TacGiaModel> ListTacGia { get; set; }
        public int? TacGiaID { get; set; }
        public int? CanBoTGID { get; set; }
        public int? CoQuanTGID { get; set; }
        public string TenTacGia { get; set; }

        public string Tap { get; set; }
        public int? NamDangTai { get; set; }
        public string LinkBaiBao { get; set; }
        public int? LinhVucNganhKhoaHoc { get; set; }
        public string HeSoAnhHuong { get; set; }
        public int? ChiSo { get; set; }
        public int? RankSCIMAG { get; set; }
        public decimal? DiemTapChi { get; set; }
        public int? CapHoiThao { get; set; }
        public DateTime? NgayHoiThao { get; set; }
        public string DiaDiemToChuc { get; set; }
        public int? LoaiDaoTao { get; set; }
        public string TenHocVien { get; set; }
        public string TenLuanVan { get; set; }
        public string NguoiHuongDan { get; set; }
        public int? NamBaoVe { get; set; }
        public string GhiChu { get; set; }
        public int? NamXuatBan { get; set; }
        public int? ChuBienID { get; set; }
        public int? CoQuanChuBienID { get; set; }

        public int? HoatDongKhoaHocID { get; set; }
        public int? NhiemVu { get; set; }
        public string HoatDongKH { get; set; }
        public Boolean? PublicCV { get; set; }

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
        public string FolderPath { get; set; }
        public List<int> DeleteFileDinhKem { get; set; }
        public string TenHuongNghienCuuChinh { get; set; }

        public int? ID { get; set; }
        public int? NguoiGioiThieuID { get; set; }
        public int? CoQuanGioiThieuID { get; set; }
        public string TenNguoiGioiThieu { get; set; }
        public string Link { get; set; }

        public int NamThucHien { get; set; }
    }

    public class HuongNghienCuuChinhModel
    {
        public int? CTNhaKhoaHocID { get; set; }
        public string TenHuongNghienCuuChinh { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }


    }
}
