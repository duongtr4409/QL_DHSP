using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_WorkingCategory")]
    public class ConversionWorkingCategory : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
    }
}