using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_PositionGroup")]
    public class PositionGroup : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Position> Positions { get; set; }
    }
}