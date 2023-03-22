using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;


namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class DanhMucLinhVucModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public int? MappingId { get; set; }
        public int Type { get; set; } // 1 - Nghiên cứu, 2- Tài chính
        public bool Status { get; set; }
        public int? Highlight { get; set; } = 0;

        public List<DanhMucLinhVucModel> Children { get; set; }
    }

}
