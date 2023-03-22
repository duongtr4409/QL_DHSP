using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("System_UserRole")]
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsLeader { get; set; }
        public virtual StaffUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}