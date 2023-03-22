using System.Web.Security;

namespace Ums.Services.Security
{
    public class SignInService : ISignInService
    {
        public void SignIn(string username)
        {
            FormsAuthentication.SetAuthCookie(username, true);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}