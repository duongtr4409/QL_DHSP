using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class DeTaiModel
    {
        public int DeTaiID { get; set; }
        public int? DeXuatID { get; set; }
        public string MaDeTai { get; set; }
        public string TenDeTai { get; set; }
        public int? LoaiHinhNghienCuu { get; set; }
        public string TenLoaiHinhNghienCuu { get; set; }
        public int? LinhVucNghienCuu { get; set; }
        public string TenLinhVucNghienCuu { get; set; }
        public int? LinhVucKinhTeXaHoi { get; set; }
        public string TenLinhVucKinhTeXaHoi { get; set; }
        public int? CapQuanLy { get; set; }
        public string TenCapQuanLy { get; set; }
        public int? NhiemVu { get; set; }       
        public string CoQuanChuQuan { get; set; }
        public string DonViQuanLyKhoaHoc { get; set; }
        public int? VaiTroChuNhiemID { get; set; }
        public int? ChuNhiemDeTaiID { get; set; }
        public int? CoQuanChuNhiemID { get; set; }
        public string TenChuNhiemDeTai { get; set; }
        public string NamBatDau { get; set; }
        public string NamKetThuc { get; set; }
        public int? SoLanGiaHanThucHien { get; set; }
        //public int? NamBatDau_Int { get; set; }
        //public int? NamKetThuc_Int { get; set; }
        public decimal? KinhPhiDHSP { get; set; }
        public decimal? NguonKhac { get; set; }
        public string MucTieu { get; set; }
        //public string CacNoiDungChinh { get; set; }
        public string SanPhamDangKy { get; set; }
        public string KhaNangUngDung { get; set; }
        public string DonViPhoiHop { get; set; }
        public int? TrangThai { get; set; }
        public int? NguoiTaoID { get; set; }
        public int? CoQuanID { get; set; }
        public string ThanhVienNghienCuuStr { get; set; }
        public string CacNoiDungChinhStr { get; set; }
        public List<NoiDungChinhModel> CacNoiDungChinh { get; set; }
        public List<ThanhVienNghienCuuModel> ThanhVienNghienCuu { get; set; }
        public List<TienDoThucHienModel> TienDoThucHien { get; set; }
        public List<KinhPhiModel> KinhPhi { get; set; }
        public List<SanPhamDeTaiModel> SanPhamDeTai { get; set; }
        public List<KetQuaChuyenGiaoModel> KetQuaChuyenGiao { get; set; }
        public List<DanhGiaGiaiDoanModel> DanhGiaGiaiDoan { get; set; }
        public List<KetQuaDanhGiaModel> KetQuaDanhGia { get; set; }
        public List<KetQuaNghienCuuModel> KetQuaNghienCuu { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
        public ThongTinChuNhiemDeTai CanBoData { get; set; }
        public List<BaiBaoTapChiModel> BaiBaoTapChi { get; set; }
        public List<SanPhamDaoTaoModel> SanPhamDaoTao { get; set; }
        public List<KetQuaNghienCuuNKHModel> KetQuaNghienCuuCongBo { get; set; }
        public List<SachChuyenKhaoModel> SachChuyenKhao { get; set; }
    }

    public class NoiDungChinhModel
    {
        public int? ChiTietDeTaiID { get; set; }
        public int DeTaiID { get; set; }
        public string TenNoiDung { get; set; }
        public string MoTa { get; set; }
        public int? NguoiTaoID { get; set; }
    }

    public class TienDoThucHienModel
    {
        public int ChiTietDeTaiID { get; set; }
        public int DeTaiID { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public string NoiDungThucHien { get; set; }
        public string KetQuaDatDuoc { get; set; }
        public string TenNoiDung { get; set; }
        public int? TrangThaiTienDo { get; set; }
        public string GhiChu { get; set; }
        public int NguoiTaoID { get; set; }
    }

    public class KinhPhiModel
    {
        public int ChiTietDeTaiID { get; set; }
        public int DeTaiID { get; set; }
        public decimal? TongKinhPhiDuocDuyet { get; set; }
        public decimal? TienDoCap { get; set; }
        public decimal? TienDoQuyetToan { get; set; }
        public int NguoiTaoID { get; set; }
    }

    public class SanPhamDeTaiModel
    {
        public int ChiTietDeTaiID { get; set; }
        public int DeTaiID { get; set; }
        public string NoiDungDaLam { get; set; }
        public string SanPham { get; set; }
        public int NguoiTaoID { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }

    public class KetQuaChuyenGiaoModel
    {
        public int ChiTietDeTaiID { get; set; }
        public int DeTaiID { get; set; }
        public string NoiDungChuyenGiao { get; set; }
        public int NguoiTaoID { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }

    public class KetQuaNghienCuuModel
    {
        public int? KetQuaNghienCuuID { get; set; }
        public int DeTaiID { get; set; }
        public int? LoaiKetQua { get; set; }
        public int? NhiemVu { get; set; }
        public string ThoiGian { get; set; }
        public string TacGia { get; set; }
        public string TieuDe { get; set; }
        public string TenTapChiSachHoiThao { get; set; }
        public string So { get; set; }
        public string Trang { get; set; }
        public string NhaXuatBan { get; set; }
        public int NguoiTaoID { get; set; }
        public Boolean Disable { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }

    public class DanhGiaGiaiDoanModel
    {
        public int ChiTietDeTaiID { get; set; }
        public int DeTaiID { get; set; }
        public string ThoiGianThucHien { get; set; }
        public string NoiDungDanhGia { get; set; }
        public string KetQuaThucHien { get; set; }
        public int NguoiTaoID { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }

    public class KetQuaDanhGiaModel
    {
        public int ChiTietDeTaiID { get; set; }
        public int DeTaiID { get; set; }
        public string ThoiGianThucHien { get; set; }
        public string NoiDungDanhGia { get; set; }
        public int? LoaiKetQua { get; set; }    
        public int? XepLoai { get; set; }
        public string XepLoaiKhac { get; set; }
        public int? LoaiNghiemThu { get; set; }
        public DateTime? NgayNghiemThu { get; set; }
        public string QuyetDinh { get; set; }
        public int NguoiTaoID { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
    }

    public class ThongTinChiTietDeTaiModel : DeTaiModel
    {
        public int? ChiTietDeTaiID { get; set; }
        public int LoaiThongTin { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public string NoiDungThucHien { get; set; }
        public string KetQuaDatDuoc { get; set; }
        public string TenNoiDung { get; set; }
        public string MoTa { get; set; }
        public int? TrangThaiTienDo { get; set; }
        public string GhiChu { get; set; }
        public decimal? TongKinhPhiDuocDuyet { get; set; }
        public decimal? TienDoCap { get; set; }
        public decimal? TienDoQuyetToan { get; set; }
        public string NoiDungDaLam { get; set; }
        public string SanPham { get; set; }
        public string NoiDungChuyenGiao { get; set; }
        public string ThoiGianThucHien { get; set; }
        public string NoiDungDanhGia { get; set; }
        public string KetQuaThucHien { get; set; }
        public int? XepLoai { get; set; }
        public string XepLoaiKhac { get; set; }
        public int? LoaiNghiemThu { get; set; }
        public DateTime? NgayNghiemThu { get; set; }
        public string QuyetDinh { get; set; }
        public int NguoiTaoChiTietID { get; set; }
        public int? NhiemVuDeTai { get; set; }

        public int ThanhVienNghienCuuID { get; set; }
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public int VaiTro { get; set; }
        public int LaCanBoTrongTruong { get; set; }
        public int? CoQuanID { get; set; }

        public int? KetQuaNghienCuuID { get; set; }  
        public int? LoaiKetQua { get; set; }   
        public string ThoiGian { get; set; }
        public string TacGia { get; set; }
        public string TieuDe { get; set; }
        public string TenTapChiSachHoiThao { get; set; }
        public string So { get; set; }
        public string Trang { get; set; }
        public string NhaXuatBan { get; set; }
        public int NguoiTaoKQID { get; set; }

        public string CoSoDaoTao { get; set; }
        public string KhoangThoiGian { get; set; }
        public int? LoaiBaiBao { get; set; }
        public int? NhiemVuBaiBao { get; set; }
        public int? LoaiNhiemVu { get; set; }
        public string ISSN { get; set; }
        public string TenHoiThao { get; set; }
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
        public int? NamXuatBan { get; set; }
        public int? ChuBienID { get; set; }
        public int? CoQuanChuBienID { get; set; }
        public List<TacGiaModel> ListTacGia { get; set; }
        public int? TacGiaID { get; set; }
        public int? CanBoTGID { get; set; }
        public int? CoQuanTGID { get; set; }
        public string TenTacGia { get; set; }

        public int FileDinhKemID { get; set; }
        public string TenFileGoc { get; set; }
        public string TenFileHeThong { get; set; }
        public int LoaiFile { get; set; } 
        public string FileUrl { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NgayTaoStr { get; set; }
        public Boolean? CoBaoMat { get; set; }
        public int NguoiTaoFileID { get; set; }
        public int? ThongTinVaoRaID { get; set; }
        public string Base64File { get; set; }
        public int LoaiUpload { get; set; } 
        public int NghiepVuID { get; set; }
        public string NoiDungFile { get; set; }
        public string FolderPath { get; set; }
        public string TenNguoiTao { get; set; }
    }

    public class ThongTinChuNhiemDeTai
    {
        public int CanBoID { get; set; }
        public string MaCanBo { get; set; }
        public string TenCanBo { get; set; }
        public int ChucDanhID { get; set; }
        public string TenChucDanh { get; set; }
        public int HocHamHocViID { get; set; }
        public string TenHocHamHocVi { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string NgaySinhStr { get; set; }
        public string GioiTinhStr { get; set; } 
        public string DiaChi { get; set; } 
        public string Email { get; set; } 
        public string DienThoai { get; set; } 
        public string Fax { get; set; } 
    }

    public class DuLieuNghienCuuKhoaHocModel
    {
        public int Id { get; set; }
        public int? YearId { get; set; }
        public int StaffId { get; set; }
        public int ConversionId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Members { get; set; }
        public int StartYear { get; set; }
        public List<string> Attach { get; set; }
        public string Desc { get; set; }
        public int WorkTime { get; set; } 
    }

    public class CTDuLieuNghienCuuKhoaHocModel
    {
        public int Id { get; set; }
        public int? YearId { get; set; }
        public int StaffId { get; set; }
        public int ConversionId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Members { get; set; }
        public int StartYear { get; set; }
        public string Attach { get; set; }
        public string Desc { get; set; }
        public int WorkTime { get; set; }
        public int FileDinhKemID { get; set; }
        public int NghiepVuID { get; set; }
        public string FileUrl { get; set; }
        public int DeTaiID { get; set; }
        public int HoatDongKhoaHocID { get; set; }
        public int CTNhaKhoaHocID { get; set; }
    }
}
