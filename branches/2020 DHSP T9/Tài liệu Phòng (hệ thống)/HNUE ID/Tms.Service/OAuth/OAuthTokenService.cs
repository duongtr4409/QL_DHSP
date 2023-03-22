using Hnue.Helper;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.OAuth;
using Ums.Core.Domain.Users;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.OAuth
{
    public class OAuthTokenService : Service<OAuthToken>, IOAuthTokenService
    {
        public OAuthTokenService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public OAuthToken CreateAccessToken(int sessionId)
        {
            var token = new OAuthToken
            {
                AccessToken = Common.RandomString(128) + Common.GetTimeString(),
                SessionId = sessionId
            };
            Update(token);
            return Get(token.Id);
        }

        public User GetUser(string accessToken)
        {
            var token = Gets().FirstOrDefault(i => i.AccessToken == accessToken);
            return token.Session?.User ?? null;
        }

        public void SignOut(int sessionId)
        {
            var token = Gets().FirstOrDefault(i => i.SessionId == sessionId);
            Delete(token);
        }
    }
}