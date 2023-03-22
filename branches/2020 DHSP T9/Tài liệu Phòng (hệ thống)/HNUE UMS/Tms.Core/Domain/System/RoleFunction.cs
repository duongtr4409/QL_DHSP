using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("System_RoleFunction")]
    public class RoleFunction : BaseEntity
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public virtual Function Function { get; set; }
    }
}