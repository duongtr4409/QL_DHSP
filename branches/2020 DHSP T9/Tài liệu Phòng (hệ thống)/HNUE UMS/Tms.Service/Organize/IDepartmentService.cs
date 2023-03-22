using System.Linq;
using Ums.Core.Domain.Organize;
using Ums.Core.Types;
using Ums.Services.Base;

namespace Ums.Services.Organize
{
    public interface IDepartmentService : IService<Department>
    {
        IQueryable<Department> Gets(DepartmentTypes[] type);
    }
}