using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Organize;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Organize
{
    public class DepartmentService : Service<Department>, IDepartmentService
    {

        public DepartmentService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public override IQueryable<Department> Gets()
        {
            return base.Gets().OrderBy(i => i.Name);
        }
    }
}