using System.Data.Entity;
using Ums.Core.Domain.Security;
using Ums.Services.Base;
using Ums.Services.Security;
using Ums.Services.Users;

namespace Ums.Services.System
{
    public class SessionService : Service<Session>, ISessionService
    {
        public SessionService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}