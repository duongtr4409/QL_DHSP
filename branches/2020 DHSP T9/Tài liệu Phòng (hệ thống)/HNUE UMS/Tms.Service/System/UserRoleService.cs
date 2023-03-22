using System.Data.Entity;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class UserRoleService : Service<UserRole>, IUserRoleService
    {
        public UserRoleService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}