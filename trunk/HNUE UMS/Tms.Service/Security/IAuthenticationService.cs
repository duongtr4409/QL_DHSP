namespace Ums.Services.Security
{
    public interface IAuthenticationService
    {
        bool Validate(string username, string password);
    }
}