using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class ThuVienViewModel
    {
        public long Id { get; set; }
        public int KhoaId { get; set; }
        public string TenKhoa { get; set; }
        public int KhoaHocId { get; set; }
        public string TenKhoahoc { get; set; }
        public int NganhId { get; set; }
        public string TenNganh { get; set; }
        public int ChuyenNganhId { get; set; }
        public string TenChuyenNganh { get; set; }
        public string HoTen { get; set; }
        public string MaNCS { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool NopLan1 { get; set; }
        public string UrlFileLan1 { get; set; }
        public bool NopLan2 { get; set; }
        public string UrlFileLan2 { get; set; }
        public DateTime QDBV_CapTruong { get; set; }
    }
}