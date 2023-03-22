using Ums.Core.Domain.Users;
using Ums.Core.Entities.Shared;
using Ums.Models.Security;
using Ums.Services.Base;

namespace Ums.Services.Users
{
    public interface IUserService : IService<User>
    {
        User GetByUserKey(string key, string type = "");
        User GetByUsername(string username, string type = "");
        User CreateUser(RegisterModel model);
        ApiResponse Validate(RegisterModel model);
        int InitRecoverPassword(string email, string url);
        string RecoverPassword(string email, string token);
        void ChangePassword(int userId, string newPassword);
        string SetTwoFactorAuth(string email);
    }
}