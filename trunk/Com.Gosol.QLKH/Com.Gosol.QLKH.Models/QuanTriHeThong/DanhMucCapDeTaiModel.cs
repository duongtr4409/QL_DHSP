using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;


namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class DanhMucCapDeTaiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? MappingId { get; set; }
        public bool Status { get; set; }
        public int? Highlight { get; set; } = 0;
        public List<DanhMucCapDeTaiModel> Children { get; set; }
        public int? VaiTroChuNhiemID { get; set; }
        public int? Type { get; set; }
    }
    public class DanhMucCapDeTaiPartialModel
    {
        public List<DanhMucCapDeTaiModel> items { get; set; }
    }
}
