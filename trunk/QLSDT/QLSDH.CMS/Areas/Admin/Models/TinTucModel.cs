using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class TinTucModel
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string MoTaNgan { get; set; }
        public string NoiDung { get; set; }
        public int DanhMuc { get; set; }
        public string files { get; set; }
    }
}