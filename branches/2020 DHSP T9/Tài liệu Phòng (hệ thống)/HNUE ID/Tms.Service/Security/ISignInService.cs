namespace Ums.Services.Security
{
    public interface ISignInService
    {
        void SignIn(int userId);
        void SignOut();
    }
}