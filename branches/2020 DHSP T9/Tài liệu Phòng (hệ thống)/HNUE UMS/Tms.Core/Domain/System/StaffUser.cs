using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.System
{
    [Table("System_User")]
    public class StaffUser : BaseEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hint { get; set; }
        public string RecoverPasswordCode { get; set; }
        public int StaffId { get; set; }
        public bool IsAdmin { get; set; }
        public bool PasswordChanged { get; set; }
        public virtual Staff Staff { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRole> UsersRoles { get; set; }
    }
}