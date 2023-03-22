using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class DiemThiNCSTheoHocPhanViewModel
    {
        public int Id { get; set; }
        public string MaNCS { get; set; }
        public string TenHocPhan { get; set; }
        public Nullable<int> TinChi { get; set; }
        public Nullable<double> Diem { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string MaHocPhan { get; set; }
        public Nullable<bool> TuChon { get; set; }
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public Nullable<double> DiemDieuKien { get; set; }
        public Nullable<double> DiemThi { get; set; }
        public string TenNCS { get; set; }

    }
}