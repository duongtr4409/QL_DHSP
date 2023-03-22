using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreAPI.Entity
{
    public class GiangVienAPI
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TitleId { get; set; }
        public string Code { get; set; }
        public int DegreeId { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public int TeachingInId { get; set; }
        public List<int> PositionIds { get; set; }

    }
}