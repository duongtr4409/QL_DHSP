using System.Data.Entity;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;
using Ums.Services.Users;

namespace Ums.Services.System
{
    public class UserGroupService : Service<UserGroup>, IUserGroupService
    {
        public UserGroupService(DbContext context, IContextService contextService) : base(context, contextService)
        {

        }
    }
}