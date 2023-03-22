using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Domain.Users;

namespace Ums.Models.Account
{
    [NotMapped]
    public class UserModel : User
    {
        public new string Birthday { get; set; }
    }
}
