using System.Data.Entity;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class RoleGradeService : Service<RoleGrade>, IRoleGradeService
    {
        public override void Delete(RoleGrade entity)
        {
            HardDelete(entity);
        }

        public RoleGradeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}