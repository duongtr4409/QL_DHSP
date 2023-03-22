using System.Web;
using System.Web.Security;
using Ums.Services.OAuth;
using Ums.Services.Users;

namespace Ums.Services.Security
{
    public class SignInService : ISignInService
    {
        private readonly ISessionService _sessionService;
        private readonly IOAuthTokenService _oAuthToken;
        public SignInService(ISessionService sessionService, IOAuthTokenService oAuthToken)
        {
            _sessionService = sessionService;
            _oAuthToken = oAuthToken;
        }

        public void SignIn(int userId)
        {
            var session = _sessionService.CreateSession(userId);
            FormsAuthentication.SetAuthCookie(session.SessionId, true);
        }
        public void SignOut()
        {
            var session = _sessionService.GetBySessionId(HttpContext.Current.User.Identity.Name);
            if (session != null)
            {
                _sessionService.Delete(session);
                _oAuthToken.SignOut(session.Id);
            }
            FormsAuthentication.SignOut();
        }
    }
}