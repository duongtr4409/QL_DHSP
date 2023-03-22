using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;

namespace Ums.Services.Personnel
{
    public interface IStaffService : IService<Staff>
    {
        IQueryable<Staff> Gets(int departmentId = 0, int moved = 0, int retired = 0, string keyword = "", int titleId = 0, int probation = 0, int movedOrRetired = 0, int teachingInId = 0, int departmentType = 0);
    }
}