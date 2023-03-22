using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.QLKH.Models.DanhMuc
{
    public class DanhMucTrangThaiModel
    {
        public int TrangThaiID { get; set; }
        public string TenTrangThai { get; set; }
        public bool TrangThaiSuDung { get; set; }
        public string GhiChu { get; set; }
    }
}
