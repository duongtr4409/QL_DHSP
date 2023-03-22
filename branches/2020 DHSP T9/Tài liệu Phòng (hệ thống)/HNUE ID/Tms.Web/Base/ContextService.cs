using Ums.Core.Domain.Users;
using Ums.Services.Security;

namespace Ums.App.Base
{
    public class ContextService : IContextService
    {
        public User GetCurrentUser()
        {
            return WorkContext.UserInfo;
        }
    }
}
