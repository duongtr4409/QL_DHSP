using System.Data.Entity;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class RoleFunctionService : Service<RoleFunction>, IRoleFunctionService
    {
        public override void Delete(RoleFunction entity)
        {
            HardDelete(entity);
        }

        public RoleFunctionService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}