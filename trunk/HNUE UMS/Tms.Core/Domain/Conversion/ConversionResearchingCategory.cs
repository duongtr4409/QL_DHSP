using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_ResearchingCategory")]
    public class ConversionResearchingCategory : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        public virtual ConversionResearchingCategory Parent { get; set; }
    }
}