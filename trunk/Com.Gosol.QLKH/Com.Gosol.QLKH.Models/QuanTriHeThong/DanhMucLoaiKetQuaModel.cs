using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.QLKH.Models.DanhMuc
{
    public class DanhMucLoaiKetQuaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? MappingId { get; set; }
        public bool Status { get; set; }
        public int? Highlight { get; set; } = 0;
        public List<DanhMucLoaiKetQuaModel> Children { get; set; }
        //public int? VaiTroChuNhiemID { get; set; }
    }
    public class DanhMucLoaiKetQuaPartialModel
    {
        public List<DanhMucLoaiKetQuaModel> items { get; set; }
    }
}
