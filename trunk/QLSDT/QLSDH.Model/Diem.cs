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
    
    public partial class Diem
    {
        public long Id { get; set; }
        public Nullable<long> HocVienId { get; set; }
        public string MaHocVien { get; set; }
        public string TenHocVien { get; set; }
        public Nullable<long> HocPhanId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<double> DiemHP1 { get; set; }
        public Nullable<double> DiemHP2 { get; set; }
        public Nullable<double> DiemHP3 { get; set; }
        public Nullable<double> DiemHP4 { get; set; }
        public Nullable<double> SoDiem { get; set; }
    }
}