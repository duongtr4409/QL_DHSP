using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreAPI.Entity
{
    public class ThongTinCanBoAPI
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public int TitleId { get; set; } //ID Chức danh
        public string Code { get; set; }

        public int DegreeId { get; set; } //ID Học hàm, học vị

        public int DepartmentId { get; set; } //ID Đơn vị

        public string Gender { get; set; }

        public int TeachingInId { get; set; }

        public bool IsMoved { get; set; }

        public bool IsRetired { get; set; }
        public bool IsProbation { get; set; }
    }
}