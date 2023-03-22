using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class HoiDongModel
    {
        public int? HoiDongID { get; set; }
        public string TenHoiDong { get; set; }
        public List<ThanhVienHoiDongModel> ThanhVienHoiDong { get; set; }
    }

    public class ThanhVienHoiDongModel
    {
        public int ChiTietHoiDongID { get; set; }
        public int? HoiDongID { get; set; }
        public int CanBoID { get; set; }
        public string VaiTro { get; set; }
        public string TenCanBo { get; set; }
        public string DonViCongTac { get; set; }
    }

    public class ChiTietHoiDongModel : HoiDongModel
    {
        public int ChiTietHoiDongID { get; set; }
        public int CanBoID { get; set; }
        public string VaiTro { get; set; }
        public string TenCanBo { get; set; }
        public string DonViCongTac { get; set; }
    }

    public class DanhSachHoiDongDanhGiaModel
    {
        public int ID { get; set; }
        public int? HoiDongID { get; set; }
        public int? DeXuatID { get; set; }
        public string MaDeXuat { get; set; }
        public string TenDeXuat { get; set; }
        public int? LinhVucNghienCuu { get; set; }
        public string TenLinhVucNghienCuu { get; set; }
        public int? LinhVucKinhTeXaHoi { get; set; }
        public string TenLinhVucKinhTeXaHoi { get; set; }
        public int? CapQuanLy { get; set; }
        public string TenCapQuanLy { get; set; }
        public int? NguoiDeXuat { get; set; }
        public string TenNguoiDeXuat { get; set; }
        public Boolean DanhGia { get; set; }

    }
}
