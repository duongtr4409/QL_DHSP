using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class DanhMucBuocThucHienModel
    {
        public int BuocThucHienID { get; set; }
        public string TenBuocThucHien { get; set; }
        public int ThuTuHienThi { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThaiSuDung { get; set; }
        public string MaBuocThucHien { get; set; }
    }
}
