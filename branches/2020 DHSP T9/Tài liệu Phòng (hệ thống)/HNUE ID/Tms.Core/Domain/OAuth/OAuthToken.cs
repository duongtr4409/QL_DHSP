using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Users;

namespace Ums.Core.Domain.OAuth
{
    [Table("OAuth_Token")]
    public class OAuthToken : BaseEntity
    {
        public int SessionId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public virtual Session Session { get; set; }
    }
}
