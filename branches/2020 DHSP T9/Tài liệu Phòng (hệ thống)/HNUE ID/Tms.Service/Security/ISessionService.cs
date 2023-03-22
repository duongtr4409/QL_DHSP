using Ums.Core.Domain.Users;
using Ums.Core.Entities.Shared;
using Ums.Models.Security;
using Ums.Services.Base;

namespace Ums.Services.Users
{
    public interface ISessionService : IService<Session>
    {
        Session CreateSession(int userId);
        void DeleteSession(string sessionId);
        User GetUser(string sessionId);
        Session GetBySessionId(string name);
    }
}