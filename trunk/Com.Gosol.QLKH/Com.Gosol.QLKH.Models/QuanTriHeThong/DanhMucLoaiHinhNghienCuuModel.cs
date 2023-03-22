using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.QLKH.Models.DanhMuc
{
    public class DanhMucLoaiHinhNghienCuuModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Note { get; set; }
    }
}
