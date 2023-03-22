using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_Year")]
    public class Year : BaseEntity
    {
        public bool Visible { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        [NotMapped]
        public string Name => From.Year + " - " + To.Year;
        [NotMapped]
        public double TotalDays => (To - From).TotalDays;
    }
}