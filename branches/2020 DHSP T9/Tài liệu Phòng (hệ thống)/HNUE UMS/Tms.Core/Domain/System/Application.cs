using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("System_Application")]
    public class SystemApplication : BaseEntity
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public bool Enabled { get; set; }
    }
}
