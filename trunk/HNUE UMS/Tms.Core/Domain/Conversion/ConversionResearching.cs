using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_Researching")]
    public class ConversionResearching : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double MaxMaterialTime { get; set; }
        public double Ratio { get; set; }
        public int MemberOffset { get; set; }
        public double Factor { get; set; }
        public int EquivalentQuantity { get; set; } = 1;
        public string Unit { get; set; }
        public bool IsDirectly { get; set; }
        public bool HasMember { get; set; }
        public virtual ConversionResearchingCategory Category { get; set; }
    }
}
