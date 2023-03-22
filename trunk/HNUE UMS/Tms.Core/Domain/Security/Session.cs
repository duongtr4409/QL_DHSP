using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Users;

namespace Ums.Core.Domain.Security
{
    [Table("User_Session")]
    public class Session : BaseEntity
    {
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastAccess { get; set; }
        public virtual User User { get; set; }
    }
}
