using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.OAuth;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.OAuth
{
    public class OAuthApplicationService : Service<OAuthApplication>, IOAuthApplicationService
    {
        public OAuthApplicationService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public OAuthApplication GetByToken(string token)
        {
            return base.Gets().FirstOrDefault(i => i.Token == token);
        }

        public bool Validate(string token)
        {
            return base.Gets().Any(i => i.Token == token);
        }
    }
}