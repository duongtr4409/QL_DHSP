using System.ComponentModel.DataAnnotations.Schema;
using Hnue.Helper;
using Newtonsoft.Json;
using Ums.Core.Base;
using Ums.Core.Entities.Shared;

namespace Ums.Core.Domain.Organize
{
    [Table("Org_Department")]
    public class Department : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string PositionMax { get; set; }
        public string ShortName { get; set; }
        [NotMapped]
        [JsonIgnore]
        public IdId[] MaxPositions => PositionMax.CastJson<IdId[]>() ?? new IdId[0];
    }
}