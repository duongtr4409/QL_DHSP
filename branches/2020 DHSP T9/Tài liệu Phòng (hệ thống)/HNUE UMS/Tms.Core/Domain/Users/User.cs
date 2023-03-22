using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using Ums.Core.Base;
using Ums.Core.Domain.Security;

namespace Ums.Core.Domain.Users
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string Avatar { get; set; }
        public string UserKey { get; set; }
        public string UserType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [JsonIgnore]
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual ICollection<Session> Sessions { get; set; }
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
    }
}
