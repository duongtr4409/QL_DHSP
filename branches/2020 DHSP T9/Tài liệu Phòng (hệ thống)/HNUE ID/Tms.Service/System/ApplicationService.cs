using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class ApplicationService : Service<SystemApplication>, IApplicationService
    {
        public ApplicationService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public SystemApplication GetByToken(string token)
        {
            return base.Gets().FirstOrDefault(i => i.Token == token);
        }

        public bool Validate(string token)
        {
            return base.Gets().Any(i => i.Token == token);
        }
    }
}