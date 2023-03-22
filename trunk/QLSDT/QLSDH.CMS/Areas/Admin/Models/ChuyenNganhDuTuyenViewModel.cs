using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class ChuyenNganhDuTuyenViewModel
    {
        public int ChuyenNghanhDuTuyenId { get; set; }
        public string TenNganh { get; set; }
        public string TenChuyenNganhDuTuyen { get; set; }
        public int SoLuong { get; set; }
    }
}