using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_TeachingType")]
    public class ConversionTeachingType : BaseEntity
    {
        public string Name { get; set; }
    }
}