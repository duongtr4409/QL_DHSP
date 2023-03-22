using Ums.Core.Domain.Users;
using Ums.Services.Base;

namespace Ums.Services.Users
{
    public interface IUserService : IService<User>
    {
        User GetByUserKey(string key, string type = "");
        User GetByUsername(string username, string type = "");
        void ChangePassword(int userId, string newPassword);
    }
}