using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.System;

namespace Ums.Core.Domain.Personnel
{
    [Table("Personnel_StaffPosition")]
    public class StaffPosition : BaseEntity
    {
        public int StaffId { get; set; }
        public int PositionId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int UpdatebyId { get; set; }
        public DateTime Updated { get; set; }
        [JsonIgnore]
        public virtual Position Position { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual StaffUser Updateby { get; set; }
    }
}