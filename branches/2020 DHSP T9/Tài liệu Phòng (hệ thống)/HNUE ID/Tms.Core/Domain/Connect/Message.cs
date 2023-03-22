using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Users;

namespace Ums.Core.Domain.Connect
{
    [Table("Connect_Message")]
    public class Message : BaseEntity
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public bool Seen { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public virtual User From { get; set; }
    }
}