using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.System
{
    [Table("User_Type")]
    public class UserType : BaseEntity
    {
        public string Name { get; set; }
    }
}