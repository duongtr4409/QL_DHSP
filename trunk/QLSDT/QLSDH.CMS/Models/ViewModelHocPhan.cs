using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Models
{
    public class ViewModelHocPhan
    {
        public long Id { get; set; }
        public string TenHocPhan { get; set; }
        public Nullable<int> SoDVHT { get; set; }
        public Nullable<bool> DieuKien { get; set; }
        public Nullable<bool> TuChon { get; set; }
        public Nullable<int> SoTietLyThuyet { get; set; }
        public Nullable<int> SoTietThucHanh { get; set; }
        public string MaHocPhan { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<int> KhoaHocId { get; set; }
        public Nullable<int> KhoaId { get; set; }
        public Nullable<int> NganhId { get; set; }
        public Nullable<int> ChuyenNganhId { get; set; }
        public string DiemHocPhan { get; set; }
        public bool TrangThaiDangKy { get; set; }
        public Nullable<int> TinChi { get; set; }
    }
}