using Ums.Core.Domain.Users;

namespace Ums.Services.Security
{
    public interface IContextService
    {
        User GetCurrentUser();
    }
}