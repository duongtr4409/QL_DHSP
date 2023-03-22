using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_TeachingCategory")]
    public class ConversionTeachingCategory : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
    }
}