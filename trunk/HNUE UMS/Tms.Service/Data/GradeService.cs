using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class GradeService : Service<Grade>, IGradeService
    {
        public override IQueryable<Grade> Gets()
        {
            return base.Gets().OrderBy(i => i.Order);
        }

        public IQueryable<Grade> GetsbyUser(StaffUser user)
        {
            if (user.IsAdmin) return Gets();
            var ids = user.UsersRoles.SelectMany(i => i.Role.RolesGrades).Select(i => i.GradeId).ToArray();
            return Gets().Where(i => ids.Contains(i.Id));
        }

        public GradeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}