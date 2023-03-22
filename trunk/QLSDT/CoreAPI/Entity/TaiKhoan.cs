using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreAPI.Entity
{
    public class TaiKhoan
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public long StaffId { get; set; }
        public string DepartmentId { get; set; }
        public string RoleName { get; set; }
        public bool isLock { get; set; }
        public long UserKey { get; set; }
        public string Phone { get; set; }


    }
}