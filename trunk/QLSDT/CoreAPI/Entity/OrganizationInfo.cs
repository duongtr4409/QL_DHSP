using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreAPI.Entity
{
    public class OrganizationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
    }
}