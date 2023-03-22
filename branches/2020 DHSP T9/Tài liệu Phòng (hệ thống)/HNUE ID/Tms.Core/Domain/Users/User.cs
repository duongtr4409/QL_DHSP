using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Users
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string Avatar { get; set; } = "/styles/images/noavatar.png";
        public string UserKey { get; set; }
        public string UserType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string RecoverPasswordCode { get; set; }
        public string GetDisplayName()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return Name;
            }
            if (!string.IsNullOrEmpty(Email))
            {
                return Email;
            }
            return Username;
        }
        [NotMapped]
        public bool IsStaff => UserType == "STAFF" || UserType == "LDAP";
        public int StaffId { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
