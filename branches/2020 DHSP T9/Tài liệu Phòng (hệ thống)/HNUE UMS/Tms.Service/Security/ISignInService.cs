namespace Ums.Services.Security
{
    public interface ISignInService
    {
        void SignIn(string username);
        void SignOut();
    }
}