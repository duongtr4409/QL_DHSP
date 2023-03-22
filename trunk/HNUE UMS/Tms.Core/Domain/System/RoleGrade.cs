using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Data;

namespace Ums.Core.Domain.System
{
    [Table("System_RoleGrade")]
    public class RoleGrade : BaseEntity
    {
        public int RoleId { get; set; }
        public int GradeId { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual Role Role { get; set; }
    }
}