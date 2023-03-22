using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hnue.Helper;
using Ums.Core.Base;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Connect
{
    [Table("Connect_Notice")]
    public class Notice : BaseEntity
    {
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string RoleData { get; set; }
        public bool Public { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public virtual Department Department { get; set; }
        public virtual Staff Staff { get; set; }
        public int[] RoleIds => RoleData.CastJson<int[]>() ?? new int[0];
    }
}