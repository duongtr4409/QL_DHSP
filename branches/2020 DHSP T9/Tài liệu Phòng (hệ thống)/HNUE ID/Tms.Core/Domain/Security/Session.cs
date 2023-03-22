using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Core.Base;

namespace Ums.Core.Domain.Users
{
    [Table("User_Session")]
    public class Session : BaseEntity
    {
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime LastAccess { get; set; } = DateTime.Now;
        public DateTime Created { get; set; } = DateTime.Now;
        public virtual User User { get; set; }
    }
}
