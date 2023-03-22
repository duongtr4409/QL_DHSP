using Ums.Core.Domain.Users;
using Ums.Models.Security;

namespace Ums.Services.Security
{
    public interface IAuthenticationService
    {
        User Validate(string username, string password, string type);
        int Validate(GoogleTokenModel model);
        int Validate(FacebookTokenModel model);
    }
}