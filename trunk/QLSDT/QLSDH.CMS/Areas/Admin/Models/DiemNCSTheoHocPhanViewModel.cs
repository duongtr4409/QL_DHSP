using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class DiemNCSTheoHocPhanViewModel
    {
        public long Id { get; set; }
        public string MaHP { get; set; }
        public string TenHP { get; set; }
        public string Khoa { get; set; }
        public string KhoaHoc { get; set; }
        public string Nganh { get; set; }
    }
}