using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Ums.Core.Base;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Users
{
    [Table("System_User")]
    public class StaffUser : BaseEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        [ScriptIgnore(ApplyToOverrides = true)]
        public string Password { get; set; }
        [JsonIgnore]
        [ScriptIgnore(ApplyToOverrides = true)]
        public string Hint { get; set; }
        public string RecoverPasswordCode { get; set; }
        public int StaffId { get; set; }
        [JsonIgnore]
        [ScriptIgnore(ApplyToOverrides = true)]
        public bool IsAdmin { get; set; }
        [JsonIgnore]
        [ScriptIgnore(ApplyToOverrides = true)]
        public bool PasswordChanged { get; set; }
        [JsonIgnore]
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual Staff Staff { get; set; }
    }
}