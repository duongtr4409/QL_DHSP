using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class ThanhVienNghienCuuModel
    {
        public int ThanhVienNghienCuuID { get; set; }
        public int DeTaiID { get; set; }
        public int? CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public int? VaiTro { get; set; }
        public int? LaCanBoTrongTruong { get; set; }
        public Boolean LaChuNhiemDeTai { get; set; }
        public int CoQuanID { get; set; }     
    }
}
