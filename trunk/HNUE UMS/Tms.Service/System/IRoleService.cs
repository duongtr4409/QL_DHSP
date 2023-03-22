using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.Base;

namespace Ums.Services.System
{
    public interface IRoleService : IService<Role>
    {
        IQueryable<Role> GetUserRoles(int userId);
    }
}