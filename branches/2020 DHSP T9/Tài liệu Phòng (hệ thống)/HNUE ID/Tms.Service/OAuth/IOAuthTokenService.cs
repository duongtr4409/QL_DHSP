using Ums.Core.Domain.OAuth;
using Ums.Core.Domain.Users;
using Ums.Services.Base;

namespace Ums.Services.OAuth
{
    public interface IOAuthTokenService : IService<OAuthToken>
    {
        OAuthToken CreateAccessToken(int sessionId);
        User GetUser(string accessToken);
        void SignOut(int sessionId);
    }
}