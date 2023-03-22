using System;
using Ums.Core.Base;

namespace Ums.Core.Domain.Students
{
    public class MasterStudent
    {
        public int Id { get; set; }
        public string MaHv { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string DT { get; set; }
        public string Code { get; set; }
        public string DiaChi { get; set; }
    }
}
