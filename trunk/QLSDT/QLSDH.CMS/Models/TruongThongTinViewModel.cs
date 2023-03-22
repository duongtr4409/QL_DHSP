using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Models
{
    public class TruongThongTinViewModel
    {
        public int STT { get; set; }
        public int Id { get; set; }
        public Nullable<int> IdDanhMuc { get; set; }
        public string TenTruongThongTin { get; set; }
        public string LoaiTruongThongTin { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int StatusUpload { get; set; }
        public string MaNCS { get; set; }
        public string Url { get; set; }
    }
}