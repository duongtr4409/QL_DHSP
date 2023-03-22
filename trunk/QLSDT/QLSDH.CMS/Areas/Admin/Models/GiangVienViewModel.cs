using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Areas.Admin.Models
{
    public class GiangVienViewModel
    {
        public int Id { get; set; }
        public int STT { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string NoiSinh { get; set; }
        public string HoKhau { get; set; }
        public string ThongTinLienLac { get; set; }
        public string ChucDanh { get; set; }
        public string HocHamHocVi { get; set; }
        public string KHoa { get; set; }
    }
}