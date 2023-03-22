using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("System_Role")]
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleFunction> RolesFunctions { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleGrade> RolesGrades { get; set; }
    }
}
