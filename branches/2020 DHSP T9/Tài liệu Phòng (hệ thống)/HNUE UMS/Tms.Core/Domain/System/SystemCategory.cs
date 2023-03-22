using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("Ums_Category")]
    public class SystemCategory : BaseEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
    }
}