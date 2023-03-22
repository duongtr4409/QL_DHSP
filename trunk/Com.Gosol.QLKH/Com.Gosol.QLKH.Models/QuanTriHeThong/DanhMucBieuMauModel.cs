using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;


namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class DanhMucBieuMauModel
    {
        public int BieuMauID { get; set; }
        public string TenBieuMau { get; set; }
        public int ThuTuHienThi { get; set; } = 1;
        public string GhiChu { get; set; }
        public FileDinhKemModel FileDinhKem { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int? NguoiCapNhat { get; set; }
        public string NguoiCapNhatStr { get; set; }
    }
}
