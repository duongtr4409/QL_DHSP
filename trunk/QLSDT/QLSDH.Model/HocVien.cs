//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TEMIS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class HocVien
    {
        public long Id { get; set; }
        public string HoTen { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public Nullable<long> TrangThai { get; set; }
        public string File { get; set; }
        public string PhanHoi { get; set; }
        public string DeTai { get; set; }
        public string NHD1 { get; set; }
        public string NHD2 { get; set; }
        public string MaHV { get; set; }
        public Nullable<int> Khoa { get; set; }
        public Nullable<int> Lop { get; set; }
        public Nullable<int> KhoaHoc { get; set; }
        public Nullable<bool> STT_BM { get; set; }
        public Nullable<bool> STT_DL { get; set; }
        public Nullable<bool> STT_Truong { get; set; }
    }
}