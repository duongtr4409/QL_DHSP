using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;
using Ums.Core.Domain.Data;

namespace Ums.Core.Domain.Personnel
{
    [Table("Personnel_StaffClassify")]
    public class StaffClassify : BaseEntity
    {
        public int StaffId { get; set; }
        public int ClassifyId { get; set; }
        public int YearId { get; set; }
        public int IndexerId { get; set; }
        public DateTime IndexedOn { get; set; }
        public bool IsLocked { get; set; }
        [JsonIgnore]
        public virtual Staff Staff { get; set; }
        [JsonIgnore]
        public virtual Classify Classify { get; set; }
        [JsonIgnore]
        public virtual Staff Indexer { get; set; }
    }
}
