using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("User_Group")]
    public class UserGroup : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}