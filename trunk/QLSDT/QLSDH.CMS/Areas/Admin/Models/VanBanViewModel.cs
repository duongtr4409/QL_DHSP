using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class VanBanViewModel
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string URL { get; set; }
        public Nullable<int> ChuyenMuc { get; set; }
        public string TenChuyenMuc { get; set; }
        public string DauMuc { get; set; }
        public string Anh { get; set; }
        public Nullable<int> HinhThuc { get; set; }
        public Nullable<bool> TrangThai { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}