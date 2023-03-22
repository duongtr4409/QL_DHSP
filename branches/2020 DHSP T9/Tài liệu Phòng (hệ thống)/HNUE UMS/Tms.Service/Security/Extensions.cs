using System.Linq;
using Ums.Core.Domain.System;

namespace Ums.Services.Security
{
    public static class Extensions
    {
        public static bool IsRoleAdmin(this StaffUser user, string functionkey)
        {
            if (user.IsAdmin) return true;
            var role = user.UsersRoles.SelectMany(i => i.Role.RolesFunctions.Select(j => new { j.RoleId, j.Function })).Where(i => i.Function.Key == functionkey).Select(i => i.RoleId).ToArray();
            return user.UsersRoles.Any(i => role.Contains(i.RoleId) && i.IsLeader);
        }
    }
}