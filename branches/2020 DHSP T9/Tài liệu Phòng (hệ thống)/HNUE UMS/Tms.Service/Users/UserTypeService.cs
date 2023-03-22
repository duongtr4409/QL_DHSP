using System.Data.Entity;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;
using Ums.Services.Users;

namespace Ums.Services.System
{
    public class UserTypeService : Service<UserType>, IUserTypeService
    {
        public UserTypeService(DbContext context, IContextService contextService) : base(context, contextService)
        {

        }
    }
}