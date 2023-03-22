using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_Semester")]
    public class Semester : BaseEntity
    {
        public int YearId { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        [NotMapped]
        public double TotalDays => (To - From).TotalDays;
        public string Desc { get; set; }
        public virtual Year Year { get; set; }
    }
}