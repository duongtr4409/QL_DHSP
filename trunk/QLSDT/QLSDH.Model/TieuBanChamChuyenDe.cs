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
    
    public partial class TieuBanChamChuyenDe
    {
        public long Id { get; set; }
        public string HoTen { get; set; }
        public string CoQuanCongTac { get; set; }
        public string VaiTroThamGia { get; set; }
        public string MaNCS { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<int> Type { get; set; }
    }
}
