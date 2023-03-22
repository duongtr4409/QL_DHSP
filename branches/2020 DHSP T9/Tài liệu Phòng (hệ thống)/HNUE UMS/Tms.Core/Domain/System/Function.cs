using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("Ums_Function")]
    public class Function : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Key { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsPublic { get; set; }
        public int Order { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleFunction> RolesFunctions { get; set; }
    }
}
