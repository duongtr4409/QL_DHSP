using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;

namespace Ums.Services.Personnel
{
    public interface IStaffService : IService<Staff>
    {
        IQueryable<Staff> GetTrainerIn(int departmentId = 0, int moved = 0, int retired = 0, int titleId = 0, string keyword = "", int movedOrRetired = 0, int teachingInId = 0);
        IQueryable<Staff> GetLecturerIn(int departmentId = 0, int moved = 0, int retired = 0);
        IQueryable<Staff> GetTeacherIn(int departmentId = 0, int moved = 0, int retired = 0);
        IQueryable<Staff> Gets(int departmentId = 0, int moved = 0, int retired = 0, string keyword = "", int titleId = 0, int probation = 0, int movedOrRetired = 0, int teachingInId = 0, int departmentType = 0, int degreeId = 0);
    }
}