using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class RoleService : Service<Role>, IRoleService
    {
        private readonly IRoleFunctionService _roleFunctionService;
        private readonly IUserRoleService _userRoleService;
        public override void Delete(Role entity)
        {
            foreach (var rf in entity.RolesFunctions)
            {
                _roleFunctionService.Delete(rf);
            }
            base.Delete(entity);
        }

        public IQueryable<Role> GetUserRoles(int userId)
        {
            return _userRoleService.Gets().Where(i => i.UserId == userId).Select(i => i.Role);
        }

        public RoleService(DbContext context, IContextService contextService, IRoleFunctionService roleFunctionService, IUserRoleService userRoleService) : base(context, contextService)
        {
            _roleFunctionService = roleFunctionService;
            _userRoleService = userRoleService;
        }
    }
}