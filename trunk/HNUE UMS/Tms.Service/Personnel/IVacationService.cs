using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;

namespace Ums.Services.Personnel
{
    public interface IVacationService : IService<Vacation>
    {
        Vacation GetByStaff(int staffId);
        IQueryable<Vacation> GetsbyDepartment(int departmentId);
    }
}