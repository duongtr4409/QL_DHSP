using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_Degree")]
    public class Degree : BaseEntity
    {
        public string Name { get; set; }
        public double Ratio { get; set; }
        public int Order { get; set; }
    }
}