using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class DeXuatDeTaiModel
    {
        public int DeXuatID { get; set; }
        public string MaDeXuat { get; set; }
        public string TenDeXuat { get; set; }
        public int? LinhVucNghienCuu { get; set; }
        public string TenLinhVucNghienCuu { get; set; }
        public int? LinhVucKinhTeXaHoi { get; set; }
        public string TenLinhVucKinhTeXaHoi { get; set; }
        public int? CapQuanLy { get; set; }
        public string TenCapQuanLy { get; set; }
        public int? NguoiDeXuat { get; set; }
        public int? CoQuanID { get; set; }
        public string TenNguoiDeXuat { get; set; }
        public string TinhCapThiet { get; set; }
        public string MucTieu { get; set; }
        public string SanPham { get; set; }
        public decimal? KinhPhiDuKien { get; set; }    
        public int? TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public DateTime NgayThucHien { get; set; }
        public int? ThoiGianNghienCuu { get; set; }
        public string GhiChu { get; set; }
        public DateTime? NgayDeXuat { get; set; }
        public string NoiDung { get; set; }
        public string DiaChiUngDung { get; set; }
        public string ThoiGianThucHien { get; set; }
        public string ThoiGianThucHienTu { get; set; }
        public string ThoiGianThucHienDen { get; set; }
        public string ThuocChuongTrinh { get; set; }
        public int? CanBoChinhSuaID { get; set; }
        public int? CoQuanChinhSuaID { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; } 
        public List<FileDinhKemModel> FileThuyetMinh { get; set; } 
        public List<LichSuDuyetDeXuatModel> ThongTinXetDuyetDeTai { get; set; }
        public List<LichSuChinhSuaDeXuatModel> ThongTinChinhSuaDeXuat { get; set; }
    }

    public class LichSuDuyetDeXuatModel
    {
        public int DuyetDeXuatID { get; set; }
        public int DeXuatID { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public string TenNguoiThucHien { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public int TrangThai { get; set; }
        public string NoiDung { get; set; }
        public List<FileDinhKemModel> FileXetDuyet { get; set; }
    }

    public class DeXuatDeTaiChiTiet : DeXuatDeTaiModel
    {
        public int DuyetDeXuatID { get; set; }  
        public string NoiDungDuyet { get; set; }
        public int TrangThaiDuyet { get; set; }
        public string TenNguoiThucHien { get; set; }
        public DateTime NgayThucHienDuyet { get; set; }
        public int? CanBoID { get; set; }

        public int FileDinhKemID { get; set; }
        public string TenFileGoc { get; set; }
        public string TenFileHeThong { get; set; }
        public int LoaiFile { get; set; }
        public string FileUrl { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NgayTaoStr { get; set; }
        public Boolean? CoBaoMat { get; set; }
        public int? NguoiTaoID { get; set; }
        public int? CoQuanTaoID { get; set; }
        public string TenNguoiTao { get; set; }
        public int? ThongTinVaoRaID { get; set; }
        public string Base64File { get; set; }
        public int LoaiUpload { get; set; }
        public int NghiepVuID { get; set; }
        public string NoiDungFile { get; set; }
        public int NguoiDungID { get; set; }
    }

    public class LichSuChinhSuaDeXuatModel
    {
        public int ID { get; set; }
        public int DeXuatID { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public string NguoiChinhSua { get; set; }
        public DateTime? NgayChinhSua { get; set; }
        public List<NoiDungChinhSua> NoiDungChinhSua { get; set; }
        public List<DeXuatDeTaiModel> Data { get; set; }
    }

    public class NoiDungChinhSua
    {
        public string key { get; set; }
        public string label { get; set; }
    }

    public class DeXuatLog
    {
        public int ID { get; set; }
        public int DeXuatID { get; set; }
        public string MaDeXuat { get; set; }
        public string MaDeXuatEdit { get; set; }
        public string TenDeXuat { get; set; }
        public string TenDeXuatEdit { get; set; }
        public int? LinhVucNghienCuu { get; set; }
        public int? LinhVucNghienCuuEdit { get; set; }
        public string TenLinhVucNghienCuu { get; set; }
        public string TenLinhVucNghienCuuEdit { get; set; }
        public int? LinhVucKinhTeXaHoi { get; set; }
        public int? LinhVucKinhTeXaHoiEdit { get; set; }
        public string TenLinhVucKinhTeXaHoi { get; set; }
        public string TenLinhVucKinhTeXaHoiEdit { get; set; }
        public int? CapQuanLy { get; set; }
        public int? CapQuanLyEdit { get; set; }
        public string TenCapQuanLy { get; set; }
        public string TenCapQuanLyEdit { get; set; }
        public int? NguoiDeXuat { get; set; }
        public int? NguoiDeXuatEdit { get; set; }
        public string TenNguoiDeXuat { get; set; }
        public string TenNguoiDeXuatEdit { get; set; }
        public int? CoQuanID { get; set; }
        public int? CoQuanIDEdit { get; set; }
        public string TinhCapThiet { get; set; }
        public string TinhCapThietEdit { get; set; }
        public string MucTieu { get; set; }
        public string MucTieuEdit { get; set; }
        public string SanPham { get; set; }
        public string SanPhamEdit { get; set; }
        public decimal? KinhPhiDuKien { get; set; }
        public decimal? KinhPhiDuKienEdit { get; set; }
        public int? TrangThai { get; set; }
        public string FileID { get; set; }
        public string FileIDEdit { get; set; }
        public DateTime NgayThucHien { get; set; }
        public int? ThoiGianNghienCuu { get; set; }
        public int? ThoiGianNghienCuuEdit { get; set; }
        public DateTime? NgayDeXuat { get; set; }
        public DateTime? NgayDeXuatEdit { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungEdit { get; set; }
        public string DiaChiUngDung { get; set; }
        public string DiaChiUngDungEdit { get; set; }
        public string ThoiGianThucHien { get; set; }
        public string ThoiGianThucHienTu { get; set; }
        public string ThoiGianThucHienEdit { get; set; }
        public string ThoiGianThucHienTuEdit { get; set; }
        public string ThoiGianThucHienDen { get; set; }
        public string ThoiGianThucHienDenEdit { get; set; }
        public string ThuocChuongTrinh { get; set; }
        public string ThuocChuongTrinhEdit { get; set; }
        public int? CanBoChinhSuaID { get; set; }
        public int? CoQuanChinhSuaID { get; set; }
        public string NguoiChinhSua { get; set; }
        public DateTime? NgayChinhSua { get; set; }
    }
}
