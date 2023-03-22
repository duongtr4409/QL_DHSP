using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class TinTucViewModel
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string MoTaNgan { get; set; }
        public string AnhDaiDien { get; set; }
        public string NoiDung { get; set; }
        public DateTime CreateDate { get; set; }
        public int DanhMucId { get; set; }
        public string TenDanhMuc { get; set; }
        public bool Status { get; set; }
    }
}