using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class HocPhanGiangVienViewModel
    {
        public int Id { get; set; }
        public long IdHocPhan { get; set; }
        public string TenHocPhan { get; set; }
        public string MaHocPhan { get; set; }
        public string TenGiangVien { get; set; }
        public int IdGiangVien { get; set; }
        public int StaffId { get; set; }
        public string HocHamHocVi { get; set; }
    }
}