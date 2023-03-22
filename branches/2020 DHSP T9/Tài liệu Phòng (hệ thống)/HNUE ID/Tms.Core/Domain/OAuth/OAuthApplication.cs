using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.OAuth
{
    [Table("OAuth_Application")]
    public class OAuthApplication : BaseEntity
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public bool Enabled { get; set; }
    }
}
