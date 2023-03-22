using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Organize;
using Ums.Core.Types;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Organize
{
    public class DepartmentService : Service<Department>, IDepartmentService
    {

        public override IQueryable<Department> Gets()
        {
            return base.Gets().OrderBy(i => i.Name);
        }

        public IQueryable<Department> Gets(DepartmentTypes[] type)
        {
            if (type.Length < 1) return Gets();
            var ts = type.Select(i => (int)i).ToArray();
            return Gets().Where(i => ts.Contains(i.TypeId)).OrderBy(i => i.Name);
        }

        public DepartmentService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}