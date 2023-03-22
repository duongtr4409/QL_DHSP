using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Ums.Core.Domain.Users;

namespace Ums.Models.Security
{
    public class TokenResponseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("score")]
        public decimal Score { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

    public class TwoFactorAuth
    {
        public string key { get; set; }
        public DateTime created { get; set; }
        public string email { get; set; }
        public int userId { get; set; }
    }
}
