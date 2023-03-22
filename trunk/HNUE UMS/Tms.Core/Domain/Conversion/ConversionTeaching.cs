using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_Teaching")]
    public class ConversionTeaching : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Ratio { get; set; }
        public string Unit { get; set; }
        public string MaxUnit { get; set; }
        public string Desc { get; set; }
        public int TeachingTypeId { get; set; }
        public int GroupSize { get; set; }
        public bool IsDirected { get; set; }
        public virtual ConversionTeachingCategory Category { get; set; }
        [ForeignKey(nameof(TeachingTypeId))]
        public virtual ConversionTeachingType Type { get; set; }
    }
}