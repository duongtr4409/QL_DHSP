using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class DiemThiViewModel
    {
        public long Id { get; set; }
        public string MaHocVien { get; set; }
        public string TenHocVien { get; set; }
        public string HocPhanId { get; set; }
        public string TenHocPhan { get; set; }
        public double SoDiem { get; set; }
        public double DiemHP1 { get; set; }
        public double DiemHP2 { get; set; }
        public double DiemHP3 { get; set; }
        public double DiemHP4 { get; set; }
        public string KhoaId { get; set; }
        public string TenKhoa { get; set; }
        public string KhoaHocId { get; set; }
        public string TenKhoaHoc { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}