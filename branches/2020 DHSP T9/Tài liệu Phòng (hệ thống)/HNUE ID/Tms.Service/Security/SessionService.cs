using Hnue.Helper;
using System;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Users;
using Ums.Core.Entities.Shared;
using Ums.Models.Security;
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

        public Session CreateSession(int userId)
        {
            var session = new Session
            {
                UserId = userId,
                SessionId = Common.RandomString(128) + Common.GetTimeString()
            };
            var id = Insert(session);
            return Get(id);
        }

        public void DeleteSession(string sessionId)
        {
            var session = Gets().FirstOrDefault(i => i.SessionId == sessionId);
            Delete(session);
        }

        public Session GetBySessionId(string sessionId)
        {
            return Gets().FirstOrDefault(i => i.SessionId == sessionId);
        }

        public User GetUser(string sessionId)
        {
            return Gets().FirstOrDefault(i => i.SessionId == sessionId)?.User ?? null;
        }
    }
}