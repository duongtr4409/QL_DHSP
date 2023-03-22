using Hnue.Helper;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Users;
using Ums.Services.Base;
using Ums.Services.Mailing;
using Ums.Services.Security;
using Ums.Services.Users;

namespace Ums.Services.System
{
    public class UserService : Service<User>, IUserService
    {
        private readonly ISettingService _settingService;
        private readonly IEmailSender _emailSender;
        public UserService(DbContext context, IContextService contextService, ISettingService settingService, IEmailSender emailSender) : base(context, contextService)
        {
            _settingService = settingService;
            _emailSender = emailSender;
        }

        public User GetByUserKey(string key, string type = "")
        {
            return Gets().FirstOrDefault(i => i.UserKey == key && i.UserType == type);
        }

        public User GetByUsername(string username, string type = "")
        {
            return Gets().FirstOrDefault(i => i.Username.ToLower() == username.ToLower() && (!string.IsNullOrEmpty(type) && i.UserType == type || true));
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var user = Get(userId);
            user.Password = Common.Md5(newPassword);
            Update(user);
        }
    }
}