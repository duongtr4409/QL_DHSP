using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("System_Setting")]
    public class SystemSetting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
    }
}