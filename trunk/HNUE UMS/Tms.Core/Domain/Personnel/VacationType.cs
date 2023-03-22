using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Personnel
{
    [Table("Personnel_VacationType")]
    public class VacationType : BaseEntity
    {
        public string Name { get; set; }
        public double Ratio { get; set; }
    }
}