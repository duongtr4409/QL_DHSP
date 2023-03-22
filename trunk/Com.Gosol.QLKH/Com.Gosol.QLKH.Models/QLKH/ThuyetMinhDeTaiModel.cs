using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class ThuyetMinhDeTaiModel
    {
        public int ThuyetMinhID { get; set; }
        public int CanBoID { get; set; }
        public int CoQuanID { get; set; }
        public FileDinhKemModel FileThuyetMinh { get; set; }
        public int DeXuatID { get; set; }
        public int TrangThai { get; set; }
        public string TenDeXuat { get; set; }
        public string MaDeXuat { get; set; }
    }

    public class DeXuatThuyetMinhModel
    {
        public List<ThuyetMinhDeTaiModel> ListThuyetMinh { get;set; }
        public int DeXuatID { get; set; }
        public string TenDeXuat { get; set; }
        public string MaDeXuat { get; set; }
        public bool DuyetDeXuat { get; set; }
    }
}
