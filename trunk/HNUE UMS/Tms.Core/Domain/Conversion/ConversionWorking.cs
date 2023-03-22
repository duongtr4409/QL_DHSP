using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Conversion
{
    [Table("Conversion_Working")]
    public class ConversionWorking : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Ratio { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
        public string Desc { get; set; }
        public virtual ConversionWorkingCategory Category { get; set; }
    }
}