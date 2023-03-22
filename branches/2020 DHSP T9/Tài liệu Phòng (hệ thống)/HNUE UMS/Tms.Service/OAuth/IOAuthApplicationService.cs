using Ums.Core.Domain.OAuth;
using Ums.Services.Base;

namespace Ums.Services.OAuth
{
    public interface IOAuthApplicationService : IService<OAuthApplication>
    {
        OAuthApplication GetByToken(string token);
        bool Validate(string token);
    }
}