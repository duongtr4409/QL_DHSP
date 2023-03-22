using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;

namespace Ums.Core.Domain.Personnel
{
    [Table("Personnel_Vacation")]
    public class Vacation : BaseEntity
    {
        public int StaffId { get; set; }
        public int TypeId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int UpdateBy { get; set; }
        public DateTime Updated { get; set; }
        [JsonIgnore]
        public virtual Staff Staff { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual VacationType Type { get; set; }
        [NotMapped]
        public int Days => (int)(End - Start).TotalDays;
    }
}