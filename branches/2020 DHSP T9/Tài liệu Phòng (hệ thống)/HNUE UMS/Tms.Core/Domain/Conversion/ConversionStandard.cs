using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Data;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_Standard")]
    public class ConversionStandard : BaseEntity
    {
        public int Teaching { get; set; }
        public int Researching { get; set; }
        public int Working { get; set; }
        public int TitleId { get; set; }
        public virtual Title Title { get; set; }
    }
}